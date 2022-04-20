using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerCam : PlayerNetworkBehaviour
{
    // LOCAL PLAYER
    [SerializeField]
    private float camSize;
    private float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private Transform target;
    private Camera cam;
    private Animator camFollowAnimator;
    private UITransition uiTransition;

    // SERVER
    private AvalancheEvents avalancheEvents;

    public override void OnStartServer()
    {
        base.OnStartServer();

        avalancheEvents = AvalancheEvents.GetInstance();
        avalancheEvents.eventAvalancheForewarn -= OnAvalancheForewarn;
        avalancheEvents.eventAvalancheForewarn += OnAvalancheForewarn;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        var camFollow = GameObject.FindGameObjectWithTag("CamFollow");
        if (camFollow == null)
            ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(camFollow));

        camFollowAnimator = camFollow.GetComponent<Animator>();

        if (camFollowAnimator == null)
            ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(camFollowAnimator));

        target = transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (cam == null)
            ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(cam));

        cam.orthographicSize = camSize;

        uiTransition = UITransition.GetInstance();
    }

    [ClientCallback]
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (target && cam)
            {
                Vector3 point = cam.WorldToViewportPoint(target.position);
                Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                Vector3 destination = cam.transform.position + delta;
                destination.z = -120f;
                cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, dampTime);
            }
        }
    }

    #region Forewarn
    [ServerCallback]
    private void OnAvalancheForewarn(object sender, EventAvalancheForewarn e)
    {
        if (!e.Conns.Contains(connectionToClient))
            return;

        TargetOnAvalancheForewarn(e.WarningsIssued);
    }

    [TargetRpc]
    public void TargetOnAvalancheForewarn(int warningsIssued) => camFollowAnimator.SetTrigger($"shake{warningsIssued}");
    #endregion

    #region AvalancheCallback

    [ServerCallback]
    protected override void OnAvalancheCallback(object sender, EventAvalanche e)
    {
        TargetOnAvalanche();
    }

    [TargetRpc]
    private void TargetOnAvalanche()
    {
        camFollowAnimator.SetBool("avalanche", true); // shake screen
        uiTransition.PlayBlankBlackScreenAnimation(true); // blankBlack
    }
    #endregion

    #region AvalancheStoppedCallback

    [ServerCallback]
    protected override void OnAvalancheStoppedCallback(object sender, EventAvalancheStopped e) => TargetOnStoppedAvalanche();

    [TargetRpc]
    private void TargetOnStoppedAvalanche()
    {
        uiTransition.PlayBlankBlackScreenAnimation(false);
        uiTransition.PlayLoseAnimation(false);
    }
    #endregion

    #region GameStateChanged

    [ServerCallback]
    protected override void OnGameStateChanged(object sender, EventGameStateChanged e)
    {
        if (e.GameState == GameState.LOSE)
            TargetOnGameStateChanged((byte) GameState.LOSE);
    }

    [TargetRpc]
    private void TargetOnGameStateChanged(byte _gameState)
    {
        var gameState = (GameState) _gameState;
        if (gameState == GameState.LOSE)
        {
            UITransition.GetInstance().PlayLoseAnimation(true);
        }
    }
    #endregion

    #region OnDestroy
    protected override void OnDestroyServer()
    {
        avalancheEvents.eventAvalancheForewarn -= OnAvalancheForewarn;
    }
    #endregion

}

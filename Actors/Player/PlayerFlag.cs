using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFlag : PlayerNetworkBehaviour
{
    private static PlayerFlag instance;
    public static PlayerFlag GetInstance() => instance;

    [SyncVar]
    private float refFlagCd;

    [SyncVar(hook = nameof(OnFlagCdChanged))]
    private float flagCd;

    [SyncVar]
    private float refRecollectFlagCd;

    [SyncVar]
    private float recollectFlagCd;

    [SyncVar(hook = nameof(OnRemainingFlagsChanged))]
    private int remainingFlags;

    [SyncVar]
    private int refRemainingFlags;

    [SyncVar]
    private int numberOfPlantedFlags;

    private Flag flag;

    public override void OnStartServer()
    {
        base.OnStartServer();

        flagCd = refFlagCd = LobbySettings.GetInstance().FlagCd;
        remainingFlags = refRemainingFlags = LobbySettings.GetInstance().RemainingFlags;
        recollectFlagCd = refRecollectFlagCd = 1f; // always 1 anyway;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;
    }

    private void Update()
    {
        if(isServer)
        {
            if (flagCd > 0)
                flagCd -= Time.deltaTime;

            if (recollectFlagCd > 0)
                recollectFlagCd -= Time.deltaTime;
        }
    }

    [ClientCallback]
    public void PlantFlag()
    {
        if (!IsMoving)
            CmdPlantFlag();
    }

    [Command]
    private void CmdPlantFlag()
    {
        if (flagCd > 0 || isOnHazard || isAvalanched || health <= 5 || remainingFlags <= 0 || isPlayingMiniGame || flag != null)
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Failed 1 more more conditions to plant flag");
            return;
        }

        flagCd = refFlagCd;

        MiniGameEvents.GetInstance().eventPlayingMiniGame?.Invoke(this, new EventPlayingMiniGame(connectionToClient, true, MiniGameName.PLANT_FLAG));

        TargetShowPlantFlagMiniGame(++numberOfPlantedFlags); // increases speed with each planted flag
    }

    [ClientCallback]
    public void RecollectFlag()
    {
        if (!IsMoving)
            CmdRecollectFlag();
    }

    [Command]
    private void CmdRecollectFlag()
    {
        if (recollectFlagCd > 0 || isOnHazard || isAvalanched || isPlayingMiniGame || flag == null || flag.GetOwner() != connectionToClient)
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Failed 1 more more conditions to recollect flag");
            return;
        }

        recollectFlagCd = refRecollectFlagCd;

        TargetRecollectFlagMiniGame(numberOfPlantedFlags); // the difficulty increases each time you plant/unplant
    }

    #region MiniGame
    [TargetRpc]
    private async void TargetShowPlantFlagMiniGame(int numberOfPlantedFlags)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("flag", LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
            await Task.Delay(100);

        ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Finished loading plantflag scene");

        FlagRoot.GetInstance().Setup(numberOfPlantedFlags, true);
    }
   
    [TargetRpc]
    private async void TargetRecollectFlagMiniGame(int numberOfPlantedFlags)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("flag", LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
            await Task.Delay(100);

        ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Finished loading flag scene");

        FlagRoot.GetInstance().Setup(numberOfPlantedFlags, false);
    }
   
    [ClientCallback]
    public void EndMiniGame(bool isWin)
    {
        SceneManager.UnloadSceneAsync("flag");
        CmdEndMiniGame(isWin);
    }

    [Command]
    private void CmdEndMiniGame(bool isWin)
    {
        if (isWin)
        {
            if (miniGameName == MiniGameName.PLANT_FLAG) // spawn flag
            {
                remainingFlags -= 1;

                Flag flag = Instantiate(MyNetwork.getInstance().findSpawnPrefabByName("Flag")).GetComponent<Flag>();
                flag.transform.position = new Vector2((int)transform.position.x, (int)transform.position.y);
                flag.owner = connectionToClient;
                NetworkServer.Spawn(flag.gameObject);

                ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Spawned flag. Remaining flags={remainingFlags}/{refRemainingFlags}");

            }
            else if (miniGameName == MiniGameName.RECOLLECT_FLAG)
            {
                flag = null;

                remainingFlags++;

                NetworkServer.Destroy(flag.gameObject);

                ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Recollected flag. Remaining flags={remainingFlags}/{refRemainingFlags}");
            }

        }
        else // didn't managed to finish mini game for whatever reason
        {

        }

        MiniGameEvents.GetInstance().eventPlayingMiniGame?.Invoke(this, new EventPlayingMiniGame(connectionToClient, false, MiniGameName.NONE));
    }
    #endregion

    [ClientCallback]
    private void OnFlagCdChanged(float oldCd, float newCd) => ToggleBtnFlag();

    [ClientCallback]
    private void OnRemainingFlagsChanged(int old, int v) => ToggleBtnFlag();

    [ClientCallback]
    private void ToggleBtnFlag()
    {
        if (isLocalPlayer)
        {
            UIBtnFlag uiBtnFlag = UIBtnFlag.GetInstance();

            if (remainingFlags <= 0 || flagCd > 0)
                uiBtnFlag.SetIsInteractable(false);
            else
                uiBtnFlag.SetIsInteractable(true);

            uiBtnFlag.SetCd(flagCd, refFlagCd);
            uiBtnFlag.SetRemainingFlags(remainingFlags);
        }
    }

    [ServerCallback]
    protected override void OnEnteredFlagCallback(object sender, EventEnteredFlag e)
    {
        if (e.TriggerRingId == 0)
        {
            SetFlag(e.Flag);

            if (e.Flag.GetOwner() == connectionToClient) // flag belongs to this owner, allow recollection
                TargetShowPlantFlagAndHideCollectFlag(false);
        }
    }

    [ServerCallback]
    protected override void OnExitedFlagCallback(object sender, EventExitedFlag e)
    {
        if (e.TriggerRingId == 0)
        {
            SetFlag(null);

            if (e.Flag.GetOwner() == connectionToClient) // flag belongs to this owner, allow recollection
                TargetShowPlantFlagAndHideCollectFlag(true);
        }
    }

    [ServerCallback]
    private void SetFlag(Flag flag) => this.flag = flag;

    [TargetRpc]
    private void TargetShowPlantFlagAndHideCollectFlag(bool b)
    {
        var uiPlayer = UIPlayer.GetInstance();
        uiPlayer.ShowBtnPlantFlag(b);
        uiPlayer.ShowBtnCollectFlag(!b);
    }
}



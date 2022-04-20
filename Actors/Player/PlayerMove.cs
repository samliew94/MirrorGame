using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

public class PlayerMove : PlayerNetworkBehaviour
{
    [SyncVar]
    public float Speed;

    private Vector2 oriPos, targetPos;
    private float timeToMove = .2f;

    private bool isMoving; // CLIENT

    private PlayerHealth playerHealth; // LOCAL PLAYER

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        playerHealth = GetComponent<PlayerHealth>();
    }

    public void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (IsMoving || isOnHazard || isAvalanched || playerHealth.GetHealth() <= 0)
                    return;

                Vector2 dir;

                if (Input.GetKey(KeyCode.W))
                    dir = Vector2.up;
                else if (Input.GetKey(KeyCode.A))
                    dir = Vector2.left;
                else if (Input.GetKey(KeyCode.S))
                    dir = Vector2.down;
                else if (Input.GetKey(KeyCode.D))
                    dir = Vector2.right;
                else
                    return;

                MovePlayer(dir);
            }

               
        }
    }

    [ClientCallback]
    private async void MovePlayer(Vector2 dir)
    {
        isMoving = true;

        oriPos = transform.position;
        targetPos = oriPos + dir;
        targetPos = new Vector2((int)targetPos.x, (int)targetPos.y);

        bool isTargetOutOfBounds = GridRenderer.GetInstance().IsTargetOutOfBounds(targetPos);

        if (isTargetOutOfBounds)
        {
            isMoving = false;
            return;
        }

        float elapsedTime = 0;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector2.Lerp(oriPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }

        transform.position = targetPos;

        isMoving = false;
    }

    /// <summary>
    /// GET accessor that should be called by every other classes to access _isMoving
    /// </summary>
    public bool GetIsMoving() => isMoving;
}

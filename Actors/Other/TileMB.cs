using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TileMB : MonoBehaviour, ITriggerable
{
    private SpriteRenderer spriteRenderer;
    private TileCode tileCode;
    private bool permaReveal;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// given x & y coordinate, color the tile by even/odd
    /// </summary>
    public void SetColorAndAlpha(int x, int y)
    {
        if (tileCode == TileCode.BLOCKED)
        {
            spriteRenderer.enabled = false;
            return;
        }

        bool isEven = (x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0);

        if (isEven)
            spriteRenderer.color = new Color(0.513f, 0.733f, 0.949f);
        else
            spriteRenderer.color = new Color(0.407f, 0.607f, 0.890f);

        Show(x < 3 ? true : false);
        SetPermaReveal(x < 3 ? true : false);
    }

    public TileCode GetTileCode() => tileCode;
    public void SetTileCode(TileCode tileCode) => this.tileCode = tileCode;
    public void Show(bool isShow)
    {
        animator.SetBool("isShow", isShow);

        var interactable = FindObjectsOfType<Interactable>()
                .FirstOrDefault(i => i.transform.position.x == transform.position.x && i.transform.position.y == transform.position.y)
                ;

        interactable?.Show(isShow);
    }

    public void SetPosition(int x, int y) => transform.position = new Vector2(x, y);
    public bool GetPermaReveal() => this.permaReveal;
    public void SetPermaReveal(bool permaReveal) => this.permaReveal = permaReveal;
    public void onCustomTriggerEnter(GameObject gameObject, int triggerRingId)
    {
        var playerTileRenderer = gameObject.GetComponentInParent<PlayerTileRenderer>();

        if (playerTileRenderer != PlayerTileRenderer.GetInstance()) // not localplayer
            return;

        Show(true);
    }

    public void onCustomTriggerExit(GameObject gameObject, int triggerRingId)
    {
        var playerTileRenderer = gameObject.GetComponentInParent<PlayerTileRenderer>();

        if (playerTileRenderer != PlayerTileRenderer.GetInstance()) // not localplayer
            return;

        if (!permaReveal)
            Show(false);
    }
}

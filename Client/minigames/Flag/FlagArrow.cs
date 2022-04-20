using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class FlagArrow : MonoBehaviour
{
    private bool isHitGoal;
    private Rigidbody2D rb2d;
    private bool isReady;

    [SerializeField]
    private float speed;

    private void Awake() => rb2d = GetComponent<Rigidbody2D>();

    private void Start() => isReady = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var colName = collision.transform.name;

        if (colName == "goal")
        {
            ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Hit goal");
            isHitGoal = true;
        }

        if (colName == "bottom")
            SetSpeed(Mathf.Abs(speed));
        else if (colName == "top")
            SetSpeed(Mathf.Abs(speed) * -1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "goal")
        {
            ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"!Hit goal");
            isHitGoal = false;
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        rb2d.velocity = new Vector2(0, speed);
    }

    public bool GetIsReady() => isReady;
    public bool GetIsHitGoal() => isHitGoal;

}



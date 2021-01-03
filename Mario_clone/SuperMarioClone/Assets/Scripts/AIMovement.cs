using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementEnum
{
    Patrolling, Stalking, OneDirBounce, Idle
}

public class AIMovement : MonoBehaviour
{
    private const float patrollGrace = 0.1f;


    [GiveTag]
    public string[] EngageOn = new string[] { };
    [GiveTag]
    public string[] BounceOn = new string[] { };

    public MovementEnum StartState;
    public MovementEnum EngagedState;
    private MovementEnum currentState;
    private MovementEnum savedState;

    public float speed;
    public float AggroRange;
    private bool isDead = false;

    public List<GameObject> CheckPoints = new List<GameObject>();
    private List<Vector2> checkPointPos = new List<Vector2>();
    private List<bool> checkPointCheck = new List<bool>();
    private bool reverse = false;
    private int checkPointIndex = 0;


    private bool engaged;
    
    private GameObject target;

    private bool bounce;
    private Vector2 directionMultiplier; // incase u want to reverse gravity or something

    private new Rigidbody2D rigidbody2D;
    private List<GameObject> allTargets = new List<GameObject>();

    void Start()
    {
        currentState = StartState;
        rigidbody2D = GetComponent<Rigidbody2D>();

        for (int i = 0; i < CheckPoints.Count; i++)
        {
            checkPointPos.Add(CheckPoints[i].transform.position);
            checkPointCheck.Add(false);
        }


        directionMultiplier = new Vector2(1, 0);
    }

    


    void Update()
    {
        if (!GetComponent<Rigidbody2D>())
            isDead = true;
           

        if (!isDead)
        {
            if (EngageOn != null)
                if (engaged == false || target == null)
                    checkEngagement();

            move();
        }
    }

     public void Freeze()
    {
        savedState = currentState;
        currentState = MovementEnum.Idle;
    }

   public void UnFreeze()
    {
        currentState = savedState;
    }

    public void Die()
    {
        isDead = true;
    }

    private void checkEngagement()
    {
        allTargets.Clear(); // clear the target list, incase another target is added or removed etc...
        for (int i = 0; i < EngageOn.Length; i++)
            allTargets.AddRange(GameObject.FindGameObjectsWithTag(EngageOn[i]));

        float tempDistance = AggroRange;
        GameObject tempTarget = null;

        for (int i = 0; i < allTargets.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, allTargets[i].transform.position);

            if (distance > AggroRange)
                continue;

            if (distance <= tempDistance)
            {
                tempDistance = distance;
                tempTarget = allTargets[i];
            }
        }
        target = tempTarget;
        if (target != null)
        {
            engaged = true;
            currentState = EngagedState;
        }
    }

    private void move()
    {
        bounce = false;
        if (currentState == MovementEnum.Stalking)
        {
            float X = target.transform.position.x - transform.position.x; // positive value = right, negative = left

            if (X > 0)
                rigidbody2D.velocity = Vector2.right * speed;

            if (X < 0)
                rigidbody2D.velocity = Vector2.left * speed;

            //    rigidbody2D.velocity = new Vector2(GetComponent<Rigidbody2D>, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (currentState == MovementEnum.OneDirBounce)
        {
            bounce = true;
            rigidbody2D.velocity = new Vector2(speed * directionMultiplier.x, rigidbody2D.velocity.y);
        }

        if (currentState == MovementEnum.Patrolling)
        {
            if (Vector2.Distance(transform.position, checkPointPos[checkPointIndex]) <= patrollGrace)
            {
                if (reverse)
                    checkPointIndex--;

                else if (!reverse)
                    checkPointIndex++;
            }

            if (checkPointIndex == CheckPoints.Count - 1)
            {
                reverse = true;
            }
            else if (checkPointIndex == 0)
            {
                reverse = false;
            }

            Vector2 desiredDir = (checkPointPos[checkPointIndex] - (Vector2)transform.position).normalized;
            rigidbody2D.velocity = desiredDir * speed;

        }

        if (currentState == MovementEnum.Idle)
        {
            rigidbody2D.velocity = Vector2.zero;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < BounceOn.Length; i++)
            if (bounce && BounceOn[i] == collision.transform.tag)
                directionMultiplier *= -1;
    }


}

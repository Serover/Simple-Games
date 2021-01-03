using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterBehavior
{
    Classic, Simple, Off
}
public class MonsterAI : MonoBehaviour
{
    [GiveTag]
    public string[] InteractWith = new string[] { };
    [SerializeField]
    private int numberOfDirs = 8;

    public float AggroRange;
    public float AttackRange;
    public MonsterBehavior AIAttacking;


    private bool engaged;
    private GameObject target;

    private int attacks;

    private new Rigidbody2D rigidbody2D;
    private InteractAble interact;


    List<GameObject> allTargets = new List<GameObject>();

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        interact = GetComponent<InteractAble>();
        attacks = interact.attacks.Count;
    }

    void Update()
    {
        if (engaged == false || target == null)
            checkEngagement();

        if (engaged == true && target != null)
        {

            for (int i = 0; i < attacks; i++)
            {
                attack(i, AIAttacking);
            }

 
        }
        else
            rigidbody2D.velocity = Vector2.zero;
    }
    private void attack(int a, MonsterBehavior ai)
    {
        if ( AttackRange >= Vector2.Distance(transform.position, target.transform.position))
        {
            Vector2 dir = getTargetDir(ai);
            interact.Attack(a, dir);
        }
    }
    private Vector2 getTargetDir(MonsterBehavior ai)
    {
        Vector2 returnDir = Vector2.zero;

        if (ai == MonsterBehavior.Simple)
            returnDir = (target.transform.position - transform.position).normalized;

        else if (ai == MonsterBehavior.Classic)
        {
            int degree = Random.Range(0, numberOfDirs);
            degree *= (360 / numberOfDirs);
            float radian = degree * Mathf.Deg2Rad;
            returnDir = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
        return returnDir;
    }
    private void checkEngagement()
    {
        allTargets.Clear(); // clear the target list, incase another target is added or removed etc...
        for (int i = 0; i < InteractWith.Length; i++)
            allTargets.AddRange(GameObject.FindGameObjectsWithTag(InteractWith[i]));

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
            engaged = true;
    }
}

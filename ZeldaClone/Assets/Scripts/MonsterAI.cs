using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterBehavior
{
    Classic, Simple
}
public class MonsterAI : MonoBehaviour
{
    [TagSelector]
    public string[] InteractWith = new string[] { };
    [SerializeField]
    private int numberOfDirs = 8;

    public float AggroRange;
    public MonsterBehavior AIMovement;
    public MonsterBehavior AIAttacking;

    public bool Movement;
    public float Speed;
    [SerializeField]
    private float movementIntervall;
    private float InterVallCountdown;

    private bool engaged;
    private GameObject target;

    private int attacks;

    private new Rigidbody2D rigidbody2D;
    private InteractAble interact;
    List<float> coolDowns = new List<float>();
    List<float> baseCoolDowns = new List<float>();

    List<GameObject> allTargets = new List<GameObject>();

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        interact = GetComponent<InteractAble>();
        attacks = interact.attacks.Count;

        for (int i = 0; i < interact.attacks.Count; i++)
            coolDowns.Add(interact.attacks[i].AttackCoolDown);

        for (int i = 0; i < interact.attacks.Count; i++)
            baseCoolDowns.Add(interact.attacks[i].AttackCoolDown);

    }

    void Update()
    {
        if (engaged == false || target == null)
            checkEngagement();

        if (CameraScript.Transitioning == false && engaged == true && target != null)
        {
            CoolDownTick();

            for (int i = 0; i < attacks; i++)
                attack(i, AIAttacking);

            if (Movement == true)
                move(AIMovement);
        }
        else
            rigidbody2D.velocity = Vector2.zero;
    }
    private void attack(int a, MonsterBehavior ai)
    {
        if (coolDowns[a] <= 0 && interact.attacks[a].AttackRange >= Vector2.Distance(transform.position, target.transform.position))
        {
            Vector2 dir = getTargetDir(ai);
            interact.Attack(a, dir);
            coolDowns[a] = baseCoolDowns[a];
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
    private void move(MonsterBehavior ai)
    {
        if (InterVallCountdown <= 0)
        {
            Vector2 dir = getTargetDir(ai);
            rigidbody2D.velocity = dir * Speed;
            InterVallCountdown = movementIntervall;
        }
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
    private void CoolDownTick()
    {
        for (int j = 0; j < coolDowns.Count; j++)
            if (coolDowns[j] >= 0)
                coolDowns[j] -= Time.deltaTime;

        if (InterVallCountdown >= 0)
            InterVallCountdown -= Time.deltaTime;
    }
    public void Die()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<MovementAnimatorController>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(this);
    }
}

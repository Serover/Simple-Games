using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class attackCode
{
    public KeyCode Button;
    public int AttackNumber;
}


public class PlayerControllAttack : MonoBehaviour
{

    public List<attackCode> AttackCalls = new List<attackCode>();


    List<float> coolDowns = new List<float>();
    List<float> baseCoolDowns = new List<float>();

    private new Rigidbody2D rigidbody2D;
    private InteractAble interact;
    private Vector2 lastFaced;
    [NonSerialized]
    public bool isActive;


    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        lastFaced = Vector2.right; // default sprite is facing to the right
        isActive = true;
        interact = GetComponent<InteractAble>();

        for (int i = 0; i < interact.attacks.Count; i++)
            coolDowns.Add(interact.attacks[i].AttackCoolDown);

        for (int i = 0; i < interact.attacks.Count; i++)
            baseCoolDowns.Add(interact.attacks[i].AttackCoolDown);
    }

    // Update is called once per frame
    void Update()
    {
        checkActive();

        if (isActive)
        {

            CoolDownTick();
            Attacking();


            float X = rigidbody2D.velocity.x;

            if (X > 0)
                lastFaced = Vector2.right;
            else if (X < 0)
                lastFaced = Vector2.left;

        }
    }

    private void attack(int a, Vector2 dir)
    {

        interact.Attack(a, dir);

    }

    private void Attacking()
    {
        for (int i = 0; i < AttackCalls.Count; i++)
        {
            if (Input.GetKeyDown(AttackCalls[i].Button))
            {


              //  if (!interact.attacks[i].Locked)
                    attack(AttackCalls[i].AttackNumber, lastFaced);
            }
        }
    }

    public void checkActive()
    {
        if (interact.isDead)
        {
            isActive = false;
        }
    }

    public void Die()
    {
        Destroy(this); //  gives the result of turning everything off, could use a isdead bool.
    }

    private void CoolDownTick()
    {
        for (int j = 0; j < coolDowns.Count; j++)
        {
            if (coolDowns[j] <= baseCoolDowns[j])
                coolDowns[j] -= Time.deltaTime;
        }
    }
}

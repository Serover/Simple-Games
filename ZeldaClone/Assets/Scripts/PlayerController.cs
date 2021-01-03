using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string HorAx = "Horizontal";
    private const string VerAx = "Vertical";

    public float speed;
    private Vector2 lastFaced;
    [HideInInspector]
    public bool engaged;

    private new Rigidbody2D rigidbody2D;
    private InteractAble interact;

    List<float> coolDowns = new List<float>();
    List<float> baseCoolDowns = new List<float>();

    void Start()
    {
        engaged = true;

        rigidbody2D = GetComponent<Rigidbody2D>();
        interact = GetComponent<InteractAble>();

        lastFaced = Vector2.right;

        for (int i = 0; i < interact.attacks.Count; i++)
            coolDowns.Add(interact.attacks[i].AttackCoolDown);

        for (int i = 0; i < interact.attacks.Count; i++)
            baseCoolDowns.Add(interact.attacks[i].AttackCoolDown);
    }

    void Update()
    {
        rigidbody2D.velocity = Vector2.zero;

        if (CameraScript.Transitioning == false && engaged == true)
        {
            CoolDownTick();
            move();
            Attacking();
        }
    }
    private void move()
    {
        float X = Input.GetAxis(HorAx);
        float Y = Input.GetAxis(VerAx);

        rigidbody2D.velocity = new Vector2(X * speed, Y * speed);

        if (X != 0 && Y != 0)
            rigidbody2D.velocity = new Vector2(X, Y).normalized * speed;
        
        if (X != 0 || Y != 0)
            lastFaced = rigidbody2D.velocity.normalized;   
    }

    private void attack(int a, Vector2 dir)
    {
        if (coolDowns[a] <= 0)
        {
            interact.Attack(a, dir);
            coolDowns[a] = baseCoolDowns[a];
        }
    }
    public void Die()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<MovementAnimatorController>());
        Destroy(this);
    }

    private void Attacking()
    {
        // "Attack 1"
        if (Input.GetKeyDown(KeyCode.Space))
            attack(0, lastFaced);
        // "Attack 2."
        if (Input.GetKeyDown(KeyCode.Mouse0))
            attack(1, lastFaced);
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

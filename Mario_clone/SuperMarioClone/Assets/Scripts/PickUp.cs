using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Unlock
{
    public int AttackNumber;
    public bool unlock; // if False, it'll will lock, if true it will unlock.

}

public class PickUp : MonoBehaviour
{
    [GiveTag]
    public string[] InteractWith = new string[] { };

    [GiveTag]
    public string[] stopMovementAt = new string[] { };



    public float healing;
    public float score;

    public List<Unlock> unlocks = new List<Unlock>();


    private void OnTriggerEnter2D(Collider2D obj)
    {
        pickingUp(obj.gameObject);

        StopAt(obj.gameObject);

        Locking(obj.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D obj)
    {
        pickingUp(obj.gameObject);

        StopAt(obj.gameObject);

        Locking(obj.gameObject);

    }
    private void pickingUp(GameObject obj)
    {
        for (int i = 0; i < InteractWith.Length; i++)
        {
            if (obj.tag != InteractWith[i] || !obj.GetComponent<InteractAble>())
                continue;

            if (obj.GetComponent<PlayerController>())
            {
                InteractAble.score += score;
                FindObjectOfType<GameMaster>().GetComponent<GameMaster>().UpdatePlayerScore();
            }
            else
                obj.GetComponent<InteractAble>().ondeath.Score += score;

            obj.GetComponent<InteractAble>().takeDamage(-healing, Schools.None);

            Destroy(this.gameObject);
        }
    }

    private void StopAt(GameObject obj)
    {
        for (int i = 0; i < stopMovementAt.Length; i++)
        {
            if (obj.tag != stopMovementAt[i])
                continue;

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;

            if (!GetComponent<BoxCollider2D>().isTrigger)
                GetComponent<BoxCollider2D>().isTrigger = true;

        }
    }

    private void Locking(GameObject obj)
    {

        for (int i = 0; i < InteractWith.Length; i++)
        {
            if (obj.tag != InteractWith[i])
                continue;

            for (int j = 0; j < unlocks.Count; j++)
            {
                if (unlocks[j].unlock)
                    obj.GetComponent<InteractAble>().attacks[unlocks[j].AttackNumber].Locked = false;

                if (!unlocks[j].unlock)
                    obj.GetComponent<InteractAble>().attacks[unlocks[j].AttackNumber].Locked = true;
            }
        }
    }
}

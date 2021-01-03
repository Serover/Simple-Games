using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [TagSelector]
    public string[] InteractWith = new string[] { };

    public float healing;
    public float score;

    private void OnTriggerEnter2D(Collider2D obj)
    {
        for (int i = 0; i < InteractWith.Length; i++)
        {
            if (obj.tag != InteractWith[i] || !obj.GetComponent<InteractAble>())
                continue;

            if (obj.GetComponent<PlayerController>())
            {
                GameMaster.Score += score;
                FindObjectOfType<GameMaster>().GetComponent<GameMaster>().UpdatePlayerScore();
            }
            else
                obj.GetComponent<InteractAble>().ondeath.Score += score;

            obj.GetComponent<InteractAble>().takeDamage(-healing, Schools.None); 

            Destroy(this.gameObject);
        }
    }

}

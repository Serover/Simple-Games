using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncy : MonoBehaviour
{

    [GiveTag]
    public string[] InteractWith = new string[] { };

    public float BounceVelocity;

    public bool BounceMe;
    public bool BounceThem;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < InteractWith.Length; i++)
        {
            if (BounceThem)
            {
                if (collision.transform.tag == InteractWith[i] && collision.transform.GetComponent<Rigidbody2D>())
                {
                    collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.GetComponent<Rigidbody2D>().velocity.x, BounceVelocity);
                }
            }

            if (BounceMe && collision.transform.tag == InteractWith[i])
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, BounceVelocity);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // bounce


        for (int i = 0; i < InteractWith.Length; i++)
        {
            if (BounceThem)
            {
                if (collision.transform.tag == InteractWith[i] && collision.transform.GetComponent<Rigidbody2D>())
                {
                    collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.GetComponent<Rigidbody2D>().velocity.x, BounceVelocity);
                }
            }

            if (BounceMe && collision.transform.tag == InteractWith[i])
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, BounceVelocity);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileWeakness : MonoBehaviour
{
    [TagSelector]
    public string[] InteractWith = new string[] { };

    public float speed;
    private float distanceTravled;
    public bool removeAfterDistance;
    public float RangeBeforeDeath;

    private Vector2 StoredSpeed;

    void Start()
    {
        StoredSpeed = GetComponent<Rigidbody2D>().velocity;
    }

    void Update()
    {
        if (removeAfterDistance)
            TravelTick();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        for (int i = 0; i < InteractWith.Length; i++)
            if (coll.gameObject.tag == InteractWith[i])
                Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        for (int i = 0; i < InteractWith.Length; i++)
            if (coll.gameObject.tag == InteractWith[i])
                Destroy(this.gameObject);
    }
    private void TravelTick()
    {
        distanceTravled = speed * Time.deltaTime;

        if (distanceTravled >= RangeBeforeDeath)
            Destroy(this.gameObject);
    }
    public void pauseMovement()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void resumeMovement()
    {
        GetComponent<Rigidbody2D>().velocity = StoredSpeed;
    }
}
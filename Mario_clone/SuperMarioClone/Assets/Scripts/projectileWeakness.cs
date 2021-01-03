using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileWeakness : MonoBehaviour
{
    [GiveTag]
    public string[] DestroyOnHitList = new string[] { };

    [GiveTag]
    public string[] LoseDurabilityOn = new string[] { };

    public int Durability;

    public float speed;
    private float distanceTravled;
    public bool removeAfterDistance;
    public float RangeBeforeDeath;

    private Vector2 StoredSpeed;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity *= speed;
        StoredSpeed = GetComponent<Rigidbody2D>().velocity;
    }

    void Update()
    {
        if (removeAfterDistance)
            TravelTick();


    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        DestroyOnHit(coll.gameObject);
        DurabilityHit(coll.gameObject);           
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        DestroyOnHit(coll.gameObject);
        DurabilityHit(coll.gameObject);
    }
    private void TravelTick()
    {
        distanceTravled += speed * Time.deltaTime;



        if (distanceTravled >= RangeBeforeDeath)
            Destroy(this.gameObject);
    }

    private void DurabilityHit(GameObject coll)
    {
        for (int i = 0; i < LoseDurabilityOn.Length; i++)
            if (coll.tag == LoseDurabilityOn[i])
            {
                Durability--;
                if (Durability <= 0)
                    Destroy(this.gameObject);
            }
    }
    private void DestroyOnHit(GameObject coll)
    {
        for (int i = 0; i < DestroyOnHitList.Length; i++)
            if (coll.tag == DestroyOnHitList[i])
            {
                DelayedDeath(1);
                this.gameObject.SetActive(false);

            }


    }

    IEnumerator DelayedDeath(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }


    public void pauseMovement()
    {
        StoredSpeed = GetComponent<Rigidbody2D>().velocity;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void resumeMovement()
    {
        GetComponent<Rigidbody2D>().velocity = StoredSpeed;
    }
}
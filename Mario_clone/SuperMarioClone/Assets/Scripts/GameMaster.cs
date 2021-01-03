using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{

    public GameObject scoreText;
    private string startText;

    private PlayerControllAttack player;
    private float lastHP;

    private Vector2 ogScale;

    [HideInInspector]

    void Start()
    {
        startText = scoreText.GetComponent<Text>().text;
        UpdatePlayerScore();
        player = FindObjectOfType<PlayerControllAttack>();
        ogScale = player.GetComponent<Transform>().localScale;


    }

    private void Update()
    {
        UpdatePlayerScore();

        if(player)
        marioHealthScaler();
    }

    public void UpdatePlayerScore()
    {
        scoreText.GetComponent<Text>().text = startText + InteractAble.score;
    }

    static public void pauseProjectileMovement()
    {
        projectileWeakness[] allProjectiles = FindObjectsOfType<projectileWeakness>();
        for (int i = 0; i < allProjectiles.Length; i++)
            allProjectiles[i].pauseMovement();

    }
    static public void resumeProjectileMovement()
    {
        projectileWeakness[] allProjectiles = FindObjectsOfType<projectileWeakness>();
        for (int i = 0; i < allProjectiles.Length; i++)
            allProjectiles[i].resumeMovement();
    }


    private void marioHealthScaler()
    {
        if (player.GetComponent<InteractAble>())
        {
            float hp = player.GetComponent<InteractAble>().stats.Health;

            if (hp != lastHP)
            {
                player.GetComponent<Transform>().localScale = hp * ogScale;
                lastHP = hp;
            }
        }

    }


    static public void Freeze()
    {
        {
            List<PlayerController> objs = new List<PlayerController>();
            objs.AddRange(FindObjectsOfType<PlayerController>());
            for (int i = 0; i < FindObjectsOfType<PlayerController>().Length; i++)
            {
                objs[i].isActive = false;
            }
        }
        {
            List<AIMovement> objs = new List<AIMovement>();
            objs.AddRange(FindObjectsOfType<AIMovement>());
            for (int i = 0; i < FindObjectsOfType<AIMovement>().Length; i++)
            {
                objs[i].Freeze();
            }
        }
        {
            List<PlayerControllAttack> objs = new List<PlayerControllAttack>();
            objs.AddRange(FindObjectsOfType<PlayerControllAttack>());
            for (int i = 0; i < FindObjectsOfType<PlayerControllAttack>().Length; i++)
            {
                objs[i].isActive = false;
            }
        }
    }

    static public void UnFreeze()
    {
        {
            List<PlayerController> objs = new List<PlayerController>();
            objs.AddRange(FindObjectsOfType<PlayerController>());
            for (int i = 0; i < FindObjectsOfType<PlayerController>().Length; i++)
            {
                objs[i].isActive = true;
            }
        }
        {
            List<AIMovement> objs = new List<AIMovement>();
            objs.AddRange(FindObjectsOfType<AIMovement>());
            for (int i = 0; i < FindObjectsOfType<AIMovement>().Length; i++)
            {
                objs[i].UnFreeze();
            }
        }
        {
            List<PlayerControllAttack> objs = new List<PlayerControllAttack>();
            objs.AddRange(FindObjectsOfType<PlayerControllAttack>());
            for (int i = 0; i < FindObjectsOfType<PlayerControllAttack>().Length; i++)
            {
                objs[i].isActive = true;
            }
        }


    }

    static public void Die(GameObject obj)
    {
        /*
        if (obj.GetComponent<MovementAnimatorController>())
            obj.GetComponent<MovementAnimatorController>().Die();

        if (obj.GetComponent<PlayerController>())
            obj.GetComponent<PlayerController>().Die();

        else if (obj.GetComponent<MonsterAI>())
            obj.GetComponent<MonsterAI>().Die();

        if (obj.GetComponent<AIMovement>())
            obj.GetComponent<AIMovement>().Die();

        if (obj.GetComponent<Rigidbody2D>())
            Destroy(obj.GetComponent<Rigidbody2D>());

        if (obj.GetComponent<MovementAnimatorController>())
            Destroy(obj.GetComponent<MovementAnimatorController>());

        if (obj.GetComponent<BoxCollider2D>())
            Destroy(obj.GetComponent<BoxCollider2D>());

        if (obj.GetComponent<PlayerControllAttack>())
            Destroy(obj.GetComponent<PlayerControllAttack>()); */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[Serializable]
public class Items
{
    public GameObject preFabObj;
    public Vector2 Direction;
    public float Speed;
}
[Serializable]
public enum Schools
{
    Physical, Nature, Fire, Water, None
}
public enum AttackTypes
{
    Slash, Line, Circle, Projectile
}

[Serializable]
public class Stats
{
    public float Health;
    public Schools Defense;
    public GameObject splashEffect;
}
[Serializable]
public class onDeath
{
    public float Score;
    public bool showDurability;
    public List<Sprite> Sprites = new List<Sprite>();
    public List<Items> InstantiateOnDeath = new List<Items>();

    public bool removeOnDeath;
    public float TimeBeforeRemoval;

}
[Serializable]
public class Attack
{
    [TagSelector]
    public string[] InteractWith = new string[] { };

    public int Damage;

    public Schools Type;
    public bool passiveDamage;
    public float AttackCoolDown;
    public float AttackRange;

    public AttackTypes attackMove;
    public GameObject VisualEffect;
}
public class InteractAble : MonoBehaviour
{
    public Stats stats;
    public List<Attack> attacks = new List<Attack>();
    public onDeath ondeath;

    private float maxHealth;
    private SpriteRenderer currentImage;

    void Start()
    {
        maxHealth = stats.Health;

        currentImage = GetComponent<SpriteRenderer>();

        if (ondeath.Sprites.Count > 0)
            currentImage.sprite = ondeath.Sprites[0]; // No real reason, just incase the designer forgets to change the original sprite renderer image i help him out.
    }
    public void Attack(int i, Vector2 direction)
    {
        if (attacks[i].attackMove == AttackTypes.Circle)
            circleAttack(i, direction);

        else if (attacks[i].attackMove == AttackTypes.Slash)
            slashAttack(i, direction);

        else if (attacks[i].attackMove == AttackTypes.Line)
            lineAttack(i, direction);

        else if (attacks[i].attackMove == AttackTypes.Projectile)
            safeInstantiate(attacks[i].VisualEffect, direction, attacks[i].VisualEffect.GetComponent<projectileWeakness>().speed, transform.position);
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        thorns(obj.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D obj)
    {
        thorns(obj.gameObject);
    }
    public void takeDamage(float dmg, Schools type)
    {
        if (stats.Defense == type)
            stats.Health -= dmg / 2;

        else if (stats.Defense == Schools.None || type == Schools.None)
            stats.Health -= dmg;

        else
            stats.Health -= dmg;

        if (stats.Health <= 0)
        {
            Die();
            ondeath.showDurability = false;
        }

        if (ondeath.showDurability == true)
            showDurability();

        damageSplash();

        if (GetComponent<PlayerController>())
            FindObjectOfType<GameMaster>().UpdatePlayerHealth(stats.Health);
    }
    private void showDurability()
    {
        float numbOfPics = ondeath.Sprites.Count;
        float breakPoint = maxHealth / numbOfPics;
        float imageNumb = (stats.Health / breakPoint);

        // se till att sista hpet visar sista bilden ifall mer bilder än hälsa.
        if (stats.Health == 1)
        {
            currentImage.sprite = ondeath.Sprites[ondeath.Sprites.Count - 1];
        }
        else
        {
            double temp;
            temp = System.Math.Round(imageNumb, System.MidpointRounding.AwayFromZero);

            currentImage.sprite = ondeath.Sprites[(ondeath.Sprites.Count) - Mathf.RoundToInt((float)temp)];
        }
    }
    public void Die()
    {
        GameMaster.Score += ondeath.Score;
        FindObjectOfType<GameMaster>().GetComponent<GameMaster>().UpdatePlayerScore();       

        for (int i = 0; i < ondeath.InstantiateOnDeath.Count; i++)
            safeInstantiate(ondeath.InstantiateOnDeath[i].preFabObj, ondeath.InstantiateOnDeath[i].Direction, ondeath.InstantiateOnDeath[i].Speed, transform.position);

        if (ondeath.removeOnDeath)
            removeIn(ondeath.TimeBeforeRemoval);

        if (GetComponent<MovementAnimatorController>())
            GetComponent<MovementAnimatorController>().Die();

        if (GetComponent<PlayerController>())
            GetComponent<PlayerController>().Die();

        else if (GetComponent<MonsterAI>())
            GetComponent<MonsterAI>().Die();

    }
    private void thorns(GameObject obj)
    {
        for (int i = 0; i < attacks.Count; i++)
        {
            if (!attacks[i].passiveDamage)
                continue;

            for (int j = 0; j < attacks[i].InteractWith.Length; j++)
            {
                if (obj.tag == attacks[i].InteractWith[i])
                {
                    if (!obj.GetComponent<InteractAble>())
                        continue;

                    obj.GetComponent<InteractAble>().takeDamage(attacks[i].Damage, attacks[i].Type);

                    if (GetComponent<projectileWeakness>())
                        takeDamage(1, Schools.None);

                    if (attacks[i].VisualEffect != null)
                        Instantiate(attacks[i].VisualEffect);
                }
            }
        }
    }
    private void safeInstantiate(GameObject obj, Vector2 dir, float speed, Vector2 pos)
    {
        GameObject instance = Instantiate(obj) as GameObject;
        instance.gameObject.transform.position = pos;

        if (dir != Vector2.zero)
        {
            if (!obj.GetComponent<Rigidbody2D>())
            {
                instance.AddComponent<Rigidbody2D>();
                Rigidbody2D rb = instance.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0;
                rb.angularDrag = 0;
                rb.isKinematic = true;
            }
            instance.GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
    }
    private void circleAttack(int i, Vector2 direction)
    {
        // play animation
        effectInstantiate(this.gameObject, attacks[i].VisualEffect, direction);

        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, attacks[i].AttackRange);
        for (int j = 0; j < objs.Length; j++)
        {
            for (int k = 0; k < attacks[i].InteractWith.Length; k++)
            {
                if (objs[j].tag == attacks[i].InteractWith[k])
                {
                    if (objs[j].GetComponent<InteractAble>())
                        objs[j].GetComponent<InteractAble>().takeDamage(attacks[i].Damage, attacks[i].Type);
                }
            }
        }
    }
    private void lineAttack(int i, Vector2 direction)
    {
        effectInstantiate(this.gameObject, attacks[i].VisualEffect, direction);

        Collider2D[] objs = Physics2D.OverlapBoxAll(transform.position, new Vector2(attacks[i].AttackRange, attacks[i].AttackRange), Mathf.Atan2(direction.x, direction.y));
        for (int j = 0; j < objs.Length; j++)
        {
            for (int k = 0; k < attacks[i].InteractWith.Length; k++)
            {
                if (objs[j].tag == attacks[i].InteractWith[k])
                {
                    if (objs[j].GetComponent<InteractAble>())
                        objs[j].GetComponent<InteractAble>().takeDamage(attacks[i].Damage, attacks[i].Type);
                }
            }
        }
    }
    private void slashAttack(int i, Vector2 direction)
    {
        // fix later.
    }
    private void effectInstantiate(GameObject father, GameObject effect, Vector2 direction)
    {
        GameObject EffectObj = Instantiate(effect) as GameObject;
        EffectObj.gameObject.transform.position = father.transform.position;
        EffectObj.gameObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((direction.y), (direction.x)) * Mathf.Rad2Deg);
        EffectObj.transform.parent = father.transform;
    }
    private void damageSplash()
    {
        if (stats.splashEffect != null)
            safeInstantiate(stats.splashEffect, Vector2.zero, 0, transform.position);
    }
    private void removeIn(float time)
    {
        gameObject.AddComponent<DestroyAfter>().secondsUntilRemoval = time;
    }
}



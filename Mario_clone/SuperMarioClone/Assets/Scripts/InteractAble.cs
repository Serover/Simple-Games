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
    public float offSetRange;
}
[Serializable]
public enum Schools
{
    Head, Foot, None
}
public enum AttackTypes
{
    Slash, Line, Circle, Projectile
}

[Serializable]
public class Stats
{
    public float Health;
    public Schools Weakness;
    public GameObject splashEffect;
}
[Serializable]
public class OnDeath
{
    public float Score;
    public bool showDurability;
    public List<Sprite> Sprites = new List<Sprite>();
    public List<Items> InstantiateOnDeath = new List<Items>();
    public List<Items> InstantiateOnDamageTaken = new List<Items>();

    public bool removeOnDeath;
    public float TimeBeforeRemoval;
}
[Serializable]
public class Attack
{
    [GiveTag]
    public string[] InteractWith = new string[] { };

    public bool Locked;

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
    [NonSerialized]
    public bool isDead;

    static public float score;

    public Stats stats;
    public List<Attack> attacks = new List<Attack>();
    public OnDeath ondeath;

    private float maxHealth;
    private SpriteRenderer currentImage;

    List<float> coolDowns = new List<float>();
    List<float> baseCoolDowns = new List<float>();

    void Start()
    {
        for (int i = 0; i < attacks.Count; i++)
            coolDowns.Add(attacks[i].AttackCoolDown);

        for (int i = 0; i < attacks.Count; i++)
            baseCoolDowns.Add(attacks[i].AttackCoolDown);

        maxHealth = stats.Health;

        currentImage = GetComponent<SpriteRenderer>();

        if (ondeath.Sprites.Count > 0)
            currentImage.sprite = ondeath.Sprites[0]; // No real reason, just incase the designer forgets to change the original sprite renderer image i help him out.
    }

    private void Update()
    {
        CoolDownTick();
    }
    public void Attack(int i, Vector2 direction)
    {
        if (coolDowns[i] <= 0 && attacks[i].Locked == false)
        {

            if (attacks[i].attackMove == AttackTypes.Circle)
                circleAttack(i, direction);

            else if (attacks[i].attackMove == AttackTypes.Slash)
                slashAttack(i, direction);

            else if (attacks[i].attackMove == AttackTypes.Line)
                lineAttack(i, direction);

            else if (attacks[i].attackMove == AttackTypes.Projectile)
            {
                float projectileSpeed = 1; // reasoning, speed is set in projectile weakness, this makes it so that speed is just a direction atm.
                safeInstantiate(attacks[i].VisualEffect, direction, projectileSpeed, transform.position, attacks[i].AttackRange);
            }


            coolDowns[i] = baseCoolDowns[i];
        }
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
        if (!isDead)
        {
            // stats.Health -= dmg / 2;     

            if (stats.Weakness == Schools.None || type == Schools.None)
            {
                stats.Health -= dmg;


                if (ondeath.showDurability == true)
                    showDurability();

                damageSplash();

                for (int i = 0; i < ondeath.InstantiateOnDamageTaken.Count; i++)
                    safeInstantiate(ondeath.InstantiateOnDamageTaken[i].preFabObj, ondeath.InstantiateOnDamageTaken[i].Direction, ondeath.InstantiateOnDamageTaken[i].Speed, transform.position, ondeath.InstantiateOnDamageTaken[i].offSetRange);
            }

            else if (stats.Weakness == type)
            {
                stats.Health -= dmg;


                if (ondeath.showDurability == true)
                    showDurability();

                damageSplash();

                for (int i = 0; i < ondeath.InstantiateOnDamageTaken.Count; i++)
                    safeInstantiate(ondeath.InstantiateOnDamageTaken[i].preFabObj, ondeath.InstantiateOnDamageTaken[i].Direction, ondeath.InstantiateOnDamageTaken[i].Speed, transform.position, ondeath.InstantiateOnDamageTaken[i].offSetRange);
            }

            if (stats.Health <= 0)
            {
                isDead = true;
                stats.Health = 1; // wierd at first, but the math for switching sprites doesent work when a number becomes 0, 
                Die();
            }        
        }
    }
    private void showDurability()
    {
        float numbOfPics = ondeath.Sprites.Count;
        float breakPoint = maxHealth / numbOfPics;
        float imageNumb = (stats.Health / breakPoint);

        // show last picture on 1 health, incase there's more pictures than life. aka hardcoded interaction
        if (stats.Health <= 1)
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
        if (ondeath.Score > 0)
        {
            score += ondeath.Score;
        }

        for (int i = 0; i < ondeath.InstantiateOnDeath.Count; i++)
            safeInstantiate(ondeath.InstantiateOnDeath[i].preFabObj, ondeath.InstantiateOnDeath[i].Direction, ondeath.InstantiateOnDeath[i].Speed, transform.position, ondeath.InstantiateOnDeath[i].offSetRange);




        if (ondeath.removeOnDeath)
        {
            if (GetComponent<Rigidbody2D>())
                Destroy(GetComponent<Rigidbody2D>());

            if (GetComponent<BoxCollider2D>())
                Destroy(GetComponent<BoxCollider2D>());

            removeIn(ondeath.TimeBeforeRemoval);
        }
    }
    private void thorns(GameObject obj)
    {
        //Schools sam; // byt namn
        for (int i = 0; i < attacks.Count; i++)
        {
            if (!attacks[i].passiveDamage || attacks[i].Locked)
                continue;

            if (coolDowns[i] <= 0)
            {
                for (int j = 0; j < attacks[i].InteractWith.Length; j++)
                {
                    if (obj.tag == attacks[i].InteractWith[j])
                    {
                        if (!obj.GetComponent<InteractAble>())
                            continue;

                        Vector2 boxSize = obj.transform.GetComponent<BoxCollider2D>().size * 0.5f;
                        float boxWidth = Mathf.Abs(boxSize.x * obj.transform.localScale.x);
                        float boxHeight = Mathf.Abs(boxSize.y * obj.transform.localScale.y);

                        float XDistanceFromMiddle = (obj.transform.position.x - transform.position.x);
                        float YDistanceFromMiddle = ((obj.transform.position.y - (obj.GetComponent<BoxCollider2D>().size.y * 0.5f)) - (transform.position.y - (obj.GetComponent<BoxCollider2D>().size.y * 0.5f)));
                        boxWidth += Mathf.Abs(GetComponent<BoxCollider2D>().size.x * 0.5f * transform.localScale.x * attacks[i].AttackRange);
                        YDistanceFromMiddle -= (Mathf.Abs(GetComponent<BoxCollider2D>().size.y * transform.localScale.y * 0.5f));

                        if (attacks[i].Type == Schools.None)
                        {
                            if (Mathf.Abs(XDistanceFromMiddle) <= boxWidth * attacks[i].AttackRange)
                                obj.GetComponent<InteractAble>().takeDamage(attacks[i].Damage, attacks[i].Type);
                        }

                        else if (attacks[i].Type == Schools.Foot)
                        {
                            if (Mathf.Abs(XDistanceFromMiddle) <= boxWidth * attacks[i].AttackRange)
                                if (YDistanceFromMiddle <= boxHeight * 0.5f)
                                {
                                    obj.GetComponent<InteractAble>().takeDamage(attacks[i].Damage, attacks[i].Type);
                                }
                        }
                        else if (attacks[i].Type == Schools.Head)
                        {
                            if (Mathf.Abs(XDistanceFromMiddle) <= boxWidth * attacks[i].AttackRange)
                                if (YDistanceFromMiddle > 0)
                                    obj.GetComponent<InteractAble>().takeDamage(attacks[i].Damage, attacks[i].Type);
                        }

                        coolDowns[i] = baseCoolDowns[i];



                        if (attacks[i].VisualEffect != null)
                            Instantiate(attacks[i].VisualEffect);
                    }
                }
            }
        }
    }
    private void safeInstantiate(GameObject obj, Vector2 dir, float speed, Vector2 pos, float range)
    {
        GameObject instance = Instantiate(obj) as GameObject;
        instance.gameObject.transform.position = pos + (dir * range);

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
            safeInstantiate(stats.splashEffect, Vector2.zero, 0, transform.position, 0);
    }
    private void removeIn(float time)
    {
        Destroy(gameObject, time);



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



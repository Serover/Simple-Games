using UnityEngine;
using System.Collections;

public class MovementAnimatorController : MonoBehaviour
{
    private const string animWalk = "walk";
    private const string animDead = "dead";
    private const string animJump = "jump";

    float X;
    float Y;
    Animator anim;
    private new Rigidbody2D rigidbody2D;
    private bool faceright; //face side of sprite activated
    private bool isDead = false;

    void Start()
    {
        faceright = true;//Default for all the Sprites is to face right on start.
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool(animWalk, false);//Walking animation is deactivated
        anim.SetBool(animDead, false);//Dying animation is deactivated
        anim.SetBool(animJump, false);//Jumping animation is deactivated

        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        if (!GetComponent<Rigidbody2D>())
            Die();

        anim.SetBool(animWalk, false); // Incase the characther got "stunned" or something the like we dont want the animation to keep moving. AKA start idle movement.

        if (!isDead && GetComponent<Rigidbody2D>())
            setAnimation();
    }
    public void Die()
    {
        anim.SetBool(animDead, true);
        if (GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        isDead = true;
    }
    void Flip()
    {
        if (faceright == false && X > 0 || faceright == true && X < 0)
        {
            faceright = !faceright;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    private void setAnimation()
    {
        X = rigidbody2D.velocity.x;
        Y = rigidbody2D.velocity.y;

        if (Y != 0)
        {
            anim.SetBool(animJump, true);
            Flip();
        }


        else if (X > 0)
        {//Go right
            anim.SetBool(animJump, false);

            anim.SetBool(animWalk, true);//Walking animation is activated

            Flip();
        }
        else if ((X < 0))
        {//Go left
            anim.SetBool(animJump, false);

            anim.SetBool(animWalk, true);

            Flip();
        }
        /* else if (Y > 0)
         {// Go Up
             anim.SetBool(animWalk, true);
             if (faceright == false) // for no real reason I want The characther to face Right when walking up.
                 Flip();
         }
         else if (Y < 0)
         {// Go Down
             anim.SetBool(animWalk, true); // for no real reason I want The characther to face Left when walking down.
             if (faceright == true)
                 Flip();
         } */
        if (X == 0 && Y == 0)
            anim.SetBool(animWalk, false);
    }
}

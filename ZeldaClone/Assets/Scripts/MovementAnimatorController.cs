using UnityEngine;
using System.Collections;

public class MovementAnimatorController : MonoBehaviour
{
    private const string animWalk = "walk";
    private const string animDead = "dead";
    private const string animJump = "jump";

    Animator anim;
    private new Rigidbody2D rigidbody2D;
    private bool faceright; //face side of sprite activated
    private bool isdead = false;
 
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
        anim.SetBool(animWalk, false); // Incase the characther got "stunned" or something the like we dont want the animation to keep moving. AKA start idle movement.

        if (CameraScript.Transitioning == false || isdead)
            setAnimation();
    }
    public void Die()
    {
        anim.SetBool(animDead, true);
        rigidbody2D.velocity = Vector2.zero;
        isdead = true;
    }
    void Flip()
    {
        faceright = !faceright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void setAnimation()
    {
        float X = rigidbody2D.velocity.x;
        float Y = rigidbody2D.velocity.y;

        if (X > 0)
        {//Go right
            anim.SetBool(animWalk, true);//Walking animation is activated
            if (faceright == false)
                Flip();
        }
        else if ((X < 0))
        {//Go left
            anim.SetBool(animWalk, true);
            if (faceright == true)
                Flip();
        }
        else if (Y > 0)
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
        }
        if (X == 0 && Y == 0)
            anim.SetBool(animWalk, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //keyboard input
    private void FixedUpdate(){
        if(Input.GetKey("d") || Input.GetKey("right")){
            rigidbody2D.velocity = new Vector2(2, rigidbody2D.velocity.y);
            //spriteRenderer.flipX = false;
        }else if(Input.GetKey("a") || Input.GetKey("left")){
            rigidbody2D.velocity = new Vector2(-2, rigidbody2D.velocity.y);
            //spriteRenderer.flipX = true;
        }else{
            //eliminate sliding
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
        
        if(Input.GetKey("space")){
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 2);
        }
        if(Input.GetKey("q")){
            animator.Play("PlayerPunch");
            StartCoroutine(DelayedAnimation());
        }else if(Input.GetKey("w")){
            animator.Play("PlayerKick");
            StartCoroutine(DelayedAnimation());
        }else if(Input.GetKey("e")){
            animator.Play("PlayerBlock");
            StartCoroutine(DelayedAnimation());
        }

    }

    IEnumerator DelayedAnimation ()
     {
         yield return new WaitForSeconds(0.5f);
         animator.Play("PlayerIdle");
     }
}

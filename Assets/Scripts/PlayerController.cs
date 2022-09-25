using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    bool isAttacking = false;
    [SerializeField]
    GameObject PunchHitbox;
    [SerializeField]
    GameObject KickHitbox;
    [SerializeField]
    GameObject BlockHitbox;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PunchHitbox.SetActive(false);
        KickHitbox.SetActive(false);
        BlockHitbox.SetActive(false);
    }

    void Update(){
        if(Input.GetButtonDown("Punch") && !isAttacking){
            isAttacking = true;
            animator.Play("PlayerPunch");
            isAttacking = false;
            StartCoroutine(GotPunched());

        }else if(Input.GetButtonDown("Kick") && !isAttacking){
            isAttacking = true;
            animator.Play("PlayerKick");
            isAttacking = false;
            StartCoroutine(GotKicked());

        }else if(Input.GetButtonDown("Block")){
            isAttacking = true;
            animator.Play("PlayerBlock");
            isAttacking = false;
            StartCoroutine(Blocked());
        }
    }

    //keyboard input
    private void FixedUpdate(){
        if(Input.GetKey("d") || Input.GetKey("right")){
            //check if player grounded functionality?
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

    }

    IEnumerator GotPunched ()
    {
        PunchHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        PunchHitbox.SetActive(false);
        if(!isAttacking){
            animator.Play("PlayerIdle");
        }
    }

    IEnumerator GotKicked ()
    {
        KickHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        KickHitbox.SetActive(false);
        if(!isAttacking){
            animator.Play("PlayerIdle");
        }
    }
    IEnumerator Blocked ()
    {
        BlockHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        BlockHitbox.SetActive(false);
        if(!isAttacking){
            animator.Play("PlayerIdle");
        }
    }
}

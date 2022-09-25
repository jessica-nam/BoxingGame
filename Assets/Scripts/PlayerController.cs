using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    Animator animator;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    public bool isAttacking = false;
    public bool isBlocking = false;

    bool punchDamage = false;
    bool kickDamage = false;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    [SerializeField]
    GameObject PunchHitbox;
    [SerializeField]
    GameObject KickHitbox;
    [SerializeField]
    GameObject BlockHitbox;

    private void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PunchHitbox.SetActive(false);
        KickHitbox.SetActive(false);
        BlockHitbox.SetActive(false);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
            isBlocking = true;
            animator.Play("PlayerBlock");
            isBlocking = false;
            StartCoroutine(Blocked());
        }

        if(punchDamage){
            //transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
            animator.Play("PlayerOuch");
            GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(10);
            punchDamage = false;
        }   
        if(kickDamage){
            animator.Play("PlayerOuch");
            GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(20);
            kickDamage = false;
        }  
        if (currentHealth == 0){
            
            StartCoroutine(AnimationDone());
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

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.name == "PunchHitbox1"){
            punchDamage = true;
            Debug.Log("Ouch!");
            
        }else if(collision.gameObject.name == "KickHitbox1"){
            kickDamage = true;
            Debug.Log("Ouch!");
        }
        
    }

    void Damage(int damage){
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
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
        if(!isBlocking){
            animator.Play("PlayerIdle");
        }
    }

    IEnumerator AnimationDone()
    {
        animator.Play("PlayerDead");
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Animator>().enabled = false;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public AudioSource audioPlayer;
    public AudioSource audioPlayer1;

    Animator animator;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isDead = false;

    bool isGrounded = true;
    [SerializeField] Transform groundCheck;

    bool punchDamage = false;
    bool kickDamage = false;
    public bool noDamageToAI = false;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    [SerializeField] GameObject PunchHitbox;
    [SerializeField] GameObject KickHitbox;
    [SerializeField] GameObject BlockHitbox;

    [SerializeField] GameObject PunchHitbox1;
    [SerializeField] GameObject KickHitbox1;
    [SerializeField] GameObject BlockHitbox1;

    [SerializeField] private Image KO;

    private void Awake()
    {
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

    void Update()
    {
        if (Countdown.instance.CountDownDone && EnergyBar.instance.currentEnergy >= 20)
        {
            if (Input.GetButtonDown("Punch") && !isAttacking)
            {
                isAttacking = true;
                animator.Play("PlayerPunch");
                EnergyBar.instance.UseEnergy(20);
                StartCoroutine(GotPunched());

            }
            else if (Input.GetButtonDown("Kick") && !isAttacking)
            {
                isAttacking = true;
                animator.Play("PlayerKick");
                EnergyBar.instance.UseEnergy(30);
                StartCoroutine(GotKicked());

            }
            else if (Input.GetButtonDown("Block"))
            {
                isBlocking = true;
                animator.Play("PlayerBlock");
                isBlocking = false;
                StartCoroutine(Blocked());
            }else{
                Debug.Log("Cannot attack - wait for recharge");
            }
        }

        if (punchDamage)
        {
            //transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
            animator.Play("PlayerOuch");
            EnergyBarEnemy.instance.UseEnergyEnemy(20);
            GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(5);
            punchDamage = false;
        }
        if (kickDamage)
        {
            animator.Play("PlayerOuch");
            EnergyBarEnemy.instance.UseEnergyEnemy(30);
            GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(10);
            kickDamage = false;
        }
        if (currentHealth <= 0)
        {
            isDead = true;
            KO.enabled = true;
            ShakeBehavior.instance.TriggerShake();
            StartCoroutine(AnimationDone());
            StartCoroutine(GameOver());
        }else{
            KO.enabled = false;
        }
    }

    //keyboard input
    private void FixedUpdate()
    {
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
            Debug.Log(isGrounded);
        }
        else
        {
            isGrounded = false;
        }
        if (Countdown.instance.CountDownDone && !EnemyAI.instance.isAttackingAI)
        {

            if ((Input.GetKey("d") || Input.GetKey("right")) && isGrounded)
            {
                //check if player grounded functionality?
                rigidbody2D.velocity = new Vector2(2, rigidbody2D.velocity.y);
                //spriteRenderer.flipX = false;
            }
            else if ((Input.GetKey("a") || Input.GetKey("left")) && isGrounded)
            {
                rigidbody2D.velocity = new Vector2(-2, rigidbody2D.velocity.y);
                //spriteRenderer.flipX = true;
            }
            else
            {
                //eliminate sliding
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }

        }
        // fixes wanky sliding jumps
        if (isGrounded)
        {
            if (Input.GetKey("space"))
            {
                Debug.Log("Jump");
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 3);
                isGrounded = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("got hit: " + collision.gameObject.name);

        if (collision.gameObject.name == "PunchHitbox1")
        {
            Debug.Log("AI punched me");
            punchDamage = true;
            audioPlayer.Play();
            //Debug.Log("Ouch!");

        }
        else if (collision.gameObject.name == "KickHitbox1")
        {
            kickDamage = true;
            audioPlayer1.Play();
            Debug.Log("Ai kicked me!");
        }
        else if (collision.gameObject.name == "BlockHitbox1")
        {
            noDamageToAI = true;
            Debug.Log("Ai blocked me!");
        }

    }

    void Damage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator GotPunched()
    {
        PunchHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        PunchHitbox.SetActive(false);
        isAttacking = false;
        if (!isAttacking)
        {
            animator.Play("PlayerIdle");
        }
    }

    IEnumerator GotKicked()
    {
        KickHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        KickHitbox.SetActive(false);
        isAttacking = false;
        if (!isAttacking)
        {
            animator.Play("PlayerIdle");
        }
    }
    IEnumerator Blocked()
    {
        BlockHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        BlockHitbox.SetActive(false);
        EnemyAI.instance.noDamageToPlayer = false;
        if (!isBlocking)
        {
            animator.Play("PlayerIdle");
        }
    }

    IEnumerator AnimationDone()
    {
        animator.Play("PlayerDead");
        yield return new WaitForSeconds(0.7f);
        gameObject.GetComponent<Animator>().enabled = false;

    }

    IEnumerator GameOver(){
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("GameOver");
    }
}

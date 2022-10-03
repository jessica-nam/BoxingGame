using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EnemyAI : MonoBehaviour
{
    public static EnemyAI instance;

    public AudioSource audioPlayer;
    public AudioSource audioPlayer1;

    Animator animator;

    bool punchDamage = false;
    bool kickDamage = false;
    bool attack = true;
    public bool noDamageToPlayer = false;
    bool isDeadAI = false;
    public bool isAttackingAI = false;

    private int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 0.7f;
    bool isStopped;

    public HealthBar healthBar;

    [SerializeField] GameObject PunchHitbox1;
    [SerializeField] GameObject KickHitbox1;
    [SerializeField] GameObject BlockHitbox1;

    [SerializeField] private Image KO1;

    public int roll;

    private void Awake()
    {
        instance = this;
    }


    //float shakeAmount = 0.2f;
    //Vector2 startPos;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        PunchHitbox1.SetActive(false);
        KickHitbox1.SetActive(false);
        BlockHitbox1.SetActive(false);
        yield return new WaitForSeconds(4);
        // if(EnergyBarEnemy.instance.currentEnergy >= 20){
        while (attack && !isDeadAI)
        {
            int rand = Random.Range(1, 3);
            yield return new WaitForSeconds(rand);
            int roll = Random.Range(0, 5);
            animator.SetInteger("AttackIndex", roll);
            if (EnergyBarEnemy.instance.currentEnergyEnemy >= 20)
            {
                if (roll == 0 || roll == 3)
                {

                    StartCoroutine(GetPunched());


                    //EnergyBarEnemy.instance.UseEnergy(20);
                    //}

                }
                else if (roll == 1 || roll == 4)
                {
                    //if(EnergyBarEnemy.instance.currentEnergy >= 30){
                    StartCoroutine(GetKicked());
                    //EnergyBarEnemy.instance.UseEnergy(30);
                    //}
                }
            }
            else if (roll == 2)
            {
                StartCoroutine(Blocked());
            }

            if (PlayerController.instance.isDead)
            {
                attack = false;
                isDeadAI = true;
            }
            //}
            // animator.SetTrigger("Attack");  
        }

    }

    IEnumerator GetPunched()
    {
        isAttackingAI = true;
        PunchHitbox1.SetActive(true);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        PunchHitbox1.SetActive(false);
        isAttackingAI = false;

    }
    IEnumerator GetKicked()
    {
        isAttackingAI = true;
        KickHitbox1.SetActive(true);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        KickHitbox1.SetActive(false);
        isAttackingAI = false;
    }
    IEnumerator Blocked()
    {
        BlockHitbox1.SetActive(true);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        BlockHitbox1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Countdown.instance.CountDownDone)
        {
            AiMoveAndStopAtTarget();
        }

        if (punchDamage)
        {
            //transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
            animator.Play("EnemyPunchOuch");
            audioPlayer.Play();
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(7);
            punchDamage = false;
        }
        if (kickDamage)
        {
            animator.Play("EnemyPunchOuch");
            audioPlayer1.Play();
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(14);
            kickDamage = false;
        }
        if (PlayerController.instance.noDamageToAI)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, GetComponent<Rigidbody2D>().velocity.y);
            PlayerController.instance.noDamageToAI = false;
        }
        if (currentHealth <= 0)
        {
            isDeadAI = true;
            KO1.enabled = true;
            ShakeBehavior.instance.TriggerShake();
            StartCoroutine(AnimationDone());
            StartCoroutine(GameOver());
        }
        else
        {
            KO1.enabled = false;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PunchHitbox")
        {
            punchDamage = true;
            Debug.Log("Ouch!");

        }
        else if (collision.gameObject.name == "KickHitbox")
        {
            kickDamage = true;
            Debug.Log("Ouch!");
        }
        else if (collision.gameObject.name == "BlockHitbox")
        {
            noDamageToPlayer = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, GetComponent<Rigidbody2D>().velocity.y);
        }

    }

    void Damage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator AnimationDone()
    {
        animator.Play("EnemyDead");
        yield return new WaitForSeconds(0.7f);
        gameObject.GetComponent<Animator>().enabled = false;
        StopEnemy();

    }

    void AiMoveToTarget()
    {
        isStopped = false;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    private void StopEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0 * Time.deltaTime);
        isStopped = true;
    }

    private void AiMoveAndStopAtTarget()
    {
        float dist = Vector2.Distance(transform.position, target.transform.position);
        //Debug.Log(dist);
        if (currentHealth > 0)
        {
            if (dist < 0.7f)
            {
                StopEnemy();
            }
            else
            {
                AiMoveToTarget();
            }
        }
        if (currentHealth == 0)
        {
            StopEnemy();
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("GameOver");
    }



}

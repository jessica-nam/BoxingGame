using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    Animator animator;

    bool punchDamage = false;
    bool kickDamage = false;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    //float shakeAmount = 0.2f;
    //Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        //startPos = transform.position;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(punchDamage){
            //transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
            animator.Play("EnemyPunchOuch");
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(10);
            punchDamage = false;
        }   
        if(kickDamage){
            animator.Play("EnemyPunchOuch");
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, GetComponent<Rigidbody2D>().velocity.y);
            Damage(20);
            kickDamage = false;
        }  
        if (currentHealth == 0){
            
            StartCoroutine(AnimationDone());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.name == "PunchHitbox"){
            punchDamage = true;
            Debug.Log("Ouch!");
        }else if(collision.gameObject.name == "KickHitbox"){
            kickDamage = true;
            Debug.Log("Ouch!");
        }
        
    }

    void Damage(int damage){
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator AnimationDone()
    {
        animator.Play("EnemyDead");
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Animator>().enabled = false;
        
    }


}

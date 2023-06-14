using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyTroll : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;
    [SerializeField]
    protected GameObject attackArea;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float attackDist;
    [SerializeField]
    protected float backDist;
    [SerializeField]
    protected float knockTime;
    [SerializeField]
    protected Transform startPos;

    protected float immuneTime = 0.5f;
    protected float lastImmune;
    protected Transform target;
    public Vector2 moveDirectory; //protected
    protected bool isAttacking;
    protected bool goBack;
    protected Vector3 pushDirection;
    protected float pushForce;

    protected Transform otherEnemy;

    protected float distBeetwenEnemies;

    public float distanceEnemies;
    public float readyDist;
    protected bool defMode;
    protected bool oddalanie;

    public GameObject legs;
    private Animator legAnimator;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        legAnimator = legs.GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (health < 0)
        {
            Destroy(rb);
            Death();

        }

        if (isAttacking == false)
            attackArea.GetComponent<BoxCollider2D>().enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        if (rb != null) //DODAJ STREFFE W KTOREJ BO ZBLIZENIU PODNOSI TARCZE I IDZIE WOLNIEJ DODAJ KULKE OKRAZAJACA GRACZA ZA KTORA BEDZIE PODAZAL I DODAJ FUNKCJE PRZEZ KTORA NIE BEDA WCHODZIC W SIEBIE EWENTUALNIE ISC SLALOMEM SZUKAJ CHANGE RIGIDBODY.VELOCITY DIRECTIOn

        {
            //WROG W ZASIEGU
            if (target != null)
            {
                float dist = Vector3.Distance(target.position, transform.position);
                if (goBack == false && dist > readyDist && isAttacking == false)// && distBeetwenEnemies > distanceEnemies) //TEST TEGO ZEBY NIE BLOKOWALI SIE O SIEBIE
                {
                    moveDirectory = (target.position - transform.position).normalized;
                    rb.velocity = moveDirectory * moveSpeed;
                }


                if (dist < readyDist && dist > attackDist && isAttacking == false)
                {
                    moveDirectory = (target.position - transform.position).normalized;
                    rb.velocity = moveDirectory * moveSpeed / 2;
                    if (gameObject.name == "goblin")
                        animator.Play("goblin_walk_def");
                }
                else if (dist < attackDist)
                    Attack();
                else
                    UnAttack();


                //WYCOFANIE PO ATAKU
                if (goBack == true)
                {
                    moveDirectory = (transform.position - target.position).normalized;
                    rb.velocity = moveDirectory * moveSpeed / 2;
                    if (dist > backDist)
                    {
                        moveDirectory = Vector3.zero;
                        goBack = false;
                    }
                }



                //if(otherEnemy != null && goBack == false)
                //{
                //    distBeetwenEnemies = Vector3.Distance(otherEnemy.transform.position, transform.position);
                //    if(distBeetwenEnemies < distanceEnemies)
                //    {
                //        oddalanie = true;
                //       Debug.Log("Oddalam sie");
                //       int rand = Random.Range(1, 3);
                //        Debug.Log(rand);
                //        if(rand == 0)
                //            rb.velocity = (new Vector2(moveDirectory.x + 0.33f, moveDirectory.y + 0.33f) * moveSpeed);
                //        else if(rand == 1)
                //            rb.velocity = (new Vector2(moveDirectory.x - 0.33f, moveDirectory.y - 0.33f) * moveSpeed);
                //    }
                //    else
                //    {
                //        oddalanie = false;
                //        Debug.Log("nie oddalam sie"); //IF movedirectory.x - otherEnemydirectory.x > 0 to cos tam

                //   }
                //}

            }
            else if (target == null)
            {
                if (Vector3.Distance(startPos.position, transform.position) > 0.1)
                {
                    moveDirectory = (startPos.position - transform.position).normalized;
                    rb.velocity = moveDirectory * moveSpeed;
                }
                else
                {
                    moveDirectory = Vector2.zero;
                    rb.velocity = Vector2.zero;
                    //animator.Play("goblin_idle");
                }

            }


            if (goBack == false)// && distBeetwenEnemies < distanceEnemies) // TEST PAMIETAJ O TYM TODO:
            {
                if (moveDirectory.x > 0)  // ZMIANA KIERUNKU SPRITE
                    transform.localScale = new Vector3(-0.6f, 0.6f, 1);
                else if (moveDirectory.x < 0)
                    transform.localScale = new Vector3(0.6f, 0.6f, 1);
            }
            if (rb.velocity == Vector2.zero)  //ZMIANA ANIMACJI RUCHU NA BIEG
                legAnimator.SetBool("isMoving", false);
            else
                legAnimator.SetBool("isMoving", true);

            if (pushDirection != Vector3.zero)  //KNOCKBACK
            {
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                StartCoroutine(Knockback());
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            target = collision.transform;

        if (collision.tag == "Fighter") //TEST TEGO ZEBY NIE BLOKOWALI SIE O SIEBIE
            otherEnemy = collision.transform;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            target = null;

        if (collision.tag == "Fighter") //TEST TEGO ZEBY NIE BLOKOWALI SIE O SIEBIE
            otherEnemy = null;

    }
    private IEnumerator Knockback()
    {
        yield return new WaitForSeconds(knockTime);
        pushDirection = Vector2.zero;
        pushForce = 0;
    }

    protected virtual void switchToIdle()
    {
    }

    protected virtual void enableAttack()
    {

        attackArea.GetComponent<BoxCollider2D>().enabled = true;
    }

    protected virtual void disableAttack()
    {
        attackArea.GetComponent<BoxCollider2D>().enabled = false;
        goBack = true;
    }

    protected virtual void ReciveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            pushDirection = (transform.position - target.transform.position).normalized;
            pushForce = dmg.pushForce;
            GameManager.instance.ShowText(transform.position, dmg.damageAmount.ToString());
            health -= dmg.damageAmount;
        }
    }

    protected virtual void Attack()
    {
        animator.Play("troll_attack");
        isAttacking = true;
    }

    protected virtual void UnAttack()
    {
        animator.Play("troll_idle");
        isAttacking = false;
    }




    protected virtual void Death()
    {
        target = null;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }


}

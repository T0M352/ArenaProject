using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    bool start = true;

    protected override void Start()
    {
        base.Start();
        start = true;
    }

    protected override void FixedUpdate()
    {


        if (rb != null && isDead == false) //DODAJ STREFFE W KTOREJ BO ZBLIZENIU PODNOSI TARCZE I IDZIE WOLNIEJ DODAJ KULKE OKRAZAJACA GRACZA ZA KTORA BEDZIE PODAZAL I DODAJ FUNKCJE PRZEZ KTORA NIE BEDA WCHODZIC W SIEBIE EWENTUALNIE ISC SLALOMEM SZUKAJ CHANGE RIGIDBODY.VELOCITY DIRECTIO
        {
            if (start == true)
                animator.Play("skeleton_start");
            //WROG W ZASIEGU
            if (target != null && start == false)
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
                        defAnimation();
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
        if (rb != null)
        {
            if (goBack == false)// && distBeetwenEnemies < distanceEnemies) // TEST PAMIETAJ O TYM TODO:
            {
                if (moveDirectory.x > 0)  // ZMIANA KIERUNKU SPRITE
                    transform.localScale = new Vector3(0.3f, 0.3f, 1);
                else if (moveDirectory.x < 0)
                    transform.localScale = new Vector3(-0.3f, 0.3f, 1);
            }
        }
    }

    protected override void switchToIdle()
    {
        animator.Play("skeleton_idle");
    }

    protected override void Attack()
    {
        base.Attack();
        animator.Play("skeleton_attack");
    }

    protected override void UnAttack()
    {
        base.UnAttack();
        animator.Play("skeleton_idle");

    }

    protected override void Death()
    {
        base.Death();
        animator.Play("skeleton_death");
    }

    protected override void defAnimation()
    {
        animator.Play("skeleton_def");
    }

    private void startFalse()
    {
        start = false;
    }
}

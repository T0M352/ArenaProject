using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitPoint = 10; //ZLACZ TA KLASE Z KLASA PLAYERONE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public int maxHitPoint = 10;
    public float pushRecoverySpeed = 0.2f;

    protected float immuneTime = 0.5f; //TODO: POBAW SIE Z TYM
    protected float lastImmune;

    protected Vector3 pushDirection;
    //public Rigidbody2D rigidbody2D;
    protected Vector2 pushDir;
    protected float PushForce;



    protected virtual void ReciveDamage(Damage dmg)
    {
        if(Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized;
            PushForce = dmg.pushForce;
            if(gameObject.name != "blok")
                GameManager.instance.ShowText(transform.position, dmg.damageAmount.ToString());
            else
                GameManager.instance.ShowText(transform.position, "BLOK");

            if (hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
        }
    }


    protected virtual void Death()
    {

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateClosing : MonoBehaviour
{
    Animator animator;
    private bool closed = false;
    BoxCollider2D boxCollider;
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(GameManager.dungeonMode == 3 && closed == false)
        //{
        //    animator.Play("gate_close");
        //    closed = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && closed == false)
        {
            animator.Play("gate_close");
            boxCollider.enabled = true;
            closed = true;
        }
    }

    private void closedNow()
    {
        animator.Play("gate_closed");
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction;
    GameObject mole;
    MoleAttack moleAttack;
    [Header ("Hitbox properties")]
    public int health = 5;
    public int riccochet = 10;
    public int hitCount { get; private set; } = 0;
    public HitState state { get; private set; }

    public delegate void DeathEvent();
    public static event DeathEvent OnDeath;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector3.up;
        mole = transform.parent.gameObject;
        state = HitState.Alive;
        moleAttack = mole.GetComponent<MoleAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Make player shoot up in air
            GameObject player = collision.gameObject;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            Vector2 bounceDirection = direction * riccochet;
            playerRb.linearVelocity = bounceDirection;

            hitCount++;
            CheckHits();
        }
    }

    private void CheckHits()
    {
        if (hitCount >= health && mole.CompareTag("Boss")) {
            TriggerEvent();
        }
        else if(hitCount >= health && moleAttack != null)
        {
            moleAttack.Death();
        }
    }

    public void TriggerEvent()
    {  
         OnDeath?.Invoke();
         state = HitState.Dead;
    }

    public enum HitState
    {
        Alive,
        Dead
    }
}

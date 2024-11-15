using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction;
    GameObject mole;
    public int health = 5;
    public int ricochet = 10;
    public int hitCount { get; private set; } = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector3.up;
        mole = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Make player shoot up in air
            GameObject player = collision.gameObject;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            Vector2 bounceDirection = direction * ricochet;
            playerRb.linearVelocity = bounceDirection;

            hitCount++;
            CheckHits();
        }
    }

    private void CheckHits()
    {
        if (hitCount >= health) {
            Destroy(mole);
        }
    }
}

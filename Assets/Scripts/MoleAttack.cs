using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleAttack : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction;
    Transform player;
    public float speed = 1.2f;
    [SerializeField] MoleState state;
    void Start()
    {
        state = MoleState.Idle;
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 500; //mass set high so player can't push mole
    }

    void Update()
    {
        if (state == MoleState.Hostile)
            Attack(player);
    }

    private void OnTriggerEnter2D(Collider2D collision) //use a trigger collider with a large radius
    {
        if (collision.CompareTag("Player"))
        {
            state = MoleState.Hostile;
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            state = MoleState.Idle;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //regular collider
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //do damage logic to player
        }
    }

    private void Attack(Transform player)
    {
        direction = (player.position - transform.position).normalized;
        rb.MovePosition((direction * speed) * Time.deltaTime);
    }
}

public enum MoleState
{
    Idle,
    Hostile
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MoleAttack : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction;
    Transform player;
    Animator anim;
    public float speed = 1.2f;
    [SerializeField] MoleState state;
    void Start()
    {
        state = MoleState.Idle;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            StartCoroutine(AwakeTimer());
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            state = MoleState.Idle;
            anim.SetBool("hostile", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //regular collider
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //do damage logic to player
        }
    }

    IEnumerator AwakeTimer()
    {
        float cooldown = 2f;
        anim.SetTrigger("arise");
        while (cooldown > 0)
        {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        anim.SetBool("hostile", true);
    }

    private void Attack(Transform player)
    {
        direction = (player.position - transform.position).normalized;
        transform.Translate((direction * speed) * Time.deltaTime,Space.World);
    }
}

public enum MoleState
{
    Idle,
    Hostile
}

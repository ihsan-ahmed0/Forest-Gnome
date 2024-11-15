using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;
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
        rb.mass = 500; // mass set high so player can't push mole
    }

    void Update()
    {
        if (state == MoleState.Hostile)
            Attack(player);
    }

    // Flip the direction of the mole whenever it switches directions.
    private void Flip(float relativeToPlayer)
    {
        bool facingLeft = transform.localScale.x > 0;
        bool movingLeft = relativeToPlayer < 0;
        if (facingLeft != movingLeft)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // use a trigger collider with a large radius
    {
        if (collision.CompareTag("Player"))
        {
            state = MoleState.Hostile;
            StartCoroutine(AwakeTimer());
            Debug.Log("chase");
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

    private void OnCollisionStay2D(Collision2D collision) // regular collider
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // do damage logic to player
            PlayerController.player.Damage(15);
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
        
    }

    private void Attack(Transform player)
    {
        anim.SetBool("hostile", true);
        direction = (player.position - transform.position).normalized;
        transform.Translate((direction * speed) * Time.deltaTime,Space.World);
        Flip(direction.x);
    }
}

public enum MoleState
{
    Idle,
    Hostile
}

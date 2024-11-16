using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class MoleBoss : MonoBehaviour
{
    Vector3 direction;
    Transform player;
    bool isGoingRight;
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] Hitbox hitbox;
    [SerializeField] GameObject star;
    public float speed = 1.2f;
    public float dodgeX = -50f; //must be negative
    public float dodgeCooldown = 5f;
    public MoleState state;
    [SerializeField] bool canDodge;
    float currentCooldown;

    private float collisionCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Hitbox.OnDeath += BossDeath; //Adds listener for death event

        transform.Rotate(0, 180, 0); //faces sprite to the right
        isGoingRight = true;
        rb.mass = 500;
        canDodge = true;
        currentCooldown = dodgeCooldown;
        state = MoleState.PhaseOne;
        collisionCooldown = 0;
    }

    void Update()
    {
        Movement();
    }
    void FixedUpdate()
    {
        if (collisionCooldown > 0)
        {
            collisionCooldown -= Time.deltaTime;
        }
    }

    private void Movement()
    {
        Vector3 moveDirect = (isGoingRight) ? transform.right : -transform.right;
        moveDirect *= Time.deltaTime * speed;
        if (state != MoleState.Burrow && hitbox.state != Hitbox.HitState.Dead)
            transform.Translate(moveDirect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhaseCheck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && state != MoleState.Burrow)
        {
            // do damage logic to player
            PlayerController.player.Damage(15);
        }

        if (collision.gameObject.CompareTag("Finish") && collisionCooldown <= 0) {
            collisionCooldown = 3;
            isGoingRight = !isGoingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    void PhaseCheck()
    {
        if (hitbox.hitCount >= (hitbox.health / 2) && state == MoleState.PhaseOne)
        {
            state = MoleState.Burrow;
            anim.SetBool("isRetreat",true);
            Burrow();
        }
        else if (state == MoleState.Burrow)
        {
            hitbox.gameObject.SetActive(true);
            GetComponent<CircleCollider2D>().enabled = true;
            anim.Play("Arise");
            anim.SetBool("isRetreat",false);
            state = MoleState.PhaseTwo;
            //Mole is more aggresive and harder to hit
            dodgeCooldown /= 2;
            speed += 0.5f;
        }
    }

    void BossDeath()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        hitbox.gameObject.SetActive(false); 
        anim.Play("FaintBoss");
        StartCoroutine(DeathTimer());
    }
    void Burrow()
    {
        //Mole will be invincible, still, and have durability increased
        hitbox.gameObject.SetActive(false);
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(BurrowTimer());
    }

    IEnumerator DeathTimer()
    {
        float cooldown = 3f;
        while (cooldown > 0)
        {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        Instantiate(star, new Vector3(rb.position.x, rb.position.y + 5, 0), Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator BurrowTimer()
    {
        float cooldown = 5f;
        hitbox.health += 3;   //increase amount of hits that can be taken

        while (cooldown > 0)
        {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        PhaseCheck();
    }

    public enum MoleState
    {
        PhaseOne,
        Burrow,
        PhaseTwo
    }
}
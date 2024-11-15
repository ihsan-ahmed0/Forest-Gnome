using System.Collections;
using UnityEngine;

public class MoleBoss : MonoBehaviour
{
    Vector3 direction;
    Transform player;
    bool isGoingRight;
    Rigidbody2D rb;
    [SerializeField] Hitbox hitbox;
    public float speed = 1.2f;
    public float dodgeX = -50f; //must be negative
    public float dodgeCooldown = 5f;
    public MoleState state;
    [SerializeField] bool canDodge;
    float currentCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGoingRight = true;
        rb.mass = 500;
        canDodge = true;
        currentCooldown = dodgeCooldown;
        state = MoleState.PhaseOne;
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 moveDirect = (isGoingRight) ? transform.right : -transform.right;
        moveDirect *= Time.deltaTime * speed;
        if (state != MoleState.Burrow)
            transform.Translate(moveDirect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canDodge)
        {
            player = collision.transform;
            Dodge(player);
            StartCoroutine(DodgeTimer());
        }
        PhaseCheck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //do damage logic to player
        }

       if(collision.gameObject.CompareTag("Finish"))
            isGoingRight = !isGoingRight;
    }

    private void Dodge(Transform player)
    {
        //Will dodge the left if the player is on the right and vice versa
        float dodgeDirection = (player.position.x <= transform.position.x) ? -dodgeX : dodgeX;
        direction = new Vector3(transform.position.x + dodgeDirection, transform.position.y, transform.position.z);

        transform.Translate((direction - transform.position) * speed * Time.deltaTime, Space.World);
        canDodge = false;
        currentCooldown = dodgeCooldown;
    }

    void PhaseCheck()
    {
        if (hitbox.hitCount >= (hitbox.health / 2) && state == MoleState.PhaseOne)
        {
            state = MoleState.Burrow;
            Debug.Log("Burrow");
            Burrow();
        }
        else if (state == MoleState.Burrow)
        {
            hitbox.gameObject.SetActive(true);
            GetComponent<CircleCollider2D>().enabled = true;
            state = MoleState.PhaseTwo;
            //Mole is more aggresive and harder to hit
            dodgeCooldown /= 2;
            speed += 0.5f;
        }
    }

    void Burrow()
    {
        //Mole will be invincible, still, and have durability increased
        hitbox.gameObject.SetActive(false);
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(BurrowTimer());
    }
    IEnumerator DodgeTimer()
    {
        while (currentCooldown > 0)
        {
            currentCooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        canDodge = true;
    }

    IEnumerator BurrowTimer()
    {
        float cooldown = 5f;
        hitbox.health *= 2;   //increase amount of hits that can be taken

        while (cooldown > 0)
        {
            rb.linearVelocity *= 0;
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
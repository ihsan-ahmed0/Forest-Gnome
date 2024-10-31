using UnityEngine;
using UnityEngine.UIElements;

public class MoleBoss : MonoBehaviour
{
    Vector3 direction;
    Transform player;
    bool isGoingRight;
    Rigidbody2D rb;
    public float speed = 1.2f;
    public float dodgeX = -50; //must be negative
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGoingRight = true; //used for direction change
        rb.mass = 500; //make boss unpushable
    }

    void Update()
    {
        Vector3 directionTranslation = (isGoingRight) ? transform.right : -transform.right;
        directionTranslation *= Time.deltaTime * speed;

        transform.Translate(directionTranslation);
    }

    private void OnTriggerEnter2D(Collider2D collision) //use a trigger collider with a large radius
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            Dodge(player);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) //regular collider
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //do damage logic to player
        }

        if (collision.gameObject.CompareTag("Finish"))
            isGoingRight = !isGoingRight;
    }

    private void Dodge(Transform player)
    {
        //make sure boss dodges right when player is left. the default dodge is left
        if (player.position.x <= transform.position.x)
            dodgeX *= -1;
        direction = new Vector3((player.position.x + dodgeX), transform.position.y, transform.position.z);
        transform.Translate((direction * speed) * Time.deltaTime, Space.World);
    }

}

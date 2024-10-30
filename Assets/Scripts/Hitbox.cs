using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 direction;
    GameObject mole;
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
            Destroy(mole);
        }
    }
}

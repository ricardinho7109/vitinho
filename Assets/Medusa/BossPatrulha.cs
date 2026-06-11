using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatrulha : MonoBehaviour
{
    [Header("Patrulha")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float patrolSpeed = 2f;
    [SerializeField] bool movingRight = true;
    [SerializeField] bool patrulhando = false;
    public float distancia = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (patrulhando)
        {
            Patrulhando();
        }
        spriteRenderer.flipX = !movingRight;
    }


    public void Patrulhar(bool patrulhar)
    {
        patrulhando = patrulhar;
    }

    private void Patrulhando()
    {
        // Movimento entre dois pontos
        if (movingRight)
        {
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);

            if (Vector2.Distance(transform.position, rightPoint.position) < distancia)
                movingRight = false;
        }
        else
        {
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);

            if (Vector2.Distance(transform.position, leftPoint.position) < distancia)
                movingRight = true;
        }
        anim.Play("Correr");
    }
}
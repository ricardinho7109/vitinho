using System;
using System.Collections;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Configurações")]
    public float moveSpeed = 2f;       // Velocidade de movimento
    public int maxHealth = 2;          // Vida do inimigo
    public float knockbackForce = 5f;  // Força do recuo ao levar dano
    public float distance = 1f;
    [SerializeField] bool movingRight = true;   // Direção inicial do movimento
    [SerializeField] Transform rightPoint;
    [SerializeField] Transform leftPoint;
    private bool vivo = true;
    private bool isKnockBacked = false;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D col; // Usado para desligar a colisão na morte

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ✅ CORRIGIDO: Ativado para evitar o erro de travamento na morte!
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isKnockBacked || !vivo) return;

        // Movimento básico para frente
        Patrulhar();
    }

    private void Patrulhar()
    {
        // Movimento entre dois pontos
        if (movingRight)
        {
            if (Vector2.Distance(transform.position, rightPoint.position) < distance)
            {
                movingRight = false;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, leftPoint.position) < distance)
            {
                movingRight = true;
            }
        }

        Move();
    }

    void Move()
    {
        float direction = movingRight ? 1 : -1;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        MirrorSprite(direction);

        anim.SetFloat("Velocidade", Mathf.Abs(rb.velocity.x));
    }

    private void MirrorSprite(float moveInput)
    {
        if (moveInput < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Só dá dano se o inimigo ainda estiver vivo
        if (vivo && collision.gameObject.CompareTag("Player"))
        {
            SistemaDeVida sistemaDeVida = collision.gameObject.GetComponent<SistemaDeVida>();
            if (sistemaDeVida != null)
            {
                sistemaDeVida.AplicarDano(10);
            }
        }
    }

    public void EfeitoDeRecuo()
    {
        if (!vivo) return; // Não toma recuo se já estiver morto

        isKnockBacked = true;

        float knockbackDirection = movingRight ? -1 : 1;
        Vector2 force = new(knockbackDirection * knockbackForce, 0);

        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockback());
    }

    IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        isKnockBacked = false;
    }

    public void EfeitoDePiscar()
    {
        StartCoroutine(Piscar());
    }

    IEnumerator Piscar()
    {
        Color corOriginal = spriteRenderer.color;
        Color corTransparente = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = corTransparente;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = corOriginal;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AnimacaoDeDano()
    {
        if (!vivo) return;
        anim.SetTrigger("Machucado");
        StartCoroutine(ResetMachucado());
    }

    IEnumerator ResetMachucado()
    {
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("Machucado");
    }

    // 💀 ONDE A MORTE É TRATADA:
    internal void AnimacaoDeMorte()
    {
        if (!vivo) return; // Evita rodar a morte duas vezes por acidente

        vivo = false;

        // Para totalmente a física do inimigo para ele não cair no limbo ou ficar deslizando
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Desliga o colisor para o Player passar por dentro do corpo dele sem trombar
        if (col != null) col.enabled = false;

        // Ativa a animação de morte baseada na sua Unity (muda o Bool "Vivo" para false)
        anim.SetBool("Vivo", vivo);

        // Efeito visual enquanto some
        EfeitoDePiscar();

        // Destrói o objeto após 3 segundos (tempo para a animação terminar)
        Destroy(gameObject, 3f);
    }
}
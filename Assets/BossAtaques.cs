using System.Collections;
using UnityEngine;


public enum BossState { Idle, ChooseAttack, Attacking, Recover }

public class BossAtaques : MonoBehaviour
{
    public Transform player;

    [Header("Aoe config")]
    public float aoeRadius = 3f;
    public float aoeDuration = 3f;
    public int aoeDamage = 2;

    public GameObject aoePrefab;
    [Header("Sword config")]
    public GameObject swordPrefab;

    [Header("Dive config")]
    public float dashForce = 10f;

    private BossState currentState = BossState.Idle;

    private Rigidbody2D rb;
    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public BossState GetState()
    {
        return currentState;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("DiagonalDive");
            Attack_DiagonalDive();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Aoe");
            Attack_AoE();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Espada");
            Attack_ThrowSword();
        }
    }

    // ------------------- ATAQUES ------------------- //

    // 1. Investida Diagonal vinda do céu
    public void Attack_DiagonalDive()
    {
        // Vector2 direction = (player.position - transform.position).normalized;
        // rb.velocity = Vector2.zero; // reseta movimento antes
        // rb.AddForce(new Vector2(direction.x, -1f).normalized * dashForce, ForceMode2D.Impulse);

        // Debug.Log("Boss faz investida diagonal do céu!");
    }

    // 2. Explosão em área no ar
    public void Attack_AoE()
    {
        currentState = BossState.Attacking;
        StartCoroutine(nameof(IenumeratorAOE));
    }

    IEnumerator IenumeratorAOE()
    {
        Vector2 destination = transform.position + Vector3.up * 3.5f;
        rb.gravityScale = 0;//Corta a gravidade
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;


        anim.Play("Pulo");
        //Faz o boss pular
        while (Vector2.Distance(transform.position, destination) > 1)
        {
            transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime);
            yield return null;
        }

        anim.Play("Ataque Em Area");
        // instanciar efeito visual de AOE
        GameObject aoe = Instantiate(aoePrefab, transform.position, Quaternion.identity);
        Destroy(aoe, aoeDuration - 0.5f);

        float timer = 0;
        while (timer < aoeDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    hit.GetComponent<PlayerController>().TakeDamage(aoeDamage);
                }
            }

            timer += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        anim.Play("Idle");''
        rb.gravityScale = 1;//Volta a ter gravidade
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        currentState = BossState.Idle;
    }

    // 3. Arremesso da espada para frente e puxada de volta
    public void Attack_ThrowSword()
    {
        // Vector2 direction = (player.position - transform.position).normalized;

        // GameObject spawnedSword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
        // BossEspada espada = spawnedSword.GetComponent<BossEspada>();

        // espada.IniciarAtaque(transform, direction);
    }

    // Gizmo para visualizar área do ataque no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // 🔴 NÃO começa como true
    private bool isGrounded;

    // ✅ ADICIONADO (ground check)
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask groundLayer;
    
    [Space(5)]
    [Header("Fireball")]
    public GameObject fireballPrefab;
    public Transform shootPosition;
    public float shootCooldown;
    private float lastShootTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ✅ VERIFICAÇÃO DE CHÃO (NOVA)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 🔍 DEBUG (pode apagar depois)
        //Debug.Log(isGrounded);

        UpdateAnimator();
        Movement();
        Jump();
        Attack();
        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HUDController.Instance.PausarJogo();
        }
    }


    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("IsJumping", !isGrounded);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Attack");
            Vector3 position = shootPosition.position;
            Quaternion rotation = fireballPrefab.transform.rotation;
            if (spriteRenderer.flipX)
            {
                //position.y = shootPosition.position.y * -1;
                rotation.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                //position.y = shootPosition.position.y * 1;
                rotation.eulerAngles = new Vector3(0, 0, 0);
            }


            Instantiate(fireballPrefab, position, rotation);
            //AudioManager.Instance.Play("Fireball");
            lastShootTime = Time.time + shootCooldown;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        MirrorSprite(moveInput);
    }

    private void MirrorSprite(float moveInput)
    {
        if (moveInput < 0)
            spriteRenderer.flipX = true;
        else if (moveInput > 0)
            spriteRenderer.flipX = false;
    }

    // ✅ DEBUG VISUAL (bola no pé)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
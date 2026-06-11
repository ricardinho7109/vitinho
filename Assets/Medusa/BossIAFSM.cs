using UnityEngine;

public class BossIAFSM : MonoBehaviour
{
    public Transform player;
    public float attackCooldown = 2f;

    private BossState currentState = BossState.Idle;
    private float cooldownTimer = 0f;
    private int chosenAttack = -1;

    private BossAtaques bossAtaques;
    private BossPatrulha bossPatrulha;
    void Awake()
    {
        bossAtaques = GetComponent<BossAtaques>();
        bossPatrulha = GetComponent<BossPatrulha>();
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                IdleState();
                break;

            case BossState.ChooseAttack:
                ChooseAttackState();
                break;

            case BossState.Attacking:
                AttackState();
                break;

            case BossState.Recover:
                RecoverState();
                break;
        }
    }

    void IdleState()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            bossPatrulha.Patrulhar(false);
            currentState = BossState.ChooseAttack;
        }
        else
        {
            bossPatrulha.Patrulhar(true);
        }
    }

    void ChooseAttackState()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < 3f)
            chosenAttack = 0; // Aoe
        else if (distance < 6f)
            chosenAttack = 1; // Ataque Diagonal
        else
            chosenAttack = 2; // Lançar Espada

        switch (chosenAttack)
        {
            case 0:
                bossAtaques.Attack_AoE();
                break;
            case 1:
                Debug.Log("FSM: Ataque 2 → Ataque Diagonal!");
                break;
            case 2:
                Debug.Log("FSM: Ataque 3 → Lançar Espada!");
                break;
        }

        currentState = BossState.Attacking;
    }

    void AttackState()
    {
        if (bossAtaques.GetState() == BossState.Idle)
        {
            currentState = BossState.Recover;
        }
    }

    void RecoverState()
    {
        cooldownTimer = attackCooldown;
        currentState = BossState.Idle;
    }
}
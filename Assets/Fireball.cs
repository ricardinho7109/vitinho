using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 20f; // Velocidade da bala
    public float lifetime = 2f; // Tempo de vida da bala antes de ser destruída

    [Range(0f, 20
        f)]
    public float porcentagemDano = 25f; // Quanto por cento de vida o inimigo vai perder (Ex: 25%)

    private void Start()
    {
        // Destrói a bala após um certo tempo
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move a bala na direção em que ela está apontando
        transform.Translate(speed * Time.deltaTime * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se a colisão foi com um inimigo
        if (other.CompareTag("Inimigo"))
        {
            // Tenta pegar o script de vida que está no inimigo
            VidaInimigo scriptVida = other.GetComponent<VidaInimigo>();

            if (scriptVida != null)
            {
                // Aplica o dano baseado na porcentagem
                scriptVida.ReceberDanoPorcentagem(porcentagemDano);
            }
        }

        // Destrói a bala
        Destroy(gameObject);
    }
}
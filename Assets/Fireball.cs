using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 20f; // Velocidade da bala
    public float lifetime = 2f; // Tempo de vida da bala antes de ser destruída
    public Vector2 direction; // Direção da bala

    [Range(0f, 100f)]
    public float porcentagemDano = 25f; // Quanto por cento de vida o inimigo vai perder (Ex: 25%)

    private void Start()
    {
        // Destrói a bala após um certo tempo
        Destroy(gameObject, lifetime);
    }

    

    private void Update()
    {
        // Move a bala na direção em que ela está apontando
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se a colisão foi com um inimigo
        if (other.CompareTag("Inimigo"))
        {
            // Tenta pegar o script de vida que está no inimigo
            SistemaDeVidaInimigo scriptVida = other.GetComponent<SistemaDeVidaInimigo>();

            if (scriptVida != null)
            {
                // CORRIGIDO: Passando 'porcentagemDano' em vez de 'Dano'
                scriptVida.ReceberDanoPorcentagem(porcentagemDano);
            }
        }

        // Destrói a bala
        Destroy(gameObject);
    }
}
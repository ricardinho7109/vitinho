
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDeVidaInimigo : SistemaDeVida
{
    Inimigo inimigo;
    BarraDeVidaInimigo barraDeVidaInimigo;

    new void Start()
    {
        base.Start();
        inimigo = GetComponent<Inimigo>();
        barraDeVidaInimigo = GetComponentInChildren<BarraDeVidaInimigo>();
    }

    // NOVA FUNÇÃO: Chamada pela Fireball para calcular a porcentagem de dano
    public void ReceberDanoPorcentagem(float porcentagem)
    {
        // Calcula quanto vale a porcentagem em relação à vida máxima
        float valorDoDano = vidaMaxima * (porcentagem / 100f);

        // Envia esse valor para a função AplicarDano que já cuida do resto
        AplicarDano(valorDoDano);
    }

    public override void AplicarDano(float dano)
    {
        vidaAtual -= dano;
        if (vidaAtual <= 0)
        {
            Morrer();
        }

        inimigo.AnimacaoDeDano();
        inimigo.EfeitoDePiscar();
        inimigo.EfeitoDeRecuo();
        AtualizarVida();
    }

    override protected void Morrer()
    {
        inimigo.AnimacaoDeMorte();
    }

    void AtualizarVida()
    {
        // Uma segurança extra: evita erro se o inimigo não tiver barra de vida
        if (barraDeVidaInimigo != null)
        {
            barraDeVidaInimigo.AtualizarUI(vidaAtual / vidaMaxima);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public bool jogoPausado { get; private set; }
    private MenuPausa menuPausa;

    public static HUDController Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        menuPausa = GetComponentInChildren<MenuPausa>(includeInactive: true);
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Fase01");
    }

    public void PausarJogo()
    {
        jogoPausado = true;
        Time.timeScale = 0;
        menuPausa.gameObject.SetActive(true);
    }

    public void RetornarJogo()
    {
        jogoPausado = false;
        Time.timeScale = 1;
        menuPausa.gameObject.SetActive(false);
    }

    public void MostrarGameOver()
    {
        /*jogoPausado = true;
        Time.timeScale = 0;*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Reiniciar()
    {
        jogoPausado = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🏆 ADICIONADO: Função para carregar a tela de vitória
    public void MostrarVitoria()
    {
        jogoPausado = false;
        Time.timeScale = 1;
        // Subentende-se que você criará uma cena chamada "TelaVitoria" no seu projeto
        SceneManager.LoadScene("TelaVitoria");
    }
}
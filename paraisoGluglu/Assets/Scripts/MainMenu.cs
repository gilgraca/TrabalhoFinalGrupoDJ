// Script responsável por gerir os botões de navegação entre menus, níveis e sair do jogo
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class MainMenu : MonoBehaviour
{
    // Nome da cena do primeiro nível (jogo novo ou recomeço)
    public string nivelACarregar;
    // Nome da cena do menu principal
    public string menuprincipal;
    // Nome da cena do tutorial
    public string tutorial;
    // Tempo de espera antes de mudar de cena (para som de clique tocar)
    private float atrasoCena = 0.2f;
    // Inicia um novo jogo, resetando pontuação e GameManager
    public void NewGame()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("apanhados", 0);
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        StartCoroutine(CarregarCenaComAtraso(nivelACarregar));
    }
    public void NewGameAfterDeath()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("apanhados", 0);
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        StartCoroutine(CarregarCenaComAtraso(nivelACarregar));
    }
    // Volta ao menu principal a partir de qualquer cena
    public void MenuPrincipal()
    {
        StartCoroutine(CarregarCenaComAtraso(menuprincipal));
    }
    // Sai do jogo (funciona só na build, não no editor)
    public void QuitGame()
    {
        Application.Quit();
    }
    // Ir para o Tutorial
    public void Tutorial()
    {
        StartCoroutine(CarregarCenaComAtraso(tutorial));
    }
    // Corrotina para carregar a cena com atraso
    private IEnumerator CarregarCenaComAtraso(string nomeCena)
    {
        yield return new WaitForSeconds(atrasoCena);
        SceneManager.LoadScene(nomeCena);
    }
}
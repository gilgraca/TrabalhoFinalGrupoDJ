// Script responsável por gerir os botões de navegação entre menus, níveis e sair do jogo
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nome da cena do menu principal
    public string menuprincipal;
    // Nome da cena dos creditos
    public string creditos;
    // Inicia um novo jogo, resetando pontuação e GameManager
    public void NewGame()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("apanhados", 0);
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
    }
    public void NewGameAfterDeath()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("apanhados", 0);
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
    }
    // Volta ao menu principal a partir de qualquer cena
    public void MenuPrincipal()
    {
        // Debug para confirmar o nome da cena
        //Debug.Log("A carregar cena: " + menuprincipal);

        // Retoma o tempo de jogo se estiver pausado
        Time.timeScale = 1f;

        // Carrega a cena pelo nome
        SceneManager.LoadScene(menuprincipal);
    }
    // Volta ao menu principal a partir de qualquer cena
    public void Creditos()
    {
        // Debug para confirmar o nome da cena
        //Debug.Log("A carregar cena: " + creditos);

        // Retoma o tempo de jogo se estiver pausado
        Time.timeScale = 1f;

        // Carrega a cena pelo nome
        SceneManager.LoadScene(creditos);
    }
}
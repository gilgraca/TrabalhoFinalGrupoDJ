// Script responsável por gerir os botões de navegação entre menus, níveis e sair do jogo
using UnityEngine;
public class MainMenu : MonoBehaviour
{
    // Nome da cena do menu principal
    public string menuprincipal;
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
        StartCoroutine(menuprincipal);
    }
}
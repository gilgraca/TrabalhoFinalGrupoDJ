using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public string menuprincipal;
	public string creditos;
	public string nomeCenaJogo; // <- Nome da primeira cena do jogo

	public void NewGame()
	{
		PlayerPrefs.SetInt("score", 0);
		PlayerPrefs.SetInt("apanhados", 0);

		if (GameManager.Instance != null)
		{
			Destroy(GameManager.Instance.gameObject);
		}

		SceneManager.LoadScene(nomeCenaJogo); // <- Corrigido aqui
	}

	public void NewGameAfterDeath()
	{
		PlayerPrefs.SetInt("score", 0);
		PlayerPrefs.SetInt("apanhados", 0);

		if (GameManager.Instance != null)
		{
			Destroy(GameManager.Instance.gameObject);
		}

		SceneManager.LoadScene(nomeCenaJogo); // <- Também aqui
	}

	public void MenuPrincipal()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(menuprincipal);
	}

	public void Creditos()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(creditos);
	}

	public void SairDoJogo()
	{
		Application.Quit();
	}
}

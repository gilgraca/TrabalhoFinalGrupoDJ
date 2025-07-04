using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public string menu;

	// Dados do jogador
	public int vidaJogador = 5;
	public int milhoTotal = 0;

	public bool usarDoubleJump = false;
	public bool usarDash = false;
	public bool usarInvencibilidade = false;
	public bool usarInvisibilidade = false;
	public bool usarAtaqueEspecial = false;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);

			// Garante valor de vida válido ao iniciar o jogo
			if (vidaJogador <= 0)
				vidaJogador = 5;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SairParaMenu()
	{
		Time.timeScale = 1f;
		Destroy(GameManager.Instance.gameObject);
		SceneManager.LoadScene(menu);
	}
}

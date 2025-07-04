using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVida : MonoBehaviour
{
	[Header("HUD de Vida")]
	[SerializeField] private GameObject[] hp_items;
	[SerializeField] private int vidaMaxima = 5;
	private int vidaAtual;

	[Header("Som ao levar dano")]
	[SerializeField] private AudioClip somDano;
	private AudioSource audioSource;

	private PlayerPowerUp powerUps;

	void Start()
	{
		powerUps = GetComponent<PlayerPowerUp>();

		// Carrega a vida do GameManager, se existir
		if (GameManager.Instance != null)
		{
			vidaAtual = Mathf.Clamp(GameManager.Instance.vidaJogador, 0, vidaMaxima);
		}
		else
		{
			vidaAtual = vidaMaxima;
			Debug.LogWarning("GameManager.Instance está nulo. A usar vida máxima por defeito.");
		}

		SetVida(vidaAtual); // Atualiza o HUD

		// Garante que tem um AudioSource
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f;
	}

	public void LevarDano(int dano)
	{
		if (dano <= 0) return;

		vidaAtual -= dano;

		// Atualiza o GameManager, se estiver disponível
		if (GameManager.Instance != null)
			GameManager.Instance.vidaJogador = vidaAtual;

		// Toca som de dano
		if (somDano != null && audioSource != null)
		{
			audioSource.PlayOneShot(somDano);
			//Debug.Log("Som de dano tocado.");
		}

		// Desativa um item de vida no HUD
		if (vidaAtual >= 0 && vidaAtual < hp_items.Length)
			hp_items[vidaAtual].SetActive(false);

		// Ativa invencibilidade temporária se aplicável
		if (powerUps != null && !powerUps.EstaInvencivelPorDano())
			StartCoroutine(powerUps.InvencivelPorDano());

		if (vidaAtual <= 0)
			Morrer();
	}

	private void Morrer()
	{
		SceneManager.LoadScene("GameOver");
	}

	public int VidaAtual() => vidaAtual;

	public int VidaMaxima() => vidaMaxima;

	public void SetVida(int novaVida)
	{
		vidaAtual = Mathf.Clamp(novaVida, 0, vidaMaxima);

		for (int i = 0; i < hp_items.Length; i++)
		{
			hp_items[i].SetActive(i < vidaAtual);
		}
	}
}

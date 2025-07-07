using UnityEngine;

public class InimigoZonaAtaque : MonoBehaviour
{
	[SerializeField] private int dano = 1;
	[SerializeField] private float forcaPushback = 5f;

	[Header("Som de ataque")]
	[SerializeField] private AudioClip somAtaque;
	private AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f; // 0 para 2D, 1 para 3D
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerPowerUp powerUps = other.GetComponent<PlayerPowerUp>();
			PlayerVida playerVida = other.GetComponent<PlayerVida>();

			if (powerUps != null && !powerUps.EstaInvencivel() && !powerUps.EstaInvencivelPorDano())
			{
				// Aplica dano
				if (playerVida != null)
					playerVida.LevarDano(dano);

				// Aplica impulso
				Rigidbody rb = other.GetComponent<Rigidbody>();
				if (rb != null)
				{
					Vector3 direcao = (other.transform.position - transform.position);
					direcao.y = 0f;
					direcao = direcao.normalized;

					Vector3 impulsoFinal = direcao * forcaPushback + Vector3.up * 2f;
					rb.AddForce(impulsoFinal, ForceMode.Impulse);
				}

				// Toca o som de ataque
				TocarSom();
			}
		}
	}

	private void TocarSom()
	{
		if (audioSource != null && somAtaque != null)
			audioSource.PlayOneShot(somAtaque);
	}
}

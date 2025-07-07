using UnityEngine;

public class ChavePickup : MonoBehaviour
{
	[Header("ID da Chave")]
	public int idChave = 1;

	[Header("Som ao pegar a chave")]
	[SerializeField] private AudioClip somPegarChave;
	private AudioSource audioSource;

	private bool foiColetada = false;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f; // 2D
	}

	void OnTriggerEnter(Collider other)
	{
		if (foiColetada) return;

		if (other.CompareTag("Player"))
		{
			foiColetada = true;

			// Salva a chave no inventário do jogador (caso use sistema de chaves)
			ChavePlayer chaveScript = other.GetComponent<ChavePlayer>();
			if (chaveScript != null)
			{
				chaveScript.AdicionarChave(idChave);
			}

			// TOCA O SOM
			if (somPegarChave != null)
				audioSource.PlayOneShot(somPegarChave);

			// Esconde a chave visualmente
			GetComponentInChildren<MeshRenderer>().enabled = false;

			// Destroi após o som
			Destroy(gameObject, somPegarChave != null ? somPegarChave.length : 1f);
		}
	}
}

using UnityEngine;

public class Chave : MonoBehaviour
{
	public int idChave = 1;
	public float amplitude = 0.25f;
	public float velocidade = 2f;
	private Vector3 posInicial;

	[SerializeField] private AudioClip somColetaChave;
	private AudioSource audioSource;

	private bool foiColetada = false;

	void Start()
	{
		posInicial = transform.position;

		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f;
	}

	void Update()
	{
		float novaY = Mathf.Sin(Time.time * velocidade) * amplitude;
		transform.position = new Vector3(posInicial.x, posInicial.y + novaY, posInicial.z);
		transform.Rotate(Vector3.up * 90f * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if (foiColetada) return;

		if (other.CompareTag("Player"))
		{
			foiColetada = true;

			// Adiciona a chave ao jogador (opcional, se usar sistema de chaves)
			ChavePlayer chavesScript = other.GetComponent<ChavePlayer>();
			if (chavesScript != null)
				chavesScript.AdicionarChave(idChave);

			// Ativa a porta diretamente
			PortaChaveController porta = FindFirstObjectByType<PortaChaveController>();
			if (porta != null)
				porta.TentarAbrirPorta(); // Sem necessidade de encostar

			// Toca som da chave
			if (somColetaChave != null && audioSource != null)
				audioSource.PlayOneShot(somColetaChave);

			// Esconde o modelo
			GetComponentInChildren<MeshRenderer>().enabled = false;

			// Destroi após o som
			Destroy(gameObject, somColetaChave != null ? somColetaChave.length : 0f);
		}
	}
}

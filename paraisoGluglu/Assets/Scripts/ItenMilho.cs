using UnityEngine;

public class ItenMilho : MonoBehaviour
{
	public int pointToAdd;
	public Vector3 rotationSpeed = new Vector3(0, 100, 0);

	private bool foiApanhado = false;

	[Header("Áudio")]
	[SerializeField] private AudioClip somColeta;
	private AudioSource audioSource;

	void Start()
	{
		// Cria e configura o AudioSource se não existir
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		// Configurações do som
		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 1f; // 3D sound
		audioSource.loop = false;
	}

	void Update()
	{
		transform.Rotate(rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if (foiApanhado) return;
		if (other.GetComponent<PlayerToast>() == null) return;

		foiApanhado = true;

		// Pontuação
		ScoreManager.AddPoints(pointToAdd);

		// GameManager
		if (GameManager.Instance != null)
		{
			GameManager.Instance.milhoTotal += 1;
		}

		// Tracker
		NivelTracker tracker = FindFirstObjectByType<NivelTracker>();
		if (tracker != null)
		{
			tracker.MostrarToastManual();
			tracker.AdicionarMilho();
		}

		// Toca som de coleta
		float tempoSom = 1f;
		if (somColeta != null && audioSource != null)
		{
			audioSource.PlayOneShot(somColeta);
			tempoSom = somColeta.length;
		}

		// Esconde o milho visualmente
		GetComponentInChildren<MeshRenderer>().enabled = false;

		// Destroi o milho após o som
		Destroy(gameObject, tempoSom);
	}
}

using UnityEngine;

// Script para a destruição da caixa
public class DestruirCaixa : MonoBehaviour
{
	// Prefab da tábua que vai saltar
	public GameObject tabuaPrefab;

	// Quantidade de tábuas que vão saltar
	public int quantidadeTabuas = 5;

	// Força da explosão que empurra as tábuas
	public float forcaExplosao = 300f;

	// Raio da explosão
	public float raioExplosao = 2f;

	// Som da destruição
	[SerializeField] private AudioClip somDestruicao;
	private AudioSource audioSource;

	void Start()
	{
		// Obtém ou adiciona um AudioSource
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 1f; // Som 3D (opcional)
	}

	// Método chamado para destruir a caixa
	public void QuebrarCaixa()
	{
		// Toca o som da caixa quebrando
		if (somDestruicao != null && audioSource != null)
		{
			audioSource.PlayOneShot(somDestruicao);
		}

		// Cria tábuas no mesmo local
		for (int i = 0; i < quantidadeTabuas; i++)
		{
			Vector3 pos = transform.position + Random.insideUnitSphere * 0.5f;
			GameObject tabua = Instantiate(tabuaPrefab, pos, Random.rotation);

			Rigidbody rb = tabua.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			rb.AddExplosionForce(forcaExplosao, transform.position, raioExplosao);

			Destroy(tabua, 3f);
		}

		// Destroi a caixa após o som tocar
		Destroy(gameObject, (somDestruicao != null) ? somDestruicao.length : 0f);
	}
}

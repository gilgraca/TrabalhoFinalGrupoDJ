using UnityEngine;

public class Chave : MonoBehaviour
{
	public int idChave = 1; // ID da chave (definido no Inspetor)
	public float amplitude = 0.25f;
	public float velocidade = 2f;

	private Vector3 posInicial;

	void Start()
	{
		posInicial = transform.position;
	}

	void Update()
	{
		// Movimento de flutuação + rotação
		float novaY = Mathf.Sin(Time.time * velocidade) * amplitude;
		transform.position = new Vector3(posInicial.x, posInicial.y + novaY, posInicial.z);
		transform.Rotate(Vector3.up * 90f * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Chaves chavesScript = other.GetComponent<Chaves>();
			if (chavesScript != null)
			{
				chavesScript.AdicionarChave(idChave);
				Destroy(gameObject); // Remove a chave após pegar
			}
		}
	}
}


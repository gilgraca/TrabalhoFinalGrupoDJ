using UnityEngine;

public class PortaComChaveAuto : MonoBehaviour
{
	public int idChaveNecessaria = 1;                  // ID da chave necess�ria
	public Transform parteVisualDaPorta;               // Parte que gira
	public Vector3 rotacaoAberta = new Vector3(0, 90, 0); // Rota��o final da porta
	public float velocidadeAbertura = 2f;              // Velocidade da abertura

	private bool aberta = false;
	private bool jogadorProximo = false;

	void Update()
	{
		if (jogadorProximo && !aberta)
		{
			GameObject jogador = GameObject.FindGameObjectWithTag("Player");
			Chaves chavesScript = jogador.GetComponent<Chaves>();

			if (chavesScript != null && chavesScript.TemChave(idChaveNecessaria))
			{
				aberta = true;
				//Debug.Log("Porta aberta com sucesso.");
			}
			else
			{
				//Debug.Log("Voc� precisa da chave correta.");
			}
		}

		if (aberta && parteVisualDaPorta != null)
		{
			Quaternion alvo = Quaternion.Euler(rotacaoAberta);
			parteVisualDaPorta.rotation = Quaternion.Lerp(parteVisualDaPorta.rotation, alvo, Time.deltaTime * velocidadeAbertura);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			jogadorProximo = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			jogadorProximo = false;
		}
	}
}

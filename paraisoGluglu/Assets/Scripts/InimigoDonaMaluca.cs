using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class InimigoDonaMaluca : MonoBehaviour
{
	public float distanciaAtaque = 1.5f;
	public float tempoEntreAtaques = 2f;

	private float tempoProximoAtaque = 0f;
	private Transform jogador;
	private PlayerPowerUp jogadorPowerUps;

	private NavMeshAgent agent;
	private Animator animator;

	[Header("Feedback Visual de Dano")]
	[SerializeField] private Renderer meuRenderer;
	[SerializeField] private Material materialNormal;
	[SerializeField] private Material materialDano;
	[SerializeField] private float duracaoPiscar = 0.5f;

	[Header("Som de Dano")]
	[SerializeField] private AudioClip somDano;
	private AudioSource audioSource;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();

		GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
		if (objJogador != null)
		{
			jogador = objJogador.transform;
			jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
		}

		animator = GetComponentInChildren<Animator>();

		if (meuRenderer == null)
			meuRenderer = GetComponentInChildren<Renderer>();

		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f;
	}

	void Update()
	{
		if (jogador == null || jogadorPowerUps == null) return;
		if (jogadorPowerUps.EstaInvisivel()) return;

		agent.SetDestination(jogador.position);

		float distancia = Vector3.Distance(transform.position, jogador.position);
		if (distancia <= distanciaAtaque && Time.time >= tempoProximoAtaque)
		{
			AtacarJogador();
			tempoProximoAtaque = Time.time + tempoEntreAtaques;
		}
	}

	void AtacarJogador()
	{
		if (jogador == null) return;

		if (animator != null)
			animator.SetTrigger("attack");

		PlayerVida vida = jogador.GetComponent<PlayerVida>();
		PlayerPowerUp powerUps = jogador.GetComponent<PlayerPowerUp>();

		if (powerUps != null && (powerUps.EstaInvencivel() || powerUps.EstaInvencivelPorDano()))
			return;

		if (vida != null)
			vida.LevarDano(1);

		Rigidbody rb = jogador.GetComponent<Rigidbody>();
		if (rb != null)
		{
			Vector3 direcao = (jogador.position - transform.position).normalized;
			rb.AddForce(direcao * 5f, ForceMode.Impulse);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("AtaquePlayer"))
		{
			TocarSomDano();
			StartCoroutine(PiscarDano());
		}

		if (other.CompareTag("Caixa"))
		{
			DestruirCaixa caixa = other.GetComponent<DestruirCaixa>();
			if (caixa != null)
				caixa.QuebrarCaixa();
			else
				Destroy(other.gameObject);
		}
	}

	private IEnumerator PiscarDano()
	{
		if (meuRenderer == null || materialNormal == null || materialDano == null)
			yield break;

		meuRenderer.material = materialDano;
		yield return new WaitForSeconds(duracaoPiscar);
		meuRenderer.material = materialNormal;
	}

	private void TocarSomDano()
	{
		if (audioSource != null && somDano != null)
			audioSource.PlayOneShot(somDano);
	}
}

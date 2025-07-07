using System.Collections;
using UnityEngine;

public class PlayerAtaque : MonoBehaviour
{
	[Header("Ataques")]
	[SerializeField] private GameObject prefabPena;
	[SerializeField] private Transform pontoDisparo;
	[SerializeField] private Transform disparo1;
	[SerializeField] private Transform disparo2;

	[Header("Cooldowns")]
	[SerializeField] private float cooldownAtaqueBasico = 0.5f;
	[SerializeField] private float cooldownAtaqueEspecial = 1f;
	[SerializeField] private float tempoCooldownTeclas = 10f;

	[Header("Habilidades Ativas")]
	[SerializeField] private bool podeAtaqueEspecial = false;

	private bool ataqueBasicoDisponivel = true;
	private bool ataqueEspecialDisponivel = true;

	[Header("Referências")]
	[SerializeField] private Animator animator;
	[SerializeField] private PowerUpCooldownUI ataqueEspecialCooldownUI;
	[SerializeField] private PowerUpCooldownUI ataqueNormalCooldownUI;

	[Header("Áudio")]
	[SerializeField] private AudioClip somAtaqueBasico;
	[SerializeField] private AudioClip somAtaqueEspecial;
	[SerializeField] private AudioClip somShift;
	[SerializeField] private AudioClip somTeclaE;
	[SerializeField] private AudioClip somTeclaQ;

	private AudioSource audioSource;
	private PlayerPowerUp powerUp;

	private float tempoProximoUsoShift = 0f;
	private float tempoProximoUsoE = 0f;
	private float tempoProximoUsoQ = 0f;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f;

		powerUp = GetComponent<PlayerPowerUp>();
		podeAtaqueEspecial = GameManager.Instance.usarAtaqueEspecial;
	}

	void Update()
	{
		if (MenuPausa.jogoPausado) return;

		// === Ataque Básico ===
		if (Input.GetMouseButtonDown(0) && ataqueBasicoDisponivel)
			AtacarBasico();

		// === Ataque Especial ===
		if (Input.GetMouseButtonDown(1) && podeAtaqueEspecial && ataqueEspecialDisponivel)
			AtivarAtaqueEspecial();

		// === Dash com Shift ===
		if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= tempoProximoUsoShift)
		{
			if (powerUp != null && powerUp.PodeDash() && !powerUp.EstaADashar())
			{
				if (RealizarDash())
				{
					TocarSom(somShift);
					tempoProximoUsoShift = Time.time + tempoCooldownTeclas;
				}
			}
		}

		// === Invisibilidade com E ===
		if (Input.GetKeyDown(KeyCode.E) && Time.time >= tempoProximoUsoE)
		{
			if (powerUp != null && powerUp.PodeInvisibilidade() && !powerUp.EstaInvisivel())
			{
				if (AtivarPoderE())
				{
					TocarSom(somTeclaE);
					tempoProximoUsoE = Time.time + tempoCooldownTeclas;
				}
			}
		}

		// === Invencibilidade com Q ===
		if (Input.GetKeyDown(KeyCode.Q) && Time.time >= tempoProximoUsoQ)
		{
			if (powerUp != null && powerUp.PodeInvencibilidade() && !powerUp.EstaInvencivel())
			{
				if (AtivarPoderQ())
				{
					TocarSom(somTeclaQ);
					tempoProximoUsoQ = Time.time + tempoCooldownTeclas;
				}
			}
		}
	}

	private void AtacarBasico()
	{
		ataqueBasicoDisponivel = false;

		Vector3 direcaoDisparo = transform.forward;
		Quaternion rotacaoFinal = Quaternion.Euler(90f, Quaternion.LookRotation(direcaoDisparo).eulerAngles.y, 0f);

		GameObject novaPena = Instantiate(prefabPena, pontoDisparo.position, rotacaoFinal);
		novaPena.GetComponent<PenaAtaque>().DefinirDirecao(direcaoDisparo);

		animator.SetBool("isAttacking", true);
		StartCoroutine(DesativarBool("isAttacking", 0.5f));

		TocarSom(somAtaqueBasico);

		if (ataqueNormalCooldownUI != null)
			ataqueNormalCooldownUI.IniciarCooldown(cooldownAtaqueBasico);

		StartCoroutine(ReporCooldownAtaqueBasico());
	}

	private IEnumerator ReporCooldownAtaqueBasico()
	{
		yield return new WaitForSeconds(cooldownAtaqueBasico);
		ataqueBasicoDisponivel = true;
	}

	private void AtivarAtaqueEspecial()
	{
		if (!ataqueEspecialDisponivel) return;

		ataqueEspecialDisponivel = false;

		Vector3 direcao = transform.forward;
		CriarPena(disparo1.position, direcao);
		CriarPena(pontoDisparo.position, direcao);
		CriarPena(disparo2.position, direcao);

		animator.SetBool("isSpAttack", true);
		StartCoroutine(DesativarBool("isSpAttack", 0.5f));

		TocarSom(somAtaqueEspecial);

		if (ataqueEspecialCooldownUI != null)
			ataqueEspecialCooldownUI.IniciarCooldown(cooldownAtaqueEspecial);

		StartCoroutine(ReporCooldownAtaqueEspecial());
	}

	private IEnumerator ReporCooldownAtaqueEspecial()
	{
		yield return new WaitForSeconds(cooldownAtaqueEspecial);
		ataqueEspecialDisponivel = true;
	}

	private IEnumerator DesativarBool(string nomeBool, float tempo)
	{
		yield return new WaitForSeconds(tempo);
		animator.SetBool(nomeBool, false);
	}

	private void CriarPena(Vector3 posicao, Vector3 direcao)
	{
		GameObject novaPena = Instantiate(prefabPena, posicao, Quaternion.identity);
		PenaAtaque pena = novaPena.GetComponent<PenaAtaque>();
		if (pena != null)
			pena.DefinirDirecao(direcao);
	}

	private void TocarSom(AudioClip clip)
	{
		if (audioSource != null && clip != null)
			audioSource.PlayOneShot(clip);
	}

	// Habilidades customizadas
	private bool RealizarDash()
	{
		// Aqui entra tua lógica real de dash
		return true;
	}

	private bool AtivarPoderE()
	{
		// Aqui entra tua lógica real do poder E
		return true;
	}

	private bool AtivarPoderQ()
	{
		// Aqui entra tua lógica real do poder Q
		return true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Caixa"))
		{
			Vector3 normal = collision.contacts[0].normal;
			if (Vector3.Dot(normal, Vector3.up) > 0.5f)
			{
				DestruirCaixa caixa = collision.collider.GetComponent<DestruirCaixa>();
				if (caixa != null)
					caixa.QuebrarCaixa();
			}
		}
	}

	public void SetAtaqueEspecial(bool estado)
	{
		podeAtaqueEspecial = estado;
	}
}

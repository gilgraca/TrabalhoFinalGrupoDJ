using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
	private Rigidbody rb;

	[Header("Double Jump")]
	[SerializeField] private bool podeDoubleJump = false;
	private bool jaUsouDoubleJump = false;
	[SerializeField] public float forcaSaltoExtra = 6f;

	[Header("Dash")]
	[SerializeField] private bool podeDash = false;
	[SerializeField] private float forcaDash = 12f;
	[SerializeField] private float tempoDash = 0.2f;
	[SerializeField] private float cooldownDash = 1f;
	private bool estaADashar = false;
	public bool dashDisponivel = true;

	[Header("Invencibilidade")]
	[SerializeField] private bool podeInvencibilidade = false;
	private bool estaInvencivel = false;
	[SerializeField] private float duracaoInvencivel = 2f;
	[SerializeField] private float cooldownInvencibilidade = 1f;
	public bool invencibilidadeDisponivel = true;

	[Header("Invisibilidade")]
	[SerializeField] private bool podeInvisibilidade = false;
	private bool estaInvisivel = false;
	[SerializeField] private float duracaoInvisivel = 5f;
	[SerializeField] private float cooldownInvisibilidade = 1f;
	public bool invisibilidadeDisponivel = true;

	[Header("Referências Visuais e Materiais")]
	[SerializeField] private Renderer meuRenderer;
	[SerializeField] private Material materialNormal;
	[SerializeField] private Material materialTransparente;
	[SerializeField] private Material materialDano;
	[SerializeField] private Animator animator;

	[Header("Cooldown UI")]
	[SerializeField] private PowerUpCooldownUI dashCooldownUI;
	[SerializeField] private PowerUpCooldownUI invisibilidadeCooldownUI;
	[SerializeField] private PowerUpCooldownUI invencibilidadeCooldownUI;

	private bool estaInvencivelDano = false;
	[SerializeField] private float duracaoInvencivelDano = 1.5f;

	private PlayerMovimentoCrash playerMovimento;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		meuRenderer = GetComponentInChildren<Renderer>();
		playerMovimento = GetComponent<PlayerMovimentoCrash>();

		podeDash = GameManager.Instance.usarDash;
		podeDoubleJump = GameManager.Instance.usarDoubleJump;
		podeInvencibilidade = GameManager.Instance.usarInvencibilidade;
		podeInvisibilidade = GameManager.Instance.usarInvisibilidade;
	}

	void Update()
	{
		if (podeDash && Input.GetKeyDown(KeyCode.LeftShift) && dashDisponivel && !estaADashar)
			AtivarDash();

		if (podeInvencibilidade && Input.GetKeyDown(KeyCode.Q) && !estaInvencivel)
			AtivarInvencibilidade();

		if (podeInvisibilidade && Input.GetKeyDown(KeyCode.E) && !estaInvisivel)
			AtivarInvisibilidade();
	}

	public void ResetarDoubleJump() { jaUsouDoubleJump = false; }

	public void AtivarDash()
	{
		if (estaADashar || !dashDisponivel) return;
		dashDisponivel = false;
		StartCoroutine(FazerDash());
		if (dashCooldownUI != null) dashCooldownUI.IniciarCooldown(cooldownDash + tempoDash);
	}

	private IEnumerator FazerDash()
	{
		estaADashar = true;
		dashDisponivel = false;

		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		Vector3 forward = Camera.main.transform.forward;
		Vector3 right = Camera.main.transform.right;
		forward.y = 0; right.y = 0;
		forward.Normalize(); right.Normalize();
		Vector3 direcao = (forward * input.z + right * input.x).normalized;
		if (direcao == Vector3.zero) direcao = transform.forward;

		float tempoDecorrido = 0f;
		while (tempoDecorrido < tempoDash)
		{
			rb.linearVelocity = direcao * forcaDash;
			tempoDecorrido += Time.deltaTime;
			yield return null;
		}
		estaADashar = false;
		rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
		yield return new WaitForSeconds(cooldownDash);
		dashDisponivel = true;
	}

	public void AtivarInvencibilidade()
	{
		if (estaInvencivel || !invencibilidadeDisponivel) return;
		invencibilidadeDisponivel = false;
		StartCoroutine(InvencivelTemporariamente());
		if (invencibilidadeCooldownUI != null)
			invencibilidadeCooldownUI.IniciarCooldown(cooldownInvencibilidade + duracaoInvencivel);
	}

	private IEnumerator InvencivelTemporariamente()
	{
		estaInvencivel = true;
		float tempoPassado = 0f;
		while (tempoPassado < duracaoInvencivel)
		{
			if (meuRenderer != null && materialNormal != null && materialDano != null)
			{
				meuRenderer.material = materialDano;
				yield return new WaitForSeconds(0.1f);
				meuRenderer.material = materialNormal;
				yield return new WaitForSeconds(0.1f);
			}
			tempoPassado += 0.2f;
		}
		estaInvencivel = false;
		yield return new WaitForSeconds(cooldownInvencibilidade);
		invencibilidadeDisponivel = true;
	}

	public void AtivarInvisibilidade()
	{
		if (estaInvisivel || !invisibilidadeDisponivel) return;
		invisibilidadeDisponivel = false;
		StartCoroutine(InvisivelTemporariamente());
		if (invisibilidadeCooldownUI != null)
			invisibilidadeCooldownUI.IniciarCooldown(cooldownInvisibilidade + duracaoInvisivel);
	}

	private IEnumerator InvisivelTemporariamente()
	{
		estaInvisivel = true;
		if (meuRenderer != null && materialTransparente != null)
			meuRenderer.material = materialTransparente;

		yield return new WaitForSeconds(duracaoInvisivel);

		if (meuRenderer != null && materialNormal != null)
			meuRenderer.material = materialNormal;

		estaInvisivel = false;
		yield return new WaitForSeconds(cooldownInvisibilidade);
		invisibilidadeDisponivel = true;
	}

	public IEnumerator InvencivelPorDano()
	{
		estaInvencivelDano = true;
		float tempoPassado = 0f;
		while (tempoPassado < duracaoInvencivelDano)
		{
			if (meuRenderer != null && materialNormal != null && materialDano != null)
			{
				meuRenderer.material = materialDano;
				yield return new WaitForSeconds(0.1f);
				meuRenderer.material = materialNormal;
				yield return new WaitForSeconds(0.1f);
			}
			tempoPassado += 0.2f;
		}
		estaInvencivelDano = false;
	}

	public bool EstaInvencivelPorDano() => estaInvencivelDano;

	// === MÉTODOS GETTERS para PlayerAtaque ===
	public bool PodeDash() => podeDash;
	public bool PodeInvencibilidade() => podeInvencibilidade;
	public bool PodeInvisibilidade() => podeInvisibilidade;
	public bool EstaADashar() => estaADashar;
	public bool EstaInvisivel() => estaInvisivel;
	public bool EstaInvencivel() => estaInvencivel;

	// SETTERS opcionais para atualizar dinamicamente
	public void SetDash(bool estado) => podeDash = estado;
	public void SetInvisibilidade(bool estado) => podeInvisibilidade = estado;
	public void SetInvencibilidade(bool estado) => podeInvencibilidade = estado;
	public void SetDoubleJump(bool estado) => podeDoubleJump = estado;
	public bool PodeDoubleJump() => podeDoubleJump;
}

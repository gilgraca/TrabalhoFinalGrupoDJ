using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovimentoSpyro : MonoBehaviour
{
	[Header("Movimento")]
	[SerializeField] public float velocidade = 5f;
	[SerializeField] public float forcaSalto = 7f;
	[SerializeField] public int saltosMaximos = 1;

	private int saltosDisponiveis;
	private bool estaNoChao;
	private bool jaSaltou = false;
	private bool podeVerificarChao = true;

	[Header("Referências")]
	public Transform pontoChao;
	[SerializeField] public float distanciaRaycast = 0.2f;
	private Rigidbody rb;
	[SerializeField] private Animator animator;

	private float inputX;
	private float inputZ;

	[Header("Áudio de Passos")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] sonsPassos;       // Genérico
	[SerializeField] private AudioClip[] passosGrama;
	[SerializeField] private AudioClip[] passosMadeira;
	[SerializeField] private AudioClip[] passosPedra;
	[SerializeField] private float intervaloPassos = 0.5f;

	[Header("Áudio de Ações")]
	[SerializeField] private AudioClip somDePulo;

	private float tempoProximoPasso = 0f;
	private string tagDoChao = "";

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		saltosDisponiveis = saltosMaximos;
	}

	void Update()
	{
		inputX = Input.GetAxis("Horizontal");
		inputZ = Input.GetAxis("Vertical");

		// === DETECÇÃO DO CHÃO COM RAYCAST E MATERIAL ===
		if (podeVerificarChao)
		{
			RaycastHit hit;
			if (Physics.Raycast(pontoChao.position, Vector3.down, out hit, distanciaRaycast))
			{
				estaNoChao = hit.collider.CompareTag("Ground");

				if (estaNoChao)
				{
					Renderer rend = hit.collider.GetComponent<Renderer>();
					if (rend != null)
					{
						string nomeMaterial = rend.material.name.ToLower();

						if (nomeMaterial.Contains("grama"))
							tagDoChao = "Ground_Grama";
						else if (nomeMaterial.Contains("madeira"))
							tagDoChao = "Ground_Madeira";
						else if (nomeMaterial.Contains("pedra"))
							tagDoChao = "Ground_Pedra";
						else
							tagDoChao = "Ground";
					}
				}
			}
			else
			{
				estaNoChao = false;
				tagDoChao = "";
			}
		}
		else
		{
			estaNoChao = false;
			tagDoChao = "";
		}

		// Saltos disponíveis
		PlayerPowerUp powerUp = GetComponent<PlayerPowerUp>();
		if (estaNoChao && !jaSaltou)
		{
			saltosDisponiveis = (powerUp != null && powerUp.PodeDoubleJump()) ? 2 : 1;
		}

		// Salto
		if (Input.GetKeyDown(KeyCode.Space) && saltosDisponiveis > 0)
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

			if (powerUp != null && powerUp.PodeDoubleJump() && saltosDisponiveis == 1)
			{
				rb.AddForce(Vector3.up * powerUp.forcaSaltoExtra, ForceMode.Impulse);
			}
			else
			{
				rb.AddForce(Vector3.up * forcaSalto, ForceMode.Impulse);
			}

			// === TOCA O SOM DO PULO ===
			if (somDePulo != null && audioSource != null)
			{
				audioSource.PlayOneShot(somDePulo);
			}

			saltosDisponiveis--;
			jaSaltou = true;
			podeVerificarChao = false;
			Invoke("AtivarDeteccaoChao", 0.2f);
		}

		// Pulo variável
		if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, rb.linearVelocity.z);
		}

		if (estaNoChao)
		{
			jaSaltou = false;
		}

		float velocidadeHorizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

		animator.SetFloat("speed", velocidadeHorizontal);
		animator.SetFloat("verticalspeed", rb.linearVelocity.y);
		animator.SetBool("estaNoChao", estaNoChao);

		// === SOM DOS PASSOS ===
		if (estaNoChao && velocidadeHorizontal > 0.1f)
		{
			if (Time.time >= tempoProximoPasso)
			{
				TocarSomDePasso();
				tempoProximoPasso = Time.time + intervaloPassos;
			}
		}
		else
		{
			tempoProximoPasso = Time.time;
		}
	}

	void FixedUpdate()
	{
		Vector3 forwardCam = Camera.main.transform.forward;
		Vector3 rightCam = Camera.main.transform.right;
		forwardCam.y = 0f;
		rightCam.y = 0f;
		forwardCam.Normalize();
		rightCam.Normalize();

		Vector3 direcaoDesejada = forwardCam * inputZ + rightCam * inputX;
		Vector3 movimento = direcaoDesejada.normalized * velocidade;

		PlayerPowerUp powerUp = GetComponent<PlayerPowerUp>();
		if (powerUp == null || !powerUp.EstaADashar())
		{
			rb.linearVelocity = new Vector3(movimento.x, rb.linearVelocity.y, movimento.z);
		}

		if (movimento != Vector3.zero)
		{
			Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoDesejada);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 10f);
		}
	}

	public bool EstaNoChao()
	{
		return estaNoChao;
	}

	void AtivarDeteccaoChao()
	{
		podeVerificarChao = true;
	}

	void TocarSomDePasso()
	{
		AudioClip[] clips = null;

		switch (tagDoChao)
		{
			case "Ground_Grama":
				clips = passosGrama;
				break;
			case "Ground_Madeira":
				clips = passosMadeira;
				break;
			case "Ground_Pedra":
				clips = passosPedra;
				break;
			default:
				clips = sonsPassos; // Som genérico
				break;
		}

		if (clips == null || clips.Length == 0 || audioSource == null) return;

		int index = Random.Range(0, clips.Length);
		audioSource.PlayOneShot(clips[index]);
	}

	private void OnDrawGizmosSelected()
	{
		if (pontoChao != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(pontoChao.position, Vector3.down * distanciaRaycast);
		}
	}
}

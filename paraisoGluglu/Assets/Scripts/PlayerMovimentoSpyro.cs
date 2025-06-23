// Script responsável por gerir o movimento do jogador (sem dash nem double jump)
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovimentoSpyro : MonoBehaviour
{
    // === VARIÁVEIS DE MOVIMENTO ===

    // Velocidade de deslocamento do jogador
    [SerializeField] public float velocidade = 5f;

    // Força aplicada no salto
    [SerializeField] public float forcaSalto = 7f;

    // Número máximo de saltos (apenas 1 salto por agora)
    [SerializeField] public int saltosMaximos = 1;

    // Saltos restantes disponíveis
    private int saltosDisponiveis;

    // Se o jogador está no chão ou não
    private bool estaNoChao;

    // Controla se já saltou depois de tocar no chão
    private bool jaSaltou = false;

    // Controla se pode verificar o chão com raycast
    private bool podeVerificarChao = true;

    // Objeto vazio que marca onde verificar o chão
    public Transform pontoChao;

    // Distância do raycast para verificar o chão
    [SerializeField] public float distanciaRaycast = 0.2f;

    // Referência ao Rigidbody
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    // Inputs guardados para usar na física
    private float inputX;
    private float inputZ;

    void Start()
    {
        // Vai buscar o componente Rigidbody
        rb = GetComponent<Rigidbody>();

        // Inicia com o número máximo de saltos
        saltosDisponiveis = saltosMaximos;
    }

    void Update()
    {// === MOVIMENTO E ROTAÇÃO DO JOGADOR ===

        // Captura o input horizontal (X) e vertical (Z)
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        // Detecção de chão com Raycast
        if (podeVerificarChao)
        {
            RaycastHit hit;
            if (Physics.Raycast(pontoChao.position, Vector3.down, out hit, distanciaRaycast))
            {
                // Verifica se o objeto tocado tem a Tag "Ground" e mete na consola
                estaNoChao = hit.collider.CompareTag("Ground");

                //Debug.Log("Player bateu no: " + hit.collider.name);

            }
            else
            {
                estaNoChao = false;
            }
        }
        else
        {
            estaNoChao = false;
        }
        // Reinicia o double jump se tiver o script
        PlayerPowerUp powerUp = GetComponent<PlayerPowerUp>();
        if (powerUp != null)
        {
        }
        // Se estiver no chão e não tiver saltado, define os saltos com base no power-up
        if (estaNoChao && !jaSaltou)
        {
            if (powerUp != null && powerUp.PodeDoubleJump())
            {
                saltosDisponiveis = 2; // 1 salto + 1 extra
            }
            else
            {
                saltosDisponiveis = 1; // Apenas salto base
            }
        }

        // Salto (se ainda houver saltos disponíveis)
        if (Input.GetKeyDown(KeyCode.Space) && saltosDisponiveis > 0)
        {
            // Anula a velocidade vertical antes de aplicar força de salto
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * forcaSalto, ForceMode.Impulse);
            saltosDisponiveis--;
            jaSaltou = true;
            // Impede a deteção de chão temporariamente
            podeVerificarChao = false;
            Invoke("AtivarDeteccaoChao", 0.2f);

            //Debug.Log("Saltos disponíveis: " + saltosDisponiveis);

        }
        // Pulo variável (se largar a tecla a meio do salto, corta altura)
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, rb.linearVelocity.z);
        }
        // Se voltou ao chão, pode saltar de novo
        if (estaNoChao)
        {
            jaSaltou = false;
        }
        // Calcula a velocidade horizontal total (sem contar com o Y)
        // Isto assegura que o valor é sempre positivo e correto para animações de andar/parar
        float velocidadeHorizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

        // Define o valor da "speed" no Animator com base na velocidade horizontal total
        animator.SetFloat("speed", velocidadeHorizontal);

        // Define também a velocidade vertical (caso tenhas animações para saltar/cair)
        animator.SetFloat("verticalspeed", rb.linearVelocity.y);

        // Define se o jogador está no chão (útil para evitar bugs de salto)
        animator.SetBool("estaNoChao", estaNoChao);
    }

    void FixedUpdate()
    {
        // Obtém a direção da câmara (sem inclinação vertical)
        Vector3 forwardCam = Camera.main.transform.forward;
        Vector3 rightCam = Camera.main.transform.right;

        // Remove componente vertical (para não andar no ar)
        forwardCam.y = 0f;
        rightCam.y = 0f;

        // Normaliza as direções para manter velocidade constante
        forwardCam.Normalize();
        rightCam.Normalize();

        // Movimento relativo à câmara
        Vector3 direcaoDesejada = forwardCam * inputZ + rightCam * inputX;
        Vector3 movimento = direcaoDesejada.normalized * velocidade;

        // Aplica o movimento à velocidade do rigidbody (mantém a velocidade vertical atual)
        // Só aplica movimento normal se não estiver a fazer dash
        PlayerPowerUp powerUp = GetComponent<PlayerPowerUp>();
        if (powerUp == null || !powerUp.EstaADashar())
        {
            // Aplica o movimento à velocidade do Rigidbody
            rb.linearVelocity = new Vector3(movimento.x, rb.linearVelocity.y, movimento.z);

            // Log para confirmar que está a aplicar o movimento normal
            Debug.Log("Movimento normal aplicado: " + movimento);
        }
        else
        {
            // Log para confirmar que o dash está ativo e ignora o movimento normal
            Debug.Log("A ignorar movimento normal porque está a dashar.");
        }

        // Se houver movimento (input diferente de zero), roda o jogador suavemente nessa direção
        if (movimento != Vector3.zero)
        {
            // Direção alvo para onde o jogador deve olhar
            Vector3 direcaoAlvo = direcaoDesejada;

            // Calcula a rotação alvo com base na direção
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoAlvo);

            // Suaviza a rotação atual para a rotação alvo
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 10f); // 10f é a velocidade da rotação, ajusta se quiseres

            // Log para ver a rotação a cada frame
            //Debug.Log("Rotação suavizada para: " + transform.rotation.eulerAngles);
        }
    }
    // Retorna se o jogador está ou não no chão (para outros scripts)
    public bool EstaNoChao()
    {
        return estaNoChao;
    }

    // Reativa a verificação de chão após o salto
    void AtivarDeteccaoChao()
    {
        podeVerificarChao = true;
    }

    // Gizmo para mostrar o raycast de chão no editor
    private void OnDrawGizmosSelected()
    {
        if (pontoChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(pontoChao.position, Vector3.down * distanciaRaycast);
        }
    }
}

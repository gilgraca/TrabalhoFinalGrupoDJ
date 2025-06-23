// Script responsável por gerir o movimento do jogador (sem dash nem double jump)
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovimentoCrash : MonoBehaviour
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

    void Start()
    {
        // Vai buscar o componente Rigidbody
        rb = GetComponent<Rigidbody>();

        // Inicia com o número máximo de saltos
        saltosDisponiveis = saltosMaximos;
    }

    void FixedUpdate()
    {
        if (MenuPausa.jogoPausado) return;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 movimento = new Vector3(inputX, 0f, inputZ).normalized * velocidade;
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
            Vector3 direcaoAlvo = new Vector3(movimento.x, 0f, movimento.z);

            // Calcula a rotação alvo com base na direção
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoAlvo);

            // Suaviza a rotação atual para a rotação alvo
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.fixedDeltaTime * 10f); // 10f é a velocidade da rotação, ajusta se quiseres

            // Log para ver a rotação a cada frame
            //Debug.Log("Rotação suavizada para: " + transform.rotation.eulerAngles);
        }
    }

    void Update()
    {// === MOVIMENTO E ROTAÇÃO DO JOGADOR ===
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
        // Se carregar na tecla espaço e ainda tiver saltos disponíveis
        if (Input.GetKeyDown(KeyCode.Space) && saltosDisponiveis > 0)
        {
            // Anula a velocidade vertical atual (para salto mais consistente)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // Se tiver o power-up ativo e estiver no segundo salto (ou seja, no ar)
            if (powerUp != null && powerUp.PodeDoubleJump() && saltosDisponiveis == 1)
            {
                // Aplica o salto extra com a força vinda do power-up
                rb.AddForce(Vector3.up * powerUp.forcaSaltoExtra, ForceMode.Impulse);

                //Debug.Log("DOUBLE JUMP Crash com força: " + powerUp.GetForcaSaltoExtra());
            }
            else
            {
                // Aplica o salto normal
                rb.AddForce(Vector3.up * forcaSalto, ForceMode.Impulse);

                //Debug.Log("SALTO BASE Crash com força: " + forcaSalto);
            }

            // Reduz o número de saltos restantes
            saltosDisponiveis--;

            // Marca que já saltou
            jaSaltou = true;

            // Impede a deteção de chão temporariamente
            podeVerificarChao = false;
            Invoke("AtivarDeteccaoChao", 0.2f);
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

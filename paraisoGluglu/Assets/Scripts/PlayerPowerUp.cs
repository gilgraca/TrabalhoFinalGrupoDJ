using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    // Rigidbody do player
    private Rigidbody rb;
    // DOUBLE JUMP
    // Se o jogador pode usar o salto duplo
    [SerializeField] private bool podeDoubleJump = false;
    // Se já usou o salto extra nesta sequência de saltos
    private bool jaUsouDoubleJump = false;
    // Força aplicada no salto extra
    [SerializeField] private float forcaSaltoExtra = 6f;

    // Referência ao PlayerMovimento (para saber se está no chão)
    private PlayerMovimentoCrash playerMovimento;

    // DASH
    [SerializeField] private bool podeDash = false;
    // Força do dash
    [SerializeField] private float forcaDash = 12f;
    // Tempo que dura o dash
    [SerializeField] private float tempoDash = 0.2f;
    // Cooldown do dash
    [SerializeField] private float cooldownDash = 1f;
    // Se está a fazer dash ou não
    private bool estaADashar = false;
    // Se o dash está disponível ou não
    private bool dashDisponivel = true;

    // INVENCIBILIDADE
    [SerializeField] private bool podeInvencibilidade = false;
    private bool estaInvencivel = false;
    [SerializeField] private float duracaoInvencivel = 2f;
    // Referência ao renderer (para efeito visual tipo piscar)
    private Renderer meuRenderer;
    // Cooldown da invencibilidade
    [SerializeField] private float cooldownInvencibilidade = 1f;
    // Se a invencibilidade está dispontoponível ou não
    private bool invencibilidadeDisponivel = true;

    // INVISIBILIDADE
    [SerializeField] private bool podeInvisibilidade = false;
    private bool estaInvisivel = false;
    [SerializeField] private float duracaoInvisivel = 5f;
    // Cooldown do invisibilidade
    [SerializeField] private float cooldownInvisibilidade = 1f;
    // Se a invisibilidade está disponível ou não
    private bool invisibilidadeDisponivel = true;

    // Referência ao material para ajustar transparência
    private Color corOriginal;
    [SerializeField]
    private Animator animator;

    // Referências visuais dos cooldowns
    [SerializeField] private PowerUpCooldownUI dashCooldownUI;
    [SerializeField] private PowerUpCooldownUI invisibilidadeCooldownUI;
    [SerializeField] private PowerUpCooldownUI invencibilidadeCooldownUI;

    // Material normal (visível)
    [SerializeField] private Material materialNormal;
    // Material transparente (invisível)
    [SerializeField] private Material materialTransparente;

    // Material de dano (para piscar durante invencibilidade)
    [SerializeField] private Material materialDano;

    // --- INVENCIBILIDADE DE DANO ---
    // Ativada automaticamente ao levar dano
    private bool estaInvencivelDano = false; // Estado atual
    [SerializeField] private float duracaoInvencivelDano = 1.5f; // Tempo da invencibilidade após levar dano

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meuRenderer = GetComponentInChildren<Renderer>();
        playerMovimento = GetComponent<PlayerMovimentoCrash>();

        // Carrega os estados dos power-ups guardados no GameManager
        podeDash = GameManager.Instance.usarDash;
        podeDoubleJump = GameManager.Instance.usarDoubleJump;
        podeInvencibilidade = GameManager.Instance.usarInvencibilidade;
        podeInvisibilidade = GameManager.Instance.usarInvisibilidade;
    }

    // Update is called once per frame
    void Update()
    {
        // DOUBLE JUMP com tecla espaço (se permitido e ainda não usado)
        if (Input.GetKeyDown(KeyCode.Space) && podeDoubleJump && !jaUsouDoubleJump)
        {
            // Só ativa se o jogador NÃO está no chão
            if (playerMovimento != null && !playerMovimento.EstaNoChao())
            {
                // Aplica impulso vertical com força personalizada
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * forcaSaltoExtra, ForceMode.Impulse);

                jaUsouDoubleJump = true;

                //Debug.Log("DOUBLE JUMP ativado!");
            }
        }
        // DASH (Shift)
        // DASH (Shift) — só se o jogador tiver o power-up e estiver disponível
        if (podeDash && Input.GetKeyDown(KeyCode.LeftShift) && dashDisponivel && !estaADashar)
        {
            AtivarDash();
        }

        // Pressiona Q para ativar invencibilidade temporária
        if (podeInvencibilidade && Input.GetKeyDown(KeyCode.Q) && !estaInvencivel)
        {
            AtivarInvencibilidade();
        }
        // Pressiona E para ativar invisibilidade temporária
        if (podeInvisibilidade && Input.GetKeyDown(KeyCode.E) && !estaInvisivel)
        {
            AtivarInvisibilidade();
        }
    }

    public void ResetarDoubleJump()
    {
        jaUsouDoubleJump = false;
    }
    public void AtivarDash()
    {
        // Verifica se já está a dashar ou se está em cooldown
        if (estaADashar || !dashDisponivel)
        {
            //Debug.Log("Dash indisponível");
            return;
        }

        // Marca como indisponível e começa o dash
        dashDisponivel = false;
        StartCoroutine(FazerDash());

        // Cooldown visual do dash
        if (dashCooldownUI != null)
            dashCooldownUI.IniciarCooldown(cooldownDash + tempoDash);

    }
    // Dash temporário com cooldown
    private System.Collections.IEnumerator FazerDash()
    {
        // Marca que o jogador está a fazer dash e bloqueia o próximo até ao cooldown
        estaADashar = true;
        dashDisponivel = false;

        //Debug.Log("Dash dado");

        // Determina a direção do dash com base no input atual (Horizontal/Vertical)
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        // Obtém a direção da câmara
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Remove o componente vertical
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Direção baseada na câmara
        Vector3 direcao = (forward * input.z + right * input.x).normalized;

        // Se não houver input (jogador parado), dasha para a frente
        if (direcao == Vector3.zero)
            direcao = transform.forward;
        float tempoDecorrido = 0f;
        // Enquanto o tempo do dash não acabar, move o jogador continuamente
        while (tempoDecorrido < tempoDash)
        {
            // Aplica velocidade constante na direção do dash
            rb.linearVelocity = direcao * forcaDash;
            // Acumula o tempo que passou
            tempoDecorrido += Time.deltaTime;
            // Espera um frame antes de repetir (loop por tempoDash)
            yield return null;
        }
        // Marca que terminou o dash
        estaADashar = false;
        // Retoma o movimento vertical (mantém a gravidade e velocidade no eixo Y)
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        // Espera o tempo de cooldown antes de permitir outro dash
        yield return new WaitForSeconds(cooldownDash);
        dashDisponivel = true;

        //Debug.Log("Dash disponível");
    }
    public void AtivarInvencibilidade()
    {
        // Verifica se já está invencível ou se está em cooldown
        if (estaInvencivel || !invencibilidadeDisponivel)
        {
            //Debug.Log("Não é possível ativar invencibilidade agora.");
            return;
        }

        // Marca como indisponível
        invencibilidadeDisponivel = false;

        // Inicia o efeito
        StartCoroutine(InvencivelTemporariamente());

        // Cooldown visual da invencibilidade
        if (invencibilidadeCooldownUI != null)
            invencibilidadeCooldownUI.IniciarCooldown(cooldownInvencibilidade + duracaoInvencivel);

    }
    private IEnumerator InvencivelTemporariamente()
    {
        // Marca o jogador como invencível
        estaInvencivel = true;

        //Debug.Log("INVENCIBILIDADE ATIVADA — começa a piscar entre materiais");

        float tempoPassado = 0f;

        // Enquanto a duração da invencibilidade não for atingida
        while (tempoPassado < duracaoInvencivel)
        {
            // Se o renderer e os dois materiais forem válidos
            if (meuRenderer != null && materialNormal != null && materialDano != null)
            {
                // Troca para o material de dano
                meuRenderer.material = materialDano;
                // Espera 0.1 segundos
                yield return new WaitForSeconds(0.1f);

                // Troca de volta para o material normal
                meuRenderer.material = materialNormal;
                // Espera mais 0.1 segundos
                yield return new WaitForSeconds(0.1f);
            }

            // Soma 0.2 segundos ao tempo total
            tempoPassado += 0.2f;
        }

        // Termina o estado de invencibilidade
        estaInvencivel = false;

        //Debug.Log("INVENCIBILIDADE TERMINOU — material normal restaurado");

        // Espera cooldown para poder usar outra vez
        yield return new WaitForSeconds(cooldownInvencibilidade);
        invencibilidadeDisponivel = true;

        //Debug.Log("Invencibilidade disponível novamente");
    }

    public bool EstaInvencivel()
    {
        return estaInvencivel;
    }
    public void AtivarInvisibilidade()
    {
        // Verifica se já está invisível ou se está em cooldown
        if (estaInvisivel || !invisibilidadeDisponivel)
        {
            //Debug.Log("Não é possível ativar invisibilidade agora.");
            return;
        }

        // Marca como indisponível
        invisibilidadeDisponivel = false;

        // Inicia o efeito
        StartCoroutine(InvisivelTemporariamente());

        // Cooldown visual da invisibilidade
        if (invisibilidadeCooldownUI != null)
            invisibilidadeCooldownUI.IniciarCooldown(cooldownInvisibilidade + duracaoInvisivel);

    }

    // Coroutine que ativa a invisibilidade temporária
    private IEnumerator InvisivelTemporariamente()
    {
        // Marca o estado como invisível
        estaInvisivel = true;

        // Troca o material para o transparente
        if (meuRenderer != null && materialTransparente != null)
        {
            meuRenderer.material = materialTransparente;
        }

        //Debug.Log("INVISIBILIDADE ATIVADA — material transparente aplicado");

        // Espera a duração da invisibilidade
        yield return new WaitForSeconds(duracaoInvisivel);

        // Volta ao material normal
        if (meuRenderer != null && materialNormal != null)
        {
            meuRenderer.material = materialNormal;
        }

        //Debug.Log("INVISIBILIDADE TERMINADA — material normal restaurado");

        // Marca como visível novamente
        estaInvisivel = false;

        // Espera cooldown para permitir nova invisibilidade
        yield return new WaitForSeconds(cooldownInvisibilidade);
        invisibilidadeDisponivel = true;

        //Debug.Log("Invisibilidade disponível novamente");
    }

    public bool EstaInvisivel()
    {
        return estaInvisivel;
    }
    // Ativa ou desativa o DOUBLE JUMP
    public void SetDoubleJump(bool estado)
    {
        podeDoubleJump = estado;
    }
    // Ativa ou desativa o DASH
    public void SetDash(bool estado)
    {
        podeDash = estado;
    }
    // Ativa ou desativa a INVENCIBILIDADE
    public void SetInvencibilidade(bool estado)
    {
        podeInvencibilidade = estado;
    }
    // Ativa ou desativa a INVISIBILIDADE
    public void SetInvisibilidade(bool estado)
    {
        podeInvisibilidade = estado;
    }
    // Devolve se o jogador pode usar o double jump
    public bool PodeDoubleJump()
    {
        return podeDoubleJump;
    }

    // Coroutine que ativa a invencibilidade ao levar dano
    public IEnumerator InvencivelPorDano()
    {
        estaInvencivelDano = true;

        // Debug para testes
        //Debug.Log("Jogador está invencível devido a dano!");

        float tempoPassado = 0f;

        // Enquanto durar, pisca
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

        // Debug
        //Debug.Log("Invencibilidade de dano terminou.");
    }

    // Método auxiliar para verificar se está invencível por dano
    public bool EstaInvencivelPorDano()
    {
        return estaInvencivelDano;
    }

}

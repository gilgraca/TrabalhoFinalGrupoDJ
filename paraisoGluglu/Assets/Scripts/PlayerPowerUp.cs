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
    private PlayerMovimento playerMovimento;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meuRenderer = GetComponentInChildren<Renderer>();
        playerMovimento = GetComponent<PlayerMovimento>();

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

                Debug.Log("DOUBLE JUMP ativado!");
            }
        }
        // DASH (Shift)
        // DASH (Shift) — só se o jogador tiver o power-up e estiver disponível
        if (podeDash && Input.GetKeyDown(KeyCode.LeftShift) && dashDisponivel && !estaADashar)
        {
            AtivarDash();
        }

        // Pressiona X para ativar invencibilidade temporária
        if (podeInvencibilidade && Input.GetKeyDown(KeyCode.X) && !estaInvencivel)
        {
            AtivarInvencibilidade();
        }
        // Pressiona Z para ativar invisibilidade temporária
        if (podeInvisibilidade && Input.GetKeyDown(KeyCode.Z) && !estaInvisivel)
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
    }
    // Dash temporário com cooldown
    private System.Collections.IEnumerator FazerDash()
    {
        // Marca que o jogador está a fazer dash e bloqueia o próximo até ao cooldown
        estaADashar = true;
        dashDisponivel = false;

        //Debug.Log("Dash dado");

        // Determina a direção do dash com base no input atual (Horizontal/Vertical)
        Vector3 direcao = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
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
    }
    private IEnumerator InvencivelTemporariamente()
    {
        estaInvencivel = true;
        //Debug.Log("INVENCIBILIDADE ATIVADA");

        // Piscar como efeito visual (opcional)
        float tempoPassado = 0f;
        while (tempoPassado < duracaoInvencivel)
        {
            if (meuRenderer != null)
            {
                meuRenderer.enabled = false; // invisível
                yield return new WaitForSeconds(0.1f);
                meuRenderer.enabled = true;  // visível
                yield return new WaitForSeconds(0.1f);
            }

            tempoPassado += 0.2f;
        }

        estaInvencivel = false;
        //Debug.Log("INVENCIBILIDADE TERMINOU");

        // Espera o tempo de cooldown antes de permitir outro invencibilidade
        yield return new WaitForSeconds(cooldownInvencibilidade);
        invencibilidadeDisponivel = true;

        //Debug.Log("Invencibilidade disponível");
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
    }

    private IEnumerator InvisivelTemporariamente()
    {
        estaInvisivel = true;
        //Debug.Log("INVISIBILIDADE ATIVADA");
        // Referência ao material do renderer
        Material mat = meuRenderer.material;
        // === MODO TRANSPARENTE ===
        mat.SetFloat("_Mode", 2);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        // Torna a cor com alpha reduzido
        Color corTransparente = mat.color;
        corTransparente.a = 0.5f;
        mat.color = corTransparente;
        // Espera o tempo de invisibilidade
        yield return new WaitForSeconds(duracaoInvisivel);
        // === VOLTA AO MODO OPACO ===
        mat.SetFloat("_Mode", 0);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = -1;
        // Volta à cor original com alpha 1
        Color corOriginal = mat.color;
        corOriginal.a = 1f;
        mat.color = corOriginal;
        estaInvisivel = false;
        //Debug.Log("Invisibilidade terminou");
        // Espera o tempo de cooldown antes de permitir outro invisibilidade
        yield return new WaitForSeconds(cooldownInvisibilidade);
        invisibilidadeDisponivel = true;

        //Debug.Log("Invisibilidade disponível");
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
}

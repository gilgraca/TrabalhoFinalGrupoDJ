using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Rigidbody do player
    private Rigidbody rb;

    // MOVIMENTAÇÃO
    // Velocidade
    [SerializeField] public float velocidade;

    // SALTO
    // Força do salto
    [SerializeField] public float forcaSalto;
    // Número máximo de saltos
    // Melhorar no futuro para permitir um ou duplo salto consoante o escolhido no tutorial
    [SerializeField] public int saltosMaximos;
    // Saltos disponíveis no momento
    private int saltosDisponiveis;
    // Se está no chão ou não
    private bool estaNoChao;
    // Se já saltou desde que tocou no chão
    private bool jaSaltou = false;
    // Controla se pode verificar o chão (para evitar deteção logo após o salto)
    private bool podeVerificarChao = true;
    // Objeto auxiliar (vazio) colocado debaixo do jogador para lançar o Raycast
    public Transform pontoChao;
    // Distância do Raycast para detetar o chão
    [SerializeField] public float distanciaRaycast;

    // DASH
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

    //ATAQUE
    // PREFAB da pena
    [SerializeField] private GameObject prefabPena;
    // Posição de onde sai a pena
    [SerializeField] private Transform pontoDisparo;
    // PONTOS EXTRA PARA ATAQUE ESPECIAL
    [SerializeField] private Transform disparo1;
    [SerializeField] private Transform disparo2;
    // Cooldown do ataque especial
    [SerializeField] private float cooldownAtaqueEspecial = 1f;
    // Se o ataque especial está disponível ou não
    private bool ataqueEspecialDisponivel = true;

    // INVENCIBILIDADE
    [SerializeField] private float duracaoInvencivel = 2f; // duração em segundos
    private bool estaInvencivel = false;
    // Referência ao renderer (para efeito visual tipo piscar)
    private Renderer meuRenderer;
    // Cooldown da invencibilidade
    [SerializeField] private float cooldownInvencibilidade = 1f;
    // Se a invencibilidade está disponível ou não
    private bool invencibilidadeDisponivel = true;

    // VIDA
    [SerializeField] private int vidaMaxima = 3;
    private int vidaAtual;

    // INVISIBILIDADE
    [SerializeField] private float duracaoInvisivel = 5f;
    private bool estaInvisivel = false;
    // Cooldown do invisibilidade
    [SerializeField] private float cooldownInvisibilidade = 1f;
    // Se a invisibilidade está disponível ou não
    private bool invisibilidadeDisponivel = true;

    // Referência ao material para ajustar transparência
    private Color corOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        saltosDisponiveis = saltosMaximos;
        meuRenderer = GetComponentInChildren<Renderer>();
        vidaAtual = vidaMaxima;
    }

    void Update()
    {
        // Movimentação X e Z do player
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        // Aplica movimento
        Vector3 movimento = new Vector3(inputX, 0f, inputZ) * velocidade;
        rb.linearVelocity = new Vector3(movimento.x, rb.linearVelocity.y, movimento.z);
        // Detecção de chão com Raycast
        if (podeVerificarChao)
        {
            RaycastHit hit;
            if (Physics.Raycast(pontoChao.position, Vector3.down, out hit, distanciaRaycast))
            {
                // Verifica se o objeto tocado tem a Tag "Ground" e mete na consola
                estaNoChao = hit.collider.CompareTag("Ground");

                // Debug.Log("Player bateu no: " + hit.collider.name);

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
        // Se estiver no chão e ainda não saltou, reseta os saltos
        if (estaNoChao && !jaSaltou)
        {
            saltosDisponiveis = saltosMaximos;
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

            // Debug.Log("Saltos disponíveis: " + saltosDisponiveis);

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
        // DASH (Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashDisponivel && !estaADashar)
        {
            StartCoroutine(FazerDash());
        }
        // Disparo da pena com botão esquerdo do rato
        if (Input.GetMouseButtonDown(0)) // Botão esquerdo
        {
            // Instancia a pena na posição do ponto de disparo, com a rotação do jogador
            GameObject novaPena = Instantiate(prefabPena, pontoDisparo.position, Quaternion.identity);

            // Calcula a direção para a frente do jogador
            Vector3 direcaoDisparo = transform.forward;

            // Atribui a direção à pena
            novaPena.GetComponent<Pena>().DefinirDirecao(direcaoDisparo);

            Debug.Log("Pena disparada com botão esquerdo!");
        }
        // Ataque especial (botão direito) com cooldown
        if (Input.GetMouseButtonDown(1) && ataqueEspecialDisponivel)
        {
            // Bloqueia o ataque especial até ao fim do cooldown
            ataqueEspecialDisponivel = false;

            Vector3 direcao = transform.forward;

            // Dispara penas das 3 posições
            CriarPena(disparo1.position, direcao);
            CriarPena(pontoDisparo.position, direcao);
            CriarPena(disparo2.position, direcao);

            Debug.Log("Ataque especial com 3 penas disparado!");

            // Inicia o cooldown do ataque especial
            StartCoroutine(ReporCooldownAtaqueEspecial());
        }
        // Pressiona G para ativar invencibilidade temporária
        if (Input.GetKeyDown(KeyCode.G))
        {
            AtivarInvencibilidade();
        }
        if (Input.GetKeyDown(KeyCode.T)) // tecla T ativa invisibilidade
        {
            AtivarInvisibilidade();
        }
    }

    // Ativa a verificação do chão novamente após o atraso
    void AtivarDeteccaoChao()
    {
        podeVerificarChao = true;
    }

    // Mostra uma linha vermelha no editor indicando o Raycast para o chão
    private void OnDrawGizmosSelected()
    {
        if (pontoChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(pontoChao.position, Vector3.down * distanciaRaycast);
        }
    }

    // Dash temporário com cooldown
    private System.Collections.IEnumerator FazerDash()
    {
        // Marca que o jogador está a fazer dash e bloqueia o próximo até ao cooldown
        estaADashar = true;
        dashDisponivel = false;

        Debug.Log("Dash dado");

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

        Debug.Log("Dash disponível");
    }
    // Corrotina para gerir o cooldown do ataque especial
    private IEnumerator ReporCooldownAtaqueEspecial()
    {
        // Espera o tempo definido no cooldown
        yield return new WaitForSeconds(cooldownAtaqueEspecial);

        // Ativa novamente o ataque especial
        ataqueEspecialDisponivel = true;
        Debug.Log("Ataque especial disponível novamente!");
    }

    // Cria uma pena com direção específica// Cria uma pena a partir de uma posição e direção específicas
    private void CriarPena(Vector3 posicao, Vector3 direcao)
    {
        GameObject novaPena = Instantiate(prefabPena, posicao, Quaternion.identity);

        Pena penaScript = novaPena.GetComponent<Pena>();
        if (penaScript != null)
        {
            penaScript.DefinirDirecao(direcao);
        }
    }
    public void AtivarInvencibilidade()
    {
        // Verifica se já está invencível ou se está em cooldown
        if (estaInvencivel || !invencibilidadeDisponivel)
        {
            Debug.Log("Não é possível ativar invencibilidade agora.");
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
        Debug.Log("INVENCIBILIDADE ATIVADA");

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
        Debug.Log("INVENCIBILIDADE TERMINOU");

        // Espera o tempo de cooldown antes de permitir outro invencibilidade
        yield return new WaitForSeconds(cooldownInvencibilidade);
        invencibilidadeDisponivel = true;

        Debug.Log("Invencibilidade disponível");
    }
    public bool EstaInvencivel()
    {
        return estaInvencivel;
    }
    public void LevarDano(int dano)
    {
        if (estaInvencivel)
        {
            Debug.Log("Jogador é invencível. Dano ignorado.");
            return;
        }

        // Reduz vida
        vidaAtual -= dano;
        Debug.Log("Jogador levou " + dano + " de dano. Vida atual: " + vidaAtual);

        // Ativa invencibilidade temporária para não levar dano em loop
        AtivarInvencibilidade();

        // Verifica se morreu
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }
    private void Morrer()
    {
        Debug.Log("JOGADOR MORREU!");
        gameObject.SetActive(false);
    }
    public void AtivarInvisibilidade()
    {
        // Verifica se já está invisível ou se está em cooldown
        if (estaInvisivel || !invisibilidadeDisponivel)
        {
            Debug.Log("Não é possível ativar invisibilidade agora.");
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
        Debug.Log("INVISIBILIDADE ATIVADA");
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
        Debug.Log("Invisibilidade terminou");
        // Espera o tempo de cooldown antes de permitir outro invisibilidade
        yield return new WaitForSeconds(cooldownInvisibilidade);
        invisibilidadeDisponivel = true;

        Debug.Log("Invisibilidade disponível");
    }

    public bool EstaInvisivel()
    {
        return estaInvisivel;
    }
}

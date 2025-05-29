// Cria e gere os ataques do jogador
using System.Collections;
using UnityEngine;

public class PlayerAtaque : MonoBehaviour
{
    // PREFAB da pena
    [SerializeField] private GameObject prefabPena;

    // Posição do disparo normal
    [SerializeField] private Transform pontoDisparo;

    // Disparos laterais para o ataque especial
    [SerializeField] private Transform disparo1;
    [SerializeField] private Transform disparo2;

    // Cooldown do ataque especial
    [SerializeField] private float cooldownAtaqueEspecial = 1f;

    // Flag se está disponível ou não
    private bool ataqueEspecialDisponivel = true;
    // Se o jogador tem o power-up para usar ataque especial
    [SerializeField] private bool podeAtaqueEspecial = false;

    // Animações
    [SerializeField] private Animator animator;

    [SerializeField] private PowerUpCooldownUI ataqueEspecialCooldownUI;
    // Tempo de cooldown do ataque básico
    [SerializeField] private float cooldownAtaqueBasico = 0.5f;

    // Flag que indica se o ataque básico pode ser usado
    private bool ataqueBasicoDisponivel = true;
    [SerializeField] private PowerUpCooldownUI ataqueNormalCooldownUI;

    void Start()
    {
        // Carrega o estado do ataque especial guardado no GameManager
        podeAtaqueEspecial = GameManager.Instance.usarAtaqueEspecial;
    }

    void Update()
    {
        // Impede ataques se o jogo estiver pausado
        if (MenuPausa.jogoPausado) return;

        // Disparo normal com botão esquerdo
        if (Input.GetMouseButtonDown(0) && ataqueBasicoDisponivel)
        {
            // Marca como indisponível
            ataqueBasicoDisponivel = false;

            // Cria a pena na posição base
            GameObject novaPena = Instantiate(prefabPena, pontoDisparo.position, Quaternion.identity);

            // Direção à frente
            Vector3 direcaoDisparo = transform.forward;

            // Define a direção
            novaPena.GetComponent<PenaAtaque>().DefinirDirecao(direcaoDisparo);

            // Animação
            animator.SetBool("isAttacking", true);
            StartCoroutine(DesativarBool("isAttacking", 0.5f));

            // Inicia cooldown visual
            if (ataqueNormalCooldownUI != null)
                ataqueNormalCooldownUI.IniciarCooldown(cooldownAtaqueBasico);

            // Inicia cooldown funcional
            StartCoroutine(ReporCooldownAtaqueBasico());
        }

        // Disparo especial com botão direito
        if (podeAtaqueEspecial && Input.GetMouseButtonDown(1) && ataqueEspecialDisponivel)

        {
            AtivarAtaqueEspecial(); // Usa novo sistema com cooldown
        }
    }

    // Corrotina para repor o cooldown do ataque básico
    private IEnumerator ReporCooldownAtaqueBasico()
    {
        // Espera o tempo definido
        yield return new WaitForSeconds(cooldownAtaqueBasico);

        // Torna o ataque básico novamente disponível
        ataqueBasicoDisponivel = true;

        // Log para testes
        //Debug.Log("Ataque básico disponível novamente!");
    }


    // Método para ativar o ataque especial
    private void AtivarAtaqueEspecial()
    {
        // Se não está disponível, sai
        if (!ataqueEspecialDisponivel) return;

        // Marca como indisponível
        ataqueEspecialDisponivel = false;

        // Direção do disparo
        Vector3 direcao = transform.forward;

        // Disparo em 3 posições
        CriarPena(disparo1.position, direcao);
        CriarPena(pontoDisparo.position, direcao);
        CriarPena(disparo2.position, direcao);

        // Animação
        animator.SetBool("isSpAttack", true);
        // Desativa a animação após um tempo
        StartCoroutine(DesativarBool("isSpAttack", 0.5f));

        // Inicia o cooldown
        StartCoroutine(ReporCooldownAtaqueEspecial());

        // Cooldown visual do ataque especial
        if (ataqueEspecialCooldownUI != null)
            ataqueEspecialCooldownUI.IniciarCooldown(cooldownAtaqueEspecial);

    }

    // Desativa o bool após um tempo
    private IEnumerator DesativarBool(string nomeBool, float tempo)
    {
        yield return new WaitForSeconds(tempo);
        animator.SetBool(nomeBool, false);
    }


    // Corrotina para o cooldown
    private IEnumerator ReporCooldownAtaqueEspecial()
    {
        // Espera o tempo
        yield return new WaitForSeconds(cooldownAtaqueEspecial);

        // Ativa de novo
        ataqueEspecialDisponivel = true;
        //Debug.Log("Ataque especial disponível novamente!");
    }

    // Cria uma pena a partir de uma posição e direção
    private void CriarPena(Vector3 posicao, Vector3 direcao)
    {
        GameObject novaPena = Instantiate(prefabPena, posicao, Quaternion.identity);
        PenaAtaque penaScript = novaPena.GetComponent<PenaAtaque>();
        if (penaScript != null)
        {
            penaScript.DefinirDirecao(direcao);
        }
    }

    // Interação física com caixas
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Caixa"))
        {
            Vector3 normal = collision.contacts[0].normal;

            if (Vector3.Dot(normal, Vector3.up) > 0.5f)
            {
                DestruirCaixa caixaScript = collision.collider.GetComponent<DestruirCaixa>();

                if (caixaScript != null)
                {
                    caixaScript.QuebrarCaixa();
                }
            }
        }
    }
    // Permite ativar ou desativar o ataque especial via menu
    public void SetAtaqueEspecial(bool estado)
    {
        podeAtaqueEspecial = estado;
    }

}

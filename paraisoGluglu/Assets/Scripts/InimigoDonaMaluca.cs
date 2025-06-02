// Script responsável por perseguir e atacar o jogador (sem patrulha)
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class InimigoDonaMaluca : MonoBehaviour
{

    // Distância mínima para atacar o jogador
    public float distanciaAtaque = 1.5f;

    // Velocidade com que o inimigo persegue o jogador
    public float velocidade = 3f;

    // Tempo entre ataques (cooldown)
    public float tempoEntreAtaques = 2f;

    // Tempo que falta para poder atacar novamente
    private float tempoProximoAtaque = 0f;

    // Referência ao jogador
    private Transform jogador;

    // Referência aos power-ups do jogador (para verificar invisibilidade)
    private PlayerPowerUp jogadorPowerUps;

    // Componente de movimentação
    private CharacterController controller;

    private Animator animator;

    void Start()
    {
        // Obter o componente CharacterController deste inimigo
        controller = GetComponent<CharacterController>();

        // Encontrar o objeto do jogador com a tag "Player"
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            // Guardar a posição do jogador
            jogador = objJogador.transform;

            // Obter o script de power-ups do jogador
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
        }
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Se não há jogador ou não tem o script de power-ups, não faz nada
        if (jogador == null || jogadorPowerUps == null) return;

        // Se o jogador está invisível, o inimigo ignora
        if (jogadorPowerUps.EstaInvisivel()) return;


        PerseguirJogador();
        Debug.Log("Tentou mover");

        // Medir a distância entre o inimigo e o jogador
        float distancia = Vector3.Distance(transform.position, jogador.position);

        // Se está dentro da distância de ataque e o tempo permite, ataca
        if (distancia <= distanciaAtaque && Time.time >= tempoProximoAtaque)
        {
            AtacarJogador();
            // Atualiza o tempo do próximo ataque
            tempoProximoAtaque = Time.time + tempoEntreAtaques;
        }
    }

    void PerseguirJogador()
    {
        //LOG para confirmar que entrou
        Debug.Log("Está a tentar perseguir o jogador");

        // Calcula a direção até ao jogador
        Vector3 direcao = (jogador.position - transform.position).normalized;

        // Garante que o movimento é apenas horizontal
        direcao.y = 0f;

        // Move o inimigo na direção do jogador
        controller.Move(direcao * velocidade * Time.deltaTime);

        // Roda o inimigo para olhar na direção do jogador
        Quaternion rotacao = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5f);
    }

    void AtacarJogador()
    {
        // Verifica se o jogador ainda existe
        if (jogador == null) return;

        // Ativa o trigger de ataque na animação
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        // Acede aos componentes necessários
        PlayerVida vida = jogador.GetComponent<PlayerVida>();
        PlayerPowerUp powerUps = jogador.GetComponent<PlayerPowerUp>();

        if (powerUps != null && (powerUps.EstaInvencivel() || powerUps.EstaInvencivelPorDano()))
        {
            Debug.Log("Jogador dentro do alcance, mas invencível.");
            return;
        }

        if (vida != null)
        {
            vida.LevarDano(1);
            Debug.Log("Jogador levou dano do ataque do inimigo.");
        }

        Rigidbody rb = jogador.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direcao = (jogador.position - transform.position).normalized;
            rb.AddForce(direcao * 5f, ForceMode.Impulse);
            Debug.Log("Empurrão aplicado pelo ataque à distância.");
        }
    }
}

// Script responsável por perseguir o jogador com Rigidbody
using UnityEngine;

// Garante que há sempre um Rigidbody no GameObject
[RequireComponent(typeof(Rigidbody))]
public class InimigoTerrestreAtaque : MonoBehaviour
{
    // Distância mínima para começar a perseguir
    public float distanciaDetecao = 5f;

    // Velocidade de perseguição
    public float velocidade = 3f;

    // Referência ao transform do jogador
    private Transform jogador;

    // Referência ao script de power-ups do jogador
    private PlayerPowerUp jogadorPowerUps;

    // Referência ao Rigidbody do inimigo
    private Rigidbody rb;

    [SerializeField] private Animator animator;


    // Método chamado no início
    void Start()
    {
        // Obtemos o Rigidbody
        rb = GetComponent<Rigidbody>();

        // Procuramos o jogador pela tag
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            // Guardamos a referência ao transform do jogador
            jogador = objJogador.transform;

            // E ao componente de power ups
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();

            // Log de sucesso
            //Debug.Log("Jogador encontrado com sucesso.");
        }
        else
        {
            // Log de erro
            //Debug.LogError("Jogador com tag 'Player' não encontrado!");
        }
    }

    // Usado para lógica de física (chamado de forma fixa)
    void FixedUpdate()
    {
        // Se o jogador não existir, não faz nada
        if (jogador == null) return;

        // Se o jogador está invisível, o inimigo ignora
        if (jogadorPowerUps != null && jogadorPowerUps.EstaInvisivel())
        {
            //Debug.Log("Jogador está invisível. Inimigo não reage.");
            return;
        }

        // Calcula a distância ao jogador
        float distancia = Vector3.Distance(transform.position, jogador.position);

        // Log de distância para debug
        //Debug.Log("Distância ao jogador: " + distancia);

        // Se estiver dentro da zona de deteção, persegue
        if (distancia <= distanciaDetecao)
        {
            PerseguirJogador(distancia);
        }

    }

    // Método que move o inimigo em direção ao jogador
    void PerseguirJogador(float distancia)
    {
        // Calcula a direção horizontal para o jogador
        Vector3 direcao = (jogador.position - transform.position).normalized;
        direcao.y = 0f;

        // Aplica a velocidade de perseguição ao Rigidbody
        rb.linearVelocity = direcao * velocidade;

        // Log para ver o que foi aplicado
        //Debug.Log("Rigidbody.linearVelocity aplicado: " + rb.linearVelocity);

            // Direção horizontal do inimigo para o jogador
            Vector3 lookDirection = jogador.position - transform.position;
            lookDirection.y = 0f;


            // Rotação desejada para olhar para o jogador
            Quaternion rotacaoAlvo = Quaternion.LookRotation(lookDirection);


            // Roda o modelo 180 graus no Y para compensar o rabo virado
            transform.rotation = rotacaoAlvo * Quaternion.Euler(0f, 180f, 0f);

            // Log de controlo
            //Debug.Log("ModeloVisual rodado com compensação de 180°.");


            // Log para confirmar
            //Debug.Log("ModeloVisual rodado instantaneamente para o jogador.");

            if(distancia< 6f) if (animator) animator.SetTrigger("Attack");



    }
}

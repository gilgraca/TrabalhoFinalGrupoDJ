// Importa o namespace de navegação
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class InimigoDonaMaluca : MonoBehaviour
{
    public float distanciaAtaque = 1.5f;
    public float tempoEntreAtaques = 2f;

    private float tempoProximoAtaque = 0f;
    private Transform jogador;
    private PlayerPowerUp jogadorPowerUps;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        // Obter o componente de navegação
        agent = GetComponent<NavMeshAgent>();

        // Procurar o jogador
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
        }

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (jogador == null || jogadorPowerUps == null) return;
        if (jogadorPowerUps.EstaInvisivel()) return;

        //LOG para confirmar que está a usar o NavMesh
        Debug.Log("A usar NavMesh para perseguir o jogador");

        // Define o destino como a posição do jogador
        agent.SetDestination(jogador.position);

        float distancia = Vector3.Distance(transform.position, jogador.position);
        if (distancia <= distanciaAtaque && Time.time >= tempoProximoAtaque)
        {
            AtacarJogador();
            tempoProximoAtaque = Time.time + tempoEntreAtaques;
        }
    }

    void AtacarJogador()
    {
        if (jogador == null) return;

        if (animator != null) animator.SetTrigger("attack");

        PlayerVida vida = jogador.GetComponent<PlayerVida>();
        PlayerPowerUp powerUps = jogador.GetComponent<PlayerPowerUp>();

        if (powerUps != null && (powerUps.EstaInvencivel() || powerUps.EstaInvencivelPorDano()))
        {
            Debug.Log("Jogador invencível.");
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
            Debug.Log("Empurrão aplicado.");
        }
    }
}

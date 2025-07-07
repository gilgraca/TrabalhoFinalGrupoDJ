// Bibliotecas necessárias
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class InimigoTalhante : MonoBehaviour
{
    // Referência ao jogador
    private Transform jogador;

    // Prefab do machado
    [Header("Ataque")]
    public GameObject prefabMachado; // Machado que será instanciado
    public Transform pontoDisparo;   // Ponto onde o machado é lançado
    public float intervaloMachado = 3f; // Intervalo entre lançamentos (editável no Inspector)

    private float timerMachado; // Timer para controlar intervalo de disparo

    // NavMesh para seguir o jogador
    private NavMeshAgent agent;

    // Estados e timers
    [Header("Comportamento")]
    public float tempoPerseguir = 5f; // Tempo que persegue o jogador
    public float tempoParado = 3f;    // Tempo que fica parado antes de voltar a perseguir

    private float timerPerseguir;
    private float timerParado;
    private bool estaParado;

    // Controla se levou dano
    private bool levouDano;

    private Animator animator;

    void Start()
    {

        animator = GetComponent<Animator>();

        // Procura o jogador por tag
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
            jogador = objJogador.transform;

        // Inicia o NavMesh
        agent = GetComponent<NavMeshAgent>();

        // === DEBUG NAVMESH ===
        // Mostra se o componente foi encontrado
        Debug.Log("[TALHANTE] NavMeshAgent encontrado? " + (agent != null));

        // Verifica se o talhante está sobre a NavMesh
        if (agent != null)
        {
            Debug.Log("[TALHANTE] Está na NavMesh? " + agent.isOnNavMesh);

            if (!agent.isOnNavMesh)
            {
                Debug.LogWarning("[TALHANTE] Fora da NavMesh. A tentar recolocar...");

                // Tenta encontrar posição próxima válida na NavMesh
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                {
                    Debug.Log("[TALHANTE] Encontrada posição próxima na NavMesh: " + hit.position);
                    agent.Warp(hit.position);
                }
                else
                {
                    Debug.LogError("[TALHANTE] Não encontrou nenhuma posição válida na NavMesh.");
                }
            }
        }

        // Define os timers iniciais
        timerPerseguir = tempoPerseguir;
        timerMachado = intervaloMachado;
    }

    void Update()
    {
        if (jogador == null) return;

        // Só conta o tempo do machado se não estiver parado
        if (!estaParado)
        {
            // Decrementa o timer do machado
            timerMachado -= Time.deltaTime;

            // Se o tempo chegou a zero, atira o machado
            if (timerMachado <= 0f)
            {
                AtirarMachado();
                timerMachado = intervaloMachado;
            }
        }

        // === CONTROLO DE ESTADOS ===
        if (estaParado)
        {
            timerParado -= Time.deltaTime;

            if (timerParado <= 0 || levouDano)
            {
                estaParado = false;
                levouDano = false;
                timerPerseguir = tempoPerseguir;
            }

            return; // ainda parado, não persegue
        }

        // Persegue o jogador
        agent.SetDestination(jogador.position);

        // Reduz o tempo de perseguição
        timerPerseguir -= Time.deltaTime;

        if (timerPerseguir <= 0)
        {
            estaParado = true;
            timerParado = tempoParado;
            agent.ResetPath();
        }
    }

    // Método para lançar o machado
    private void AtirarMachado()
    {
        animator.SetTrigger("Attack");
        // Calcula a direção do machado para o jogador
        Vector3 direcao = (jogador.position - pontoDisparo.position).normalized;

        // Cria a rotação para olhar para o jogador
        Quaternion rotacao = Quaternion.LookRotation(direcao);
        // Instancia o machado na orientação do ponto de disparo
        Instantiate(prefabMachado, pontoDisparo.position, rotacao);

        //Debug.Log("Machado instanciado.");
    }


    // Método chamado quando o inimigo leva dano
    public void LevarDano()
    {
        if (estaParado)
        {
            levouDano = true;
        }
    }
}

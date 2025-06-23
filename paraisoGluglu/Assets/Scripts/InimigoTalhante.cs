using UnityEngine;
using UnityEngine.AI;

public class InimigoTalhante : MonoBehaviour
{
    // Referência ao jogador
    private Transform jogador;

    // Prefab do machado
    [Header("Ataque")]
    public GameObject prefabMachado; // Machado que será instanciado
    public Transform pontoDisparo;   // Ponto onde o machado é lançado
    public float intervaloMachado = 3f; // Intervalo entre lançamentos (editável no Inspector)

    // Controlo do tempo de disparo
    private float tempoProximoMachado;

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

    void Start()
    {
        // Procura o jogador por tag
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
            jogador = objJogador.transform;

        // Inicia o NavMesh
        agent = GetComponent<NavMeshAgent>();

        // Define o tempo inicial
        timerPerseguir = tempoPerseguir;
        tempoProximoMachado = Time.time;
    }

    void Update()
    {
        // Se não tiver jogador, não faz nada
        if (jogador == null) return;

        // Se estiver parado...
        if (estaParado)
        {
            timerParado -= Time.deltaTime;
            //LOG para testes
            Debug.Log("Talhante está parado. Tempo restante: " + timerParado);

            // Quando tempo parado acabar OU levou dano, volta a perseguir
            if (timerParado <= 0 || levouDano)
            {
                estaParado = false;
                levouDano = false;
                timerPerseguir = tempoPerseguir;
                Debug.Log("Talhante voltou a perseguir!");
            }

            // Não faz mais nada enquanto está parado
            return;
        }

        // Perseguir jogador
        agent.SetDestination(jogador.position);

        //LOG para testes
        Debug.Log("Talhante a seguir jogador. Tempo restante: " + timerPerseguir);

        // Reduz tempo de perseguição
        timerPerseguir -= Time.deltaTime;

        // Se tempo acabar, pára
        if (timerPerseguir <= 0)
        {
            estaParado = true;
            timerParado = tempoParado;
            agent.ResetPath();
            Debug.Log("Talhante parou!");
        }

        // Disparo de machado
        if (Time.time >= tempoProximoMachado)
        {
            AtirarMachado();
            tempoProximoMachado = Time.time + intervaloMachado;
        }
    }

    // Método para atirar o machado
    void AtirarMachado()
    {
        if (prefabMachado == null || pontoDisparo == null) return;

        // Instancia o machado e dá-lhe força
        GameObject machado = Instantiate(prefabMachado, pontoDisparo.position, Quaternion.identity);

        // Calcula direção para o jogador
        Vector3 direcao = (jogador.position - pontoDisparo.position).normalized;

        // Adiciona força ao machado
        Rigidbody rb = machado.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(direcao * 10f, ForceMode.Impulse); // Podes ajustar esta força

        //LOG para testes
        Debug.Log("Machado lançado!");
    }

    // Este método deve ser chamado quando o inimigo leva dano
    public void LevarDano()
    {
        //LOG para testes
        Debug.Log("Talhante levou dano!");

        // Se estava parado, força a voltar a perseguir
        if (estaParado)
        {
            levouDano = true;
        }
    }
}

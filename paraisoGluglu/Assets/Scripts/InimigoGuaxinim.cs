using UnityEngine;

public class InimigoGuaxinim : MonoBehaviour
{
    public Transform jogador;
    public Transform pontoEspera;
    public float velocidade = 3f;
    public float velocidadeAtaque = 6f;
    public float distanciaAtaque = 5f;
    public float velocidadeRotacao = 5f;
    private enum Estado { Esperar, Atacar, Voltar }
    private Estado estadoAtual = Estado.Esperar;
    private bool podeAtacar = true;

    private GameObject zonaAtaque;
    private Vector3 destinoAtaque;

    void Start()
    {
        zonaAtaque = transform.Find("ZonaDano")?.gameObject;

        if (zonaAtaque != null)
            zonaAtaque.SetActive(false);
        else
        //Debug.LogWarning("[Guaxinim] ZonaDano (zona de ataque) não encontrada!");

        if (pontoEspera == null)
        {
            //Debug.LogWarning("[Guaxinim] pontoDeEspera (atribuído no Inspetor) está nulo!"); 
        }
    }

    void Update()
    {
        if (jogador == null || pontoEspera == null)
        {
            //Debug.LogWarning("[Guaxinim] Jogador ou ponto de espera nulo!");
            return;
        }

        float distanciaJogador = Vector3.Distance(transform.position, jogador.position);
        //Debug.Log($"[Guaxinim] Distância até ao jogador: {distanciaJogador:F2}");
        //Debug.Log($"[Guaxinim] Estado atual: {estadoAtual}");

        switch (estadoAtual)
        {
            case Estado.Esperar:
                //Debug.Log("[Guaxinim] No estado: ESPERAR");

                if (distanciaJogador <= distanciaAtaque && podeAtacar)
                {
                    //Debug.Log("[Guaxinim] Jogador dentro da distância. Vai ATACAR!");
                    estadoAtual = Estado.Atacar;

                    // Guarda posição do jogador mas mantém o Y do guaxinim para não subir
                    destinoAtaque = new Vector3(
                        jogador.position.x,
                        transform.position.y,
                        jogador.position.z
                    );
                }
                break;

            case Estado.Atacar:
                //Debug.Log("[Guaxinim] No estado: ATACAR");

                if (zonaAtaque != null && !zonaAtaque.activeInHierarchy)
                {
                    zonaAtaque.SetActive(true);
                    //Debug.Log("[Guaxinim] Zona de ataque ATIVADA");
                }

                Vector3 direcaoJogador = (destinoAtaque - transform.position).normalized;
                transform.forward = new Vector3(direcaoJogador.x, 0, direcaoJogador.z);
                transform.position = Vector3.MoveTowards(transform.position, destinoAtaque, velocidadeAtaque * Time.deltaTime);

                float distDestino = Vector3.Distance(transform.position, destinoAtaque);
                if (distDestino <= 0.1f)
                {
                    //Debug.Log("[Guaxinim] Ataque concluído. Vai VOLTAR!");
                    estadoAtual = Estado.Voltar;
                    podeAtacar = false;
                }
                break;

            case Estado.Voltar:
                //Debug.Log("[Guaxinim] No estado: VOLTAR");

                if (zonaAtaque != null && zonaAtaque.activeInHierarchy)
                {
                    zonaAtaque.SetActive(false);
                    //Debug.Log("[Guaxinim] Zona de ataque DESATIVADA");
                }

                Vector3 direcaoEspera = (pontoEspera.position - transform.position).normalized;

                // Rotação suave
                if (direcaoEspera != Vector3.zero)
                {
                    Quaternion rotacaoAlvo = Quaternion.LookRotation(new Vector3(direcaoEspera.x, 0, direcaoEspera.z));
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotacaoAlvo, velocidadeRotacao * Time.deltaTime);
                }

                transform.position = Vector3.MoveTowards(transform.position, pontoEspera.position, velocidade * Time.deltaTime);

                float distPonto = Vector3.Distance(transform.position, pontoEspera.position);
                //Debug.Log($"[Guaxinim] Distância até ponto de espera: {distPonto:F2}");

                if (distPonto < 0.1f)
                {
                    //Debug.Log("[Guaxinim] Chegou ao ponto de espera. Vai ESPERAR novamente.");
                    estadoAtual = Estado.Esperar;
                    podeAtacar = true;
                }
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (pontoEspera != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pontoEspera.position, 0.5f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}

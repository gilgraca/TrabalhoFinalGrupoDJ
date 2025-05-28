using System.Collections;
using UnityEngine;

public class InimigoAereo : MonoBehaviour
{
    // Centro do movimento circular no ar
    private Vector3 centroPatrulha;
    // Altura a que patrulha normalmente
    public float alturaPatrulha = 5f;
    // Raio do movimento circular (horizontal)
    public float raioPatrulha = 3f;
    // Velocidade do movimento circular
    public float velocidadeRotacao = 1f;
    // Distância de deteção no chão (raio horizontal)
    public float raioDetecao = 2f;
    // Tempo entre ataques
    public float cooldownAtaque = 5f;
    // Altura a que o inimigo ataca
    public float alturaAtaque = 1.2f;
    // Referência ao jogador
    private Transform jogador;
    // Flag se o inimigo pode atacar
    private bool podeAtacar = true;
    // Flag se está a atacar (para parar o movimento)
    private bool aAtacar = false;
    // Ângulo de rotação do círculo
    private float angulo = 0f;
    // Velocidade da descida/subida do ataque
    public float velocidadeAtaque = 1f;
    void Start()
    {
        // Guarda a posição inicial como centro do círculo
        centroPatrulha = transform.position;

        // Encontra o jogador pela tag
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        // Se está a atacar, não faz patrulha
        if (aAtacar) return;
        // === MOVIMENTO CIRCULAR DE PATRULHA ===
        // Atualiza o ângulo com base na rotação
        angulo += velocidadeRotacao * Time.deltaTime;
        // Calcula a posição circular no plano XZ
        float x = Mathf.Cos(angulo) * raioPatrulha;
        float z = Mathf.Sin(angulo) * raioPatrulha;
        // Cria a nova posição de patrulha baseada no centro
        Vector3 novaPosicao = new Vector3(centroPatrulha.x + x, centroPatrulha.y, centroPatrulha.z + z);
        // Aplica a nova posição
        transform.position = novaPosicao;
        // Calcula a direção tangente ao círculo (para rodar o inimigo)
        Vector3 direcaoMovimento = new Vector3(-Mathf.Sin(angulo), 0f, Mathf.Cos(angulo));
        // Aplica rotação suave com ângulo Z a 90° (deitado)
        if (direcaoMovimento != Vector3.zero)
        {
            Quaternion rotacaoBase = Quaternion.LookRotation(direcaoMovimento);
            Quaternion rotacaoExtraZ = Quaternion.Euler(0f, 0f, 90f);
            Quaternion rotacaoFinal = rotacaoBase * rotacaoExtraZ;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoFinal, 10f * Time.deltaTime);
        }
        // === DETEÇÃO E ATAQUE ===
        // Verifica se o jogador está invisível
        Player p = jogador.GetComponent<Player>();
        if (p != null && p.EstaInvisivel())
        {
            // Se estiver invisível, não ataca — mas continua a patrulhar
            return;
        }
        // Se pode atacar, verifica distância horizontal ao jogador
        if (podeAtacar)
        {
            // Calcula distância 2D entre o inimigo e o jogador (plano XZ)
            Vector2 inimigoXZ = new Vector2(transform.position.x, transform.position.z);
            Vector2 jogadorXZ = new Vector2(jogador.position.x, jogador.position.z);
            float distancia = Vector2.Distance(inimigoXZ, jogadorXZ);

            // Se estiver dentro do raio de deteção, inicia ataque
            if (distancia <= raioDetecao)
            {
                //Debug.Log("Jogador detetado! A atacar.");
                StartCoroutine(Atacar(jogador.position));
            }
        }
    }
    IEnumerator Atacar(Vector3 posJogador)
    {
        // Marca que está a atacar
        aAtacar = true;
        podeAtacar = false;
        // Posição alvo é onde o jogador estava no momento da deteção
        Vector3 destino = new Vector3(posJogador.x, alturaAtaque, posJogador.z);
        Vector3 partida = transform.position;
        float tempo = 0f;
        // Desce suavemente até ao ponto do jogador
        while (tempo < 1f)
        {
            transform.position = Vector3.Lerp(partida, destino, tempo);
            tempo += Time.deltaTime * velocidadeAtaque;

            yield return null;
        }
        //Debug.Log("Ataque concluído!");
        // Sobe imediatamente de volta à patrulha
        tempo = 0f;
        while (tempo < 1f)
        {
            transform.position = Vector3.Lerp(destino, partida, tempo);
            tempo += Time.deltaTime * velocidadeAtaque;
            yield return null;
        }
        // Marca que terminou o ataque e retoma patrulha
        aAtacar = false;
        // Espera o cooldown antes de poder atacar novamente
        //Debug.Log("Cooldown iniciado...");
        yield return new WaitForSeconds(cooldownAtaque);
        podeAtacar = true;
        //Debug.Log("Pode atacar novamente.");
    }
}

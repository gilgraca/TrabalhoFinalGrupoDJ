using System.Collections;
using UnityEngine;

public class InimigoTerrestre : MonoBehaviour
{
    // INIMIGO
    // Define os estados possíveis do inimigo
    private enum EstadoInimigo { Patrulhar, Perseguir }
    // Estado atual do inimigo (começa a patrulhar)
    private EstadoInimigo estadoAtual = EstadoInimigo.Patrulhar;
    // Lista de pontos de patrulha no cenário
    public Transform[] pontosPatrulha;
    private int indiceAtual = 0;
    // Velocidade
    public float velocidade = 3f;
    // Distância para detectar o jogador
    public float alcanceVisao = 5f;
    // Se está à espera num ponto de patrulha
    private bool aEsperarNoPonto = false;
    // Se está à espera após perder o jogador
    private bool aVoltarAPatrulhar = false;
    // Guarda o estado anterior para logs
    private EstadoInimigo estadoAnterior;
    // Vida do inimigo (podes ajustar o valor inicial)
    [SerializeField] private int vida = 3;

    // PLAYER
    // Referência ao jogador
    private Transform jogador;

    void Start()
    {
        // Encontra o jogador pela tag
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Se está à espera para voltar a patrulhar, ignora tudo
        if (aVoltarAPatrulhar)
            return;

        // Verifica se o jogador está invisível
        Player p = jogador.GetComponent<Player>();
        bool jogadorInvisivel = p != null && p.EstaInvisivel();

        // Calcula a distância até ao jogador
        float distanciaJogador = Vector3.Distance(transform.position, jogador.position);

        // Se o jogador está dentro do alcance E NÃO está invisível
        if (distanciaJogador <= alcanceVisao && !jogadorInvisivel)
        {
            // Muda para perseguir
            estadoAtual = EstadoInimigo.Perseguir;
        }
        else
        {
            // Se estava a perseguir mas perdeu o jogador OU ele está invisível, começa a voltar à patrulha
            if (estadoAtual == EstadoInimigo.Perseguir)
            {
                StartCoroutine(EsperarParaVoltarAPatrulhar());
            }
        }

        // Mostra log quando o estado muda
        if (estadoAtual != estadoAnterior)
        {
            //Debug.Log("Estado mudou para: " + estadoAtual);
            estadoAnterior = estadoAtual;
        }

        // Executa o comportamento atual
        switch (estadoAtual)
        {
            case EstadoInimigo.Patrulhar:
                if (!aEsperarNoPonto)
                    Patrulhar();
                break;

            case EstadoInimigo.Perseguir:
                Perseguir();
                break;
        }
    }

    void Patrulhar()
    {
        // Se estiver à espera no ponto, não se move
        if (aEsperarNoPonto)
        {
            //Debug.Log("Inimigo está a esperar antes de mudar de ponto.");
            return;
        }

        // Obtém o ponto atual de patrulha
        Transform alvo = pontosPatrulha[indiceAtual];
        Vector3 direcao = alvo.position - transform.position;
        Vector3 direcaoNormalizada = direcao.normalized;

        // Move-se até ao ponto
        if (direcao.magnitude > 0.1f)
        {
            transform.position += direcaoNormalizada * velocidade * Time.deltaTime;
            //Debug.Log("Inimigo a andar para o ponto " + indiceAtual);

            // Roda suavemente para o ponto
            Quaternion rotacaoDesejada = Quaternion.LookRotation(direcaoNormalizada);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoDesejada, 5f * Time.deltaTime);
            //Debug.Log("Inimigo a rodar para o próximo ponto");
        }

        // Quando chega perto, espera 3 segundos
        if (direcao.magnitude < 0.5f)
        {
            StartCoroutine(EsperarAntesDeMudar());
        }
    }

    void Perseguir()
    {
        // Direção até ao jogador
        Vector3 direcao = jogador.position - transform.position;
        Vector3 direcaoNormalizada = direcao.normalized;

        // Move-se na direção do jogador
        transform.position += direcaoNormalizada * velocidade * Time.deltaTime;
        //Debug.Log("Inimigo a perseguir o jogador");

        // Roda na direção do jogador
        if (direcao.magnitude > 0.1f)
        {
            Quaternion rotacaoDesejada = Quaternion.LookRotation(direcaoNormalizada);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoDesejada, 5f * Time.deltaTime);
            //Debug.Log("Inimigo a rodar para o jogador");
        }
        else
        {
            //Debug.Log("Inimigo muito perto do jogador. Não está a rodar.");
        }
    }

    private IEnumerator EsperarAntesDeMudar()
    {
        aEsperarNoPonto = true;
        //Debug.Log("Inimigo chegou ao ponto " + indiceAtual + ". Vai esperar 3 segundos.");

        yield return new WaitForSeconds(3f);

        // Avança para o próximo ponto
        indiceAtual = (indiceAtual + 1) % pontosPatrulha.Length;
        //Debug.Log("Inimigo mudou para o ponto " + indiceAtual);

        aEsperarNoPonto = false;
    }

    private IEnumerator EsperarParaVoltarAPatrulhar()
    {
        // Já está a esperar? então ignora
        if (aVoltarAPatrulhar)
            yield break;

        aVoltarAPatrulhar = true;
        //Debug.Log("Inimigo perdeu o jogador. Vai voltar a patrulhar em 3 segundos.");

        // Espera 3 segundos
        yield return new WaitForSeconds(3f);

        // Só aqui muda o estado
        estadoAtual = EstadoInimigo.Patrulhar;
        aVoltarAPatrulhar = false;

        //Debug.Log("Inimigo voltou a patrulhar.");
    }

    // Quando leva dano
    public void ReceberDano(int dano)
    {
        vida -= dano;
        //Debug.Log("Inimigo terrestre levou dano. Vida restante: " + vida);

        if (vida <= 0)
        {
            // Aqui podes pôr animação ou efeitos
            Destroy(gameObject);
            //Debug.Log("Inimigo terrestre morreu!");
        }
    }
}

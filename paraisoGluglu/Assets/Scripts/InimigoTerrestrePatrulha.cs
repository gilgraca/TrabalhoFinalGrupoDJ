// Script responsável por patrulha de inimigo terrestre (sem ataque)
using System.Collections;
using UnityEngine;

public class InimigoTerrestrePatrulha : MonoBehaviour
{
    // Estados possíveis do inimigo
    private enum Estado { Patrulhar, Parado }
    private Estado estadoAtual = Estado.Patrulhar;

    // Pontos de patrulha atribuídos manualmente
    public Transform[] pontosPatrulha;
    private int indiceAtual = 0;

    // Tempo de espera entre cada ponto
    public float tempoEspera = 2f;
    private bool aEsperar = false;

    // Velocidade de movimento
    public float velocidade = 2f;

    // Referência ao jogador
    private Transform jogador;
    private PlayerPowerUp jogadorPowerUps;

    void Start()
    {
        // Procurar o jogador
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
        }
    }

    void Update()
    {
        if (estadoAtual == Estado.Patrulhar && !aEsperar)
        {
            Patrulhar();
        }
    }

    void Patrulhar()
    {
        if (pontosPatrulha.Length == 0) return;

        // Move-se em direção ao ponto atual
        Transform destino = pontosPatrulha[indiceAtual];
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidade * Time.deltaTime);

        // Roda para o ponto
        Vector3 direcao = (destino.position - transform.position).normalized;
        if (direcao != Vector3.zero)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5f);
        }

        // Chegou ao ponto?
        if (Vector3.Distance(transform.position, destino.position) < 0.1f)
        {
            StartCoroutine(EsperarAntesDeAvancar());
        }
    }

    IEnumerator EsperarAntesDeAvancar()
    {
        aEsperar = true;
        yield return new WaitForSeconds(tempoEspera);
        indiceAtual = (indiceAtual + 1) % pontosPatrulha.Length;
        aEsperar = false;
    }
}

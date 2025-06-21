// Script responsável por patrulha de inimigo terrestre com Rigidbody
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InimigoTerrestrePatrulha : MonoBehaviour
{
    private enum Estado { Patrulhar, Parado }
    private Estado estadoAtual = Estado.Patrulhar;

    public Transform[] pontosPatrulha;
    private int indiceAtual = 0;

    public float tempoEspera = 2f;
    private bool aEsperar = false;

    public float velocidade = 2f;

    private Transform jogador;
    private PlayerPowerUp jogadorPowerUps;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
        }
    }

    void FixedUpdate()
    {
        if (estadoAtual == Estado.Patrulhar && !aEsperar)
        {
            Patrulhar();
        }
    }

    void Patrulhar()
    {
        if (pontosPatrulha.Length == 0) return;

        Transform destino = pontosPatrulha[indiceAtual];
        Vector3 direcao = (destino.position - transform.position).normalized;
        direcao.y = 0f;

        // Aplica movimento via velocidade no Rigidbody
        rb.linearVelocity = direcao * velocidade;

        // Roda o inimigo para o ponto
        if (direcao != Vector3.zero)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcao) * Quaternion.Euler(0f, 180f,0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, destino.position) < .8f)
        {
            StartCoroutine(EsperarAntesDeAvancar());
        }
    }

    IEnumerator EsperarAntesDeAvancar()
    {
        aEsperar = true;
        rb.linearVelocity = Vector3.zero; // pára o movimento
        yield return new WaitForSeconds(tempoEspera);
        indiceAtual = (indiceAtual + 1) % pontosPatrulha.Length;
        aEsperar = false;
    }
}

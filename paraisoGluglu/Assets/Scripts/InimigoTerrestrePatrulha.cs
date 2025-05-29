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
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
        }

        // LOG para confirmar que o Start correu
        Debug.Log("InimigoTerrestrePatrulha: Start() iniciado. Estado atual: " + estadoAtual);
    }

    void Update()
    {
        // LOG para ver se o Update corre
        Debug.Log("InimigoTerrestrePatrulha: Update() chamado. Estado: " + estadoAtual);

        if (estadoAtual == Estado.Patrulhar && !aEsperar)
        {
            Patrulhar();
        }
    }

    void Patrulhar()
    {
        if (pontosPatrulha.Length == 0)
        {
            Debug.LogWarning("InimigoTerrestrePatrulha: Nenhum ponto de patrulha definido!");
            return;
        }

        Transform destino = pontosPatrulha[indiceAtual];
        Debug.Log("InimigoTerrestrePatrulha: A mover para ponto " + indiceAtual + " - " + destino.name);

        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidade * Time.deltaTime);

        Vector3 direcao = (destino.position - transform.position).normalized;
        if (direcao != Vector3.zero)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, destino.position) < 0.1f)
        {
            Debug.Log("InimigoTerrestrePatrulha: Chegou ao ponto " + indiceAtual);
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

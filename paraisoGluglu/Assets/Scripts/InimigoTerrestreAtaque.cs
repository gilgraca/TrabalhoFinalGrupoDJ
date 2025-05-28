// Script responsável por perseguir e atacar o jogador (sem patrulha)
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class InimigoTerrestreAtaque : MonoBehaviour
{
    // Distância mínima para perseguir
    public float distanciaDetecao = 5f;
    // Velocidade de perseguição
    public float velocidade = 3f;

    private Transform jogador;
    private PlayerPowerUp jogadorPowerUps;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
        }
    }

    void Update()
    {
        if (jogador == null || jogadorPowerUps == null) return;

        // Se o jogador estiver invisível, o inimigo não o vê
        if (jogadorPowerUps.EstaInvisivel()) return;

        float distancia = Vector3.Distance(transform.position, jogador.position);
        if (distancia <= distanciaDetecao)
        {
            PerseguirJogador();
        }
    }

    void PerseguirJogador()
    {
        Vector3 direcao = (jogador.position - transform.position).normalized;
        direcao.y = 0f; // mantém o movimento no plano horizontal
        controller.Move(direcao * velocidade * Time.deltaTime);

        // Rodar para o jogador
        Quaternion rotacao = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5f);
    }
}

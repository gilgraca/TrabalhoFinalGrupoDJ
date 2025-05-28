// Script responsável por ataque do inimigo aéreo
using System.Collections;
using UnityEngine;

public class InimigoAereoAtaque : MonoBehaviour
{
    // Distância horizontal de deteção
    public float raioDetecao = 2f;
    // Tempo entre ataques
    public float cooldownAtaque = 5f;
    // Altura de mergulho do ataque
    public float alturaAtaque = 1.2f;
    // Velocidade da descida e subida
    public float velocidadeAtaque = 1f;

    private Transform jogador;
    private PlayerPowerUp jogadorPowerUps;
    private InimigoAereoPatrulha patrulha;

    private bool podeAtacar = true;

    void Start()
    {
        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
            jogadorPowerUps = objJogador.GetComponent<PlayerPowerUp>();
        }

        // Procura o script de patrulha no mesmo GameObject
        patrulha = GetComponent<InimigoAereoPatrulha>();
    }

    void Update()
    {
        if (jogador == null || jogadorPowerUps == null || !podeAtacar) return;

        if (jogadorPowerUps.EstaInvisivel()) return;

        Vector2 posInimigoXZ = new Vector2(transform.position.x, transform.position.z);
        Vector2 posJogadorXZ = new Vector2(jogador.position.x, jogador.position.z);
        float distancia = Vector2.Distance(posInimigoXZ, posJogadorXZ);

        if (distancia <= raioDetecao)
        {
            StartCoroutine(Atacar(jogador.position));
        }
    }

    IEnumerator Atacar(Vector3 posJogador)
    {
        // Interrompe patrulha
        if (patrulha != null) patrulha.aAtacar = true;

        podeAtacar = false;

        Vector3 partida = transform.position;
        Vector3 destino = new Vector3(posJogador.x, alturaAtaque, posJogador.z);

        float tempo = 0f;
        while (tempo < 1f)
        {
            transform.position = Vector3.Lerp(partida, destino, tempo);
            tempo += Time.deltaTime * velocidadeAtaque;
            yield return null;
        }

        tempo = 0f;
        while (tempo < 1f)
        {
            transform.position = Vector3.Lerp(destino, partida, tempo);
            tempo += Time.deltaTime * velocidadeAtaque;
            yield return null;
        }

        if (patrulha != null) patrulha.aAtacar = false;

        yield return new WaitForSeconds(cooldownAtaque);
        podeAtacar = true;
    }
}

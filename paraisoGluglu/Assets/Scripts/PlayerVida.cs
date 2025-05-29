// Script responsável por gerir a vida do jogador (sem invencibilidade)
using UnityEngine;

public class PlayerVida : MonoBehaviour
{
    // Itens do HUD que representam a vida (ex: corações ou penas)
    [SerializeField] private GameObject[] hp_items;

    // Vida máxima do jogador
    [SerializeField] private int vidaMaxima = 5;
    // Vida atual
    private int vidaAtual;

    // Referência ao script PlayerPowerUps (para ativar invencibilidade)
    private PlayerPowerUp powerUps;


    void Start()
    {
        // Começa com a vida cheia
        vidaAtual = vidaMaxima;

        // Vai buscar o componente Player no mesmo GameObject
        powerUps = GetComponent<PlayerPowerUp>();
    }

    // Método chamado quando o jogador leva dano
    public void LevarDano(int dano)
    {
        // Reduz a vida
        vidaAtual -= dano;

        // Atualiza também o GameManager
        GameManager.Instance.vidaJogador = vidaAtual;

        // Atualiza o HUD
        if (vidaAtual >= 0 && vidaAtual < hp_items.Length)
        {
            hp_items[vidaAtual].SetActive(false);
        }

        //Debug.Log("Jogador levou " + dano + " de dano. Vida atual: " + vidaAtual);

        // Ativa apenas a invencibilidade de dano
        if (powerUps != null && !powerUps.EstaInvencivelPorDano())
        {
            StartCoroutine(powerUps.InvencivelPorDano());
        }

        // Se a vida chegar a 0 ou menos, morre
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    // Método que desativa o jogador quando morre
    private void Morrer()
    {
        gameObject.SetActive(false);
    }

    // Método auxiliar para recuperar a vida atual externamente
    public int VidaAtual()
    {
        return vidaAtual;
    }

    // Método auxiliar para recuperar a vida máxima externamente
    public int VidaMaxima()
    {
        return vidaMaxima;
    }
    // Método para definir a vida atual do jogador a partir de um valor externo
    public void SetVida(int novaVida)
    {
        // Garante que o valor não ultrapassa os limites
        vidaAtual = Mathf.Clamp(novaVida, 0, vidaMaxima);

        // Atualiza o HUD para refletir a nova vida
        for (int i = 0; i < hp_items.Length; i++)
        {
            // Ativa os itens de vida até ao valor atual
            hp_items[i].SetActive(i < vidaAtual);
        }

        //Debug.Log("Vida do jogador definida para: " + vidaAtual);
    }

}

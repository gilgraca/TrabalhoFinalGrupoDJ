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

        // Atualiza o HUD
        if (vidaAtual >= 0 && vidaAtual < hp_items.Length)
        {
            hp_items[vidaAtual].SetActive(false);
        }

        Debug.Log("Jogador levou " + dano + " de dano. Vida atual: " + vidaAtual);

        powerUps?.AtivarInvencibilidade();

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
}

using UnityEngine;

public class AplicarPowerUps : MonoBehaviour
{
    void Start()
    {
        // Encontra o jogador na cena
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");

        // Referências aos scripts do jogador
        PlayerPowerUp powerUp = jogador.GetComponent<PlayerPowerUp>();
        PlayerAtaque ataque = jogador.GetComponent<PlayerAtaque>();

        // Aplica os power-ups com base nas escolhas do menu
        powerUp.SetDoubleJump(MenuPowerUps.usarDoubleJump);
        powerUp.SetDash(MenuPowerUps.usarDash);
        powerUp.SetInvencibilidade(MenuPowerUps.usarInvencibilidade);
        powerUp.SetInvisibilidade(MenuPowerUps.usarInvisibilidade);
        ataque.SetAtaqueEspecial(MenuPowerUps.usarAtaqueEspecial);

        Debug.Log("PowerUps aplicados do menu inicial.");
    }
}

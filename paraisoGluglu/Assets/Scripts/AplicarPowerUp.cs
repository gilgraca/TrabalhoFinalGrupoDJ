using UnityEngine;

public class AplicarPowerUps : MonoBehaviour
{
    void Start()
    {
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");

        PlayerPowerUp powerUp = jogador.GetComponent<PlayerPowerUp>();
        PlayerAtaque ataque = jogador.GetComponent<PlayerAtaque>();

        powerUp.SetDoubleJump(GameManager.Instance.usarDoubleJump);
        powerUp.SetDash(GameManager.Instance.usarDash);
        powerUp.SetInvencibilidade(GameManager.Instance.usarInvencibilidade);
        powerUp.SetInvisibilidade(GameManager.Instance.usarInvisibilidade);
        ataque.SetAtaqueEspecial(GameManager.Instance.usarAtaqueEspecial);

        // Aplicar vida guardada ao PlayerVida
        PlayerVida vida = jogador.GetComponent<PlayerVida>();
        if (vida != null)
        {
            vida.SetVida(GameManager.Instance.vidaJogador);
        }

        // Debug opcional
        Debug.Log("PowerUps e vida aplicados com GameManager.");
    }

}

using UnityEngine;

public class InimigoZonaAtaque : MonoBehaviour
{
    // Dano que causa ao tocar
    [SerializeField] private int dano = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se colidiu com o player
        if (other.CompareTag("Player"))
        {
            PlayerPowerUp powerUps = other.GetComponent<PlayerPowerUp>();
            PlayerVida playerVida = other.GetComponent<PlayerVida>();

            if (powerUps != null && !powerUps.EstaInvencivel())
            {
                playerVida.LevarDano(dano);

                //Debug.Log("Jogador levou dano do focinho/pata!");
            }
            else
            {
                //Debug.Log("Jogador tocado mas está invencível.");
            }
        }
    }
}

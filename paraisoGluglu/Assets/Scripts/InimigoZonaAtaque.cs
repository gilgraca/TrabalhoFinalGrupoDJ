using UnityEngine;

public class InimigoZonaAtaque : MonoBehaviour
{
    // Dano que causa ao tocar
    [SerializeField] private int dano = 1;

    // Força do empurrão (podes ajustar no Inspector)
    [SerializeField] private float forcaPushback = 5f;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se colidiu com o player
        if (other.CompareTag("Player"))
        {
            // Acede ao script de power ups e à vida do jogador
            PlayerPowerUp powerUps = other.GetComponent<PlayerPowerUp>();
            PlayerVida playerVida = other.GetComponent<PlayerVida>();

            // Verifica se o jogador não está invencível
            if (powerUps != null && !powerUps.EstaInvencivel() && !powerUps.EstaInvencivelPorDano())
            {
                // Aplica dano
                playerVida.LevarDano(dano);

                // Empurra o jogador (pushback)
                Rigidbody rb = other.GetComponent<Rigidbody>(); // Acede ao Rigidbody do jogador
                if (rb != null)
                {
                    // Calcula a direção de empurrão (do inimigo para o jogador)
                    Vector3 direcao = (other.transform.position - transform.position).normalized;

                    // Aplica força no Rigidbody
                    rb.AddForce(direcao * forcaPushback, ForceMode.Impulse);

                    //Log para testes
                    Debug.Log("Jogador levou dano + pushback");
                }
            }
            else
            {
                //Log para testes
                Debug.Log("Jogador tocado mas está invencível.");
            }
        }
    }
}

using UnityEngine;

public class ZonaDanoInimigo : MonoBehaviour
{
    // Dano que causa ao tocar
    [SerializeField] private int dano = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se colidiu com o player
        if (other.CompareTag("Player"))
        {
            // Tenta obter o script Player
            Player player = other.GetComponent<Player>();

            if (player != null && !player.EstaInvencivel())
            {
                // Aplica o dano
                player.LevarDano(dano);
                //Debug.Log("Jogador levou dano do focinho/pata!");
            }
            else
            {
                //Debug.Log("Jogador tocado mas está invencível.");
            }
        }
    }
}

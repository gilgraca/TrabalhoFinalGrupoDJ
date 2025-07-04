using UnityEngine;

public class InimigoZonaAtaque : MonoBehaviour
{
    // Dano que causa ao tocar
    [SerializeField] private int dano = 1;

    // Força horizontal do empurrão (ajustável no Inspector)
    [SerializeField] private float forcaPushback = 5f;

    // Esta função é chamada quando o trigger colide com algo
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se colidiu com o jogador
        if (other.CompareTag("Player"))
        {
            // Acede ao script de power-ups e vida do jogador
            PlayerPowerUp powerUps = other.GetComponent<PlayerPowerUp>();
            PlayerVida playerVida = other.GetComponent<PlayerVida>();

            // Verifica se o jogador existe e não está invencível
            if (powerUps != null && !powerUps.EstaInvencivel() && !powerUps.EstaInvencivelPorDano())
            {
                // Aplica dano ao jogador
                if (playerVida != null)
                    playerVida.LevarDano(dano);

                // Acede ao Rigidbody do jogador para aplicar impulso
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // === CALCULAR DIREÇÃO DO PUSHBACK ===

                    // Direção horizontal (sem componente vertical)
                    Vector3 direcao = (other.transform.position - transform.position);
                    direcao.y = 0f; // ignora Y
                    direcao = direcao.normalized;

                    // === CRIAR IMPULSO FINAL ===

                    // Cria o vetor de impulso horizontal + impulso para cima
                    Vector3 impulsoFinal = direcao * forcaPushback + Vector3.up * 2f;

                    // Aplica a força no Rigidbody
                    rb.AddForce(impulsoFinal, ForceMode.Impulse);

                    // Debug para confirmar que aplicou impulso
                    //Debug.Log("Jogador levou dano e foi empurrado.");
                }
            }
            else
            {
                // Debug se estiver invencível
                //Debug.Log("Jogador tocado mas está invencível.");
            }
        }
    }
}

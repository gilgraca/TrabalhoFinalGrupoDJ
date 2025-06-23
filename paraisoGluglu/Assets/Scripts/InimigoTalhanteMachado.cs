using UnityEngine;

public class InimigoTalhanteMachado : MonoBehaviour
{
    // Dano causado ao jogador
    public int dano = 1;

    // Força do empurrão
    public float forcaPushback = 5f;

    // Tempo antes de desaparecer automaticamente
    public float tempoDestruir = 5f;

    void Start()
    {
        // Destroi automaticamente após X segundos
        Destroy(gameObject, tempoDestruir);

        // LOG de criação
        Debug.Log("Machado criado e ativo.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // LOG de colisão
        Debug.Log("Machado colidiu com: " + collision.gameObject.name);

        // Verifica se o que foi atingido é o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Tenta obter os scripts de vida e powerups do jogador
            PlayerVida playerVida = collision.gameObject.GetComponent<PlayerVida>();
            PlayerPowerUp powerUps = collision.gameObject.GetComponent<PlayerPowerUp>();

            // Se não está invencível...
            if (powerUps != null && !powerUps.EstaInvencivel() && !powerUps.EstaInvencivelPorDano())
            {
                // Aplica dano
                if (playerVida != null)
                {
                    playerVida.LevarDano(dano);
                    Debug.Log("Jogador levou " + dano + " de dano do machado.");
                }

                // Aplica empurrão
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direcao = (collision.transform.position - transform.position).normalized;
                    rb.AddForce(direcao * forcaPushback, ForceMode.Impulse);
                    Debug.Log("Empurrão aplicado ao jogador.");
                }
            }
            else
            {
                Debug.Log("Jogador colidiu mas está invencível.");
            }
        }

        // Destrói o machado após qualquer colisão
        Destroy(gameObject);
    }
}

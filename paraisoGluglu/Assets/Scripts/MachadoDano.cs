// using UnityEngine;
using UnityEngine;  // Biblioteca Unity

public class MachadoDano : MonoBehaviour
{
    // Velocidade do machado
    [SerializeField] private float velocidade = 18f;

    // Dano que causa
    [SerializeField] private int dano = 1;

    // Tempo até ser destruído caso não acerte em nada
    [SerializeField] private float tempoDestruir = 4f;

    // Referência ao Rigidbody
    private Rigidbody rb;

    // ================ START ================
    void Start()
    {
        // Guarda componente
        rb = GetComponent<Rigidbody>();

        // Define a velocidade imediatamente
        rb.linearVelocity = transform.forward * velocidade;

        // LOG para confirmar
        //Debug.Log("Machado lançado com velocidade: " + rb.linearVelocity.magnitude);

        // Auto-destroy
        Destroy(gameObject, tempoDestruir);
    }

    // ================ COLISÃO ================
    private void OnTriggerEnter(Collider other)
    {
        // Se atingir o player
        if (other.CompareTag("Player"))
        {
            // Aceder scripts de vida e power-ups
            PlayerVida vida = other.GetComponent<PlayerVida>();
            PlayerPowerUp pwr = other.GetComponent<PlayerPowerUp>();

            // Verifica invencibilidade
            if (vida != null && pwr != null && !pwr.EstaInvencivel() && !pwr.EstaInvencivelPorDano())
            {
                // Aplica dano
                vida.LevarDano(dano);

                // LOG
                //Debug.Log("Jogador atingido pelo machado. Dano: " + dano);
            }

            // Destroi o machado após impacto
            Destroy(gameObject);
        }
    }
}

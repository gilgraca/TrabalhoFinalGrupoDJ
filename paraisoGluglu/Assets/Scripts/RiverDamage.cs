using UnityEngine;

public class RiverDamage : MonoBehaviour
{
    // Tempo de espera antes de teleportar novamente (anti-spam)
    [SerializeField] private float cooldown = 1f;

    // Guarda o tempo da última ativação
    private float tempoUltimoRespawn = -999f;

    // Quando algo entra no trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se é o jogador
        if (other.CompareTag("Player"))
        {
            // Verifica se já passou o cooldown
            if (Time.time - tempoUltimoRespawn > cooldown)
            {
                // Atualiza o tempo do último respawn
                tempoUltimoRespawn = Time.time;

                // Procura o ponto de respawn com a tag
                GameObject pontoRespawn = GameObject.FindGameObjectWithTag("Respawn");

                PlayerVida vida = other.GetComponent<PlayerVida>();
                if (vida != null)
                {
                    vida.LevarDano(1); // ou outro valor
                }

                // Se o ponto existir
                if (pontoRespawn != null)
                {
                    // Teleporta o jogador para o ponto
                    other.transform.position = pontoRespawn.transform.position;

                    // Se quiseres, também podes resetar a velocidade aqui
                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    //Log para testes
                    //Debug.Log("Jogador caiu na água e foi teleportado.");
                }
                else
                {
                    //Debug.LogWarning("Nenhum ponto de respawn com tag 'Respawn' encontrado!");
                }
            }
        }
    }
}

// Importa o necessário
using UnityEngine;

public class Chave : MonoBehaviour
{
    // ID da chave (para expandir no futuro se tiveres várias portas)
    public int idChave = 1;

    // Animação de flutuação da chave (opcional)
    public float amplitude = 0.25f;
    public float velocidade = 2f;

    // Guarda a posição inicial para animar
    private Vector3 posInicial;

    void Start()
    {
        // Guarda a posição base da chave
        posInicial = transform.position;
    }

    void Update()
    {
        // Faz a chave flutuar suavemente
        float novaY = Mathf.Sin(Time.time * velocidade) * amplitude;
        transform.position = new Vector3(posInicial.x, posInicial.y + novaY, posInicial.z);

        // Faz a chave rodar
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Se o jogador tocar na chave
        if (other.CompareTag("Player"))
        {
            // Vai buscar o script "Chaves" no jogador
            ChavePlayer chavesScript = other.GetComponent<ChavePlayer>();

            if (chavesScript != null)
            {
                // Regista a chave apanhada
                chavesScript.AdicionarChave(idChave);

                // Ativa a porta na cena
                PortaChaveController porta = FindFirstObjectByType<PortaChaveController>();
                if (porta != null)
                {
                    porta.TentarAbrirPorta(idChave);
                }

                // Log para testes
                // Debug.Log("Chave apanhada e porta ativada!");

                // Destroi esta chave do mundo
                Destroy(gameObject);
            }
        }
    }
}

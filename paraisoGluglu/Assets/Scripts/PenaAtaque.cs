using UnityEngine;

// Script responsável pelo movimento e deteção de colisão da pena
public class PenaAtaque : MonoBehaviour
{
    // Velocidade da pena
    public float velocidade = 10f;

    // Dano que causa
    public int dano = 1;

    // Distância máxima antes de desaparecer
    public float distanciaMaxima = 10f;

    // Direção fixa onde a pena deve seguir
    private Vector3 direcao;

    // Posição inicial da pena
    private Vector3 posicaoInicial;

    void Start()
    {
        // Guarda a posição de onde foi disparada
        posicaoInicial = transform.position;
    }

    // Define a direção assim que a pena é criada
    public void DefinirDirecao(Vector3 novaDirecao)
    {
        // Guarda a direção normalizada
        direcao = novaDirecao.normalized;
    }

    void Update()
    {
        // Move-se sempre na direção definida (transform.Translate estava errado)
        transform.position += direcao * velocidade * Time.deltaTime;

        // Verifica a distância percorrida
        float distanciaPercorrida = Vector3.Distance(posicaoInicial, transform.position);
        if (distanciaPercorrida >= distanciaMaxima)
        {
            //Debug.Log("Pena atingiu a distância máxima. Vai desaparecer.");
            Destroy(gameObject);
        }
    }

    // Quando o ataque colide com algo
    private void OnTriggerEnter(Collider other)
    {
        // Mostra na consola com quem colidiu
        //Debug.Log("Ataque colidiu com: " + other.name);

        // Tenta obter o script de vida diretamente no objeto atingido
        InimigoVida vida = other.GetComponent<InimigoVida>();

        // Se não encontrar, tenta no objeto pai (caso o script esteja no pai)
        if (vida == null)
            vida = other.GetComponentInParent<InimigoVida>();

        // Se encontrou o script de vida
        if (vida != null)
        {
            // Aplica dano ao inimigo
            vida.LevarDano(dano);

            // Mostra feedback de dano na consola
            //Debug.Log("Dano causado ao inimigo " + other.name + ": " + dano);

            // Destrói o ataque após o impacto
            Destroy(gameObject);
            return;
        }

        // Se o ataque colide com um objeto com a tag "Caixa"
        if (other.CompareTag("Caixa"))
        {
            // Tenta obter o script da caixa
            DestruirCaixa caixa = other.GetComponent<DestruirCaixa>();

            // Se encontrou a caixa
            if (caixa != null)
            {
                // Parte a caixa
                caixa.QuebrarCaixa();

                // Feedback na consola
                //Debug.Log("Caixa destruída pelo ataque!");
            }

            // Destrói o ataque após impactar a caixa
            Destroy(gameObject);
        }
    }
}


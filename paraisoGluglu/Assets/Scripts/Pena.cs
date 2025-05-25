using UnityEngine;

// Script responsável pelo movimento e deteção de colisão da pena
public class Pena : MonoBehaviour
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
            Debug.Log("Pena atingiu a distância máxima. Vai desaparecer.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pena colidiu com: " + other.name);

        if (other.CompareTag("Inimigo"))
        {
            // Tenta inimigo terrestre
            InimigoTerrestre terrestre = other.GetComponent<InimigoTerrestre>();
            if (terrestre == null)
                terrestre = other.GetComponentInParent<InimigoTerrestre>();

            if (terrestre != null)
            {
                terrestre.ReceberDano(dano);
                Debug.Log("Dano causado ao inimigo terrestre");
            }

            // Tenta inimigo aéreo
            InimigoAereo aereo = other.GetComponent<InimigoAereo>();
            if (aereo == null)
                aereo = other.GetComponentInParent<InimigoAereo>();

            if (aereo != null)
            {
                aereo.ReceberDano(dano);
                Debug.Log("Dano causado ao inimigo aéreo");
            }

            Destroy(gameObject);
        }
        // Se atingir uma caixa
        if (other.CompareTag("Caixa"))
        {
            // Verifica se tem o script Caixa
            Caixa caixa = other.GetComponent<Caixa>();
            if (caixa != null)
            {
                caixa.QuebrarCaixa();
                Debug.Log("Caixa destruída por pena!");
            }

            Destroy(gameObject); // destrói a pena
        }
    }
}

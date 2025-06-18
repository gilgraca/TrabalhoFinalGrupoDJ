using UnityEngine;

public class MainCamaraSpyro : MonoBehaviour
{
    // Jogador a seguir
    public Transform jogador;
    
    // Distância da câmara ao jogador
    public float distancia = 6.0f;

    // Altura da câmara em relação ao jogador
    public float altura = 3.0f;

    // Velocidade de rotação da câmara com o rato
    public float sensibilidadeRato = 3.0f;

    // Suavidade do movimento
    public float suavidade = 5.0f;

    // Ângulo atual da câmara (horizontal)
    private float anguloAtual = 0.0f;

    void LateUpdate()
    {
        if (jogador == null) return;

        // Lê movimento horizontal do rato (ou substitui por Input do analógico)
        float movimentoRato = Input.GetAxis("Mouse X");

        // Atualiza o ângulo com base no rato
        anguloAtual += movimentoRato * sensibilidadeRato;

        // Calcula nova posição da câmara com base no ângulo
        Quaternion rotacao = Quaternion.Euler(0, anguloAtual, 0);
        Vector3 direcao = rotacao * Vector3.back;

        // Define posição desejada da câmara (atrás e acima do jogador)
        Vector3 posicaoDesejada = jogador.position + direcao * distancia + Vector3.up * altura;

        // Suaviza o movimento da câmara
        transform.position = Vector3.Lerp(transform.position, posicaoDesejada, Time.deltaTime * suavidade);

        // Aponta para o jogador
        transform.LookAt(jogador);

        // LOG para testar posição e rotação
        // Debug.Log("Câmara pos: " + transform.position + " | Ângulo: " + anguloAtual);
    }
}

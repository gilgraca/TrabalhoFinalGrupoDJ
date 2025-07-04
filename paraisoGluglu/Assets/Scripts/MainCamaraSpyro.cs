using UnityEngine;

public class MainCamaraSpyro : MonoBehaviour
{
    // Referência ao jogador
    public Transform jogador;

    // Distância da câmara ao jogador
    public float distancia = 6.0f;

    // Altura da câmara em relação ao jogador
    public float altura = 3.0f;

    // Sensibilidade do rato
    public float sensibilidadeRato = 3.0f;

    // Ângulo da câmara em relação ao jogador
    private float anguloHorizontal = 0.0f;

    void LateUpdate()
    {
        // Bloqueia o movimento se estiver pausado ou jogador nulo
        if (MenuPausa.jogoPausado || jogador == null) return;

        // Movimento do rato horizontal
        float movimentoRato = Input.GetAxis("Mouse X");

        // Atualiza ângulo de rotação em torno do jogador
        anguloHorizontal += movimentoRato * sensibilidadeRato;

        // Calcula a posição da câmara a girar em torno do jogador
        Vector3 offset = Quaternion.Euler(0, anguloHorizontal, 0) * new Vector3(0, 0, -distancia);
        Vector3 posicaoDesejada = jogador.position + offset + Vector3.up * altura;

        // Aplica diretamente a posição
        transform.position = posicaoDesejada;

        // Aplica diretamente a rotação para olhar para o jogador (sem Slerp, sem LookAt)
        transform.rotation = Quaternion.LookRotation(jogador.position - transform.position);
    }
}

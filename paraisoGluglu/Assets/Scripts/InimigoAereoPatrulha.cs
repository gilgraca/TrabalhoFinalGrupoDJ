// Script responsável pelo patrulhamento aéreo circular do inimigo
using UnityEngine;

public class InimigoAereoPatrulha : MonoBehaviour
{
    // Centro do movimento circular
    private Vector3 centroPatrulha;
    // Altura padrão do voo
    public float alturaPatrulha = 5f;
    // Raio horizontal da patrulha
    public float raioPatrulha = 3f;
    // Velocidade angular
    public float velocidadeRotacao = 1f;

    // Estado de ataque (interrompe a patrulha)
    [HideInInspector] public bool aAtacar = false;

    // Ângulo atual para o círculo
    private float angulo = 0f;

    void Start()
    {
        // Guarda a posição inicial como centro do círculo
        centroPatrulha = transform.position;
    }

    void Update()
    {
        if (aAtacar) return;

        // Atualiza ângulo e calcula nova posição
        angulo += velocidadeRotacao * Time.deltaTime;
        float x = Mathf.Cos(angulo) * raioPatrulha;
        float z = Mathf.Sin(angulo) * raioPatrulha;
        Vector3 novaPosicao = new Vector3(centroPatrulha.x + x, centroPatrulha.y, centroPatrulha.z + z);
        transform.position = novaPosicao;

        // Gira o inimigo com rotação deitado (eixo Z)
        Vector3 direcao = new Vector3(-Mathf.Sin(angulo), 0f, Mathf.Cos(angulo));
        if (direcao != Vector3.zero)
        {
            Quaternion rotacaoBase = Quaternion.LookRotation(direcao);
            Quaternion rotacaoExtraZ = Quaternion.Euler(0f, 0f, 90f);
            Quaternion rotacaoFinal = rotacaoBase * rotacaoExtraZ;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoFinal, 10f * Time.deltaTime);
        }
    }
}

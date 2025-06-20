using UnityEngine;

public class MainCamaraScriptCrash : MonoBehaviour
{
    // O player a seguir
    public Transform player;

    // Offset da câmara em relação ao player
    public Vector3 offset = new Vector3(0f, 5f, -7f);

    // Suavidade da transição
    public float suavidade = 5f;

    void LateUpdate()
    {
        // Se o player não estiver definido, sai
        if (player == null) return;

        // Calcula a posição desejada com base no offset
        Vector3 destinoDesejado = player.position + offset;

        // Suaviza a transição até à posição desejada
        transform.position = Vector3.Lerp(transform.position, destinoDesejado, suavidade * Time.deltaTime);

        // Calcula a rotação desejada para olhar para o player
        Quaternion rotacaoDesejada = Quaternion.LookRotation(player.position - transform.position);

        // Suaviza a rotação com Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoDesejada, Time.deltaTime * suavidade);
    }
}

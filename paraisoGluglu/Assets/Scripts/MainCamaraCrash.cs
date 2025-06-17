using UnityEngine;

public class MainCamaraScriptCrash : MonoBehaviour
{
    // O player
    public Transform player;
    // Posição do personagem
    public Vector3 offset = new Vector3(0f, 5f, -7f); 
    // Suavidade da transição
    public float suavidade = 5f;       

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 destinoDesejado = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, destinoDesejado, suavidade * Time.deltaTime);
        // Mantém a câmara sempre virada para o player
        transform.LookAt(player);
    }
}

// Importa o TextMeshPro
using TMPro;
using UnityEngine;

public class PortaTextoController : MonoBehaviour
{
    // Referência ao componente TMP_Text
    public TMP_Text texto;

    // Texto novo quando a porta estiver pronta
    public string textoComChave = "A porta está aberta!";

    public void MudarTexto()
    {
        if (texto != null)
        {
            texto.text = textoComChave;
            Debug.Log("Texto da porta foi alterado.");
        }
    }
}

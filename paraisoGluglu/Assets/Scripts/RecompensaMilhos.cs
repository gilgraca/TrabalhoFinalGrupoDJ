using UnityEngine;

public class RecompensaMilhos : MonoBehaviour
{
    // Referência ao elemento de UI que mostra a recompensa
    public GameObject recompensaUI;

    // Quantidade de milhos necessários para mostrar a recompensa
    public int milhosNecessarios = 0;

    void Start()
    {
        // Garante que configurámos a referência à UI; caso contrário avisa e sai
        if (recompensaUI == null)
        {
            //Debug.LogWarning(" Nenhum GameObject de recompensa atribuído!");
            return;
        }
        // Verifica se o jogador apanhou milhos suficientes
        if (GameManager.Instance.milhoTotal >= milhosNecessarios)
        {
            // Ativa o elemento de UI para mostrar a recompensa
            recompensaUI.SetActive(true);
            //Debug.Log("Todos os milhos recolhidos — recompensa ativada!");
            //Debug.Log("Total de milhos: " + GameManager.Instance.milhoTotal + ", Milhos necessários: " + milhosNecessarios);
        }
        else
        {
            // Desativa o elemento de UI porque faltaram milhos
            recompensaUI.SetActive(false);
            //Debug.Log("Milhos insuficientes — recompensa mantida oculta.");
        }
    }
}

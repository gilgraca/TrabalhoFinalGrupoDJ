using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeCenas : MonoBehaviour
{
    // Próxima cena a carregar
    [SerializeField] private string proximaCena;

    // Se usar loading, define o nome da cena de loading
    [SerializeField] private bool usarLoading = false;

    // Chamado por botão, trigger ou ao fim da cutscene
    public void CarregarProximaCena()
    {
        //Debug.Log("A carregar cena: " + proximaCena + " | Usar loading? " + usarLoading);

        if (usarLoading)
        {
            CarregadorDeCenas.IrParaCenaComLoading(proximaCena);
        }
        else
        {
            SceneManager.LoadScene(proximaCena);
        }
    }


    // Ou versão com delay (ex: após fade out)
    public void CarregarCenaComDelay(float tempo)
    {
        Invoke(nameof(CarregarProximaCena), tempo);
    }
}

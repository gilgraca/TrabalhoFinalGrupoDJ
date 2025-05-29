using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeCenas : MonoBehaviour
{
    // Próxima cena a carregar
    [SerializeField] private string proximaCena;

    // Chamado por botão, trigger ou ao fim da cutscene
    public void CarregarProximaCena()
    {
        SceneManager.LoadScene(proximaCena);
    }

    // Ou versão com delay (ex: após fade out)
    public void CarregarCenaComDelay(float tempo)
    {
        Invoke(nameof(CarregarProximaCena), tempo);
    }
}

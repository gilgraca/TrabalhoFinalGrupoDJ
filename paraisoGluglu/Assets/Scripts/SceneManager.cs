using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        // Impede que o trigger funcione se este script estiver desativado
        if (!enabled) return;

        // Verifica se o objeto que colidiu tem o tag "Player"
        if (other.CompareTag("Player"))
        {
            // Carrega a próxima cena
            CarregarProximaCena();
        }
    }


    // Ou versão com delay (ex: após fade out)
    public void CarregarCenaComDelay(float tempo)
    {
        Invoke(nameof(CarregarProximaCena), tempo);
    }
}

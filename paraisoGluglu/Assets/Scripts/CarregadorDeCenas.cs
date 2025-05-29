using UnityEngine;
using UnityEngine.SceneManagement;

public static class CarregadorDeCenas
{
    // Chama isto de qualquer lado para iniciar o loading
    public static void IrParaCenaComLoading(string nomeDaCenaDestino)
    {
        // Define a próxima cena
        CarregadorGlobal.ProximaCena = nomeDaCenaDestino;

        // Vai para a cena de loading
        SceneManager.LoadScene("LoadingScreen");
    }
}

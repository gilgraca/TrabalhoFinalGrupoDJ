using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPowerUps : MonoBehaviour
{
    // Referência ao objeto Player na cena de jogo (só é usada quando carrega a cena)
    [SerializeField] private GameObject playerPrefab;

    // Referências aos toggles da UI
    [Header("Toggles de PowerUps")]
    public Toggle toggleDoubleJump;
    public Toggle toggleDash;
    public Toggle toggleInvencibilidade;
    public Toggle toggleInvisibilidade;
    public Toggle toggleAtaqueEspecial;

    // Variáveis públicas que serão lidas na próxima cena
    public static bool usarDoubleJump = false;
    public static bool usarDash = false;
    public static bool usarInvencibilidade = false;
    public static bool usarInvisibilidade = false;
    public static bool usarAtaqueEspecial = false;

    // Método chamado pelo botão "Começar Jogo"
    public void IniciarJogo(string nomeCena)
    {
        // Lê o estado dos toggles e guarda nas variáveis estáticas
        usarDoubleJump = toggleDoubleJump.isOn;
        usarDash = toggleDash.isOn;
        usarInvencibilidade = toggleInvencibilidade.isOn;
        usarInvisibilidade = toggleInvisibilidade.isOn;
        usarAtaqueEspecial = toggleAtaqueEspecial.isOn;

        // Carrega a cena do jogo
        SceneManager.LoadScene(nomeCena);
    }
}

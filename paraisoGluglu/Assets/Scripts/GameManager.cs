using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instância global acessível de qualquer script
    public static GameManager Instance;
    // Nome da cena do menu principal
    public string menu;

    // Número de vidas do jogador
    // --- DADOS DO JOGADOR ---

    // Vida atual
    public int vidaJogador = 5;

    // Quantidade de milhos recolhidos
    public int milhoTotal = 0;

    // Power-ups ativos
    public bool usarDoubleJump = false;
    public bool usarDash = false;
    public bool usarInvencibilidade = false;
    public bool usarInvisibilidade = false;
    public bool usarAtaqueEspecial = false;

    void Awake()
    {
        // Se ainda não há GameManager, define este e mantém entre cenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém mesmo ao carregar outra cena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
    // Sai do jogo atual para o menu principal
    public void SairParaMenu()
    {
        Time.timeScale = 1f;
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(menu);
    }
}

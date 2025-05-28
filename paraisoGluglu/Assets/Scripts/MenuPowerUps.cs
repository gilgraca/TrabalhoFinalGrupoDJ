using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPowerUps : MonoBehaviour
{
    // Referência ao objeto Player na cena de jogo (só é usada quando carrega a cena)
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject backgroundImage;

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

    private void Update()
    {
        if (backgroundImage != null)
        {
            // Rotaciona 30 graus por segundo no eixo Z
            backgroundImage.transform.Rotate(0f, 0f, 30f * Time.deltaTime);
        }
    }
    public void IniciarJogo(string nomeCena)
    {
        GameManager.Instance.usarDoubleJump = toggleDoubleJump.isOn;
        GameManager.Instance.usarDash = toggleDash.isOn;
        GameManager.Instance.usarInvencibilidade = toggleInvencibilidade.isOn;
        GameManager.Instance.usarInvisibilidade = toggleInvisibilidade.isOn;
        GameManager.Instance.usarAtaqueEspecial = toggleAtaqueEspecial.isOn;
        SceneManager.LoadScene(nomeCena);
    }
    public void DefinirAtaqueEspecial(bool estado)
    {
        GameManager.Instance.usarAtaqueEspecial = estado;
    }

    public void DefinirDash(bool estado)
    {
        GameManager.Instance.usarDash = estado;
    }

    public void DefinirDoubleJump(bool estado)
    {
        GameManager.Instance.usarDoubleJump = estado;
    }

    public void DefinirInvencibilidade(bool estado)
    {
        GameManager.Instance.usarInvencibilidade = estado;
    }

    public void DefinirInvisibilidade(bool estado)
    {
        GameManager.Instance.usarInvisibilidade = estado;
    }
}

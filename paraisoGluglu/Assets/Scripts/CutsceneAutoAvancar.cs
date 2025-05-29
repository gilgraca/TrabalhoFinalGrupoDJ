using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CutsceneAutoAvancarComBarra : MonoBehaviour
{
    // Nome da próxima cena
    [SerializeField] private string proximaCena;

    // Tempo total da cutscene
    [SerializeField] private float duracaoCutscene = 19f;

    // Tempo que o jogador tem de manter a tecla F para avançar manualmente
    [SerializeField] private float tempoPressionadoNecessario = 2f;

    // Referência à barra de UI (Image com Fill)
    [SerializeField] private Image barraProgresso;

    // Contador do tempo da tecla pressionada
    private float tempoPressionado = 0f;

    // Flag para impedir chamadas múltiplas
    private bool jaAvancou = false;

    void Start()
    {
        // Evita conflito com UI: limpa o foco de elementos de UI ativos
        EventSystem.current?.SetSelectedGameObject(null);

        // Avança automaticamente ao fim da cutscene se o jogador não fizer nada
        Invoke(nameof(AvancarCena), duracaoCutscene);

        // Começa com a barra vazia
        if (barraProgresso != null)
            barraProgresso.fillAmount = 0f;
    }

    void Update()
    {
        // Não faz nada se já avançou
        if (jaAvancou) return;

        // Se o jogador está a manter a tecla F
        if (Input.GetKey(KeyCode.F))
        {
            // Aumenta o tempo pressionado
            tempoPressionado += Time.deltaTime;

            // Atualiza a barra (clamp entre 0 e 1)
            if (barraProgresso != null)
                barraProgresso.fillAmount = Mathf.Clamp01(tempoPressionado / tempoPressionadoNecessario);

            // Se passou o tempo necessário, avança
            if (tempoPressionado >= tempoPressionadoNecessario)
            {
                AvancarCena();
            }
        }
        else
        {
            // Se largou a tecla, reinicia tudo
            tempoPressionado = 0f;

            if (barraProgresso != null)
                barraProgresso.fillAmount = 0f;
        }
    }

    void AvancarCena()
    {
        if (jaAvancou) return;
        jaAvancou = true;

        SceneManager.LoadScene(proximaCena);
    }
}

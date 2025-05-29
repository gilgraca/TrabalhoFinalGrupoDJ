using UnityEngine;
using UnityEngine.UI;

public class PowerUpCooldownUI : MonoBehaviour
{
    // Referência à imagem de preenchimento (a camada escura)
    [SerializeField] private Image imagemCooldown;

    // Se o cooldown está ativo ou não
    private bool emCooldown = false;

    // Tempo total do cooldown
    private float tempoTotal = 1f;
    // Tempo que já passou desde que começou
    private float tempoDecorrido = 0f;

    // Atualiza o cooldown (chamado externamente ao ativar o power-up)
    public void IniciarCooldown(float tempo)
    {
        tempoTotal = tempo;
        tempoDecorrido = 0f;
        emCooldown = true;
        imagemCooldown.fillAmount = 1f;

        // Log para testes
        //Debug.Log("Cooldown iniciado: " + tempo + "s");
    }

    void Update()
    {
        if (!emCooldown) return;

        // Aumenta o tempo decorrido
        tempoDecorrido += Time.deltaTime;

        // Calcula o progresso
        float progresso = Mathf.Clamp01(tempoDecorrido / tempoTotal);

        // Diminui o fill de 1 para 0
        imagemCooldown.fillAmount = 1f - progresso;

        // Quando o cooldown terminar
        if (tempoDecorrido >= tempoTotal)
        {
            emCooldown = false;
            imagemCooldown.fillAmount = 0f;

            // Log para testes
            //Debug.Log("Cooldown terminado!");
        }
    }
}

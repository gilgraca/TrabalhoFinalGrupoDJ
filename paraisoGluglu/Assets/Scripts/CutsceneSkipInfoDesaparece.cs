using UnityEngine;
using System.Collections;

public class CutsceneSkipDesapareceComFade : MonoBehaviour
{
    // Tempo visível antes de iniciar o fade out
    public float tempoVisivel = 3f;

    // Duração do fade out
    public float duracaoFade = 1f;

    // Referência ao CanvasGroup
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Busca o componente CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();

        // Se não tiver CanvasGroup, avisa na consola
        if (canvasGroup == null)
        {
            // Debug.LogError("[CutsceneSkipDesapareceComFade] Falta o componente CanvasGroup!");
        }
    }

    void OnEnable()
    {
        // Garante que está totalmente visível ao ativar
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        // Log de início
        // Debug.Log("[CutsceneSkipDesapareceComFade] Ativado. Vai iniciar fade out após " + tempoVisivel + " segundos.");

        // Inicia o fade após tempoVisivel
        StartCoroutine(IniciarFadeOut());
    }

    IEnumerator IniciarFadeOut()
    {
        // Espera o tempo visível
        yield return new WaitForSeconds(tempoVisivel);

        // Debug.Log("[CutsceneSkipDesapareceComFade] A iniciar fade out.");

        float tempo = 0f;

        // Loop para reduzir o alpha gradualmente
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;

            // Reduz o alpha com base no progresso
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, tempo / duracaoFade);

            yield return null;
        }

        // Garante que o alpha é 0 no fim
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        // Desativa o GameObject no fim do fade
        // Debug.Log("[CutsceneSkipDesapareceComFade] Fade out concluído. Objeto desativado.");
        gameObject.SetActive(false);
    }
}

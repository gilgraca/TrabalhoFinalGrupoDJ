using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    // Garante que só existe um (singleton simples)
    private static FPSLimiter instancia;

    void Awake()
    {
        // Se já existe um, destrói o duplicado
        if (instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        // Marca como o único e impede que desapareça entre cenas
        instancia = this;
        DontDestroyOnLoad(gameObject);

        // Desativa o VSync
        QualitySettings.vSyncCount = 0;

        // Limita os FPS a 60
        Application.targetFrameRate = 60;

        // LOG para confirmar
        //Debug.Log("FPS limitado a 60 (global entre cenas)");
    }
}

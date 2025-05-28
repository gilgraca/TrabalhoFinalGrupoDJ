using System.Collections;
using UnityEngine;

public class PlayerToast : MonoBehaviour
{
    // === VARIÁVEIS DE CONTROLO PARA O TOAST MANUAL ===
    // Tempo de espera entre ativações do toast com tecla I
    [SerializeField] private float cooldownToastManual = 10f;
    // Se pode ou não ativar novamente
    private bool podeAtivarToastManual = true;

    void Update()
    {
        // === TOAST COM COOLDOWN AO PRESSIONAR I ===
        // Controla se pode ativar o toast
        if (Input.GetKeyDown(KeyCode.I) && podeAtivarToastManual)
        {
            // Log de teste para consola
            Debug.Log("Tecla I pressionada — toast ativado manualmente.");

            // Ativa o toast
            FindFirstObjectByType<NivelTracker>()?.MostrarToastManual();

            // Impede novo uso até passar o cooldown
            podeAtivarToastManual = false;

            // Inicia o cooldown
            StartCoroutine(ReporCooldownToastManual());
        }
    }

    // Corrotina que espera X segundos antes de permitir nova ativação
    private IEnumerator ReporCooldownToastManual()
    {
        // Espera o tempo definido
        yield return new WaitForSeconds(cooldownToastManual);

        // Ativa novamente
        podeAtivarToastManual = true;

        //Debug.Log("Toast manual disponível novamente.");
    }
}

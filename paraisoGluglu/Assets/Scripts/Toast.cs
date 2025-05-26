// Script responsável por mostrar uma pequena mensagem (toast) ao ativar um checkpoint com movimento lateral completo
using UnityEngine;
using System.Collections;

public class CheckpointToast : MonoBehaviour
{
    public GameObject toastUI; // Elemento visual (UI) a ser mostrado como "toast"
    private bool activated = false; // Impede que o toast seja ativado mais do que uma vez
    public float moveDistance = 100f; // Distância que o toast se move (em pixels)
    public float moveDuration = 2f; // Tempo para se mover para o lado ou voltar (em segundos)

    void Start()
    {
        // Garante que o toast comece invisível
        if (toastUI != null)
            toastUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Impede reativação múltipla
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // player.AtivarCheckpoint(transform);
            }
            if (toastUI != null)
                StartCoroutine(ShowMessage());
        }
    }

    private IEnumerator ShowMessage()
    {
        // Guarda a posição inicial
        Vector3 startPosition = toastUI.transform.localPosition;

        // Ativa o toast
        toastUI.SetActive(true);
        Debug.Log("Toast ativado!");

        // Espera 1 segundo antes de se mover
        yield return new WaitForSeconds(1f);
        Debug.Log("Toast parado (1s).");

        // Calcula a posição para onde vai deslizar
        Vector3 targetPosition = startPosition + new Vector3(moveDistance, 0, 0);

        float elapsed = 0f;

        // Move o toast para o lado em 2 segundos
        Debug.Log("Toast a mover para o lado...");
        while (elapsed < moveDuration)
        {
            toastUI.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        toastUI.transform.localPosition = targetPosition;

        // Fica parado 3 segundos na nova posição
        Debug.Log("Toast parado na nova posição (3s).");
        yield return new WaitForSeconds(3f);

        // Volta para a posição original em 2 segundos
        Debug.Log("Toast a regressar...");
        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            toastUI.transform.localPosition = Vector3.Lerp(targetPosition, startPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        toastUI.transform.localPosition = startPosition;

        // Fica parado mais 1 segundo antes de desaparecer
        Debug.Log("Toast parado antes de desaparecer (1s).");
        yield return new WaitForSeconds(1f);

        // Desativa o toast
        toastUI.SetActive(false);
        Debug.Log("Toast desativado.");
    }
}

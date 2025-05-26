// Script responsável por mostrar uma pequena mensagem (toast) ao ativar um checkpoint
// Também atualiza o ponto de retorno (safePoint) do jogador
using UnityEngine;
using System.Collections;
public class CheckpointToast : MonoBehaviour
{
    public GameObject toastUI; // Elemento visual (UI) a ser mostrado como "toast"
    private bool activated = false; // Impede que o toast seja ativado mais do que uma vez
    //public AudioSource somCheckpoint; // Som a tocar ao ativar o checkpoint
    void Start()
    {
        // Garante que o toast comece invisível
        if (toastUI != null)
            toastUI.SetActive(false);
    }
    // Detecta entrada do jogador no checkpoint
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return; // Ignora se já tiver sido ativado
        if (other.CompareTag("Player"))
        {
            activated = true;
            Player player = other.GetComponent<Player>(); // Atualiza o safePoint do jogador (ponto de respawn)
            if (player != null)
            {
                //player.AtivarCheckpoint(transform); // Define este checkpoint como novo ponto de retorno
            }
            // Ativa a mensagem temporária na tela
            if (toastUI != null)
                StartCoroutine(ShowMessage(2f)); // Mostra o toast por 2 segundos
        }
    }
    // Coroutine que mostra e depois esconde o toast após determinado tempo
    private IEnumerator ShowMessage(float duration)
    {
        toastUI.SetActive(true);
        //if (somCheckpoint != null)
            ///somCheckpoint.Play();
        yield return new WaitForSeconds(duration);
        toastUI.SetActive(false);
    }
}
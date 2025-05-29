// Script responsável por acompanhar o número de colecionáveis apanhados num nível específico
// Mostra uma mensagem (toast) quando o jogador apanha todos os colecionáveis do nível
using UnityEngine;
using System.Collections;

public class NivelTracker : MonoBehaviour
{
    // Número total de colecionáveis disponíveis neste nível
    [SerializeField] public int colecionaveisNoNivel;
    // Número de colecionáveis já apanhados pelo jogador neste nível
    private int apanhados = 0;
    // Referência ao objeto UI (toast) que será mostrado ao apanhar todos os colecionáveis
    public GameObject toastFinal;
    // Som a tocar quando todas as pedras forem apanhadas
    //public AudioSource somToastFinal;
    // Elemento visual (UI) a ser mostrado como "toast"
    public GameObject toastUI;
    //Texto 
    public TMPro.TMP_Text toastText;


    void Start()
    {
        apanhados = PlayerPrefs.GetInt("apanhados", 0);
        // Inicializa o contador de colecionáveis apanhados
        apanhados = 0;
        // Garante que o toast começa invisível
        // Comentado por não aparecer o texto
        // if (toastFinal != null)
        //     toastFinal.SetActive(false);
    }

    // Função pública chamada sempre que o jogador apanha uma pedra
    public void AdicionarMilho()
    {
        // Soma 1 ao número de pedras apanhadas
        apanhados++;
        PlayerPrefs.SetInt("apanhados", apanhados);
        //Debug.Log("Pedra apanhada! Total: " + apanhados + "/" + colecionaveisNoNivel);
        // Se o jogador já apanhou todas as pedras, mostra o toast
        if (apanhados == colecionaveisNoNivel && toastFinal != null)
        {
            // Ativa a mensagem na tela
            toastFinal.SetActive(true);
            //if (somToastFinal != null)
            //    somToastFinal.Play();

            // Inicia a rotina para esconder após 3 segundos
            StartCoroutine(MostrarToastAnimado());
        }
    }
    public IEnumerator MostrarToastAnimado()
    {
        // Guarda a posição inicial do toast
        Vector3 startPosition = toastUI.transform.localPosition;

        // Ativa o toast
        toastUI.SetActive(true);
        //Debug.Log(toastText);
        // Ativa o texto (caso exista)
        if (toastText != null)
            toastText.gameObject.SetActive(true);


        //Debug.Log("Toast ativado!");

        // Espera 0.5 segundos
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("Toast parado (0.5s).");

        // Calcula a posição final
        Vector3 targetPosition = startPosition + new Vector3(220f, 0, 0);

        float elapsed = 0f;
        float duration = 0.7f;

        // Move para o lado
        //Debug.Log("Toast a mover para o lado...");
        while (elapsed < duration)
        {
            toastUI.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        toastUI.transform.localPosition = targetPosition;

        // Espera 3 segundos na nova posição
        //Debug.Log("Toast parado (3s).");
        yield return new WaitForSeconds(3f);

        // Volta à posição inicial
        elapsed = 0f;
        //Debug.Log("Toast a regressar...");
        while (elapsed < duration)
        {
            toastUI.transform.localPosition = Vector3.Lerp(targetPosition, startPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        toastUI.transform.localPosition = startPosition;

        // Espera 0.5 segundos antes de desaparecer
        //Debug.Log("Toast parado antes de desaparecer (0.5s).");
        yield return new WaitForSeconds(0.5f);

        // Desativa o toast e o texto
        // if (toastText != null)
        //     toastText.gameObject.SetActive(false);


        toastUI.SetActive(false);
        //Debug.Log("Toast desativado.");
    }

    // Método público para ativar o toast manualmente
    public void MostrarToastManual()
    {
        StartCoroutine(MostrarToastAnimado());
    }
}

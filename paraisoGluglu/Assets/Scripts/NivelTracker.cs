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
    // Guarda o tempo do último toast manual
    private float ultimoToastManual = -999f;
    // Cooldown global para o toast manual
    [SerializeField] private float cooldownToastManual = 10f;
    // Flag para evitar múltiplas ativações da animação do toast
    private bool toastAnimacaoAtiva = false;

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
        // Se a animação já está ativa, ignora nova chamada
        if (toastAnimacaoAtiva)
            yield break;

        // Marca como ativa
        toastAnimacaoAtiva = true;

        // Guarda a posição inicial do toast
        Vector3 startPosition = toastUI.transform.localPosition;

        // Ativa o toast
        toastUI.SetActive(true);

        // Ativa o texto (caso exista)
        if (toastText != null)
            toastText.gameObject.SetActive(true);

        // Espera 0.5 segundos
        yield return new WaitForSeconds(0.5f);

        // Calcula a posição final
        Vector3 targetPosition = startPosition + new Vector3(220f, 0, 0);

        float elapsed = 0f;
        float duration = 0.7f;

        // Move para o lado
        while (elapsed < duration)
        {
            toastUI.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        toastUI.transform.localPosition = targetPosition;

        // Espera 3 segundos na nova posição
        yield return new WaitForSeconds(3f);

        // Volta à posição inicial
        elapsed = 0f;
        while (elapsed < duration)
        {
            toastUI.transform.localPosition = Vector3.Lerp(targetPosition, startPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        toastUI.transform.localPosition = startPosition;

        // Espera 0.5 segundos antes de desaparecer
        yield return new WaitForSeconds(0.5f);

        // Desativa o toast e o texto
        toastUI.SetActive(false);
        if (toastText != null)
            toastText.gameObject.SetActive(false);

        // Liberta a flag para permitir futuras ativações
        toastAnimacaoAtiva = false;
    }

    // Método público para ativar o toast manualmente
    public void MostrarToastManual()
    {
        StartCoroutine(MostrarToastAnimado());
    }
    // Mostra o toast manual apenas se o cooldown tiver passado
    public void TentarMostrarToastManual()
    {
        // Verifica se já passou o tempo suficiente desde o último toast
        if (Time.time - ultimoToastManual >= cooldownToastManual)
        {
            // Mostra o toast
            MostrarToastManual();

            // Atualiza o tempo do último toast
            ultimoToastManual = Time.time;

            // Log para testar
            //Debug.Log("Toast manual mostrado com cooldown.");
        }
        else
        {
            // Log para testar
            //Debug.Log("Cooldown ativo: ainda não é possível mostrar toast.");
        }
    }

}

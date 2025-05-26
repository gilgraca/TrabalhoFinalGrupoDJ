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
    public AudioSource somToastFinal;

    void Start()
    {
        apanhados = PlayerPrefs.GetInt("apanhados", 0);
        // Inicializa o contador de colecionáveis apanhados
        apanhados = 0;
        // Garante que o toast começa invisível
        if (toastFinal != null)
            toastFinal.SetActive(false);
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
            if (somToastFinal != null)
                somToastFinal.Play();

            // Inicia a rotina para esconder após 3 segundos
            StartCoroutine(EsconderToastFinal());
        }
    }
    // Coroutine que esconde o toast após 3 segundos
    private IEnumerator EsconderToastFinal()
    {
        // Espera 3 segundos
        yield return new WaitForSeconds(3f);
        // Esconde a mensagem
        toastFinal.SetActive(false);
    }
}

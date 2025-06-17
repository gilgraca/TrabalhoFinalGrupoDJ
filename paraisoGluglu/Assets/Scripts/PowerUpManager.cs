using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.EventSystems;

// Este script gere o hover sobre os toggles e atualiza o vídeo + 3 textos (descrição, cooldown, tecla)
public class PowerUpHoverManager : MonoBehaviour
{
    // Referência ao VideoPlayer na UI
    [Header("Referências de UI")]
    public VideoPlayer videoPlayer; // Componente do vídeo onde serão mostradas as habilidades

    public TextMeshProUGUI descricaoText; // Texto com a descrição da habilidade
    public TextMeshProUGUI cooldownText;  // Texto com o tempo de cooldown
    public TextMeshProUGUI teclaText;     // Texto com a tecla associada à habilidade

    // Lista de URLs dos vídeos (mesma ordem dos toggles)
    [Header("URLs dos Vídeos")]
    public string[] videoURLs = new string[6]; // Lista com links para os vídeos (no GitHub por exemplo)

    // Lista de descrições (uma por habilidade)
    [Header("Descrições")]
    [TextArea]
    public string[] descricoes = new string[6];

    // Lista de tempos de cooldown
    [Header("Cooldowns")]
    public string[] cooldowns = new string[6];

    // Lista de teclas associadas
    [Header("Teclas")]
    public string[] teclas = new string[6];

    // Guarda o último índice mostrado (para reaparecer ao sair do hover)
    private int ultimoIndice = 0;

    // Chamada quando passamos o rato por cima de um botão
    public void MostrarPreview(int index)
    {
        // Guarda o índice atual
        ultimoIndice = index;

        // ---- VÍDEO ----
        videoPlayer.source = VideoSource.Url;         // Define que o vídeo vem de uma URL
        videoPlayer.url = videoURLs[index];           // Aponta o URL para o da habilidade
        videoPlayer.Prepare();                        // Prepara o vídeo
        videoPlayer.prepareCompleted += (vp) => vp.Play(); // Reproduz quando estiver pronto

        // ---- TEXTOS ----
        descricaoText.text = descricoes[index];       // Mostra a descrição correspondente
        cooldownText.text = cooldowns[index];         // Mostra o cooldown correspondente
        teclaText.text = teclas[index];               // Mostra a tecla associada à habilidade

        // Log para testes
        Debug.Log($"[Hover] Habilidade {index} | Vídeo: {videoURLs[index]} | Cooldown: {cooldowns[index]} | Tecla: {teclas[index]}");
    }

    // Chamada quando o rato sai do botão (mantém o último selecionado visível)
    public void ResetarPreview()
    {
        MostrarPreview(ultimoIndice); // Repete o último conteúdo mostrado
    }
}

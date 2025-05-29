using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;


public class LoadingScript : MonoBehaviour
{
    [SerializeField] private GameObject painelLoading;             // Painel principal de loading
    [SerializeField] private TMP_Text textoPressioneTecla;             // Texto a mostrar ("Pressione qualquer tecla")
    [SerializeField] private VideoPlayer videoPlayer;              // (Opcional) vídeo de loading

    private AsyncOperation asyncLoad;
    private bool prontoParaAvancar = false;

    void Start()
    {
        painelLoading.SetActive(true);

        // Esconde o texto até estar tudo carregado
        if (textoPressioneTecla != null)
            textoPressioneTecla.gameObject.SetActive(false);

        StartCoroutine(CarregarCena());
    }

    IEnumerator CarregarCena()
    {
        // Se tiveres vídeo, prepara e toca
        if (videoPlayer != null)
        {
            videoPlayer.isLooping = true;
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared) yield return null;
            videoPlayer.Play();
        }

        // Começa a carregar a cena real (sem ativar ainda)
        asyncLoad = SceneManager.LoadSceneAsync(CarregadorGlobal.ProximaCena);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        // Cena está pronta, agora espera input do jogador
        if (textoPressioneTecla != null)
            textoPressioneTecla.gameObject.SetActive(true);

        prontoParaAvancar = true;
    }

    void Update()
    {
        // Se estiver pronto e o jogador pressionar qualquer tecla
        if (prontoParaAvancar && Input.anyKeyDown)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }
}

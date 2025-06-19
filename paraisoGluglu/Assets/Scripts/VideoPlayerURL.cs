// Importa o namespace do VideoPlayer
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerURL : MonoBehaviour
{
    // Referência ao componente VideoPlayer no GameObject
    public VideoPlayer videoPlayer;

    // URL do vídeo que queres reproduzir
    public string videoURL;

    void Start()
    {
        // Verifica se o VideoPlayer foi atribuído no Inspector
        if (videoPlayer == null)
        {
            //Debug.LogError("VideoPlayer não foi atribuído no Inspector!");
            return;
        }

        // Define que o vídeo vai ser reproduzido a partir de uma URL
        videoPlayer.source = VideoSource.Url;

        // Atribui o URL ao VideoPlayer
        videoPlayer.url = videoURL;
        //Debug.Log("URL atribuído ao VideoPlayer: " + videoURL);

        // Quando o vídeo estiver pronto, chama a função OnPrepared
        videoPlayer.prepareCompleted += OnPrepared;

        // Se houver erro, chama a função OnVideoError
        videoPlayer.errorReceived += OnVideoError;

        // Começa a preparar o vídeo
        videoPlayer.Prepare();
        //Debug.Log("A preparar vídeo...");
    }

    // Chamado quando o vídeo está pronto a ser reproduzido
    void OnPrepared(VideoPlayer vp)
    {
        //Debug.Log("Vídeo preparado. A iniciar reprodução...");
        vp.Play();
    }

    // Chamado se houver erro
    void OnVideoError(VideoPlayer vp, string mensagem)
    {
        //Debug.LogError("Erro ao carregar vídeo: " + mensagem);
    }
}

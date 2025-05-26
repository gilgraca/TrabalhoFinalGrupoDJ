using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScript : MonoBehaviour
{
    public GameObject LoadingScene;    
    public VideoPlayer videoPlayer;
    private AsyncOperation asyncLoad;

    public void LoadScene(string newScene)
    {
        LoadingScene.SetActive(true);
        StartCoroutine(PlayVideoThenLoadScene(newScene));
    }

    IEnumerator PlayVideoThenLoadScene(string newScene)
    {
        // Show loading UI


        // Ensure video is prepared before playing
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Play video
        videoPlayer.Play();

        // Begin async loading in background, but don't switch yet
        asyncLoad = SceneManager.LoadSceneAsync(newScene);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is mostly loaded
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // At this point, the scene is loaded and waiting
        // Optionally wait for a fixed time or end video
        // Here, we just wait a little to ensure the video plays
        yield return new WaitForSeconds(0.2f);

        /*
        // Stop video and hide UI
        videoPlayer.Stop();
        LoadingScene.SetActive(false);
        */
        // Activate the new scene
        asyncLoad.allowSceneActivation = true;
    }
}

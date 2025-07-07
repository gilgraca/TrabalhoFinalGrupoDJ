using UnityEngine;
using UnityEngine.UI;

public class PortaChaveController : MonoBehaviour
{
	public GameObject portaComSceneManagement;
	public PortaTextoController textoController;
	public int idChaveNecessaria = 1;

	[SerializeField] private Material materialDisponivel;
	[SerializeField] private Material materialIndisponivel;
	[SerializeField] private Renderer indicadorRenderer;
	[SerializeField] private GameObject keyHUD;

	[Header("Áudio")]
	[SerializeField] private AudioClip somAbrirPorta;
	private AudioSource audioSource;

	private bool portaAberta = false;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f;

		if (portaComSceneManagement != null)
		{
			SceneManagement sceneScript = portaComSceneManagement.GetComponent<SceneManagement>();
			if (sceneScript != null)
				sceneScript.enabled = false;
		}

		if (indicadorRenderer != null && materialIndisponivel != null)
			indicadorRenderer.material = materialIndisponivel;

		if (keyHUD != null)
			keyHUD.GetComponent<Image>().color = new Color(1f, 1f, 1f, .24f);
	}

	public void TentarAbrirPorta()
	{
		if (portaAberta) return;

		portaAberta = true;

		SceneManagement sceneScript = portaComSceneManagement.GetComponent<SceneManagement>();
		if (sceneScript != null)
			sceneScript.enabled = true;

		if (textoController != null)
			textoController.MudarTexto();

		if (indicadorRenderer != null && materialDisponivel != null)
			indicadorRenderer.material = materialDisponivel;

		if (keyHUD != null)
			keyHUD.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

		if (audioSource != null && somAbrirPorta != null)
			audioSource.PlayOneShot(somAbrirPorta);
	}
}

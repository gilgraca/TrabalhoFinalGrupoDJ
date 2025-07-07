using UnityEngine;

public class CursorManager : MonoBehaviour
{
	[Header("Cursor")]
	[SerializeField] private bool mostrarCursor = false;

	[Header("Som ao pressionar F")]
	[SerializeField] private AudioClip somCursorGirando;
	private AudioSource audioSource;

	void Start()
	{
		// Configuração do cursor
		if (mostrarCursor)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Inicializa o AudioSource
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.clip = somCursorGirando;
		audioSource.loop = true;
		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f; // Som 2D
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (somCursorGirando != null && !audioSource.isPlaying)
				audioSource.Play();
		}

		if (Input.GetKeyUp(KeyCode.F))
		{
			if (audioSource.isPlaying)
				audioSource.Stop();
		}
	}
}

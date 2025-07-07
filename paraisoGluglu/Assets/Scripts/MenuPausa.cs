using UnityEngine;

public class MenuPausa : MonoBehaviour
{
	[SerializeField] private GameObject menuPausa;
	public static bool jogoPausado = false;

	void Start()
	{
		if (menuPausa != null)
			menuPausa.SetActive(false);

		// Garante que o som começa ligado
		AudioListener.volume = 1f;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (jogoPausado)
				RetomarJogo();
			else
				PausarJogo();
		}
	}

	public void PausarJogo()
	{
		Time.timeScale = 0f;
		menuPausa.SetActive(true);
		jogoPausado = true;

		// Mute do som
		AudioListener.volume = 0f;

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void RetomarJogo()
	{
		Time.timeScale = 1f;
		menuPausa.SetActive(false);
		jogoPausado = false;

		// Som volta
		AudioListener.volume = 1f;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void AlternarPausa()
	{
		if (menuPausa == null) return;

		if (Time.timeScale > 0f)
			PausarJogo();
		else
			RetomarJogo();
	}
}

using UnityEngine;
using UnityEngine.UI;

public class SomToggleEsc : MonoBehaviour
{
	[Header("Ícone opcional para feedback visual")]
	[SerializeField] private Image iconeSom;
	[SerializeField] private Sprite somLigadoSprite;
	[SerializeField] private Sprite somDesligadoSprite;

	private bool somLigado = true;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			AlternarSom();
		}
	}

	void AlternarSom()
	{
		somLigado = !somLigado;

		// Define o volume global de áudio
		AudioListener.volume = somLigado ? 1f : 0f;

		// Atualiza o ícone, se configurado
		if (iconeSom != null)
		{
			iconeSom.sprite = somLigado ? somLigadoSprite : somDesligadoSprite;
		}
	}

	// Método público (caso queiras chamar pelo botão também)
	public void AlternarSomBotao()
	{
		AlternarSom();
	}
}

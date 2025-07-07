using UnityEngine;
using UnityEngine.UI;

public class SomToggleEsc : MonoBehaviour
{
	[Header("�cone opcional para feedback visual")]
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

		// Define o volume global de �udio
		AudioListener.volume = somLigado ? 1f : 0f;

		// Atualiza o �cone, se configurado
		if (iconeSom != null)
		{
			iconeSom.sprite = somLigado ? somLigadoSprite : somDesligadoSprite;
		}
	}

	// M�todo p�blico (caso queiras chamar pelo bot�o tamb�m)
	public void AlternarSomBotao()
	{
		AlternarSom();
	}
}

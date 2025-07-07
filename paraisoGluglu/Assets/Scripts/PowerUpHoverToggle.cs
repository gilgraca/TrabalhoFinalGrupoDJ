using UnityEngine;
using UnityEngine.EventSystems;

// Script colocado em cada toggle para reagir ao hover e clique
[RequireComponent(typeof(AudioSource))]
public class HoverToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	// Índice deste botão (0 a 5)
	public int indice;

	// Referência ao manager que gere as trocas
	public PowerUpHoverManager hoverManager;

	[Header("Som ao clicar")]
	[SerializeField] private AudioClip somClique;
	private AudioSource audioSource;

	void Start()
	{
		// Pega ou cria o AudioSource
		audioSource = GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f; // Som 2D
	}

	// Quando o mouse entra no botão
	public void OnPointerEnter(PointerEventData eventData)
	{
		hoverManager.MostrarPreview(indice);
	}

	// Quando o mouse sai do botão
	public void OnPointerExit(PointerEventData eventData)
	{
		hoverManager.ResetarPreview();
	}

	// Quando o botão é clicado
	public void OnPointerClick(PointerEventData eventData)
	{
		if (somClique != null)
			audioSource.PlayOneShot(somClique);
	}
}

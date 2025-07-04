using UnityEngine;
using UnityEngine.EventSystems;

// Script colocado em cada toggle para reagir ao hover
public class HoverToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Índice deste botão (0 a 5)
    public int indice;

    // Referência ao manager que gere as trocas
    public PowerUpHoverManager hoverManager;

    // Quando o rato entra no botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverManager.MostrarPreview(indice); // Atualiza vídeo e textos
    }

    // Quando o rato sai do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverManager.ResetarPreview(); // Reapresenta a última habilidade
    }
}

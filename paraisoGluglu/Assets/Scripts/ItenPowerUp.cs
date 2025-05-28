// Importa a biblioteca do Unity
using UnityEngine;

// Define os tipos de power-ups possíveis
public enum TipoPowerUp
{
    Invencibilidade,
    Invisibilidade
}

// Script que deve ser colocado nos prefabs dos power-ups
public class ItemPowerUp : MonoBehaviour
{
    // Escolhe o tipo deste power-up no Inspector
    public TipoPowerUp tipo;

    // Quando o jogador colide com este power-up
    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerUp jogadorPowerUps = other.GetComponent<PlayerPowerUp>();
        if (jogadorPowerUps == null) return;

        switch (tipo)
        {
            case TipoPowerUp.Invencibilidade:
                jogadorPowerUps.AtivarInvencibilidade();
                Debug.Log("Power-up de Invencibilidade ativado!");
                break;
            case TipoPowerUp.Invisibilidade:
                jogadorPowerUps.AtivarInvisibilidade();
                Debug.Log("Power-up de Invisibilidade ativado!");
                break;
        }


        // Destrói o power-up da cena (podes animar/desaparecer com delay se preferires)
        Destroy(gameObject);
    }
}

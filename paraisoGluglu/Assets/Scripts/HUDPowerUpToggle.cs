using UnityEngine;

public class HUDPowerUpToggle : MonoBehaviour
{
    // Referências aos GameObjects das frames individuais
    [Header("Frames das habilidades no HUD")]
    public GameObject frameInvenc;
    public GameObject frameInvis;
    public GameObject frameDash;
    public GameObject frameSpAttack;

    void Start()
    {
        // Desativa tudo primeiro
        frameInvenc.SetActive(false);
        frameInvis.SetActive(false);
        frameDash.SetActive(false);
        frameSpAttack.SetActive(false);

        // Ativa só os que foram escolhidos no menu
        if (GameManager.Instance.usarInvencibilidade)
            frameInvenc.SetActive(true);

        if (GameManager.Instance.usarInvisibilidade)
            frameInvis.SetActive(true);

        if (GameManager.Instance.usarDash)
            frameDash.SetActive(true);

        if (GameManager.Instance.usarAtaqueEspecial)
            frameSpAttack.SetActive(true);

        // Debug
        //Debug.Log("DoubleJump: " + GameManager.Instance.usarDoubleJump); 
        //Debug.Log("Invencível: " + GameManager.Instance.usarInvencibilidade);
        //Debug.Log("Invisível: " + GameManager.Instance.usarInvisibilidade);
        //Debug.Log("Dash: " + GameManager.Instance.usarDash);
        //Debug.Log("Especial: " + GameManager.Instance.usarAtaqueEspecial);
    }

}

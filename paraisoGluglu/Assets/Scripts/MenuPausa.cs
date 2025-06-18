using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    public static bool jogoPausado = false;

    void Start()
    {
        if (menuPausa != null)
            menuPausa.SetActive(false);
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
        // Pausa o tempo do jogo
        Time.timeScale = 0f;

        // Ativa o menu de pausa
        menuPausa.SetActive(true);

        // Marca o jogo como pausado
        jogoPausado = true;

        // Mostra o cursor para navegar no menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Jogo pausado — cursor visível");
    }

    public void RetomarJogo()
    {
        // Retoma o tempo do jogo
        Time.timeScale = 1f;

        // Esconde o menu de pausa
        menuPausa.SetActive(false);

        // Marca o jogo como não pausado
        jogoPausado = false;

        // Esconde o cursor e bloqueia-o para o centro do ecrã
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Jogo retomado — cursor escondido");
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

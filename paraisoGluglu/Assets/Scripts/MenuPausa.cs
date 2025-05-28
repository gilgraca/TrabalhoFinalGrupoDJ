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
        Time.timeScale = 0f;
        menuPausa.SetActive(true);
        jogoPausado = true;
    }

    public void RetomarJogo()
    {
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        jogoPausado = false;
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

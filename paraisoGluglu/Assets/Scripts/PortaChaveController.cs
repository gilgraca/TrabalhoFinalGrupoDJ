// Importa o necessário
using UnityEngine;

public class PortaChaveController : MonoBehaviour
{
    // Porta com o SceneManagement (fica desativada no início)
    public GameObject portaComSceneManagement;

    // Referência ao texto da porta (opcional)
    public PortaTextoController textoController;

    // ID da chave necessária
    public int idChaveNecessaria = 1;

    // Material quando a porta está ativa
    [SerializeField] private Material materialDisponivel;

    // Material quando a porta está inativa
    [SerializeField] private Material materialIndisponivel;

    // Objeto que tem o renderer do indicador
    [SerializeField] private Renderer indicadorRenderer;


    void Start()
    {
        // Se existir a porta
        if (portaComSceneManagement != null)
        {
            // Desativa apenas o script SceneManagement
            SceneManagement sceneScript = portaComSceneManagement.GetComponent<SceneManagement>();
            if (sceneScript != null)
            {
                sceneScript.enabled = false;
                //Debug.Log("SceneManagement.cs foi desativado no início.");
            }
        }
        if (indicadorRenderer != null && materialIndisponivel != null)
        {
            indicadorRenderer.material = materialIndisponivel;
        }
    }

    public void TentarAbrirPorta(int idChave)
    {
        // Verifica se é a chave correta
        if (idChave == idChaveNecessaria)
        {
            // Ativa o script quando tiver a chave
            SceneManagement sceneScript = portaComSceneManagement.GetComponent<SceneManagement>();
            if (sceneScript != null)
            {
                sceneScript.enabled = true;
                //Debug.Log("SceneManagement.cs foi ativado.");
            }

            // Muda o texto se for necessário
            if (textoController != null)
            {
                textoController.MudarTexto();
            }

            indicadorRenderer.material = materialDisponivel;

            //Debug.Log("Porta ativada com a chave correta.");
        }
    }
}

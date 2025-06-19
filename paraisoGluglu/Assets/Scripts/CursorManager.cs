
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // Define se o cursor deve estar visível (true = menus, false = gameplay)
    [SerializeField] private bool mostrarCursor = false;

    void Start()
    {
        if (mostrarCursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //Debug.Log("Cursor visível e desbloqueado (menu).");
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //Debug.Log("Cursor escondido e bloqueado (nível).");
        }
    }
}

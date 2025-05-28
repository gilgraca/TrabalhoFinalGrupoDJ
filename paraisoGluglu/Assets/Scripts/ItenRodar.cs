using UnityEngine;

public class ItenRodar : MonoBehaviour
{
    // Velocidade de rotação
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    void Update()
    {
        // Faz o objeto girar ao redor de seus eixos locais na velocidade especificada
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}

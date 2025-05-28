// Script responsável por dar pontos ao jogador ao apanhar milhos e reproduzir som
using UnityEngine;
public class ItenMilho : MonoBehaviour
{
    // Quantidade de pontos que esta moeda adiciona
    public int pointToAdd;

    // Som a ser tocado ao apanhar a moeda
    //private AudioSource CoinPickupEffect;

    // Velocidade de rotação da moeda (graus por segundo)
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    void Start()
    {
        // Referência ao AudioSource ligado à moeda
        //CoinPickupEffect = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Rotate object around its local axes at rotationSpeed degrees/second
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Garante que só o jogador pode apanhar a moeda
        if (other.GetComponent<PlayerToast>() == null)
            return;

        // Adiciona os pontos ao ScoreManager
        ScoreManager.AddPoints(pointToAdd);

        // Toca o som de apanhar moeda
        //CoinPickupEffect.Play();

        // Esconde o Mesh do milho (opcional — dá sensação de recolhida antes de desaparecer)
        GetComponentInChildren<MeshRenderer>().enabled = false;

        // Avisa o NivelTracker local
        FindFirstObjectByType<NivelTracker>()?.AdicionarMilho();

        // Destroi o objeto após 1 segundo (dá tempo para o som tocar)
        Destroy(gameObject, 1.0f);
    }
}
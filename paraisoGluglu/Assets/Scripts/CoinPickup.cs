// Script responsável por dar pontos ao jogador ao apanhar moedas e reproduzir som
using UnityEngine;
public class CoinPickup : MonoBehaviour
{
    // Quantidade de pontos que esta moeda adiciona
    public int pointToAdd;
    // Som a ser tocado ao apanhar a moeda
    //private AudioSource CoinPickupEffect;
    void Start()
    {
        // Referência ao AudioSource ligado à moeda
        //CoinPickupEffect = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        // Garante que só o jogador pode apanhar a moeda
        if (other.GetComponent<Player>() == null)
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
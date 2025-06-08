// Script responsável por dar pontos ao jogador ao apanhar milhos e reproduzir som
using System.Collections;
using UnityEngine;
public class ItenMilho : MonoBehaviour
{
    // Quantidade de pontos que esta moeda adiciona
    public int pointToAdd;

    // Som a ser tocado ao apanhar a moeda
    //private AudioSource CoinPickupEffect;

    // Velocidade de rotação da moeda (graus por segundo)
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    // Flag para evitar múltiplas ativações do trigger
    private bool foiApanhado = false;
    // Se pode ou não ativar novamente
    private bool podeAtivarToastManual = true;
    // Tempo de espera entre ativações do toast com tecla I
    [SerializeField] private float cooldownToastManual = 10f;
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
        // Se já foi apanhado, ignora
        if (foiApanhado)
            return;

        // Garante que só o jogador pode apanhar a moeda
        if (other.GetComponent<PlayerToast>() == null)
            return;

        // Marca como apanhado
        foiApanhado = true;

        // Adiciona os pontos ao ScoreManager
        ScoreManager.AddPoints(pointToAdd);

        // Atualiza o total de milhos apanhados
        GameManager.Instance.milhoTotal += 1;

        // Mostrar Toast Global
        FindFirstObjectByType<NivelTracker>()?.MostrarToastManual();

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
using UnityEngine;

public class TesteSpawnChaves : MonoBehaviour
{
    // Prefab da chave a instanciar
    [SerializeField] private GameObject chavePrefab;

    // Posições onde as chaves serão colocadas
    [SerializeField] private Transform[] pontosDeSpawn;

    // Offset vertical para evitar enterrar
    [SerializeField] private float alturaOffset = 0.5f;

    // Tag usada nos milhos (neste caso "Corn")
    [SerializeField] private string tagMilho = "Corn";

    void Update()
    {
        // Verifica se a tecla C foi carregada
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Log para confirmar que o input funcionou
            Debug.Log("Tecla C pressionada: a transformar todos os Corn em chaves.");

            // Procura todos os objetos com a tag "Corn"
            GameObject[] milhos = GameObject.FindGameObjectsWithTag(tagMilho);

            // Destrói cada um dos Corn encontrados
            foreach (GameObject milho in milhos)
            {
                Destroy(milho);
            }

            // Para cada ponto de spawn definido
            foreach (Transform ponto in pontosDeSpawn)
            {
                // Calcula a posição com um pequeno offset no eixo Y
                Vector3 posicao = ponto.position + Vector3.up * alturaOffset;

                // Instancia uma chave nessa posição
                Instantiate(chavePrefab, posicao, Quaternion.identity);

                // Log para ver onde foi colocada cada chave
                Debug.Log("Chave colocada em: " + posicao);
            }
        }
    }
}

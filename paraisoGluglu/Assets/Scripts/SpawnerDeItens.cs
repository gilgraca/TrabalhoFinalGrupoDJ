// Importa o necessário
using UnityEngine;

public class SpawnerDeItens : MonoBehaviour
{
    // Prefab do milho
    public GameObject milhoPrefab;

    // Prefab da chave (tem o script Porta.cs)
    public GameObject chavePrefab;

    // Posições onde os objetos vão aparecer
    public Transform[] pontosDeSpawn;

    // ID da chave a ser usada
    public int idChave = 1;

    // Offset para a altura da chave
    [SerializeField] private float alturaOffsetChave = 0.5f;

    void Start()
    {
        // Verifica se há posições definidas
        if (pontosDeSpawn.Length == 0)
        {
            //Debug.LogError("Nenhum ponto de spawn definido.");
            return;
        }

        // Escolhe aleatoriamente onde vai aparecer a chave
        int indiceChave = Random.Range(5, pontosDeSpawn.Length); //os primerios 5 milhos são no corredor

        for (int i = 0; i < pontosDeSpawn.Length; i++)
        {
            // Se for o índice sorteado, mete a chave
            if (i == indiceChave)
            {
                // Calcula posição um pouco acima do chão
                Vector3 posicaoComOffset = pontosDeSpawn[i].position + Vector3.up * alturaOffsetChave;

                // Instancia a chave nessa nova posição
                GameObject chave = Instantiate(chavePrefab, posicaoComOffset, Quaternion.identity);

                // Passa o ID da chave ao script Porta
                Chave portaScript = chave.GetComponent<Chave>();
                if (portaScript != null)
                {
                    portaScript.idChave = idChave;
                }
            }
            else
            {
                // Caso contrário, mete milho
                Instantiate(milhoPrefab, pontosDeSpawn[i].position, Quaternion.identity);
            }
        }
    }
}

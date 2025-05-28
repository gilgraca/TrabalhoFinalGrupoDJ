using UnityEngine;

// Script para a destruição da caixa
public class Caixa : MonoBehaviour
{
    // Prefab da tábua que vai saltar
    public GameObject tabuaPrefab;

    // Quantidade de tábuas que vão saltar
    public int quantidadeTabuas = 5;

    // Força da explosão que empurra as tábuas
    public float forcaExplosao = 300f;

    // Raio da explosão
    public float raioExplosao = 2f;

    // Método chamado para destruir a caixa
    public void QuebrarCaixa()
    {
        // LOG
        //Debug.Log("Caixa destruída!");

        // Cria tábuas no mesmo local
        for (int i = 0; i < quantidadeTabuas; i++)
        {
            // Gera posição aleatória perto da caixa
            Vector3 pos = transform.position + Random.insideUnitSphere * 0.5f;

            // Cria a tábua
            GameObject tabua = Instantiate(tabuaPrefab, pos, Random.rotation);

            // Obtém o Rigidbody e ativa
            Rigidbody rb = tabua.GetComponent<Rigidbody>();
            
            // Garante que o Rigidbody está ativo
            rb.isKinematic = false;

            // Aplica força de explosão
            rb.AddExplosionForce(forcaExplosao, transform.position, raioExplosao);

            // Destroi a tábua após 3 segundos
            Destroy(tabua, 3f);

            // LOG para verificar destruição futura
            //Debug.Log("Tábua criada e será destruída em 3 segundos.");
        }
        // Destroi o objeto da caixa
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;

// Script reutilizável para gerir vida e dano de qualquer inimigo
public class InimigoVida : MonoBehaviour
{
    // Vida inicial do inimigo (podes ajustar no Inspector)
    [SerializeField] private int vida = 3;
    [SerializeField] private Animator animator;
    // Referência ao Rigidbody do inimigo
    private Rigidbody rb;
    // Material que será usado para piscar (vermelho)
    [SerializeField] private Material materialVermelhoFlash;
    // Armazena os materiais originais para restaurar depois
    private Material[][] materiaisOriginais;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Guarda os materiais originais de cada Renderer
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        materiaisOriginais = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            materiaisOriginais[i] = renderers[i].materials;
        }
    }
    public void Update()
    {
        if (rb)
        {
            // Isto assegura que o valor é sempre positivo e correto para animações de andar/parar
            float velocidadeHorizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

            // Define o valor da "speed" no Animator com base na velocidade horizontal total
            if (animator) animator.SetFloat("Speed", velocidadeHorizontal);
        }

    }

    // Método público para receber dano
    public void LevarDano(int dano)
    {
        // Reduz a vida
        vida -= dano;
        if (animator) animator.SetTrigger("Damaged");

        StartCoroutine(PiscarVermelho());

        // LOG para testar o dano recebido
        //Debug.Log($"{gameObject.name} levou {dano} de dano. Vida restante: {vida}");

        // Se a vida for 0 ou menor, o inimigo morre
        if (vida <= 0)
        {
            // LOG de morte
            //Debug.Log($"{gameObject.name} morreu!");

            // Desativa todos os scripts de comportamento
            foreach (var comp in GetComponents<MonoBehaviour>())
            {
                if (comp != this && comp.enabled) // Não desabilita o próprio InimigoVida
                    comp.enabled = false;
            }

            // Desativa o Collider do filho "ZonaDano"
            Transform zonaDano = transform.Find("ZonaDano");
            if (zonaDano != null)
            {
                Collider col = zonaDano.GetComponent<Collider>();
                if (col != null)
                    col.enabled = false;
            }

            // Animação
            if (animator) animator.SetTrigger("Died");
            // Destroi o inimigo
            StartCoroutine(DestruirAposTempo(2f));
        }


    }
    IEnumerator DestruirAposTempo(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        Destroy(gameObject);
    }
    // Método auxiliar para saber se o inimigo ainda está vivo
    public bool EstaVivo()
    {
        return vida > 0;
    }
    // Efeito visual de dano: pisca vermelho 3 vezes rapidamente
    private IEnumerator PiscarVermelho()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        int vezes = 3;
        float intervalo = 0.1f;

        for (int v = 0; v < vezes; v++)
        {
            // Troca para o material vermelho
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] novosMateriais = new Material[renderers[i].materials.Length];
                for (int j = 0; j < novosMateriais.Length; j++)
                {
                    novosMateriais[j] = materialVermelhoFlash;
                }
                renderers[i].materials = novosMateriais;
            }

            yield return new WaitForSeconds(intervalo);

            // Restaura os materiais originais
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = materiaisOriginais[i];
            }

            yield return new WaitForSeconds(intervalo);
        }

        //Debug.Log("Inimigo piscou vermelho com troca de materiais.");
    }

}

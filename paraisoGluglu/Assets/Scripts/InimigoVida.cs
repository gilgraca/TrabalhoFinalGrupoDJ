using UnityEngine;
using System.Collections;

// Script reutilizável para gerir vida e dano de qualquer inimigo
public class InimigoVida : MonoBehaviour
{
    // Vida inicial do inimigo (podes ajustar no Inspector)
    [SerializeField] private int vida = 3;
    [SerializeField] private Animator animator;
    // Método público para receber dano
    public void LevarDano(int dano)
    {
        // Reduz a vida
        vida -= dano;
        if(animator) animator.SetTrigger("Damaged");

        // LOG para testar o dano recebido
        //Debug.Log($"{gameObject.name} levou {dano} de dano. Vida restante: {vida}");

        // Se a vida for 0 ou menor, o inimigo morre
        if (vida <= 0)
        {
            // LOG de morte
            //Debug.Log($"{gameObject.name} morreu!");

            // Destroi o inimigo
            // Falta animação de morte
            if (animator) animator.SetTrigger("Died");
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
}

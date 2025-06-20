// Importa o necessário
using UnityEngine;
using System.Collections.Generic;

public class ChavePlayer : MonoBehaviour
{
    // Lista de chaves que o jogador tem
    [HideInInspector] public List<int> chavesApanhadas = new List<int>();

    // Adiciona uma chave à lista
    public void AdicionarChave(int idChave)
    {
        // Só adiciona se ainda não tiver essa chave
        if (!chavesApanhadas.Contains(idChave))
        {
            chavesApanhadas.Add(idChave);
            Debug.Log("Chave " + idChave + " apanhada.");
        }
    }

    // Verifica se o jogador tem uma chave
    public bool TemChave(int idChave)
    {
        return chavesApanhadas.Contains(idChave);
    }

    void Awake()
    {
        // Garante que o jogador tem a tag correta
        if (tag != "Player")
        {
            tag = "Player";
        }
    }
}

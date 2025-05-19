// Script responsável por gerir a pontuação do jogador e atualizá-la visualmente na UI
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    // Pontuação atual (armazenada estaticamente para acesso global)
    private static int score;
    // Referência ao ScoreManager da cena (usado para atualizar UI)
    private static ScoreManager instance;
    // Texto da UI onde a pontuação é mostrada
    public TMP_Text textoPontuacao;
    void Awake()
    {
        // Guarda esta instância para uso nos métodos estáticos
        instance = this;
    }
    void Start()
    {
        // Recupera a pontuação guardada anteriormente (se existir)
        score = PlayerPrefs.GetInt("score");
        if (score < 0) { score = 0; }
        // Atualiza o texto da UI com a pontuação inicial
        instance.textoPontuacao.text = score.ToString("D3");
    }
    // Método público e estático para adicionar pontos
    public static void AddPoints(int points)
    {
        score += points;
        // Guarda nos PlayerPrefs
        PlayerPrefs.SetInt("score", score);
        // Atualiza a UI, se o ScoreManager e o texto estiverem ativos
        if (instance != null && instance.textoPontuacao != null)
            instance.textoPontuacao.text = score.ToString("D3");
    }
    // Devolve a pontuação atual
    public static int GetPoints()
    {
        return score;
    }
    // Reseta a pontuação para zero
    public static void Reset()
    {
        score = 0;
        PlayerPrefs.SetInt("score", score);
        // Atualiza a UI, se o ScoreManager e o texto estiverem ativos
        if (instance != null && instance.textoPontuacao != null)
            instance.textoPontuacao.text = score.ToString("D3");
    }
}
using UnityEngine;
using System.Collections.Generic;

public class Chaves : MonoBehaviour
{
	[HideInInspector]
	public List<int> ChavesDoJogador = new List<int>();

	public void AdicionarChave(int idChave)
	{
		if (!ChavesDoJogador.Contains(idChave))
		{
			ChavesDoJogador.Add(idChave);
			Debug.Log("Chave apanhada! Total: " + ChavesDoJogador.Count);
		}
	}

	public bool TemChave(int idChave)
	{
		return ChavesDoJogador.Contains(idChave);
	}

	public void LimparChaves()
	{
		ChavesDoJogador.Clear();
	}

	void Awake()
	{
		if (gameObject.tag != "Player")
		{
			gameObject.tag = "Player";
		}
	}
}

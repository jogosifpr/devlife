using UnityEngine;
using System.Collections;

public class Dinheiro : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static int ObterValor(){

		int dinheiro = int.Parse(GameObject.Find("Dinheiro").transform.FindChild("Valor").GetComponent<GUIText>().text);

		return dinheiro;
	}

	public static void Descontar(int valor){

		int dinheiro = Dinheiro.ObterValor() - valor;

		GameObject.Find("Dinheiro").transform.FindChild("Valor").GetComponent<GUIText>().text = dinheiro.ToString();

	}

	public static void Receber(int valor){

		int dinheiro = Dinheiro.ObterValor() + valor;

        if (dinheiro >= 9999999) dinheiro = 9999999;

        GameObject.Find("Dinheiro").transform.FindChild("Valor").GetComponent<GUIText>().text = dinheiro.ToString();

	}

}

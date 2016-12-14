using UnityEngine;
using System.Collections;

public class TelaEstacaoTrabalhoScript : MonoBehaviour {

	private static bool telaCriada = false;

	// Use this for initialization
	void Start () {
		if(!telaCriada){
			//Salvo os dados do personagem.
			DontDestroyOnLoad(this);
			telaCriada = true;
		}else{
			Destroy(this.gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

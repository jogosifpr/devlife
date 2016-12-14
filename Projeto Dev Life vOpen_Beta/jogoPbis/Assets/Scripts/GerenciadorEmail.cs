using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GerenciadorEmail : MonoBehaviour {

	private static bool GerenciadorCriado;
	private List<int> codigoSortiados = new List<int>();
	private LeituraArquivo leituraArquivo;

	// Use this for initialization
	void Start () {

		leituraArquivo = GameObject.Find("LeituraArquivo").GetComponent<LeituraArquivo>() as LeituraArquivo;

		//Verfifica se existe um painel criado no jogo se nao destroi este gameObject.
		if(!GerenciadorCriado){
			//Salvo os dados do painel.
			DontDestroyOnLoad(this);
			GerenciadorCriado = true;
		}else{
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool addCodigo(int codigoEmail){

		if(!codigoSortiados.Contains(codigoEmail)){
			codigoSortiados.Add(codigoEmail);
			return true;
		}

		return false;
	}

	public int disponibilidadeEmails(){
		int qtdDisponivel = 0;

		foreach(int codigo in leituraArquivo.obterIds()) {
			if(!codigoSortiados.Contains(codigo)){
				qtdDisponivel++;
			}
		}

		return qtdDisponivel;
	}

	public GameObject obterCaixaEmail(int codigo){
		
		foreach(GameObject caixaEmail in GameObject.FindGameObjectsWithTag("Email")){
			if(caixaEmail.GetComponent<Email>().codigoEmail == codigo){
				return caixaEmail;
			}
		}
		
		return null;
	}
}

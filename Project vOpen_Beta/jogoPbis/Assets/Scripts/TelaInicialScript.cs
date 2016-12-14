using UnityEngine;
using System.Collections;

public class TelaInicialScript : MonoBehaviour {
	
	private Vector3 posicao;
	
	public GameObject tela;
	public GameObject PreProducaoPronto;

	public Texture trabalharAtivo;
	public Texture trabalharInativo;

	public GUITexture iconeTrabalhar;

	private GameObject telaInicial;
	private GameObject preProducao;
	
	private PreProducao scriptPreProducao;

	private static GameObject TelaAreaDeTrabalho;
	private static GameObject[] todosProjetos;
	private GameObject prefab;
	private float posicaoX;
	private float posicaoY;
	private static Projeto projetoAtivo;
	
	private int qtdProjetos = 0;
	
	public bool documentacao = false;
	public bool confirma = false;	
	public bool trabalhar = false;

		
	// Use this for initialization
	void Start(){
		TelaAreaDeTrabalho = GameObject.Find("TelaAreaDeTrabalho");
		// preProducao = GameObject.Find ("TelaEstacaoTrabalho").transform.FindChild ("PreProducao").gameObject;

		telaInicial = GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").gameObject;

		//DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame.
	void Update () {
		
	}

	void OnMouseDown(){
		if (documentacao) {
				
		} else if (confirma) {
						
		} else if (trabalhar) {

			if(projetoAtivo!= null){
				//Debug.Log ("Ao trabalhar passara o periodo");
				//telaInicial.SetActive(false);
				//Color sprite = Color.white;
				//sprite.a = 255;
				//preProducao.GetComponent<GUITexture>().color = sprite;
				//preProducao.SetActive(true);
				//scriptPreProducao = GameObject.Find ("TelaEstacaoTrabalho").transform.FindChild ("PreProducao").gameObject.GetComponent<PreProducao>();
				//scriptPreProducao.setProjeto(projetoAtivo);
                projetoAtivo.trabalhar();
			}else{
				// nenhum projeto selecionado
			}

		} else {
			
			//posicaoX = 0.16f;
			//posicaoY = 214.1f;
			//posicao = new Vector3 (posicaoX, posicaoY, 1);
			//Instantiate (tela, posicao, tela.transform.rotation);
			
		}
		
	}
	
	public void inicializarProjetos(){
		todosProjetos =  GameObject.FindGameObjectsWithTag("Projeto");
		foreach (GameObject projetinho in todosProjetos) {
			Projeto p = (Projeto)projetinho.GetComponent (typeof(Projeto));
			p.ativo = false;
			if(p.iconeDesativado!=null){
				p.icone.texture = p.iconeDesativado;
			}
			//if(!p.isProjetoCriado()){
			//	DontDestroyOnLoad (projetinho);
			//	Debug.Log ("Teste");
			//	p.setProjetoCriado(true);
			//}
		}
	}

	public string aumentarQtd(){
		qtdProjetos++;
		return qtdProjetos.ToString();
	}
	
	
	public void setProjeto(Projeto p){
		projetoAtivo = p;
		if (p == null) {
			telaInicial.transform.FindChild("botaoTrabalhar").GetComponent<GUITexture>().texture = trabalharInativo;
		} else {
			telaInicial.transform.FindChild("botaoTrabalhar").GetComponent<GUITexture>().texture = trabalharAtivo;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Email : MonoBehaviour {

	public GameObject telaResposta;
	public bool botaoCaixaEntrada;
	public bool botaoLixeirar;
	private bool voltar = false;
	private static GameObject[] caixaEmail;
	private Vector3 posicao;

	public GameObject email;
	private static GameObject TelaAreaDeTrabalho;
	private GameObject prefab;
	private GameObject[] remetenteEmail;
	private GameObject[] tituloEmail;
	private GameObject[] dataEmail;

	private static float posicaoX = 0.563f;
	private static float posicaoY = 0.807f;
	private static bool caixaEmailCriada = false;

	private LeituraArquivo leituraArquivo;
	private static TelaInicialScript telaInicialScript;
	private Resposta resposta;

	private int qtdEmailSorteado;
	private int qtdMaxSorteado = 5;
	private bool isValido;
	public int codigoEmail;
	public string dataEnvio;
	public bool isRespondido;
	public string escolhaResposta;

	private GerenciadorEmail gerenciador;


	// Use this for initialization
	void Start(){

		if(botaoCaixaEntrada){
			telaResposta = GameObject.Find("RespostaEmail(Clone)");
		}

		leituraArquivo = GameObject.Find("LeituraArquivo").GetComponent<LeituraArquivo>() as LeituraArquivo;
		gerenciador = GameObject.Find ("TelaEmail").GetComponent<GerenciadorEmail>() as GerenciadorEmail;
		//estacaoTrabalho = GameObject.Find("TelaEstacaoTrabalho").GetComponent<EstacaoDeTrabalho>() as EstacaoDeTrabalho;
	}

	// Update is called once per frame.
	void Update () {

		if(voltar){
			Destroy (telaResposta);
		}
	}

	void OnMouseDown(){

		if(botaoCaixaEntrada){
			Destroy(GameObject.Find("RespostaEmail(Clone)"));
			AlterarCaixaEmail(true);
		}

		if(botaoLixeirar){

			int codigo = GameObject.Find ("RespostaEmail(Clone)").GetComponent<Resposta>().idEmail;
			Destroy (GameObject.Find ("RespostaEmail(Clone)"));
			AlterarCaixaEmail (true);
			Destroy(gerenciador.obterCaixaEmail(codigo));
		}
	}

	public void TelaResposta(){

		AlterarCaixaEmail(false);

		if(this.escolhaResposta.Equals("aceitar")){
			this.gameObject.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("caixaEmailAceito", typeof(Sprite)) as Sprite;
		}else if(this.escolhaResposta.Equals("recusar")){
			this.gameObject.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("caixaEmailRecusado", typeof(Sprite)) as Sprite;
		}else{
			this.gameObject.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("caixaEmailOk", typeof(Sprite)) as Sprite;
		}

		posicao = new Vector3 (0.5f, 0.55f, 1);
		Instantiate (telaResposta, posicao, telaResposta.transform.rotation);

		GameObject.Find("RespostaEmail(Clone)").transform.FindChild("Foto").GetComponent<GUITexture>().texture = Resources.Load<Texture2D>(leituraArquivo.Foto(this.codigoEmail));
		GameObject.Find("RespostaEmail(Clone)").transform.FindChild("Titulo").GetComponent<GUIText>().text = leituraArquivo.Titulo(this.codigoEmail);
		GameObject.Find("RespostaEmail(Clone)").transform.FindChild("Remetente").GetComponent<GUIText>().text = leituraArquivo.Remetente(this.codigoEmail);
		GameObject.Find("RespostaEmail(Clone)").transform.FindChild("Endereco").GetComponent<GUIText>().text = leituraArquivo.Endereco(this.codigoEmail);
		GameObject.Find("RespostaEmail(Clone)").transform.FindChild("Localizacao").GetComponent<GUIText>().text = leituraArquivo.Localizacao(this.codigoEmail);
		GameObject.Find("RespostaEmail(Clone)").transform.FindChild("UI/Canvas/Mensagem/Text").GetComponent<Text>().text = leituraArquivo.Mensagem(this.codigoEmail);

		//Adicionando o id, estado e resposta do e-mail no botoes de tela de resposta.
		resposta = GameObject.Find("RespostaEmail(Clone)").GetComponent<Resposta>() as Resposta;
		resposta.idEmail = this.codigoEmail;
		resposta.isRespondido = this.isRespondido;
		resposta.escolha = this.escolhaResposta;

	}

	public void AbrirEmail(){

		gerenciador = GameObject.Find ("TelaEmail").GetComponent<GerenciadorEmail>() as GerenciadorEmail;

		//Se os email foram gerados, sera exibido os emails gerados.
		if(caixaEmailCriada){
			//leituraArquivo.CarregarArquivo();
			AlterarCaixaEmail(true);

		}else{

			while(!isValido){
				qtdEmailSorteado = Random.Range(1, qtdMaxSorteado);
				if(gerenciador.disponibilidadeEmails() >= qtdEmailSorteado){
					isValido = true;
				}else if(gerenciador.disponibilidadeEmails() == 0){
					qtdEmailSorteado = 0;
					break;
				}
			}

			if(isValido){
				GerarCaixaEmail();
			}

			//Alterando o estado da caixa de email para criado.
			caixaEmailCriada = true;
		}

	}

	public void GerarCaixaEmail(){

		leituraArquivo = GameObject.Find("LeituraArquivo").GetComponent<LeituraArquivo>() as LeituraArquivo;
		leituraArquivo.CarregarArquivo();
		isValido = false;

		for(int i = 1; i<=qtdEmailSorteado; i++){

			while(!isValido){
				codigoEmail = Random.Range(1, leituraArquivo.getQuantidadeEmails()+1);
				isValido = gerenciador.addCodigo(codigoEmail);
			}

			if(GameObject.Find("Painel")){
				dataEnvio = "Dia " + Data.ObterData();
			}else{
				dataEnvio = "";
			}

			posicao = new Vector3(posicaoX, posicaoY, 1f);

			GameObject gameObjectFilha = Instantiate(email, posicao, email.transform.rotation) as GameObject;
			gameObjectFilha.transform.SetParent(GameObject.Find("GerenciadorEmail").transform);
			gameObjectFilha.transform.localScale = new Vector3(1f,1f);
			gameObjectFilha.transform.SetAsFirstSibling();

			posicaoY -= 0.1f;

			isValido = false;
		}

		remetenteEmail = GameObject.FindGameObjectsWithTag("Remetente");

		foreach(GameObject remetente in remetenteEmail){
			Email scriptEmail = remetente.transform.parent.GetComponent<Email>() as Email;
			remetente.GetComponent<Text>().text = leituraArquivo.Remetente(scriptEmail.codigoEmail);
		}

		tituloEmail = GameObject.FindGameObjectsWithTag("Titulo");

		foreach(GameObject titulo in tituloEmail){
			Email scriptEmail = titulo.transform.parent.GetComponent<Email>() as Email;
			titulo.GetComponent<Text>().text = leituraArquivo.Titulo(scriptEmail.codigoEmail);
		}

		dataEmail = GameObject.FindGameObjectsWithTag("Data");

		foreach(GameObject data in dataEmail){
			data.GetComponent<Text>().text = data.transform.parent.gameObject.GetComponent<Email>().dataEnvio;
		}

		AlterarCaixaEmail(true);
	}

	public void AlterarCaixaEmail(bool estado){
		// Altera o estado da caixa de e-mail de acordo com envio do parametro.
		foreach(Transform child in GameObject.Find("TelaEmail").transform) {
			child.gameObject.SetActive(estado);
		}
	}

	public void deletarBarraTarefa(){
		// Destroi da cena a barra de tareffa do email.
		Destroy(GameObject.Find("barraTarefaEmail(Clone)"));
	}

	public void DesativarEmail(){
		caixaEmailCriada = false;
	}
}

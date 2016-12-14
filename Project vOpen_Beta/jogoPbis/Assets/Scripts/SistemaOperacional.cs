using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaOperacional : MonoBehaviour {

	[Header("Botoes da area de trabalaho")]
	public bool botaoDesligar;
	public bool botaoFechar;
	[Header("Icones do area de trabalaho")]
	public bool IconeEmail;
	public bool IconeNavegador;
	public bool IconeEstacaoTrabalho;
	
	private static GameObject TelaAreaDeTrabalho;
	[Header("Tela que sera aberta/fechada")]
	public GameObject tela;
	[Header("Barra de tarefa da tela")]
	public GameObject barraTarefa;

	// Variavel para alterar opacidade das imagens.
	private SpriteRenderer spriteRenderer;
	private static float alpha;

	//Variavel para definir posicoes
	private float posicaoX = 0.28f;
	private float posicaoY = 0.1f;
	private Vector3 posicao;

	//Variavel para armazenar os Scripts
	private Email email;
	private Navegador nav;
	private TelaInicialScript estacaoTrabalho;

	//Variavel para verificar os estados atuais
	private static bool abrir;
	private static bool fechar;
	
	// Use this for initialization
	void Start () {

		//Se estiver marcado icone email.
		if(IconeEmail || botaoDesligar){
            email = (Email)Resources.Load("Prefabs/CaixaEmail", typeof(Email)) as Email;
            tela = GameObject.Find("TelaEmail").transform.FindChild("CaixaEntrada").gameObject;

            //GameObject objTemp = (GameObject)Instantiate(Resources.Load("Prefabs/CaixaEmail"));
            //email = objTemp.GetComponent<Email>() as Email;
            /*
            email = GameObject.Find("TelaEmail")
                                        .transform
                                        .FindChild("CaixaEntrada")
                                        .GetComponent<Email>() as Email;
            */

            

        }

        //Se estiver marcado icone navegador.
        if (IconeNavegador){
			tela = GameObject.Find("TelaNavegador").transform.FindChild("TelaNoticias").gameObject;
		}

		//Se estiver marcado icone estacao de trabalho.
		if(IconeEstacaoTrabalho){
			tela = GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").gameObject;
			estacaoTrabalho = GameObject.Find("TelaEstacaoTrabalho")
										.transform
										.FindChild("TelaInicial")
										.GetComponent<TelaInicialScript>() as TelaInicialScript;
		}


        if (GameObject.Find("Personagem")){
            if (Personagem.isTutorialComputador){
                Personagem.isTutorialComputador = false;
                GameObject.Find("Personagem").GetComponent<Tutorial>().abrirPainelDialogo();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
		// Abre tela solicitada.
		if(abrir){ 
			Abrir();
		}

		// Fecha a tela solicitada.
		if(fechar){
			Fechar();
		}
	}

	// Metodo para açao do click do mouse.
	void OnMouseDown(){

        if (!Tutorial.isTutorial){
            //Se clicou no botao fechar.
            if (botaoFechar){
                fechar = true;

                //Se clicou no botao desligar.
            }else if (botaoDesligar){
                this.DesligarComputador();

                //Se clicou em algum icone da estacao de trabalho.
            }else if (IconeEmail || IconeNavegador || IconeEstacaoTrabalho){
                this.AbrirTela();
            }
        }
		
	}

	// Metodo para desligar o computador.
	void DesligarComputador(){
		
		if(GameObject.Find("Personagem")){
            Personagem.Desbloquear();
		}

        if (GameObject.Find("Comportamento")){
            Comportamento.Obter(Comportamento.Clique.COMPUTADOR);
        }

        alpha = 0.1f;

        SceneManager.LoadScene("Casa");
	}

	// Metodo para abrir tela de acordo com icone clicado.
	void AbrirTela(){

		//Recebendo o GameObject da area de trabalho.
		TelaAreaDeTrabalho = GameObject.Find("TelaAreaDeTrabalho");
		
		//Se o tipo do icone for email.
		if(IconeEmail){

            if (GameObject.Find("Comportamento")){
                Comportamento.Obter(Comportamento.Clique.EMAIL);
            }

            GameObject.Find("TelaEmail").transform.FindChild("CanvasEmail").gameObject.SetActive(true);
			
			if(!GameObject.Find("TelaEmail")){
				tela.SetActive(true);
			}

            email.AbrirEmail();
		}
		
		if(IconeNavegador){

            if (GameObject.Find("Comportamento")){
                Comportamento.Obter(Comportamento.Clique.NAVEGADOR);
            }

        }
		
		if(IconeEstacaoTrabalho){

            if (GameObject.Find("Comportamento")){
                Comportamento.Obter(Comportamento.Clique.TRABALHO);
            }

            tela.SetActive(true);
			estacaoTrabalho.inicializarProjetos();
		}
		
		//Instancia a barra de tarefa.
		posicao = new Vector3(posicaoX, posicaoY,1);
		Instantiate(barraTarefa, posicao, barraTarefa.transform.rotation);
		
		TelaAreaDeTrabalho.SetActive(false);
		tela.SetActive(true);
		abrir = true;
	}

	// Metodo para alterar opacidade da tela do sistema operacional.
	void AplicarAlteracaoImagem(){
		GameObject[] telas = GameObject.FindGameObjectsWithTag("Imagem");
		
		foreach (GameObject imagem in telas){
			imagem.GetComponent<GUITexture>().color = new Color(0.5f,0.5f,0.5f, alpha);
		}
	}

	// Metodo para abrir tela do sistema operacional.
	void Abrir(){

		this.AplicarAlteracaoImagem();

		if(alpha > 0.5f){
			abrir = false;
			alpha = 0.5f;
		}else{
			alpha = alpha + 0.01f;
		}
	}

	// Metodo para fechar tela do sistema operacional.
	void Fechar(){

		this.AplicarAlteracaoImagem();

		if(alpha <= 0f){
			fechar = false;
			alpha = 0.1f;
		}else{
			alpha = alpha - 0.01f;
		}

		if(!fechar){ //Aplica alteraçao da tela para voltar para area de trabalho.

			if(GameObject.Find("TelaInicial")){
				GameObject.Find("TelaInicial").SetActive(false);
				Destroy(GameObject.Find("barraTarefaEstacaoTrabalho(Clone)"));

			}else if(GameObject.Find("telaDocumentacao(Clone)")){
				Destroy(GameObject.Find("telaDocumentacao(Clone)"));

			} else if(GameObject.Find("ConfiguracoesProjeto(Clone)")){
				Destroy(GameObject.Find("ConfiguracoesProjeto(Clone)"));

			}else if(GameObject.Find("TelaNoticias")){
				GameObject.Find("TelaNoticias").SetActive(false);
				Destroy(GameObject.Find("barraTarefaNavegador(Clone)"));

			}else if(GameObject.Find("TelaCurso")){
				GameObject.Find("TelaCurso").SetActive(false);
				Destroy(GameObject.Find("barraTarefaNavegador(Clone)"));

			}else if(GameObject.Find("CaixaEntrada")){
				GameObject.Find("CaixaEntrada").SetActive(false);
				GameObject.Find("TelaEmail").transform.FindChild("CanvasEmail").gameObject.SetActive(false);
				email.deletarBarraTarefa();

				if(GameObject.Find("RespostaEmail(Clone)")){
					Destroy(GameObject.Find("RespostaEmail(Clone)"));
					email.deletarBarraTarefa();
				}
			}

			TelaAreaDeTrabalho.SetActive(true);
		}
	}
}
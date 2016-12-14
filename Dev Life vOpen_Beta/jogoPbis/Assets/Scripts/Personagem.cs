using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Personagem : MonoBehaviour {
	
	public float velocidade;
	public float tempoInativo;
	private float tempoInatividade;

	/* Variaveis para verificacao de colisao com objetos das cenas */
	private bool cama;
	private bool dialogo;
    private bool interacao;
    private bool computador;
    private bool financeiro;
    private bool portaBanheiro;
    private static bool bloqueioControle;
    public static bool isInicio;
    public static bool isDormindo;
    public static bool isPagamento;
    public static bool isDialogoInicial = true;

    public Tutorial tutorial;
    public static bool isTutorialTempo = true;
    public static bool isTutorialComputador = true;
    public static bool isTutorialDividas = true;

    private Animator animacao;

    public static int retartado;

	// Use this for initialization
	void Start(){

		Mouse.Padrao();

		//Recebendo o componente Animator do GameObject jonasPixel.
		animacao = GameObject.Find("rafaelPixel").GetComponent<Animator>() as Animator;

		//Chamando metodo a cada meio segundo do jogo.
		InvokeRepeating("Inatividade", 1f,0.5f);

        tutorial = this.GetComponent<Tutorial>() as Tutorial;

        if (SceneManager.GetActiveScene().name.Equals("Casa")){
            if (!isInicio){
                isInicio = true;
                GameObject.Find("Camila").transform.gameObject.GetComponent<Dialogo>().abrirPainelDialogo();
                GameObject.Find("Camila").transform.gameObject.GetComponent<Dialogo>().gerarDialogo();
            }

            if (GameObject.Find("TelaNoticias")){
                GameObject.Find("TelaNoticias").SetActive(false);
            }

            if (GameObject.Find("TelaInicial")){
                GameObject.Find("TelaInicial").GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                GameObject.Find("TelaInicial").SetActive(false);
            }

            if (GameObject.Find("CaixaEntrada")){
                Email email = (Email)Resources.Load("Prefabs/CaixaEmail", typeof(Email)) as Email;
                email.AlterarCaixaEmail(false);
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("Empresa")){
            this.GetComponent<Interacao>().Start();
            Interacao.Movimento();
            this.GetComponent<Interacao>().aparecer();
        }

        if (!isTutorialTempo && !isTutorialComputador && isTutorialDividas){
            isTutorialDividas = false;
            tutorial.abrirPainelDialogo();
        }
    }

	void Inatividade(){

		//Verificando se passou do tempo de inatividade.
		if(tempoInatividade >= tempoInativo){

			//Se ja estiver sido executado animacao de inativo, sera reiniciado animacao
			if(animacao.GetCurrentAnimatorStateInfo(0).IsName("Inativo")){
				animacao.Play("Inativo",0,0f);

			}else{ //Senao aplica animacao de inatividade
				animacao.SetBool("Inativo", true);
			}

			//Reiniciando o tempo de inatividade
			tempoInatividade = 0;
		}

		//Realizando contagem do tempo da inatividade.
		tempoInatividade = tempoInatividade + 0.1f;
	}

    public bool isDialogo(){
        return dialogo;
    }

    public bool isFinanceiro(){
        return financeiro;
    }

    public static bool isBloquiado(){
        return bloqueioControle;
    }

	//Metodo para bloquear o movimento do personagem.
	public void bloqueio(){
		bloqueioControle = true;
		animacao.SetFloat ("Andando", 0);
	}
	
	//Metodo para desbloquear o movimento do personagem.
	public static void Desbloquear(){
		bloqueioControle = false;
	}
	
	//Metodo para reiniciar o tempo de inatividade.
	public void reiniciarTempoInativo(){
		tempoInatividade = 0;
		animacao.SetBool ("Inativo", false);
	}

	// Update is called once per frame
	void Update(){

		//Verifica se o estado do personagem se encontra bloqueado
		if(!bloqueioControle){
			Movimentar();
		}

        if (SceneManager.GetActiveScene().name.Equals("Casa")){
            if (isPagamento){
                isPagamento = false;
                Interacao.Dinheiro();
                this.GetComponent<Interacao>().aparecer();
            }
        }

		if(isDormindo){
			if(Tela.isAnimacao()){
				isDormindo = false;
                Tela.Alterar(false);
                Personagem.Desbloquear();
                
                if (Personagem.IsRetard()){
                    Personagem.RetardAlert();
                }
                

                if (isTutorialTempo){
                    isTutorialTempo = false;
                    tutorial.abrirPainelDialogo();
                    LeituraArqDialogo.idDialogo = 4;
                }
			}
		}
	}

	//Metodo para controle do personagem.
	void Movimentar(){

		// Metodo para setar animaçao do personagem.
		this.SetAnimacao();

		if(!isDialogo()){ // Se nao estiver dialogo, libera interaçao com objetos.
			this.InteragirObjetos();
		}
	}

    IEnumerator Dormir(){
        yield return new WaitForSeconds(2);
        isDormindo = true;
        Personagem.UpRetard();
        Energia.Recuperar();
        Relogio.MudarPeriodo();
        yield break;
    }

    void SetAnimacao(){
			
		//Passando a execuçao da animaçao quando o personagem entrar em movimento.
		animacao.SetFloat ("Andando", Mathf.Abs (Input.GetAxisRaw ("Horizontal")));
			
		//Tecla para direita.
		if (Input.GetAxisRaw ("Horizontal") > 0) {
			transform.Translate (Vector2.right * velocidade * Time.deltaTime);
			transform.eulerAngles = new Vector2 (0, 0);
			this.reiniciarTempoInativo();
		}
			
		//Tecla para esquerda.
		if (Input.GetAxisRaw ("Horizontal") < 0) {
			transform.Translate (Vector2.right * velocidade * Time.deltaTime);
			transform.eulerAngles = new Vector2 (0, 180);
			this.reiniciarTempoInativo();
		}
		
	}
	
	void InteragirObjetos(){

		if(Input.GetKeyDown("space")){ // Se acionar tecla spaco.
            if (!Tutorial.isTutorial){
                if (portaBanheiro){ //Se estiver na porta do banheiro.
                    this.reiniciarTempoInativo();
                    Personagem.ResetRetard();
                    Porta.Abrir();

                }else if (cama && !bloqueioControle){ //Se estiver na cama.
                    this.bloqueio();
                    this.reiniciarTempoInativo();
                    this.GetComponent<Interacao>().sumir();
                    Tela.Blink();
                    StartCoroutine(Dormir());

                }else if (computador){ //Se estiver no computador.
                    this.bloqueio();
                    this.reiniciarTempoInativo();
                    this.GetComponent<Interacao>().sumir();
                    Personagem.ResetRetard();
                    SceneManager.LoadScene("Computador");
                }
            }
		}
	}

	//Metodo para verificar se o personagem esta na porta.
	void OnTriggerEnter2D(Collider2D colisor){

        if (!Personagem.IsRetard()){
            if (colisor.gameObject.name == "portaBanheiro" && !Data.Tutorial()) {
			    portaBanheiro = true;
			    Interacao.Porta();

		    }else if(colisor.gameObject.name == "Computador" && !Data.Tutorial()) {
			    computador = true;
			    Interacao.Computador();

		    }else if(colisor.gameObject.name == "Cama" && !Tutorial.isTutorial) {
			    cama = true;
			    Interacao.Cama();

		    }else if(colisor.gameObject.name == "Financeiro" && !Data.Tutorial()){
                financeiro = true;
                Financeiro.Focar(true);
                Interacao.Financeiro();

            }else if(colisor.gameObject.name == "Camila" && !Tutorial.isTutorial && !Financeiro.isAtivo) {
                dialogo = true;
                Interacao.Dialogo();
		    }

		    if(SceneManager.GetActiveScene().name.Equals("Casa")){ 
			    if(!interacao){
				    this.GetComponent<Interacao>().aparecer();
                    interacao = true;
			    }
		    }
        }
    }

	//Metodo para verificar se o personagem saiu da porta.
	void OnTriggerExit2D(Collider2D colisor) {

		if(colisor.gameObject.name == "portaBanheiro") {
			portaBanheiro = false;

		}else if(colisor.gameObject.name == "Computador") {
			computador = false;

		}else if(colisor.gameObject.name == "Cama") {
			cama = false;

		}else if(colisor.gameObject.name == "Financeiro") {
			financeiro = false;
            Financeiro.Focar(false);
        }
        else if (colisor.gameObject.name == "Camila"){
            dialogo = false;
        }

        if (SceneManager.GetActiveScene().name.Equals("Casa")){
			this.GetComponent<Interacao>().sumir();
            interacao = false;
		}
	}

    public static void ResetRetard(){
        retartado = 0;
    }
    
    public static void UpRetard(){
        retartado++;
    }

    public static bool IsRetard(){
        return retartado >= 4;
    }

    public static void RetardAlert(){
        bloqueioControle = true;
        GameObject.Find("Musica").gameObject.SetActive(false);
        GameObject.Find("EasterEgg").transform.FindChild("Retardado").gameObject.SetActive(true);
    }
}
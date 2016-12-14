using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogo : MonoBehaviour {

	public GameObject caixaDialogo;
	private static bool isVoltar;
	private static bool isMultiDialogo;
	private static bool isSubDialogo;
	private static int codigoMultiDialogo;
	private static int codigoSubDialogo;
	private static int codigoDialogo;
	private static bool caixaInstanciada;
	
	private static Personagem personagem;
	private static Sentimento sentimento;
	private static Interacao interacao;

	void Start() {
		LeituraArqDialogo.LerArquivo(codigoDialogo);
		interacao = GameObject.Find ("Personagem").GetComponent<Interacao> () as Interacao;
		sentimento = GameObject.Find("Sentimento").GetComponent<Sentimento>() as Sentimento;
		personagem = GameObject.Find ("Personagem").GetComponent<Personagem> () as Personagem;
	}

	void Update() {

        if (personagem.isDialogo() && Input.GetKeyDown("space") && !isMultiDialogo && !Financeiro.isAtivo && !Tutorial.isTutorial)
        {
            if (!caixaInstanciada)
            {
                interacao.sumir();
                Personagem.ResetRetard();
                this.abrirPainelDialogo();
            }
            this.gerarDialogo();
        }

        if (caixaInstanciada && !personagem.isDialogo()){
            this.finalizarDialogo();
        }
	}

    void posiconarPersonagem(){

        if(this.transform.localRotation.eulerAngles.y >= 180){
            personagem.transform.eulerAngles = new Vector2(0, 0);
        }else{
            personagem.transform.eulerAngles = new Vector2(0, 180);
        }
        
        personagem.bloqueio();
    }

    public void abrirPainelDialogo(){
        this.posiconarPersonagem();
        MovimentacaoCamila.inicializarConversa();

        Vector3 posicao = new Vector3(0f, 0f, 1);
		Instantiate(caixaDialogo, posicao, caixaDialogo.transform.rotation);
		caixaInstanciada = true;

        if (LeituraArqDialogo.idDialogo < 20){
            LeituraArqDialogo.setAlterarDialogo();
        }else{
            LeituraArqDialogo.idDialogo = 3;
        }

        LeituraArqDialogo.setQuantidadeDialogo();
        //GameObject.Find("Personagem").GetComponent<Personagem>().isInicio = true;
        Personagem.isInicio = true;
    }

	public void gerarDialogo(){

		codigoDialogo++;

		if(codigoDialogo > LeituraArqDialogo.getQuantidadeDialogo()){
			this.finalizarDialogo();
		}else{
			this.setarTexto(codigoDialogo);
		}
	}

	void setarTexto(int codigoDialogo){

        LeituraArqDialogo.getDialogo(codigoDialogo, codigoSubDialogo, codigoMultiDialogo, isSubDialogo);

		Sprite dialogoFoto = Resources.Load<Sprite>(LeituraArqDialogo.Foto());
		GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Image").GetComponent<Image>().sprite = dialogoFoto;

		isMultiDialogo = LeituraArqDialogo.isMultiDialogos();

		if(isMultiDialogo){

			if(!isVoltar){
				Interacao.MultiDialogo();
				GameObject.Find("Personagem").GetComponent<Interacao>().aparecer();
			}

			string dialogoPergunta = LeituraArqDialogo.Personagem();
			string[] respostas = LeituraArqDialogo.Respostas();
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/MultiDialogo").gameObject.SetActive(true);
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Dialogo").gameObject.SetActive(false);
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/MultiDialogo/Pergunta").GetComponent<Text>().text = dialogoPergunta;
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/MultiDialogo/Resposta1/Text").GetComponent<Text>().text = respostas[0];
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/MultiDialogo/Resposta2/Text").GetComponent<Text>().text = respostas[1];
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/MultiDialogo/Resposta3/Text").GetComponent<Text>().text = respostas[2];
		}else{
			string dialogoTexto = LeituraArqDialogo.Personagem() + "\n" + LeituraArqDialogo.Texto();
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/MultiDialogo").gameObject.SetActive(false);
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Dialogo").gameObject.SetActive(true);
			GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Dialogo/TextoDialogo").GetComponent<Text>().text = dialogoTexto;
		}
	}

	void finalizarDialogo(){
		Destroy(GameObject.Find("CaixaDialogo(Clone)"));
		codigoDialogo = 0;
		isSubDialogo = false;
		caixaInstanciada = false;
        Personagem.Desbloquear();
		
		if(!sentimento.isAtivo()){
			sentimento.alterar(true);
			sentimento.situacao = true;
		}

		MovimentacaoCamila.finalizarConversa ();
	}

	void OnTriggerExit2D(){
		codigoDialogo = 0;
	}

	public void voltar(){
		Mouse.Padrao();
		interacao.sumir();
		codigoDialogo--;
		isVoltar = true;
		isMultiDialogo = false;
		this.setarTexto(codigoDialogo);
	}

	public void resposta(int id){
		Mouse.Padrao();
		interacao.sumir();
		LeituraArqDialogo.getSubDialogo(codigoDialogo, id);
		codigoMultiDialogo = codigoDialogo;
		codigoSubDialogo = id;
		codigoDialogo = 0;
		isVoltar = false;
		isSubDialogo = true;
		isMultiDialogo = false;
		this.gerarDialogo();
	}

}

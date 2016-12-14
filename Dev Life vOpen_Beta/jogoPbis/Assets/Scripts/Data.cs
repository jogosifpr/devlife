using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

	// Dia que se encontra no jogo.
	public static int dia = 0;

	/* Scripts que serao afetados com a data do jogo. */
	private static Email email;
	private static Sentimento sentimento;
	private static Financeiro financeiro;

	// Use this for initialization
	void Start () {
		email = (Email)Resources.Load("Prefabs/CaixaEmail", typeof(Email)) as Email;
		sentimento = GameObject.Find("Sentimento").GetComponent<Sentimento>() as Sentimento;
		financeiro = GameObject.Find("Financeiro").GetComponent<Financeiro>() as Financeiro;
	}

	// Update is called once per frame
	void Update () {

	}

    public static bool Tutorial(){
        return dia == 0;
    }

	public static int ObterData(){
		return dia;
	}

	public static void MudaData(){

		//Aplicando novo texto
		dia++;
		GameObject.Find("Data").GetComponent<GUIText>().text = "Dia " + dia;

		// Disparando uma cadeia de eventos para alteracao
		Data.AlterarEmail();
		Data.AlterarFinanceiro();
		Data.AlterarSentimento();
    }

	// Metodo para alterar o sistema de e-mail.
	private static void AlterarEmail(){
		email.DesativarEmail();
	}

	// Metodo para alterar o sistema de financeiro.
	private static void AlterarFinanceiro(){

		if(!financeiro.isPrazo()){
			financeiro.gerarConta();
		}else{
			if(financeiro.getPrazo() > 1){
				financeiro.diminuirPrazo();
			}else {
                financeiro.gerarDivida(financeiro.GetComponent<Financeiro>().divida);
                financeiro.reiniciarListaConta();
            }
		}
	}

	// Metodo para alterar o sistema de sentimentos da Camila.
	private static void AlterarSentimento(){
		if(!sentimento.isAtivo()){
			sentimento.alterar(false);
		}
		
		sentimento.situacao = false;
	}
}

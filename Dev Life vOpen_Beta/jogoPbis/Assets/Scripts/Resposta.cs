using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Resposta : MonoBehaviour {

	public GameObject email;
	public int idEmail;
	public bool isRespondido;
	public string escolha;
	private LeituraArquivo leituraArquivo;

	// Use this for initialization
	void Start () {

        if (!this.escolha.Equals("")){

            this.transform.FindChild("UI/Canvas/Respostas").gameObject.SetActive(false);

            // Inicializando o gameObject verificando se ele ja foi aceito ou recusado.
            if (this.escolha.Equals("aceitar")){

                //Aplicando cor preta no botao recusar caso a escolha tenha sido aceitar.
                this.transform.FindChild("UI/Canvas/Aceito").gameObject.SetActive(true);

		    }else if(this.escolha.Equals("recusar")){

                //Aplicando cor preta no botao aceitar caso a escolha tenha sido recusar.
                this.transform.FindChild("UI/Canvas/Recusado").gameObject.SetActive(true);
		    }
        }
    }

	// Update is called once per frame
	void Update () {

	}

	// Metodo para realizar acao da escolha do usuario de acordo com e-mail selecionado.
	public void opcao(string resposta){
		leituraArquivo = GameObject.Find("LeituraArquivo").GetComponent<LeituraArquivo>() as LeituraArquivo;

		// Se o e-mail nao foi respondido.
		if(!isRespondido) {

			// Busco e-mail, em seguida envio como parametro sua resposta e altero o estado para respondido.
			this.getEmail(resposta);

			if(resposta.Equals("aceitar")){

				int idProjeto = 0;

				//Debug.Log ("IdProjeto: " + leituraArquivo.IdProjetoEmail (idEmail));
				int.TryParse (leituraArquivo.IdProjetoEmail (idEmail), out idProjeto);

				if (idProjeto != 0) {

					Projeto.aumentaQtdProjetos ();
					//Debug.Log ("Quantidade projetos: " + Projeto.getQtdProjetos ());
					//Debug.Log ("Projeto " + Projeto.getQtdProjetos ().ToString ());
					Projeto p = GameObject.Find ("TelaEstacaoTrabalho").transform.FindChild ("TelaInicial/Projeto " + Projeto.getQtdProjetos ().ToString ()).GetComponent<Projeto> () as Projeto;
					p.criarProjeto (idProjeto);

				} else {
					//Debug.Log ("E-mail nao possui um projeto");
				}

                this.transform.FindChild("UI/Canvas/Respostas").gameObject.SetActive(false);
                this.transform.FindChild("UI/Canvas/Aceito").gameObject.SetActive(true);

                // leitura do arquivo de Email.xml
                leituraArquivo.CarregarArquivo ();

				//Debug.Log ("clicou para aceitar o email " + idEmail);
			} else if (resposta.Equals ("recusar")) {

                this.transform.FindChild("UI/Canvas/Respostas").gameObject.SetActive(false);
                this.transform.FindChild("UI/Canvas/Recusado").gameObject.SetActive(true);
				Navegador.AdicionarListaNoticias(idEmail);
                Desempenho.AddProjeto(idEmail, 0, false);

			} else {
				//Debug.Log ("Opçao invalida.");
			}

			// Marco a resposta da tela atual para respondido e invalidando acesso aos botoes da tela.
			this.isRespondido = true;
		}else{
			//Debug.Log("Este e-mail ja foi respondido e portanto os botoes ficaram sem acao.");
		}
	}

	// Metodo para obter o e-mail respondido e aplicar o seu estado e sua escolha.
	public void getEmail(string escolha){
		GameObject.Find("TelaEmail").transform.Find("CanvasEmail").gameObject.SetActive(true);
		email = GameObject.Find("TelaEmail").GetComponent<GerenciadorEmail>().obterCaixaEmail(idEmail);
		email.GetComponent<Email>().isRespondido = true;
		email.GetComponent<Email>().escolhaResposta = escolha;
		if(escolha.Equals("aceitar")){
			email.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("caixaEmailAceito", typeof(Sprite)) as Sprite;
		}else{
			email.GetComponent<Image>().overrideSprite = (Sprite)Resources.Load("caixaEmailRecusado", typeof(Sprite)) as Sprite;
		}
		GameObject.Find("TelaEmail").transform.Find("CanvasEmail").gameObject.SetActive(false);
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;

public class Projeto : MonoBehaviour {
	
	public GUITexture icone;
	
	public Texture2D iconeDesativado;
	public Texture2D iconeAtivado;
	
	private static int qtdProjetos = 0;
	public int pos;
	public bool ativo = false;
	public int idProjeto = 0;
	
	public string nomeProjeto;
	public int progresso;
	public int turnosTotal;
	public int turnosCorridos;
    public int progressoBarra;

    private int valor;
	private static GameObject[] todosProjetos;
	private LeituraArquivo leituraArquivo;
	
	private TelaInicialScript telaInicialScript;
	
	// Use this for initialization
	void Start(){
		todosProjetos = GameObject.FindGameObjectsWithTag("Projeto");
		leituraArquivo = GameObject.Find("LeituraArquivo").GetComponent<LeituraArquivo>() as LeituraArquivo;
		telaInicialScript = GameObject.Find ("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").GetComponent<TelaInicialScript> () as TelaInicialScript;
	}
	
	// Update is called once per frame.
	void Update () {
		
	}
	
	void OnMouseDown(){
		if (idProjeto > 0) {
			if (ativo) {
				icone.texture = iconeDesativado;
				ativo = false;
				telaInicialScript.setProjeto(null);
				
				GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").transform.FindChild("NomeProjeto").GetComponent<GUIText>().text = "";
				GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").transform.FindChild("Turnos").GetComponent<GUIText>().text = "";
				GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").transform.FindChild("Progresso").GetComponent<GUIText>().text = "";
				GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").transform.FindChild("BarraProgresso").GetComponent<GUITexture>().texture = (Texture2D)Resources.Load ("BarraProgresso/Barra0", typeof(Texture2D)) as Texture2D;
			} else {
				foreach(GameObject projetinho in todosProjetos){
					Projeto p = (Projeto)projetinho.GetComponent(typeof(Projeto));
					p.ativo = false;
					if(p.iconeDesativado!=null){
						p.icone.texture = p.iconeDesativado;
					}
				}
				this.escreverDados();
			}
            Comportamento.Obter(Comportamento.Clique.PROJETO);
		}
	}
	
	public void criarProjeto(int id){
		
		leituraArquivo = GameObject.Find("LeituraArquivo").GetComponent<LeituraArquivo>() as LeituraArquivo;
		
		leituraArquivo.LerArquivoProjeto();
		
		int.TryParse(leituraArquivo.TempoProjeto (id), out turnosTotal);
		nomeProjeto = leituraArquivo.NomeProjeto(id);

		valor = int.Parse(leituraArquivo.ValorMaxProjeto (id));
		idProjeto = id;
		
		progresso = 0;
		
		iconeAtivado = (Texture2D)Resources.Load ("IconesProjetos/" + /*id.ToString ()*/ "1" + "-1", typeof(Texture2D)) as Texture2D;
		iconeDesativado = (Texture2D)Resources.Load ("IconesProjetos/" + /*id.ToString ()*/ "1" + "-0", typeof(Texture2D)) as Texture2D;
	}
	
	public void trabalhar(){

		if (turnosTotal > 0 && progresso!=100 && Energia.ObterEstado()>0){
			turnosCorridos++;
			progresso = (100 / turnosTotal) * turnosCorridos;
			escreverDados();
			Energia.Diminuir();
			Relogio.MudarPeriodo();
		
			if (turnosTotal - turnosCorridos == 0) {
				Dinheiro.Receber (valor);
                Personagem.isPagamento = true;
				Navegador.GerarNoticias("ProjetoEntregado",idProjeto);
                Desempenho.AddProjeto(idProjeto, progresso, true);
            }
		}

	}
	
	private void escreverDados(){
		icone.texture = iconeAtivado;
		ativo = true;
		
		GameObject telaInicial = GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").gameObject;
		
		string turnos = (turnosTotal - turnosCorridos).ToString();
		
		telaInicial.transform.FindChild("NomeProjeto").GetComponent<GUIText>().text = nomeProjeto;
		telaInicial.transform.FindChild("Turnos").GetComponent<GUIText>().text = "Faltam " + turnos + " turnos";

        if (turnosCorridos > 0){
            progressoBarra = (int)Mathf.Floor(progresso / 5) * 5;
        }else{
            progressoBarra = 0;
        }

        if (turnosTotal - turnosCorridos == 0){
            telaInicial.transform.FindChild("BarraProgresso").GetComponent<GUITexture>().texture = (Texture2D)Resources.Load("BarraProgresso/Barra100", typeof(Texture2D)) as Texture2D;
            telaInicial.transform.FindChild("Progresso").GetComponent<GUIText>().text = "Progresso 100%";
			progresso = 100;
		}else{
            telaInicial.transform.FindChild("BarraProgresso").GetComponent<GUITexture>().texture = (Texture2D)Resources.Load("BarraProgresso/Barra" + progressoBarra, typeof(Texture2D)) as Texture2D;
            telaInicial.transform.FindChild("Progresso").GetComponent<GUIText>().text = "Progresso  " + progresso + "%";
		}
		
		telaInicialScript.setProjeto(this);
	}

	public static void aumentaQtdProjetos(){
		qtdProjetos++;
	}

	public static int getQtdProjetos(){
		return qtdProjetos;
	}

    public static void AvaliarDesempenho(){

        if (GameObject.Find("TelaEstacaoTrabalho")){

            GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").gameObject.SetActive(true);

            foreach (GameObject objeto in GameObject.FindGameObjectsWithTag("Projeto"))
            {
                if (Projeto.isAndamento(objeto))
                {
                    int projeto = objeto.gameObject.GetComponent<Projeto>().idProjeto;
                    int progresso = objeto.gameObject.GetComponent<Projeto>().progresso;
                    Desempenho.AddProjeto(projeto, progresso, true);
                }
            }

            if (!SceneManager.GetActiveScene().name.Equals("Computador")){
                GameObject.Find("TelaEstacaoTrabalho").transform.FindChild("TelaInicial").gameObject.SetActive(false);
            }
        }
    }

    public static bool isAndamento(GameObject objeto){
        Projeto _projeto = objeto.gameObject.GetComponent<Projeto>() as Projeto;
        return _projeto.idProjeto != 0 && _projeto.turnosCorridos != _projeto.turnosTotal;
    }



	
}

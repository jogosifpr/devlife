using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Relogio : MonoBehaviour {
		
	public Texture2D[] periodoRelogio;

	private static int tempo = 3;
    private static Texture2D[] periodo;
	private static GUITexture textureIMG;
    private static bool isInteracao;
    
	// Use this for initialization
	void Start(){

		// Atribuindo os periodos do relogio do editor para variavel static
		periodo = periodoRelogio;

		//Localizando o GUITexture no projeto
		textureIMG = GameObject.Find("Relogio").GetComponent<GUITexture>() as GUITexture;
    }

    // Metodo chamado para alterar o periodo
    public static void MudarPeriodo(){

        tempo++;

        if (Personagem.isDormindo){
            if (Data.ObterData() >= 7)                  Fim.setCenario(Fim.Cenario.CONCLUIDO);
            if (Sentimento.ObterSocializacao() == 0)    Fim.setCenario(Fim.Cenario.SOCIAL);
            if (Financeiro.ObterDespesa() >= 4)         Fim.setCenario(Fim.Cenario.FINANCEIRO);
        }

        if (GameObject.Find("WebService") && !Data.Tutorial()){
            if (tempo == 4 || Fim.getCenario()!=null){
                Projeto.AvaliarDesempenho();
                WebService.Enviar();
            }
        }

        if (tempo == 4){
            tempo = 0;
            Data.MudaData();
            if (SceneManager.GetActiveScene().name.Equals("Casa") && Data.dia > 1){
                Interacao.Email();
                isInteracao = true;
            }

            if (GameObject.Find("TelaNavegador")){
                if (SceneManager.GetActiveScene().name.Equals("Casa") && Data.dia > 1 && Navegador.ListaSize() != 0){
                    Interacao.Noticia();
                    isInteracao = true;
                }
                Navegador.GerarNoticias("ProjetoRecusado", 0);
                GameObject.Find("TelaNavegador").GetComponent<Navegador>().DescontarDuracaoNoticia();
            }

            if (isInteracao){
                isInteracao = false;
                GameObject.Find("Personagem").GetComponent<Interacao>().aparecer();
            }
        }

        if (Fim.getCenario() != null){
            Personagem.isInicio = false;
            //GameObject.Find("Personagem").GetComponent<Personagem>().isInicio = true;
            SceneManager.LoadScene("Fim");
        }

        Relogio.AlterarImagem();
        Janela.alterarPaisagem(tempo);
        MovimentacaoCamila.mudarLocal();
    }

	//Aplicando a Texture do array
	static void AlterarImagem(){
		textureIMG.texture = periodo[tempo];
	}
}

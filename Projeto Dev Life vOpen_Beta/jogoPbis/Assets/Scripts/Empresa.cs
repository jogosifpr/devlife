using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Empresa : MonoBehaviour {

    public GameObject musica;
    private static int index;
	private static float alpha;
	private static bool isAparecer;
	private static bool isDesaparecer;
	private static bool isDialogo;
	private static bool isDialogoChefe;
	private static bool isDialogoReflexivo;
	private static bool isSpriteReader;
	private static bool isCorredorAtivado;
	private static bool isAgradecimento;
	private static bool isProximaCena;
	private static bool isElevador;
    private static bool isMusica = true;
    private static bool isMusicaEnd;
    private static int codigoDialogo;
	private static int codigoFala;
	private static GameObject caixaDialogo;
    private static GameObject[] gameObject = new GameObject[4];

	// Use this for initialization
	void Start () {
		if(this.name == "Chefe"){
			this.Exibir();
			LeituraArqEmpresa.LerArquivo();
			gameObject[0] = GameObject.Find("Empresa").transform.FindChild("Chefe").gameObject;
			gameObject[1] = GameObject.Find("Empresa").transform.FindChild("Corredor").gameObject;
			gameObject[2] = GameObject.Find("Empresa").transform.FindChild("Elevador").gameObject;
			gameObject[3] = GameObject.Find("Empresa").transform.FindChild("Personagem").gameObject;
			caixaDialogo = (GameObject)Resources.Load("Prefabs/CaixaDialogo", typeof(GameObject)) as GameObject;

            this.GetComponent<Interacao>().Start();
            Interacao.Empresa();
            this.GetComponent<Interacao>().aparecer();

            StartCoroutine(IniciarMusica());
        }
	}

    IEnumerator IniciarMusica(){
        yield return new WaitForSeconds(1);
        musica.gameObject.SetActive(true);
        yield break;
    }

    IEnumerator LiberarDialogoChefe(){
		yield return new WaitForSeconds(1);
		Vector3 posicao = new Vector3(0f, 0f, 1);
        Interacao.ZerarInteracao();
		Instantiate(caixaDialogo, posicao, caixaDialogo.transform.rotation);
		codigoDialogo = 1;
		LeituraArqEmpresa.setquantidadeDialogo(codigoDialogo);
		isDialogoChefe = true;
		this.GerarDialogo();
		yield break;
	}
	
	IEnumerator LiberarCorredor(){
		yield return new WaitForSeconds(2);
		gameObject[0].SetActive(false);
		gameObject[1].SetActive(true);
		gameObject[3].SetActive(true);
		index = 1;
		isSpriteReader = true;
		this.Exibir();
		yield break;
	}

	IEnumerator LiberarDialogoReflexivo(){
		yield return new WaitForSeconds(2);
		codigoDialogo = 2;
		LeituraArqEmpresa.setquantidadeDialogo(codigoDialogo);
		Vector3 posicao = new Vector3(0f, 0f, 1);
		Instantiate(caixaDialogo, posicao, caixaDialogo.transform.rotation);
		isDialogoReflexivo = true;
		this.GerarDialogo();
		yield break;
	}
	
	IEnumerator LiberarCena(){
		yield return new WaitForSeconds(4);
		isProximaCena = true;
		yield break;
	}
	
	// Update is called once per frame
	void Update () {

        if (musica.gameObject.activeInHierarchy && isMusica){
            if (musica.GetComponent<AudioSource>().volume >= 100){
                isMusica = false;
            }else{
                musica.GetComponent<AudioSource>().volume += 0.001f;
            }
        }

		if(isAparecer){
			this.Aparecer();
		}
		
		if(isDesaparecer){
			this.Desaparecer();
		}
		
		if(isDialogoChefe && Input.GetKeyDown("space")){
			this.GerarDialogo();
		}
		
		if(isDialogoReflexivo && Input.GetKeyDown("space")){
			this.GerarDialogo();
		}
		
		if(isAgradecimento){
			this.Aparecer();
		}
		
		if(isProximaCena){
            musica.GetComponent<AudioSource>().volume -= 0.008f;
            this.Desaparecer();
        }
        	
		if(isElevador){
			this.Aparecer();
		}
	}
	
	void GerarDialogo(){

		codigoFala++;

		if(codigoFala > LeituraArqEmpresa.getQuantidadeDialogo()){
			codigoFala = 0;
			Destroy(GameObject.Find("CaixaDialogo(Clone)"));
			
			if(isDialogoChefe){
				this.Esconder();
				isDialogoChefe = false;
			}
			
			if(isDialogoReflexivo){
				alpha = 0.005f;
				isAgradecimento = true;
				isDialogoReflexivo = false;
				GameObject.Find("Empresa").transform.FindChild("Elevador/Agradecimento").gameObject.SetActive(true);
			}
			
		}else{
			this.setarTexto(codigoDialogo);
		}
	}
	
	void setarTexto(int codigoDialogo){
		
		LeituraArqEmpresa.getDialogo(codigoDialogo, codigoFala);
		string dialogoTexto = LeituraArqEmpresa.Personagem() + "\n" + LeituraArqEmpresa.Texto();
		Sprite dialogoFoto = Resources.Load<Sprite>(LeituraArqEmpresa.Foto());
		
		GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Image").GetComponent<Image>().sprite = dialogoFoto;
		GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Dialogo/TextoDialogo").GetComponent<Text>().text = dialogoTexto;
		
	}
	
	void Aparecer(){
		
		if(isAgradecimento){
			GameObject.Find("Agradecimento").GetComponent<GUIText>().color = new Color(1f,1f,1f, alpha);
		}else{
			if(isSpriteReader){
				
				if(isElevador){
					GameObject.Find("Empresa").transform.FindChild("Elevador").gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, alpha);
				}else{
					foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Imagem")){
						gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, alpha);
					}
				}
				
			}else{
				gameObject[index].GetComponent<GUITexture>().color = new Color(0.5f,0.5f,0.5f, alpha);
			}
		}
		
		if(alpha > 1f){
			isAparecer = false;
			isElevador = false;
			
			if(isAgradecimento){
				isAgradecimento = false;
				StartCoroutine(LiberarCena());
			}
			
			if(index == 0){
				StartCoroutine(LiberarDialogoChefe());
			}
			
		}else{
			alpha = alpha + 0.005f;
		}
	}
	
	void Desaparecer(){
		
		if(isProximaCena){
			
			foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(GameObject))) {
				if (gameObject.GetComponent<SpriteRenderer> () != null) {
					gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, alpha);
				}
			}
			
			GameObject.Find("Agradecimento").GetComponent<GUIText>().color = new Color(1f,1f,1f, alpha);
			
		}else{
			if(isSpriteReader){
				
				foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Imagem")){
					gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, alpha);
				}
				
			}else{
				gameObject[index].GetComponent<GUITexture>().color = new Color(0.5f,0.5f,0.5f, alpha);
			}
		}
		
		if(alpha < 0f){
			isDesaparecer = false;
			alpha = 0.005f;
			
			if(isProximaCena){
				isProximaCena = false;
                Personagem.Desbloquear();
                Application.LoadLevel("Casa");
			}
            
			
			if(index == 0){
				StartCoroutine(LiberarCorredor());
			}
			
		}else{
			alpha = alpha - 0.005f;
		}
	}
	
	void Exibir(){
		isAparecer = true;
	}
	
	void Esconder(){
		isDesaparecer = true;
	}

	void LiberarElevador(){

		foreach(GameObject elevador in GameObject.FindGameObjectsWithTag("Elevador")){
			elevador.GetComponent<Animator>().Play("Elevador");
		}
	}

	void OnTriggerEnter2D(Collider2D colisor){

		if(this.name.Equals("ColisorElevador")){
			if (colisor.gameObject.name == "Personagem"){
				GameObject.Find("Empresa").transform.FindChild("Elevador").gameObject.SetActive(true);
				alpha = 0.005f;
				isElevador = true;
				GameObject.Find("Empresa").transform.FindChild("Corredor/PortaExterna").gameObject.SetActive(false);
			}
		}else if(this.name.Equals("ElevadorAtivado")){
			if (colisor.gameObject.name == "Personagem"){
				GameObject.Find("Personagem").GetComponent<Personagem>().bloqueio();
				GameObject.Find("Personagem").transform.eulerAngles = new Vector2 (0, 0);
				GameObject.Find("Empresa").transform.FindChild("Elevador/PortaInterna").gameObject.SetActive(true);
				GameObject.Find("Empresa").transform.FindChild("Corredor/PortaExterna").gameObject.SetActive(false);
				GameObject.Find("Empresa").transform.FindChild("Corredor").gameObject.SetActive(false);
				isElevador = false;
				GameObject.Find("Empresa").transform.FindChild("Elevador").gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
				StartCoroutine(LiberarDialogoReflexivo());
				this.LiberarElevador();
			}
		}
	}

	void OnTriggerExit2D(Collider2D colisor) {

		if(this.name.Equals("ColisorElevador")){
			if(colisor.gameObject.name == "Personagem"){
				GameObject.Find("Empresa").transform.FindChild("Corredor/PortaExterna").gameObject.SetActive(true);
				GameObject.Find("Empresa").transform.FindChild("Elevador").gameObject.SetActive(false);
				isElevador = false;
				GameObject.Find("Empresa").transform.FindChild("Elevador").gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
			}
		}
	}
}
using UnityEngine;
using System.Collections;
using System.Xml;

public class Interacao : MonoBehaviour {

	private static bool isAparecer;
	private static bool isDesaparecer;
	private static float alpha;

	private static string texto;
	private static GameObject tutorial;
	private static GUIText legenda;

	private static XmlNodeList nodeList;
	private static XmlNode root;
	
	// Use this for initialization
	public void Start () {
		tutorial = GameObject.Find("Interacao").gameObject;
		legenda = GameObject.Find("Interacao").GetComponent<GUIText>() as GUIText;
		this.LerArquivo();
	}

	// Update is called once per frame
	void Update () {
		
		if(isAparecer){
			this.AparecerLegenda();
		}

		if(isDesaparecer){
			this.DesaparecerLegenda();
		}
	}

	public void aparecer(){
		tutorial.SetActive(true);
		isAparecer = true;
		StartCoroutine("AutoType");
	}

	public void sumir(){
        if (GameObject.Find("Interacao")){
            StopCoroutine("AutoType");
            ZerarInteracao();
            tutorial.SetActive(false);
        }
	}

	public static void ZerarInteracao(){
        if (GameObject.Find("Interacao")){
            alpha = 0f;
            isAparecer = false;
            isDesaparecer = false;
            texto = "";
            legenda.text = "";
        }
	}

	void AparecerLegenda(){

		legenda.color = new Color(1f,1f,1f, alpha);

		if(alpha > 1.5f){
			isAparecer = false;
		}else{
			alpha = alpha + 0.025f;
		}
	}

	void DesaparecerLegenda(){
		
		legenda.color = new Color(1f,1f,1f, alpha);

		if(alpha < 0f){
			isDesaparecer = false;
		}else{
			alpha = alpha - 0.015f;
		}
	}

	void LerArquivo(){
		XmlDocument doc = new XmlDocument();

        //DESKTOP
        //doc.Load(Application.streamingAssetsPath + "/Files/Tutorial.xml");

        //WEB
        TextAsset textAsset = Resources.Load("Files/Tutorial") as TextAsset;
        doc.LoadXml(textAsset.text);        
        
		root = doc.DocumentElement;
		nodeList = root.SelectNodes("descendant::tutorial[@id='1']");
		nodeList = nodeList[0].ChildNodes;
	}

	public static void Porta(){
		texto = nodeList[0].InnerText.ToString();
	}

	public static void Computador(){
		texto = nodeList[1].InnerText.ToString();
	}

	public static void Escada(){
		texto = nodeList[2].InnerText.ToString();
	}

	public static void Cama(){
		texto = nodeList[3].InnerText.ToString();
	}

    public static void Empresa(){
        texto = nodeList[4].InnerText.ToString();
    }

    public static void Dialogo(){
        if (!Personagem.isDialogoInicial){
            texto = nodeList[4].InnerText.ToString();
        }else{
            Personagem.isDialogoInicial = false;
            texto = "";
        }
	}

	public static void MultiDialogo(){
		texto = nodeList[5].InnerText.ToString();
	}

    public static void Financeiro(){
        texto = nodeList[6].InnerText.ToString();
    }

    public static void Movimento(){
        texto = nodeList[7].InnerText.ToString();
    }

    public static void Email(){
        texto = nodeList[8].InnerText.ToString();
    }

    public static void Dinheiro(){
        texto = nodeList[9].InnerText.ToString();
    }

    public static void Noticia(){
        texto = nodeList[10].InnerText.ToString();
    }

    IEnumerator AutoType(){
		foreach(char letra in texto.ToCharArray()){
			legenda.text += letra;
			yield return new WaitForSeconds(0.05f);
		}

		isDesaparecer = true;
	}
}

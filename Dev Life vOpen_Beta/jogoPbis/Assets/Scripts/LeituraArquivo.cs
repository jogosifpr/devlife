using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class LeituraArquivo : MonoBehaviour {

	XmlDocument doc;
	XmlNodeList nodeList;
	XmlNode root;
	private int[] arrayID;
	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public int[] obterIds(){

		XmlDocument doc = new XmlDocument();
        //DESKTOP						
        //doc.Load(Application.streamingAssetsPath+ "/Files/Email.xml");		

        //WEB
        TextAsset textAsset = Resources.Load("Files/Email") as TextAsset;
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;
		
		nodeList = root.SelectNodes("email");
		
		arrayID = new int[nodeList.Count];
		
		for (int i = 0; i < arrayID.Length; i++){
			arrayID[i] = int.Parse(nodeList[i].Attributes["id"].Value);
		}

		return arrayID;
	}

	public void CarregarArquivo(){
		doc = new XmlDocument();								
        //DESKTOP						
        //doc.Load(Application.streamingAssetsPath+ "/Files/Email.xml");		

        //WEB
        TextAsset textAsset = Resources.Load("Files/Email") as TextAsset;
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;
	}

	public void LerArquivo(int dia){
		XmlDocument doc = new XmlDocument();
        //DESKTOP						
        //doc.Load(Application.streamingAssetsPath+ "/Files/textoEmailsDia"+dia+".xml");	

        //WEB
        TextAsset textAsset = Resources.Load("Files/textoEmailsDia" + dia) as TextAsset;
        doc.LoadXml(textAsset.text);
        
		root = doc.DocumentElement;
	}

	public int getQuantidadeEmails(){
		return root.ChildNodes.Count;
	}

	public string Remetente(int id){
		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[1].InnerText.ToString();
	}

	public string Titulo(int id){
		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[0].InnerText.ToString();
	}

	public string DataEmail(int id){

		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;

		return nodeList[2].InnerText.ToString();
	}

	public string Endereco(int id){
		
		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[3].InnerText.ToString();
	}

	public string Localizacao(int id){
		
		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[4].InnerText.ToString();
	}

	public string Mensagem(int id){
		
		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[5].InnerText.ToString();
	}

	public string Foto(int id){
		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[6].InnerText.ToString();
	}
	
	public string IdProjetoEmail(int id){
        string retorno = "0";
        if (id!=null) {
            if (id >= 1 || id <= 15){
                nodeList = root.SelectNodes("descendant::email[@id='" + id + "']");
                nodeList = nodeList[0].ChildNodes;

                retorno = nodeList[7].InnerText.ToString();
            }
        }
        return retorno;
    }

	public string IdEmail(int id){
		nodeList = root.SelectNodes("descendant::email[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[8].InnerText.ToString();
	}

	// -------------------------------------PROJETOS-------------------------------------
	
	public void LerArquivoProjeto(){
        XmlDocument doc = new XmlDocument();
        //DESKTOP						
        //doc.Load(Application.streamingAssetsPath+ "/Files/Projeto.xml");

        //WEB
        TextAsset textAsset = Resources.Load("Files/Projeto") as TextAsset;
        doc.LoadXml(textAsset.text);
        
        root = doc.DocumentElement;
	}

	public string NomeProjeto(int id){
		nodeList = root.SelectNodes("descendant::projeto[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[0].InnerText.ToString();
	}

	public string DescricaoProjeto(int id){
		nodeList = root.SelectNodes("descendant::projeto[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[1].InnerText.ToString();
	}

	public string TempoProjeto(int id){
		nodeList = root.SelectNodes("descendant::projeto[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[2].InnerText.ToString();
	}
	
	public string DificuldadeProjeto(int id){
		nodeList = root.SelectNodes("descendant::projeto[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[3].InnerText.ToString();
	}
	
	public string ValorMinProjeto(int id){
		nodeList = root.SelectNodes("descendant::projeto[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[4].InnerText.ToString();
	}

	public string ValorMaxProjeto(int id){
		nodeList = root.SelectNodes("descendant::projeto[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
		
		return nodeList[5].InnerText.ToString();
	}
}

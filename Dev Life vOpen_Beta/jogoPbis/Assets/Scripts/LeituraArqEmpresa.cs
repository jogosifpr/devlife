using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class LeituraArqEmpresa : MonoBehaviour {

	private static XmlNodeList nodeList;
	private static XmlNode root;
	private static int quantidade;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void LerArquivo(){
		XmlDocument doc = new XmlDocument();

        //DESKTOP
        //doc.Load(Application.streamingAssetsPath + "/Files/Empresa.xml");

        //WEB
        TextAsset textAsset = Resources.Load("Files/Empresa") as TextAsset;
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;
	}

	public static void setquantidadeDialogo(int id){
		quantidade = root.SelectNodes("descendant::dialogo[@id='"+id+"']/fala").Count;
	}

	public static int getQuantidadeDialogo(){
		return quantidade;
	}

	public static void getDialogo(int idDialogo, int idFala){
		nodeList = root.SelectNodes("descendant::dialogo[@id='"+ idDialogo +"']"); 
		nodeList = nodeList[0].SelectNodes("descendant::fala[@id='" + idFala + "']");
		nodeList = nodeList[0].ChildNodes;
	}

	public static string Personagem(){
		return nodeList[0].InnerText.ToString();
	}
	
	public static string Foto(){
		return nodeList[1].InnerText.ToString();
	}
	
	public static string Texto(){
		return nodeList[2].InnerText.ToString();
	}
}

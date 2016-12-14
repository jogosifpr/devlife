using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class LeituraArqNoticias : MonoBehaviour {

	private static XmlNodeList nodeList;
	private static XmlNode root;

	public static void LerArquivo(string noticias){
		XmlDocument doc = new XmlDocument();

        //DESKTOP
        //doc.Load(Application.streamingAssetsPath+ "/Files/"+ noticias +".xml");		

        //WEB
        TextAsset textAsset = Resources.Load("Files/" + noticias) as TextAsset;
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;
	}

	public static void getNoticia(int id){
		nodeList = root.SelectNodes("descendant::noticia[@id='"+ id +"']");
		nodeList = nodeList[0].ChildNodes;
	}

	public static  string Titulo(){
		return nodeList[0].InnerText.ToString();
	}

	public static  string Texto(){
		return nodeList[1].InnerText.ToString();
	}
}

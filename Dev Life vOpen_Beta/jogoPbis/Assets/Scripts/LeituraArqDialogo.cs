using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class LeituraArqDialogo : MonoBehaviour{

	private static XmlNodeList nodeList;
	private static XmlNode root;
	private static int quantidade;
    public static int idDialogo = 0;


    public static void LerArquivo(int dialogo){
        XmlDocument doc = new XmlDocument();

        //DESKTOP
        //doc.Load(Application.streamingAssetsPath + "/Files/Dialogo.xml");

        //WEB
        TextAsset textAsset = Resources.Load("Files/Dialogo") as TextAsset;
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;
    }

    public static void setAlterarDialogo(){
        if (!Data.Tutorial() || idDialogo == 0){
            idDialogo++;
        }else{
            idDialogo = 2;
        }
    }

    public static void setQuantidadeDialogo(){
        quantidade = root.SelectNodes("descendant::dialogo[@id='"+ idDialogo + "']/fala").Count;
    }

	public static int getQuantidadeDialogo(){
		return quantidade;
    }

	public static void getDialogo(int id, int idSub, int idMulti, bool isSubDialogo){

		if(isSubDialogo){
            nodeList = root.SelectNodes("descendant::dialogo[@id='" + idDialogo + "']");
            nodeList = nodeList[0].SelectNodes("descendant::fala[@id='"+ idMulti +"']"); 
			nodeList = nodeList[0].SelectNodes("descendant::subDialogo[@id='" + idSub + "']");
			nodeList = nodeList[0].SelectNodes("descendant::fala[@id='" + id + "']");
		}else{
            nodeList = root.SelectNodes("descendant::dialogo[@id='" + idDialogo + "']");
			nodeList = nodeList[0].SelectNodes("descendant::fala[@id='" + id + "']");
		}
	
		nodeList = nodeList[0].ChildNodes;
	}

	public static void getSubDialogo(int codigoDialogo, int idSubDialogo){
        nodeList = root.SelectNodes("descendant::dialogo[@id='" + idDialogo + "']");
        nodeList = nodeList[0].SelectNodes("descendant::fala[@id='" + codigoDialogo + "']"); 
		nodeList = nodeList[0].SelectNodes("descendant::subDialogo[@id='" + idSubDialogo + "']");
		quantidade = nodeList[0].SelectNodes("fala").Count;
	}
	
	public static bool isMultiDialogos(){
		return nodeList.Count > 3;
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

	public static string[] Respostas(){
		string[] respostas = new string[3];
		respostas[0] = nodeList[2].InnerText.ToString();
		respostas[1] = nodeList[3].InnerText.ToString();
		respostas[2] = nodeList[4].InnerText.ToString();
		return respostas;
	}
}
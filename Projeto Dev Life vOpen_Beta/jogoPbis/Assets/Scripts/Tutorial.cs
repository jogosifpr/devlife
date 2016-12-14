using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

    public GameObject caixaDialogo;
    private bool caixaInstanciada;

    private Personagem personagem;

    private XmlNodeList nodeList;
    private XmlNode root;
    private static int quantidade;
    private static int idTutorial = 1;
    private static int idDialogo = 1;
    public static bool isTutorial;

    // Use this for initialization
    void Start(){
        this.LerArquivo();
        personagem = GameObject.Find("Personagem").GetComponent<Personagem>() as Personagem;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space") && isTutorial){
            this.gerarDialogo();
        }

        if (caixaInstanciada && !isTutorial){
            Destroy(GameObject.Find("CaixaDialogo(Clone)"));
            isTutorial = false;
            caixaInstanciada = false;
            Personagem.Desbloquear();
        }
    }

    public void abrirPainelDialogo(){
        
        this.LerArquivo();
        idDialogo = 1;
        idTutorial++;
        GameObject.Find("Personagem").transform.eulerAngles = new Vector2(0, 180);
        Vector3 posicao = new Vector3(0f, 0f, 1);
        Instantiate(caixaDialogo, posicao, caixaDialogo.transform.rotation);
        caixaInstanciada = true;
        isTutorial = true;
        this.GetComponent<Personagem>().bloqueio();
        this.setQuantidadeDialogo();
        this.setarTexto();
    }

    public void gerarDialogo(){

        idDialogo++;
        
        if (idDialogo > this.getQuantidadeDialogo()){
            Destroy(GameObject.Find("CaixaDialogo(Clone)"));
            isTutorial = false;
            caixaInstanciada = false;
            Personagem.Desbloquear();
        }else{
            this.setarTexto();
        }
    }

    void setarTexto(){

        this.getDialogo();
        Sprite dialogoFoto = Resources.Load<Sprite>(this.Foto());
        GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Image").GetComponent<Image>().sprite = dialogoFoto;

        string dialogoTexto = this.Nome() + "\n" + this.Texto();
        GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/MultiDialogo").gameObject.SetActive(false);
        GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Dialogo").gameObject.SetActive(true);
        GameObject.Find("CaixaDialogo(Clone)").transform.FindChild("Painel/Dialogo/TextoDialogo").GetComponent<Text>().text = dialogoTexto;
    }

    void LerArquivo(){
        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = Resources.Load("Files/Tutorial") as TextAsset;
        doc.LoadXml(textAsset.text);

        root = doc.DocumentElement;
        nodeList = root.SelectNodes("descendant::tutorial[@id='"+ idTutorial + "']");
        nodeList = nodeList[0].ChildNodes;
    }

    public void getDialogo(){
        nodeList = root.SelectNodes("descendant::tutorial[@id='" + idTutorial + "']");
        nodeList = nodeList[0].SelectNodes("descendant::fala[@id='" + idDialogo + "']");
        nodeList = nodeList[0].ChildNodes;
    }

    public void setQuantidadeDialogo(){
        quantidade = root.SelectNodes("descendant::tutorial[@id='" + idTutorial + "']/fala").Count;
    }

    public int getQuantidadeDialogo(){
        return quantidade;
    }

    public string Nome(){
        return nodeList[0].InnerText.ToString();
    }

    public string Foto(){
        return nodeList[1].InnerText.ToString();
    }

    public string Texto(){
        return nodeList[2].InnerText.ToString();
    }
}
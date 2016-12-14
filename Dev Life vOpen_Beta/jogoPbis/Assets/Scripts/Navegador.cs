using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Navegador : MonoBehaviour {

	private static GameObject telaNoticias;
	private static GameObject noticia;
	private static Vector3 posicao;
	private static bool NavegadorCriado;
    private static List<int> listaNoticias = new List<int>();

    public int duracao;

	void Start () {

        if (this.gameObject.name.Equals("TelaNavegador")){

            //Verfifica se existe um painel criado no jogo se nao destroi este gameObject.
            if (!NavegadorCriado)
            {
                //Salvo os dados do painel.
                DontDestroyOnLoad(this);
                NavegadorCriado = true;
            }
            else
            {
                Destroy(this.gameObject);
            }

            posicao = new Vector3(1f, 1f, 1f);
            telaNoticias = GameObject.Find("TelaNavegador").transform.FindChild("TelaNoticias").gameObject;
            noticia = (GameObject)Resources.Load("Prefabs/Noticia", typeof(GameObject)) as GameObject;
        }
		
	}

    public static int ListaSize(){
        return listaNoticias.Count;
    }

    public static void AdicionarListaNoticias(int idNoticia){
        listaNoticias.Add(idNoticia);
    }

	public static void GerarNoticias(string modo, int id){

		telaNoticias.SetActive(true);

        if(modo.Equals("ProjetoEntregado")){

            LeituraArqNoticias.LerArquivo("Noticias_Aceito");
            LeituraArqNoticias.getNoticia(id);

            GameObject gameObjectFilha = Instantiate(noticia, posicao, noticia.transform.rotation) as GameObject;
            gameObjectFilha.transform.SetParent(GameObject.Find("GerenciadorNoticias").transform);
            gameObjectFilha.transform.localScale = new Vector3(1f, 1f);
            gameObjectFilha.transform.SetAsFirstSibling();

            gameObjectFilha.transform.FindChild("Titulo").GetComponent<Text>().text = LeituraArqNoticias.Titulo();
            gameObjectFilha.transform.FindChild("Texto").GetComponent<Text>().text = LeituraArqNoticias.Texto();
            Color cor = new Color(1f, 1f, 1f, 1f);
            gameObjectFilha.transform.FindChild("Categoria").GetComponent<Image>().color = cor;
            gameObjectFilha.transform.FindChild("Categoria").GetComponent<Image>().sprite = (Sprite)Resources.Load("Noticias/" + id, typeof(Sprite)) as Sprite;

        }
        else if(modo.Equals("ProjetoRecusado")){

            foreach (int idNoticia in listaNoticias){
                LeituraArqNoticias.LerArquivo("Noticias_Recusado");
                LeituraArqNoticias.getNoticia(idNoticia);

                GameObject gameObjectFilha = Instantiate(noticia, posicao, noticia.transform.rotation) as GameObject;
                gameObjectFilha.transform.SetParent(GameObject.Find("GerenciadorNoticias").transform);
                gameObjectFilha.transform.localScale = new Vector3(1f, 1f);
                gameObjectFilha.transform.SetAsFirstSibling();

                gameObjectFilha.transform.FindChild("Titulo").GetComponent<Text>().text = LeituraArqNoticias.Titulo();
                gameObjectFilha.transform.FindChild("Texto").GetComponent<Text>().text = LeituraArqNoticias.Texto();
                Color cor = new Color(1f, 1f, 1f, 1f);
                gameObjectFilha.transform.FindChild("Categoria").GetComponent<Image>().color = cor;
                gameObjectFilha.transform.FindChild("Categoria").GetComponent<Image>().sprite = (Sprite)Resources.Load("Noticias/" + idNoticia, typeof(Sprite)) as Sprite;
            }

            listaNoticias.Clear();
        }

		telaNoticias.SetActive(false);
	}

    public void DescontarDuracaoNoticia(){

        GameObject.Find("TelaNavegador").transform.FindChild("TelaNoticias").gameObject.SetActive(true);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Noticia")){

            int tempo = obj.gameObject.GetComponent<Navegador>().duracao--;
            if(tempo==0){
                Destroy(obj.gameObject);
            }
        }

        GameObject.Find("TelaNavegador").transform.FindChild("TelaNoticias").gameObject.SetActive(false);
    }
}
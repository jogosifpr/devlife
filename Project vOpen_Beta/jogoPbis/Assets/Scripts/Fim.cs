using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fim : MonoBehaviour {

    public GameObject camera;
    public GameObject fim;
    public GameObject mensagem;
    public Sprite social;
    public Sprite financeiro;
    public Sprite concluido;
    public GUIText legenda;
    public string texto;
    public enum Cenario { SOCIAL, FINANCEIRO, CONCLUIDO };

    private float alpha;
    private bool isAbertura;
    private bool isPressioneEnter;
    private static string final;
    
    public static string getCenario(){
        return final;
    }

    public static void setCenario(Cenario cenario){
        final = cenario.ToString();
    }

    void Start () {
        Destruir();
        if(final!=null) Finalizar((Cenario)System.Enum.Parse(typeof(Cenario), final));
        alpha = 0.01f;
        isAbertura = true;
    }

    void Finalizar(Cenario cenario){

        Sprite imagem;

        switch (cenario){
            case Cenario.SOCIAL:
                texto = "Final Divorciado ";
                imagem = social;
                break;
            case Cenario.FINANCEIRO:
                texto = "Final Endividado ";
                imagem = financeiro;
                break;
            case Cenario.CONCLUIDO:
                texto = "Final Feliz ";
                imagem = concluido;
                break;
            default:
                texto = "Final Feliz ";
                imagem = concluido;
                break;
        }

        texto = texto + "[Pressione Enter]";
        fim.GetComponent<SpriteRenderer>().sprite = imagem;
    }

    void Destruir(){
        foreach(GameObject objeto in GameObject.FindObjectsOfType<GameObject>()){
            if (objeto.name != fim.name && 
                objeto.name != camera.name && 
                objeto.name != mensagem.name && 
                objeto.name != "WebService" && 
                objeto.name != "Musica"){
                Destroy(objeto);
            }
        }
    }

    void Update(){

        if (isAbertura){
            Aparecer();
        }

        if (isPressioneEnter && Input.GetKey("return") || Input.GetKey("enter")){
            Application.ExternalEval("window.location.reload();");
            //SceneManager.LoadScene("Abertura");
        }
    }

    void Aparecer(){

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);

        if (alpha > 1f){
            isAbertura = false;
            StartCoroutine("AutoType");
        }else{
            alpha = alpha + 0.01f;
        }
    }

    IEnumerator AutoType(){
        foreach (char letra in texto.ToCharArray()){
            legenda.text += letra;
            yield return new WaitForSeconds(0.05f);
        }
        isPressioneEnter = true;
    }
}
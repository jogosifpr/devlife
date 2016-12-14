using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Piscar : MonoBehaviour {

    private bool isAparecer;
    private bool isDesaparecer;
    private bool isParado;
    private bool isMusica;
    private float alpha;

    public GameObject musica;

    // Use this for initialization
    void Start () {
        alpha = 0.01f;
        isAparecer = true;
        isParado = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!isParado) {

            if (isAparecer) {
                Aparecer();
            }

            if (isDesaparecer) {
                Desaparecer();
            }

            if (isMusica){
                musica.GetComponent<AudioSource>().volume -= (float)0.01;
            }

            if (Input.GetKey("return") || Input.GetKey("enter") && !isMusica) {
                //isMusica = true;
                GameObject.Find("DevLife").transform.FindChild("Canvas").gameObject.SetActive(true);
                this.GetComponent<GUITexture>().color = new Color(1f, 1f, 1f, 0);
            }

            if (musica.GetComponent<AudioSource>().volume <= 0){
                isParado = true;
                SceneManager.LoadScene("Empresa");
            }
        }
        
    }

    void Aparecer(){

        this.GetComponent<GUITexture>().color = new Color(1f,1f,1f,alpha);

        if (alpha >= 1){
            isAparecer = false;
            isDesaparecer = true;
        }else{
            alpha = alpha + 0.01f;
        }

    }

    void Desaparecer(){

        this.GetComponent<GUITexture>().color = new Color(1f,1f,1f,alpha);

        if (alpha <= 0){
            isAparecer = true;
            isDesaparecer = false;
        }else{
            alpha = alpha - 0.01f;
        }
    }
}

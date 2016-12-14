using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sentimento : MonoBehaviour {

	private GUITexture textureIMG;
	private static int estado = 3;
	public Texture2D[] coracao;
	public bool situacao;

	// Use this for initialization
	void Start () {
		textureIMG = this.GetComponent<GUITexture>() as GUITexture;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public static int ObterSocializacao(){
        return estado;
    }
	
	public bool isAtivo(){
		return this.situacao;
	}
	
	public void alterar(bool tipo){
		
		if(tipo){
			if(estado >= 3){
				estado = 3;
			}else{
				estado++;
			}
		}else{
			if(estado <= 0){
                estado = 0;
            }
            else{
				estado--;
			}
		}
		
		textureIMG.texture = coracao[estado];
	}
}

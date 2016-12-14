using UnityEngine;
using System.Collections;

public class Janela : MonoBehaviour {

	private static float tempo;
    private static bool janelaCriada;
    private static Color32 cor;
	private static Animator animacao;
	private static GameObject[] paisagens;    
	
	// Use this for initialization
	void Start(){

        //Animaçao da janela do quarto
		animacao = GameObject.Find("Quarto").GetComponent<Animator>() as Animator;

        //Chamando metodo a cada segundo do jogo.
        InvokeRepeating("Animacao", 0f, 1f);
	}

	// Update is called once per frame
	void Update (){
		
	}

	//Realiza a animaçao a cada dez segundos e reseta o tempo
    void Animacao(){

        if(tempo>= 1f){
            animacao.Play("janelaMovendo");
            tempo = 0;
        }

        tempo = tempo + 0.1f;
    }

    //Isso que vai deixar a cor das janelas do primeiro andar da casa
    public static void alterarPaisagem(int periodo){

        paisagens = GameObject.FindGameObjectsWithTag("paisagem");
        
        if (periodo == 0){
            cor = new Color32(255, 255, 255, 255);
        }else if (periodo == 1){
            cor = new Color32(191, 226, 255, 255);
        }else if (periodo == 2){
            cor = new Color32(255, 194, 11, 255);
        }else if (periodo == 3){
            cor = new Color32(0, 25, 51, 255);
        }else{
            cor = new Color32(255, 255, 255, 255);
        }

        // Pega a cor para colocar nas paisagens
        foreach(GameObject paisagem in paisagens){
            paisagem.GetComponent<SpriteRenderer>().color = cor;
        }
    }
}
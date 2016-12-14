using UnityEngine;
using System.Collections;

public class Energia : MonoBehaviour {

	[Header("Estados dos niveis da energia")]
	public Texture2D[] energiaImagens;
	
	private static int energia = 1;				// nivel da energia 
	private static GUITexture textureIMG;		// imagem da energia que sera alterada
	private static Texture2D[] energiaTexture;	// array de imagens da energia

	// Use this for initialization
	void Start(){

		// Atribuindo o array de imagens da energia do editor para variavel static
		energiaTexture = energiaImagens;

		//Localizando o GUITexture no projeto
		textureIMG = GameObject.Find("Energia").GetComponent<GUITexture>() as GUITexture;
		
		//Aplicando a Texture do array
		textureIMG.texture = energiaImagens[energia];
		
	}
	
	// Update is called once per frame
	void Update(){
		
	}

	// Metodo para obter o estado atual da energia
	public static int ObterEstado(){
		return energia;
	}

	// Metodo para diminuir o nivel da energia
	public static void Diminuir(){
		if(energia>0){
			energia--;
			Energia.AplicarAlteracao();
		}
	}

	// Metodo para recuperar o nivel da energia
	public static void Recuperar(){
		energia = 3;
		Energia.AplicarAlteracao();
	}

	// Metodo para disparar eventos resultante das acoes
	private static void AplicarAlteracao(){
		Energia.AlterarImagem();
	}

	//Aplicando a Texture do array
	private static void AlterarImagem(){
		textureIMG.texture = energiaTexture[energia];
	}
}
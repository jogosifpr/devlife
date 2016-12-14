using UnityEngine;

public class Autodestruir : MonoBehaviour {

	private static bool isCriado;
	private static int contador;
	private static int quantidadeObjetos;

    // Método executado somente uma vez durante o tempo de vida do jogo.
    void Awake(){
        quantidadeObjetos = GameObject.FindGameObjectsWithTag("DontDestroy").Length;
    }

    // Método padrão de inicialização.
    void Start(){

		// Verfifica se existe objeto criado no jogo.
		if(!isCriado){

			// Salvo os dados do object.
			DontDestroyOnLoad(this);

            // Se 
			if(quantidadeObjetos == contador){
                isCriado = true;
			}

			contador++;

        }else{ // Se nao destroi object.
			Destroy(this.gameObject);
		}
	}
}
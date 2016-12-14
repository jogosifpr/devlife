using UnityEngine;
using System.Collections;

public class Porta : MonoBehaviour {

	private static GameObject banheiro;
	
	// Use this for initialization
	void Start(){
		banheiro = GameObject.Find("Casa").transform.FindChild("Banheiro").gameObject;
	}
	
	// Update is called once per frame
	void Update(){
		
	}
	
	// Metodo para selecionar a porta para abrir.
	public static void Abrir(){
		GameObject.Find("Personagem").GetComponent<Personagem>().reiniciarTempoInativo();
		ExibirLocal(banheiro);
	}
	
	// Metodo para exibir local.
	private static void ExibirLocal(GameObject local){
		
		// Se o GameObject estiver marcado como invisivel.
		if(!local.activeInHierarchy){
			// Passa para visivel o GameObject.
			local.SetActive(true);
            Porta.AlterarParedes(false);
            GameObject.Find("Camila").GetComponent<BoxCollider2D>().enabled = false;
            GameObject.Find("Computador").GetComponent<BoxCollider2D>().enabled = false;
        }
        else{
			// Passa para invisivel o GameObject.
			local.SetActive(false);
            Porta.AlterarParedes(true);
            GameObject.Find("Camila").GetComponent<BoxCollider2D>().enabled = true;
            GameObject.Find("Computador").GetComponent<BoxCollider2D>().enabled = true;
        }
	}

    public static void AlterarParedes(bool estado){

        int qtdParedes = GameObject.Find("Paredes").transform.childCount;

        for(int i = 0; i<qtdParedes; i++){
            GameObject parede = GameObject.Find("Paredes").transform.GetChild(i).transform.gameObject;
            parede.gameObject.SetActive(estado);
        }
    }
}
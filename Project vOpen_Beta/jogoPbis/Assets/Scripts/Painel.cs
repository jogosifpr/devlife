using UnityEngine;
using UnityEngine.SceneManagement;

public class Painel : MonoBehaviour {

	// Use this for initialization
	void Start(){
	
	}
	
	// Update is called once per frame
	void Update(){

		if(SceneManager.GetActiveScene().name.Equals("Casa")){
			this.gameObject.transform.position = new Vector3(0f,0f,0);
            this.gameObject.transform.FindChild("Energia").gameObject.transform.position = new Vector3(0.1f,0.84f,0f);
        }
        else if(SceneManager.GetActiveScene().name.Equals("Computador")){
			this.gameObject.transform.position = new Vector3(100f,100f,0);
            this.gameObject.transform.FindChild("Energia").gameObject.transform.position = new Vector3(0.895f,0.104f,10f);
		}
	}
}

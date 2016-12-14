using UnityEngine;
using UnityEngine.SceneManagement;

public class Visao : MonoBehaviour{
	
	private Transform personagem;
	public float acompanhar = 0.5f; //Velocidade que vai acompanhar a camera.
	private float velocidade = 0.0f;  //Velocidade corrente da execuçao do jogo.
	public float xMax = 175; //Valor máximo de x.
	public float xMin = -321; //Valor mínimo de x.
	public float ajusteAltura;
	
	// Use this for initialization
	void Start(){
		//Localizando a componenete Transform do gameObject Personagem.
		if(SceneManager.GetActiveScene().name.Equals("Casa")) {
			personagem = GameObject.Find("Personagem")
                        .GetComponent<Transform> () as Transform;
		}else {
			ajusteAltura = 2f;
			personagem = GameObject.Find("Empresa")
						.transform.FindChild("Personagem")
						.GetComponent<Transform> () as Transform;
		}

	}
	
	// Update is called once per frame
	void Update(){

		this.Acompanhar();
	}

	void Acompanhar(){

		//Declarando classe que vai armazenar as posicoes x e y.
		Vector2 novaPosicao2D = Vector2.zero;
		
		//Aplicando a nova posiçao de X, onde seleciona a posicao de inicio e fim de x de onde o personagem estiver. 
		novaPosicao2D.x = Mathf.SmoothDamp(transform.position.x, personagem.position.x, ref velocidade, acompanhar);
		
		//Definindo classe que vai armazenar as posiçoes de x,y e z.
		Vector3 novaPosicao = new Vector3(novaPosicao2D.x, personagem.position.y + ajusteAltura, transform.position.z);
		
		//Defnindo os valores de x em que a camera deve parar.
		novaPosicao = new Vector3(Mathf.Clamp(novaPosicao.x, xMin ,xMax),novaPosicao.y, novaPosicao.z);
		
		//Aplicando a posiçao na camera em tempo real do jogo.
		transform.position = Vector3.Slerp(transform.position, novaPosicao, Time.time);
	}
}
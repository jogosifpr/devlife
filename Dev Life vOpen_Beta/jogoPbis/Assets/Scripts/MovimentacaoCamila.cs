using UnityEngine;
using System.Collections;

public class MovimentacaoCamila : MonoBehaviour {

	private static GameObject camila;
	private float posAtual;
	private int comodo;

	public float movementSpeed = 60f;

	private int cont;
	private int valor;
	private bool parado = false;
	private static bool conversando = false;
	private int acao;

    private static Animator animacao;

    // Use this for initialization
    void Start () {
		camila = GameObject.Find("Camila");
		parado = false;
		valor = Random.Range (250, 500);
        animacao = GameObject.Find("Camila").GetComponent<Animator>() as Animator;
    }


	void FixedUpdate() {

		if (!conversando) {
			cont++;

            if (!parado) {
                animacao.Play("Andando");
                transform.Translate (Vector2.right * movementSpeed * Time.deltaTime);
            }

			if (cont == valor) {
                acao = Random.Range (1, 5);
				if (acao == 1) {//direta
					transform.eulerAngles = new Vector2 (0, 0);
                    animacao.Play("Parado");
                } else if (acao == 2) {//esquerda
					transform.eulerAngles = new Vector2 (0, 180);
                    animacao.Play("Parado");
                } else {
					parado = !parado;
                    animacao.Play("Parado");
                }
				cont = 0;
				valor = Random.Range (250, 500);
            }
		}
	}

	void OnTriggerEnter2D(Collider2D colisor) {
		if (colisor.gameObject.name == "Colisor esquerdo") {
		    transform.eulerAngles = new Vector2(0, 0);
		} else if (colisor.gameObject.name == "Colisor direito") {
		    transform.eulerAngles = new Vector2(0, 180);
		}
	}


	public static void mudarLocal(){

		int comodo = Random.Range (1, 4);

		if (comodo == 1) {//quarto
			camila.transform.position = new Vector3(229.2f,-23,0);

		} else if (comodo == 2) {//sala
			camila.transform.position = new Vector3(-255f,-23,0);

		} else if (comodo == 3) {//cozinha
			camila.transform.position = new Vector3(-79.5f,-23,0);
		}
	}

	public static void inicializarConversa(){
		conversando = true;
    animacao.Play("Parado");
  }

	public static void finalizarConversa(){
		conversando = false;
	}
}

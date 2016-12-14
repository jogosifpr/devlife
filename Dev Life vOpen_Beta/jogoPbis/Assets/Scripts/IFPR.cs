using UnityEngine;
using System.Collections;

public class IFPR : MonoBehaviour {

    public GameObject devlife;
    public Animator animacao;
    public GameObject musica;

    private float alpha;
    private bool isAbertura;
    private bool isAnimacao;

    void Start () {
        isAbertura = true;
        alpha = 0.01f;
    }
	
	void Update () {

        if (isAbertura){
            Aparecer();
        }

        if (isAnimacao){
            if (animacao.GetCurrentAnimatorStateInfo(0).IsName("logoParado")){
                StartCoroutine(LiberarAbertura());
            }
        }
    }

    void Aparecer(){

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);

        if (alpha > 1f){
            StartCoroutine(LiberarAnimacao());
            isAbertura = false;
        }else{
            alpha = alpha + 0.01f;
        }
    }

    IEnumerator LiberarAbertura(){
        yield return new WaitForSeconds(2);

        this.gameObject.SetActive(false);
        musica.gameObject.SetActive(true);
        devlife.GetComponent<Abertura>().isInicio = true;
        
        yield break;
    }

    IEnumerator LiberarAnimacao(){
        yield return new WaitForSeconds(2);

        isAnimacao = true;
        animacao.Play("logoAnimacao");

        yield break;
    }
}
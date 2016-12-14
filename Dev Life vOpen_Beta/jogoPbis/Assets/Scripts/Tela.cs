using UnityEngine;
using System.Collections;

public class Tela : MonoBehaviour {

	private static Animator topo;
	private static Animator baixo;

	// Use this for initialization
	void Start () {
		topo = GameObject.Find("Main Camera").transform.FindChild("efeitoTopo").GetComponent<Animator>();
		baixo = GameObject.Find("Main Camera").transform.FindChild("efeitoBaixo").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public static void Blink(){
        Tela.Alterar(true);
		topo.Play("efeitoTopo");
        baixo.Play("efeitoBaixo");
	}

	public static bool isAnimacao(){

        bool isAnimacaoTopo = topo.GetCurrentAnimatorStateInfo(0).IsName("efeitoTopoIdle");
        bool isAnimacaoBaixo = baixo.GetCurrentAnimatorStateInfo(0).IsName("efeitoBaixoIdle");

        if(isAnimacaoTopo && isAnimacaoBaixo) return true;
        return false;
	}

    public static void Alterar(bool estado){
        GameObject.Find("Main Camera").transform.FindChild("efeitoTopo").gameObject.SetActive(estado);
        GameObject.Find("Main Camera").transform.FindChild("efeitoBaixo").gameObject.SetActive(estado);
    }
}
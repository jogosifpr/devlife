using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Divida : MonoBehaviour {

	public string nome;
	public int valor;
	private static int total = 0;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void selecionarDivida(){

		if(this.GetComponent<Toggle> ().isOn) {
			total = total + this.valor;
		}else{
			total = total - this.valor;
		}

		this.aplicarTotal();


		if(GameObject.Find("Financeiro").GetComponent<Financeiro>().dinheiroDisponivel(total)){
			GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasAtrasada/btnPagar").gameObject.SetActive(true);
		}else{
			GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasAtrasada/btnPagar").gameObject.SetActive(false);
		}
	}

	public void aplicarTotal(){
		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasAtrasada/Destaque/Valor").GetComponent<Text>().text = "R$ " + total.ToString()+",00";
	}

	public void zerarTotal(){
		total = 0;
	}

	public int obterTotal(){
		return total;
	}



}

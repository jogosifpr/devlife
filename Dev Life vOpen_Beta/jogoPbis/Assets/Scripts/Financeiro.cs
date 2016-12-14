using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Financeiro : MonoBehaviour {

    private static int valorDivida;
	private static int saldo;
	private static string checkbox;
	private static int contasPagarPrazo = 5;
	private static bool prazo;
    private static Sprite contaOn;
    private static Sprite contaOff;
    public static bool isAtivo;

    public GameObject divida;
	public Toggle[] checkboxContas;
	

	public GameObject btnPagar;
	private GameObject[] contas;
	
	private Personagem personagem;

	// Use this for initialization
	void Start () {
        contaOn = Resources.Load("contas_on", typeof(Sprite)) as Sprite;
        contaOff = Resources.Load("contas_off", typeof(Sprite)) as Sprite;
        personagem = GameObject.Find("Personagem").GetComponent<Personagem>() as Personagem;
	}

    public static int ObterDespesa(){

        GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(true);

        int qtdConta = 0;

        foreach(GameObject conta in GameObject.FindGameObjectsWithTag("Conta")){
            if(conta.GetComponent<Toggle>().interactable){
                qtdConta++;
            }
        }

        int qdtDivida = 0;

        foreach(GameObject divida in GameObject.FindGameObjectsWithTag("Divida")){
            if(divida.GetComponent<Toggle>().interactable){
                qdtDivida++;
            }
        }

        GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(false);

        return qtdConta + qdtDivida;
    }

	public void gerarDivida(GameObject _divida){

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(true);

		foreach(GameObject conta in GameObject.FindGameObjectsWithTag("Conta")){
			if(conta.GetComponent<Toggle>().interactable){

				GameObject gameObjectFilha = Instantiate(_divida, new Vector3(0, 0, 1f), _divida.transform.rotation) as GameObject;
				string nome = conta.transform.FindChild("Nome").GetComponent<Text>().text;
				string valor = conta.transform.FindChild("Valor").GetComponent<Text>().text;

				gameObjectFilha.transform.SetParent(GameObject.Find("GerenciadorEmail").transform);
				gameObjectFilha.transform.localScale = new Vector3(1f,1f);

				gameObjectFilha.GetComponent<Divida>().nome = conta.GetComponent<Divida>().nome;
				gameObjectFilha.GetComponent<Divida>().valor = aplicarJuros(conta.GetComponent<Divida>().valor);
				gameObjectFilha.transform.FindChild("Nome").GetComponent<Text>().text = nome;
				gameObjectFilha.transform.FindChild("Valor").GetComponent<Text>().text = "R$ " + aplicarJuros(conta.GetComponent<Divida>().valor) + "(+10%)";

			}
		}

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(false);
	}

	public int aplicarJuros(int valor){

		int preco = valor + Mathf.FloorToInt(valor * 0.1f);

		return (int)preco;
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown("tab") && personagem.isFinanceiro() && !isDialogo()){
			if(!GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.activeInHierarchy){
				this.abrir();
                isAtivo = true;
                Personagem.ResetRetard();
            }else{
                isAtivo = false;
				this.fechar();
            }
		}

	}

    public static void Focar(bool isAtivo){

        if(isAtivo){
            GameObject.Find("Financeiro").GetComponent<SpriteRenderer>().sprite = contaOn;
        }else{
            GameObject.Find("Financeiro").GetComponent<SpriteRenderer>().sprite = contaOff;
        }        
    }

    public bool isDialogo(){
		return GameObject.Find("CaixaDialogo(Clone)");
	}

	public int getPrazo(){
		return contasPagarPrazo;
	}

	public bool isPrazo(){
		return prazo;
	}

	public void verificarCheckBox(string opcao){
		checkbox = opcao;
	}

	public void selecionarConta(int valor){

		foreach(Toggle checkboxConta in checkboxContas){
			if(checkbox.Equals(checkboxConta.gameObject.name)){
				if(checkboxConta.isOn){
					valorDivida += valor;
				}else{
					valorDivida -= valor;
				}
			}
		}

		btnPagar.SetActive(dinheiroDisponivel(valorDivida));

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Destaque/Valor").GetComponent<Text>().text = "R$ " + valorDivida.ToString()+",00";
	}

	public bool dinheiroDisponivel(int totalConta){
		if(totalConta <= Dinheiro.ObterValor()){
			return true;
		}
		return false;
	}

	public void abrir(){

		personagem.bloqueio();

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(true);

		foreach(GameObject conta in GameObject.FindGameObjectsWithTag("Conta")){
			if(conta.GetComponent<Toggle>().isOn){
				conta.GetComponent<Toggle>().interactable = false;
			}
		}

		GameObject.Find("TelaFinanceiro").transform.FindChild("CanvasDivida").gameObject.SetActive(false);
		GameObject.Find("TelaFinanceiro").transform.FindChild("CanvasDivida").gameObject.SetActive(true);

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Destaque/Valor").GetComponent<Text>().text = "R$ 0,00";
	}

	public void fechar(){

		foreach(GameObject conta in GameObject.FindGameObjectsWithTag("Conta")){
			if(conta.GetComponent<Toggle>().isOn && conta.GetComponent<Toggle>().interactable){
				conta.GetComponent<Toggle>().isOn = false;
			}
		}

		foreach(GameObject conta in GameObject.FindGameObjectsWithTag("Divida")){
			if(conta.GetComponent<Toggle>().isOn){
				conta.GetComponent<Toggle>().isOn = false;
			}
		}

		this.reiniciarTela();
	}

	public void pagarConta(){
		Dinheiro.Descontar(valorDivida);
		valorDivida = 0;
		this.abrir();
	}

	public void pagarDivida(){

		Divida scriptDivida = GameObject.Find("Financeiro").GetComponent<Financeiro>().divida.gameObject.GetComponent<Divida>();

		Dinheiro.Descontar(scriptDivida.obterTotal());

		foreach(GameObject divida in GameObject.FindGameObjectsWithTag("Divida")){
			if(divida.GetComponent<Toggle>().isOn){
				Destroy(divida.gameObject);
			}
		}

		scriptDivida.zerarTotal();
		scriptDivida.aplicarTotal();
	}

	public void reiniciarTela(){
        Personagem.Desbloquear();
		valorDivida = 0;
		GameObject.Find("Financeiro").GetComponent<Financeiro>().divida.gameObject.GetComponent<Divida>().valor = 0;
		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(false);
		Mouse.Padrao();
	}

	public void diminuirPrazo(){
		contasPagarPrazo--;
        GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Prazo").GetComponent<Text> ().text = "Todas as contas já chegaram.";
        //GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Prazo").GetComponent<Text> ().text = "Prazo: " + contasPagarPrazo + " dia(s)";
    }

    public void gerarConta(){

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(true);

		Transform conta = GameObject.Find ("GerenciadorFinanceiro").transform.FindChild ("TelaFinanceiro/Canvas/ContasPagar/Contas").GetComponent<Transform>() as Transform;
		int qtdContas = GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Contas").transform.childCount;

		if(qtdContas > GameObject.FindGameObjectsWithTag ("Conta").Length){

			for(int i = 1; i<=qtdContas; i++){
				if(!conta.GetChild(i).gameObject.activeInHierarchy) {
					conta.GetChild(i).gameObject.SetActive(true);
					break;
				}
			}
		}else{
			GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Prazo").gameObject.SetActive(true);
			prazo = true;
		}

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(false);
	}

	public void reiniciarListaConta(){

		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(true);

		Transform contaPagar = GameObject.Find ("GerenciadorFinanceiro").transform.FindChild ("TelaFinanceiro/Canvas/ContasPagar/Contas").GetComponent<Transform>() as Transform;
		int qtdContas = GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Contas").transform.childCount;

		for (int i = 0; i<qtdContas; i++) {

			contaPagar.GetChild(i).GetComponent<Toggle>().interactable = true;
			contaPagar.GetChild(i).gameObject.GetComponent<Toggle>().isOn = false;

			if (contaPagar.GetChild (i).gameObject.activeInHierarchy && i != 0) {
				contaPagar.GetChild (i).gameObject.SetActive(false);
			}
		}

		valorDivida = 0;
		contasPagarPrazo = 5;
		prazo = false;

        //GameObject.Find ("GerenciadorFinanceiro").transform.FindChild ("TelaFinanceiro/Canvas/ContasPagar/Prazo").gameObject.GetComponent<Text>().text = "Prazo: 5 dia(s)";
        GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro/Canvas/ContasPagar/Prazo").gameObject.GetComponent<Text>().text = "Todas as contas já chegaram.";
        GameObject.Find ("GerenciadorFinanceiro").transform.FindChild ("TelaFinanceiro/Canvas/ContasPagar/Prazo").gameObject.SetActive(false);
		GameObject.Find("GerenciadorFinanceiro").transform.FindChild("TelaFinanceiro").gameObject.SetActive(false);
	}

}

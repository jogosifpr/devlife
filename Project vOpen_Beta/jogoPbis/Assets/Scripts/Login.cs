using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour {

    public GameObject login;
    public GameObject email;
    public GameObject senha;
    public GameObject botao;

    private string Email;
    private string Senha;
    private WebService webService;

    private static string url = "http://galactus.sytes.net:8080/Galactus/rest/jogo/login";

    public void LoginButton() {

        webService = GameObject.Find("WebService").GetComponent<WebService>();

        if (Senha != "" && Email != "") {

            botao.GetComponent<Button>().interactable = false;

            WWWForm form = new WWWForm();

            form.AddField("email", Email);
            form.AddField("senha", Senha);

            WWW envio = new WWW(url, form.data, form.headers);
            StartCoroutine(WaitForRequest(envio));
        }
    }

	void Update () {

        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (email.GetComponent<InputField>().isFocused) {
                senha.GetComponent<InputField>().Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (Senha != "" && Email != "") {
                LoginButton();
            }
        }

        Email = email.GetComponent<InputField>().text;
        Senha = senha.GetComponent<InputField>().text;
    }

    IEnumerator WaitForRequest(WWW www){

        yield return www;

        // Validando Autenticação
        if (www.error == null && !www.text.Equals("")){
            Debug.Log("Autenticação com sucesso! Usuário de ID: " + www.text);
            GameObject.Find("WebService").GetComponent<WebService>().SetJogador(www.text);
            SceneManager.LoadScene("Empresa");
        }else{
            Debug.Log("Autenticação com erro: " + www.error);
            email.GetComponent<InputField>().text = "";
            senha.GetComponent<InputField>().text = "";
            botao.GetComponent<Button>().interactable = true;
        }
    }
}
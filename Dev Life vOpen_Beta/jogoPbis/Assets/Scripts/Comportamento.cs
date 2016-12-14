using UnityEngine;
using System.Collections;

public class Comportamento : MonoBehaviour {

    // Declarando os contadores de tempo e cliques.
    public static int tempoDia;
    public static int cliqueGeral;
    public static int cliqueEmail;
    public static int cliqueTrabalho;
    public static int cliqueProjeto;
    public static int cliqueNavegador;
    public static int cliqueComputador;

    // Declarando os tipos de clique.
    public enum Clique { GERAL, EMAIL, TRABALHO, PROJETO, NAVEGADOR, COMPUTADOR }

    // Método padrão de inicialização.
    void Start() {
        //Chamando método a cada segundo do jogo.
        InvokeRepeating("Tempo", 1f, 1f);
    }

    // Método para contar os segundos do dia durante o jogo.
    void Tempo() { tempoDia++; }

    // Método para obter clique geral.
    void OnMouseDown() { Obter(Clique.GERAL); }

    // Método para obter o tipo de clique.
    public static void Obter(Clique acao) {

        switch(acao){
            case Clique.GERAL:
                cliqueGeral++;
                break;
            case Clique.EMAIL:
                cliqueEmail++;
                break;
            case Clique.PROJETO:
                cliqueProjeto++;
                break;
            case Clique.TRABALHO:
                cliqueTrabalho++;
                break;
            case Clique.NAVEGADOR:
                cliqueNavegador++;
                break;
            case Clique.COMPUTADOR:
                cliqueComputador++;
                break;
        }
    }

    // Método para reiniciar os contadores.
    public static void Reiniciar(){
        tempoDia = 0;
        cliqueGeral = 0;
        cliqueEmail = 0;
        cliqueProjeto = 0;
        cliqueTrabalho = 0;
        cliqueNavegador = 0;
        cliqueComputador = 0;
    }

    // Métodos para obter os dados do comportamento.
    public static int ObterTempo() { return tempoDia; }
    public static int ObterGeral() { return cliqueGeral; }
    public static int ObterEmail() { return cliqueEmail; }
    public static int ObterProjeto() { return cliqueProjeto; }
    public static int ObterTrabalho() { return cliqueTrabalho; }
    public static int ObterNavegador() { return cliqueNavegador; }
    public static int ObterComputador() { return cliqueComputador; }
}
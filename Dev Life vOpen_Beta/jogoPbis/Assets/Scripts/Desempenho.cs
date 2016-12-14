using UnityEngine;
using System.Collections;

public class Desempenho : MonoBehaviour {

    // Declarando lista que vai armazenar os projetos.
    private static ArrayList listaProjetos = new ArrayList();

    // Declarando classe projeto.
    public class Projeto{

        // Declarando situacao(aceito/recusado), id e progresso do projeto.
        public int id;
        public int progresso;
        public bool situacao;

        // Método construtor.
        public Projeto(int _id, int _progresso, bool _situacao){
            this.id = _id;
            this.progresso = _progresso;
            this.situacao = _situacao;
        }
    }

    // Método para adicionar projeto na lista.
    public static void AddProjeto(int _id, int _progresso, bool _situacao){
        listaProjetos.Add(new Projeto(_id, _progresso, _situacao));
    }

    // Método para reiniciar a lista.
    public static void Reiniciar(){
        listaProjetos.Clear();
    }

    // Método para retornar os projetos da lista.
    public static ArrayList Lista(){
        return listaProjetos;
    }
}
using UnityEngine;
using System.Collections;

public class WebService : MonoBehaviour {

    // Declarando o url que irá realizar comunicação via websercice.
    private static string url = "http://galactus.sytes.net:8080/Galactus/rest/jogo/webservice";

    // Declarando o jogador que está na sessão.
    public static string jogador;

    // Inserindo o codigo do jogador.
    public void SetJogador(string id){
        jogador = id;
    }

    // Método para enviar os dados.
    public static void Enviar(){
        
        WWWForm form = new WWWForm();

        // Comportamento
        form.AddField("id", jogador);
        form.AddField("dia", Data.ObterData());
        form.AddField("dinheiro", Dinheiro.ObterValor());
        form.AddField("despesa", Financeiro.ObterDespesa());
        form.AddField("tempoDia", Comportamento.ObterTempo());
        form.AddField("socializacao", Sentimento.ObterSocializacao());
        form.AddField("cliqueGeral", Comportamento.ObterGeral());
        form.AddField("cliqueEmail", Comportamento.ObterEmail());
        form.AddField("cliqueProjeto", Comportamento.ObterProjeto());
        form.AddField("cliqueTrabalho", Comportamento.ObterTrabalho());
        form.AddField("cliqueNavegador", Comportamento.ObterNavegador());
        form.AddField("cliqueComputador", Comportamento.ObterComputador());
        Comportamento.Reiniciar();

        // Desempenho
        foreach (Desempenho.Projeto projeto in Desempenho.Lista()){
            form.AddField("desempenho_projeto[]", projeto.id);
            form.AddField("desempenho_progresso[]", projeto.progresso);
            form.AddField("desempenho_situacao[]", projeto.situacao.ToString());
        }

        Desempenho.Reiniciar();

        // Conclusao
        if(Fim.getCenario()!=null) form.AddField("conclusao", Fim.getCenario());

        // Enviando os dados do form através do método post.
        WWW envio = new WWW(url, form.data, form.headers);
    }
}
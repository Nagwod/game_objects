﻿using System.Collections; //Para utilizar corrotinas
using UnityEngine; //Padrão

public class Final : MonoBehaviour
{
    GameControl gameControl; //Variável do tipo gamecontrol, para acessar os métodos de lá 
    private AudioSource source;
    [SerializeField] private GameObject TelaFinal, TelaPrincipal;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") //Quando o jogador colidir com o final
        {
            ConfigGeral.completa = true;
            StartCoroutine(Delay()); //Começa a corrotina quando o jogador colidir com o final da fase
        }
    }

    private void CalculaResultados()
    {
        ConfigGeral.gravMedia = ConfigGeral.gravsoma / ConfigGeral.gravcont;
        gameControl.RetreiveSaveData(gameControl.GetNomeAdmin() +"/"+ gameControl.GetNomeJogador()); //Carrega o save do jogador
        if (gameControl.GetFaseCompleta(ConfigGeral.faseAtual-1)<2) //Verifica se a fase jogada era a última fase liberada
        {
            gameControl.SetFaseCompleta(ConfigGeral.faseAtual-1, 2); //Libera a próxima fase
            gameControl.SetResultados( //Passa os resultados de ConfigGeral para o gameControl
                ConfigGeral.faseAtual - 1,
                ConfigGeral.moedas,
                (int)System.Math.Floor(ConfigGeral.tempo),
                ConfigGeral.gravMedia,
                ConfigGeral.mortes,
                ConfigGeral.mortesBuraco,
                ConfigGeral.mortesEspinho,
                ConfigGeral.mortesParedes,
                ConfigGeral.mortesQueda,
                ConfigGeral.batidasParedes,
                ConfigGeral.batidasArvores
            );
            gameControl.SendSaveToDatabase();
        }
        
        /*
        pontosm = ConfigGeral.moedas * 1000 / moedas; //Calcula um valor entre 0 e 1000 para cada qauntidade de moedas ente a quantidade minima e maxima
        if (ConfigGeral.tempo < tempo)
        {
            pontost = 1000; //Se for maior que o tempo mínimo, a pontuação é 1000
        }
        else
        {
            if (ConfigGeral.tempo < tempo * 3)
            {
                pontost = ((((tempo * 2) - (ConfigGeral.tempo - tempo)) / tempo * 2) * 1000) / 4; //Calcula um valor de 0 a 1000 para cada valor entre o tempo mínimo e máximo
            }
            else
            {
                pontost = 0; //Se for maior que o tempo máximo (triplo do tempo mínimo), a pontuação é 0
            }
        }
        pontos = (pontosm + pontost) / 2; //A pontuação é a média entre os pontos de moedas e tempo
        gameControl.SetPontos(fase, (int)pontos, (int)System.Math.Floor(ConfigGeral.tempo), ConfigGeral.moedas);
        */
    }

    IEnumerator Delay()
    {
        source.Play(0); //Toca o som de parabéns
        TelaFinal.SetActive(true); //Aparece a tela final
        TelaPrincipal.SetActive(false); //Sai a tela principal
        Time.timeScale = 0; //Pausa o jogo
        CalculaResultados(); //Manda os resultados para serem salvos
        yield return new WaitForSeconds(0.2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameControl.gameControl; //Seta a variável gameControl
        source = GetComponent<AudioSource>(); //Seta o áudio
    }
}

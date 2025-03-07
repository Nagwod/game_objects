﻿using System.Collections; //Para utilizar corrotinas
using UnityEngine; //Padrão
using UnityEngine.UI; //Para interface

public class ConfigGeral : MonoBehaviour
{
    GameControl gameControl;
    [SerializeField] private Text tmoedas, ttempo; //Campos de texto que aparecem na tela durante a fase
    [SerializeField] private int fase; //Para setar a fase através da interface do unity
    static public int moedas; //Dados a serem coletados durantre o jogo
    static public int gravcont;
    static public float tempo, gravMedia, gravsoma; //Dados a serem coletados durantre o jogo
    static public int faseAtual; //Fase que está sendo jogada
    static public int mortes, mortesBuraco, mortesEspinho, mortesParedes, mortesQueda, batidasParedes, batidasArvores; //Dados a serem coletados durantre o jogo
    static public bool completa;

    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameControl.gameControl; //Seta o gameControl
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        completa = false; //Evita autosave quando a fase for completa
        tempo = 0;
        gravMedia = 0;
        moedas = 0; //É alterada pelo script de Moeda
        mortes = 0;
        mortesBuraco = 0; //É alterada pelo script Cai
        mortesEspinho = 0; //É alterada pelo script Espinho
        mortesParedes = 0; //É alterada pelo script Esmaga
        mortesQueda = 0; //É alterada pelo script Cai
        batidasParedes = 0; //É alterada pelo script do Tatu
        batidasArvores = 0; //É alterada pelo script do Tatu
        //pontos = 0;
        faseAtual = fase; //Recebe a fase
        gravsoma = 0;
        gravcont = 0;
        if (gameControl.GetFaseCompleta(faseAtual - 1) < 2) //Verifica se a fase não foi completa
        {
            StartCoroutine(AutoSave());
        }
    }

    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(5);
        if (!completa) //Evita autosave quando a fase for completa
        {
            if (gameControl.GetFaseCompleta(faseAtual - 1) < 1) //Verifica se a fase não foi jogada ainda
            {
                gameControl.SetFaseCompleta(faseAtual - 1, 1);
            }
            if (gameControl.GetTempos(faseAtual - 1) <= System.Math.Floor(tempo))
            {
                gameControl.SetResultados( //Passa os resultados de ConfigGeral para o gameControl
                    faseAtual - 1,
                    moedas,
                    (int)System.Math.Floor(tempo),
                    gravMedia,
                    mortes,
                    mortesBuraco,
                    mortesEspinho,
                    mortesParedes,
                    mortesQueda,
                    batidasParedes,
                    batidasArvores
                );
                gameControl.SendSaveToDatabase();
            }
            StartCoroutine(AutoSave());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        tempo += Time.deltaTime; //Aumenta o tempo
        if(Mathf.Abs(Physics.gravity.x) > Mathf.Abs(Physics.gravity.z))
        {
            gravsoma += Mathf.Abs(Physics.gravity.x);
        }
        else
        {
            gravsoma += Mathf.Abs(Physics.gravity.z);
        }
        gravcont += 1;
        gravMedia = gravsoma/gravcont;
        ttempo.text = "Tempo: " + (System.Math.Floor(tempo)); //Tempo arredondado é passado pro campo de texto
        tmoedas.text = "Moedas: "+moedas; //Quantidade de moedas é passada para o campo de texto
        //tmortes.text = "Total:" + mortes + " Buracos:" + mortesBuraco + " Espinhos:" + mortesEspinho + " Esmagado:" + mortesParedes + " Quedas:" + mortesQueda + " Batidas Arvore:" + batidasArvores + " Paredes:" + batidasParedes;
    }
}

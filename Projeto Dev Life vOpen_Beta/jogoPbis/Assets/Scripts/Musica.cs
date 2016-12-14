using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musica : MonoBehaviour {

    private static bool isMusica;
    public GameObject musica;

    // Use this for initialization
    void Start () {
        StartCoroutine(IniciarMusica());
    }

    IEnumerator IniciarMusica(){
        yield return new WaitForSeconds(1);
        isMusica = true;
        yield break;
    }

    // Update is called once per frame
    void Update () {
        if (isMusica){
            if (musica.GetComponent<AudioSource>().volume >= 100){
                isMusica = false;
            }else{
                musica.GetComponent<AudioSource>().volume += 0.001f;
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Abertura : MonoBehaviour {

    public bool isInicio;

    private float alpha;

    void Start () {
		alpha = 0.01f;
	}

    void Update () {
		if(isInicio){
			Aparecer();
		}
	}

	void Aparecer(){

        this.gameObject.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, alpha);

        if (alpha > 1f){
            isInicio = false;
            this.gameObject.transform.FindChild("Enter").gameObject.SetActive(true);
		}else{
            alpha = alpha + 0.005f;
		}
	}
}
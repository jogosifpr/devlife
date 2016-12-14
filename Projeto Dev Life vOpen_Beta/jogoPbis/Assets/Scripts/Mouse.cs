using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mouse : MonoBehaviour {

	// Modo do mouse que sera alterar o cursor virtual e nao o do hardware.
	private static CursorMode cursorMode = CursorMode.Auto;

	// Ponto de acesso do mouse.
	private static Vector2 hotSpot = Vector2.zero;

	// Imagens do mouse.
	private static Texture2D imagemMouse;
	private static Texture2D imagemMouseAtivo;

	void Start(){
		//aqui seta a imagem do mouse padrao
		Mouse.Padrao();
	}

	void OnMouseOver() {
		Mouse.Ativo();
	}

	void OnMouseExit() {
		Mouse.Padrao();
	}

	public void OnPointyEnter(){
		Mouse.Ativo();
	}
	
	public void OnPointyExit(){
		Mouse.Padrao();
	}

	public static void Padrao(){
		imagemMouse = (Texture2D)Resources.Load("mouse", typeof(Texture2D)) as Texture2D;
		Cursor.SetCursor(imagemMouse, hotSpot, cursorMode);
	}
	
	public static void Ativo(){
		imagemMouseAtivo = (Texture2D)Resources.Load("mouseAtivo", typeof(Texture2D)) as Texture2D;
		Cursor.SetCursor(imagemMouseAtivo, hotSpot, cursorMode);
	}
}

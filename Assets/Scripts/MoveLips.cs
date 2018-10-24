using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLips : MonoBehaviour {

	public Texture[] textures;
	public Renderer rend;
	int index = 0;

	bool mouseDown = true;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseDown) {
			
		
		
		}
	}

//	void MovePhotos(float posY){
//		switch (posY) {
//		case (posY > 2f && posY < 5f):
//			rend.material.mainTexture = textures [1];
//			break;
//		case(posY < )
//		}
//	}
//
	void OnMouseDown(){

		mouseDown = true;

//		index++;
//		if (index >= textures.Length) {
//			index = 0;
//		}
//		rend.material.mainTexture = textures [index];
	}

	void OnMouseUp(){
		mouseDown = false;
	}
}

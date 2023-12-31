﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Change image when clicked
/// On KT Tower, It shows circle mouse cursor
/// </summary>

public class Onclick_ : MonoBehaviour {
	public Sprite clicked;
	private Sprite aux;
	private bool mouseover=false;
	private Master_Instance master;
	// Use this for initialization
	void Start () {
		master = GameObject.Find("Instance_Point").GetComponent<Master_Instance>();
		aux = GetComponent<SpriteRenderer>().sprite;
		setLayer();
	}
    //Show hand or circle mouse cursor
	void OnMouseOver(){ 
		if(!GameObject.Find("hand")&&!GameObject.Find("circle")){showHand (true);}
		mouseover=true;
	}
	//Hide hand
	void OnMouseExit(){
		if(GameObject.Find("hand")){showHand (false);}
		mouseover=false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)&&mouseover==true){
			GetComponent<SpriteRenderer>().sprite = clicked;
            GameObject.Find("UI").GetComponent<AudioSource>().Play();
            master.anybuttonclicked=true;                           //Only 1 button at same time
		}
		if (Input.GetMouseButtonUp(0)&&GetComponent<SpriteRenderer>().sprite==clicked){
			GetComponent<SpriteRenderer>().sprite = aux;
			master.anybuttonclicked=false;
			showCircle();                                           //Show circle mouse cursor
		}
		if (Input.GetMouseButtonDown(0)&&mouseover==false){
			Invoke ("mouseDownDelay",0.01f);
		}
	}
    /// <summary>
    /// Disable zoneImage
    /// </summary>
	void mouseDownDelay(){
		if(master.anybuttonclicked==false){
			if(!GameObject.Find("circle")){
				getBrother("zoneImg").GetComponent<SpriteRenderer>().enabled=false;
				this.gameObject.transform.parent.transform.parent.GetComponent<CircleCollider2D>().enabled=true;
				Destroy (this.gameObject.transform.parent.gameObject);
			}
		}
	}
    /// <summary>
    /// Set the layer
    /// </summary>
	void setLayer(){if(LayerMask.NameToLayer("interface")!=-1){this.gameObject.layer = LayerMask.NameToLayer("interface");}}
    /// <summary>
    /// Show hand
    /// </summary>
    /// <param name="value"></param>
	void showHand(bool value){//show hand cursor
		if(value==true){
			Cursor.visible = false;
			Vector3 aux = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			aux.z=-0.5f;
			GameObject hand = Instantiate(Resources.Load("Cursor/hand"), aux , Quaternion.identity)as GameObject;
			hand.name="hand";
		}else{
			Cursor.visible = true;
			Destroy (GameObject.Find("hand"));
		}
		
	}
    /// <summary>
    /// Show the circle mouse cursor
    /// </summary>
	void showCircle(){
		Destroy (GameObject.Find("hand"));
		GameObject circle = Instantiate(Resources.Load("Cursor/circle"), GameObject.Find("Out").transform.position , Quaternion.identity)as GameObject;
		circle.transform.SetParent(this.gameObject.transform.parent.transform.parent.transform);
		circle.name="circle";
		Destroy (this.gameObject.transform.parent.gameObject);
	}
    /// <summary>
    /// Get Brother gameobject
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Brother</returns>
	private GameObject getBrother(string name){
		GameObject aux = null;
		foreach(Transform child in gameObject.transform.parent.transform.parent.transform){
			if(child.name==name){aux=child.gameObject;}
		}
		return aux;
	}
    /// <summary>
    /// Get Child GameObject
    /// </summary>
    /// <param name="name">Name of the child</param>
    /// <param name="from">Parent gameobject</param>
    /// <returns>Child</returns>
	private GameObject getChildFrom(string name, GameObject from){
		GameObject aux=null;
		foreach(Transform child in from.transform){if(child.name==name){aux=child.gameObject;}}
		return aux;
	}
}

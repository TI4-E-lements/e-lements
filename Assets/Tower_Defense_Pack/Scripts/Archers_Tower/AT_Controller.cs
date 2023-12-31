﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;

/// <summary>
/// This script is the base controller for Archer Towers
/// It can be turned off by the sorcerer
/// </summary>

public class AT_Controller : MonoBehaviour {
	public List<GameObject> enemies;        //List of enemies to attack, the enemies are detected by child called 'zone' / Zone_Controller.cs script associated
	public GameObject zone;                 //Zone to detect the enemies
	public GameObject spawner;              //Arrow spawner
	public bool shot_ = false;
	private bool mouseover=false;
	private float searchvalue = 0.1f;       //down value is best... but the performance may be affected (used to detect min speed to hit the enemy)
	//Public properties
	public float s_timer = 0.9f;            //Time between arrows
	public int accuracy_mode = 4;           //Bassically it is used by the bullet, and add a force in direction to the target.
	public int Damage_ = 1;                 //Arrow Damage
	public bool fire = false;               //Add fire to the arrow
	private bool off=false;                 //Sorcerer can turn off
    private AudioSource audio;
    private AudioClip[] arrows;
    //Show hand
    void OnMouseOver(){ 
		if(!GameObject.Find("hand")){master.showHand (true);}
		mouseover=true;
	}
    //Hide hand
	void OnMouseExit(){
		if(GameObject.Find("hand")){master.showHand (false);}
		mouseover=false;
	}

	void Start () {
        audio = this.gameObject.AddComponent<AudioSource>();
        arrows = GameObject.Find("AudioManager").GetComponent<Audio_Manager>().Arrows;
        audio.volume = master.getEffectsVolume();
        this.transform.position = master.setThisZ(this.transform.position,0.02f);   //Set Z relative to Y
		master.setLayer("tower",this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if(!master.isFinish()){                                                     //Game not finished
			if(master.getChildFrom("Interface",this.gameObject)==null){             //If this tower is not showing interface, stop showing zoneImage (It is used to show/hide the area when click on tower)
				master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=false;
				GetComponent<CircleCollider2D>().enabled=true;
			}
			if (Input.GetMouseButtonDown(0)&&mouseover==true){                      //Show the AT interface when click on the tower
					master.showInterface(this.gameObject.name,this.gameObject,zone.transform);
					GetComponent<CircleCollider2D>().enabled=false;
					master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=true;
			}
			remove_null();
			if(enemies.Count>0){
				if(shot_==false){
					shot_=true;
					Invoke("shot",s_timer);
				}
			}
		}
	}
    /// <summary>
    /// The sorcerer call this function to turn off the tower
    /// </summary>
	public void Turn_Off(){
		off=true;
		Invoke("Turn_On",GameObject.Find("Instance_Point").GetComponent<Master_Instance>().Sorcerer_Runes_Time);
	}
    /// <summary>
    /// Re enable the tower
    /// </summary>
	private void Turn_On(){
		off=false;
	}
    /// <summary>
    /// If enemies in the list, create arrow
    /// </summary>
	private void shot(){
		shot_=false;
		if(off==false){
			if(enemies.Count>0){Instantiate_Bullet();}
		}
	}
    /// <summary>
    /// Instantiate Arrow, Arrow prefab is on Resources/AT/arrow
    /// Here is configured the damage of the arrow
    /// </summary>
	private void Instantiate_Bullet(){
        audio.clip = arrows[UnityEngine.Random.Range(0, arrows.Length - 1)];
        audio.Play();                                                                   //Play sound attack
        GameObject Bullet = Instantiate(Resources.Load("AT/arrow"), new Vector3(spawner.transform.position.x,spawner.transform.position.y,spawner.transform.position.z), Quaternion.identity)as GameObject;
		Parabolic_shot_Controller BulletProperties = Bullet.GetComponent<Parabolic_shot_Controller>();
		Bullet.GetComponent<Damage>().Damage_ = Damage_;
		BulletProperties.target = enemies[enemies.Count-1];
		if(enemies[0]!=null){
            BulletProperties.maxLaunch = getminSpeed();
        }
        else{
			Destroy(Bullet);
		}
		BulletProperties.accuracy_mode=accuracy_mode;
		BulletProperties.fire = fire;
		Bullet.name="Arrow";
	}
    /// <summary>
    /// Get the min arrow speed to reach the enemies[0]
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
	private float getminSpeed(){
		float aux = 0.1f;
		while(moreSpeed(aux)==true){aux = aux + searchvalue;}
		return aux;
	}
    /// <summary>
    /// It determines the speed to reach the target
    /// </summary>
    /// <param name="speed">speed Value</param>
    /// <returns></returns>
	private bool moreSpeed(float speed){
		bool aux = false;
		float xTarget = enemies[0].transform.position.x;
		float yTarget = enemies[0].transform.position.y; 
		float xCurrent = transform.position.x;
		float yCurrent = transform.position.y;
		float xDistance = Math.Abs(xTarget - xCurrent);
		float yDistance = yTarget - yCurrent;
		float fireAngle = 1.57075f - (float)(Math.Atan((Math.Pow(speed, 2f)+ Math.Sqrt(Math.Pow(speed, 4f) - 9.8f * (9.8f * Math.Pow(xDistance, 2f) + 2f * yDistance * Math.Pow(speed, 2f) )))/(9.8f * xDistance)));
		float xSpeed = (float)Math.Sin(fireAngle) * speed;
		float ySpeed = (float)Math.Cos(fireAngle) * speed;
		if ((xTarget - xCurrent) < 0f){xSpeed = - xSpeed;}
		if(float.IsNaN(xSpeed)||float.IsNaN(ySpeed)){aux = true;}
		return aux;
	}

	/// <summary>
    /// Add enemies to the list
    /// </summary>
    /// <param name="other"></param>
	public void enemyAdd(GameObject other){enemies.Add (other);}
    /// <summary>
    /// Remove enemies to the list
    /// </summary>
    /// <param name="other"></param>
	public void enemyRemove(string other){
		for(int i=0; i<enemies.Count ;i++){
			if(enemies[i]!=null){
				if(enemies[i].name==other){enemies.RemoveAt(i);}
			}
		}
	}
    /// <summary>
    /// Remove Nulls to the list
    /// </summary>
	void remove_null(){for(int i=0; i<enemies.Count ;i++){if(enemies[i]==null){enemies.RemoveAt(i);}}}

}

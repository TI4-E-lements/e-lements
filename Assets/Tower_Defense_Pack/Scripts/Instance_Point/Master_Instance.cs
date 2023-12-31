﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FThLib;
using UnityEngine.SceneManagement;

/// <summary>
/// Instantiate enemies
/// Tower prices
/// </summary>

public class Master_Instance : MonoBehaviour {
	//To change the money when kill an enemy go to Enemies_Controller, change the value of moneyWhenKill
	//Public
	public int startWithMoney = 240;                                            //Money when start
	public bool anybuttonclicked = false;
	//public float Enemyspeed = 0.4f;//Enemies speed
	//Prices
	public int KT_price = 100;                                                  //Knights tower
	public int AT_price = 110;                                                  //Archers tower
	public int MT_price = 120;                                                  //Magicians tower
	//Tower Upgrades prices
	public int KT_Damage_price = 40;
	public int KT_Shield_price = 70;
	public int AT_Fire_price = 110;
	public int AT_Ratio_price = 100;
	public int AT_Accuracy_price = 70;
	public int MT_Fire_price = 120;
	public float Sorcerer_Runes_Time = 5f;
	public bool Finish = false;
	//Private
	private float seed = 0.2f;
	private Transform[] path;
	private int path_size=0;
	private GameObject spawner=null;
	//Creature 
	private int C1life = 20;
	//Knight 
	//private int k0life = 20;
	private int count = 0;
	private Text money;
	//--About waves
	private float callDelay = 1f;                                               //Delay between enemies
	public bool wavePlaying = false;                                            //the current wave is playing
	private bool waveAux = false;
	public int waveCount = 0;
	private Transform[] currentWave;
	private int indexcurrentWave;
	private int indexcurrentenemy;
	// Use this for initialization
	void Start () {
		path_size = getsize ();
		money = GameObject.Find("Money").GetComponent<Text>();
		addMoney(startWithMoney);
		/*Creating path array ========== */
		path = new Transform[path_size];
		for (int i=0; i<path_size-1;i++){//Searching the points, the points must be named like: "a0,a1,a2,a3..."
			path[i]=GameObject.Find("a" + i).transform;
		}
		/*============================== */
		spawner = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(Finish==false){
			if (Input.GetKey(KeyCode.Escape)){if(GameObject.Find("Interface")){Destroy (GameObject.Find("Interface"));}}
			if(Input.GetMouseButtonDown(0)){
				if(GameObject.Find("Circle")==null&&GameObject.Find("flag_")==null&&GameObject.Find("hand")==null&&GameObject.Find("trap_")==null){
					if(GameObject.Find("Interface")){Destroy (GameObject.Find("Interface"));}
				}
			}
			if(wavePlaying==true&&waveCount>0){
				if(waveAux==false){
					waveAux=true;
					Invoke("Instantiate_Enemy",callDelay);	//--------------------------------------------------------------------------------------------------
				}
			}
			if(waveCount==0){wavePlaying=false;}
		}
	}
    /// <summary>
    /// Create a new wave of enemies
    /// </summary>
    /// <param name="enemiesNumber">Number of enemies</param>
    /// <param name="enemytype">Type of enemies of this wave</param>
	public void createWave(int enemiesNumber, int enemytype){
		wavePlaying=true;
		waveCount=enemiesNumber;
	}
    /// <summary>
    /// It is used by Waves_Creator_Controller.cs (attached to Instace_Point gameobject)
    /// </summary>
    /// <param name="waveArray"></param>
    /// <param name="index"></param>
	public void createWave(Transform[] waveArray, int index){
		wavePlaying=true;
		waveCount= waveArray[index].GetComponent<waves>().enemyList.Count;
		currentWave = waveArray;
		indexcurrentWave = index;
		indexcurrentenemy = 0;
	}
    /// <summary>
    /// Instantiate an enemy
    /// </summary>
	private void Instantiate_Enemy(){
		/*Enemy prefab selection ===========================================*/
		GameObject Enemy = Instantiate(Resources.Load("Enemies/" + currentWave[indexcurrentWave].GetComponent<waves>().enemyList[indexcurrentenemy]), new Vector3(spawner.transform.position.x+ Random.Range(-seed, seed),spawner.transform.position.y+ Random.Range(-seed, seed),spawner.transform.position.z), Quaternion.identity)as GameObject;
		Enemy.transform.SetParent(this.gameObject.transform);
		PathFollower EnemyPathProperties = Enemy.GetComponent<PathFollower>();
		Enemies_Controller EnemyPropierties = Enemy.GetComponent<Enemies_Controller>();
		EnemyPropierties.type=currentWave[indexcurrentWave].GetComponent<waves>().enemyList[indexcurrentenemy];
		/*Enemy properties: Name, Path array, speed and life================*/
		Enemy.name="Enemy" + count;
		Transform[] path_ = GeneratePath(currentWave[indexcurrentWave].GetComponent<waves>().pathList[indexcurrentenemy]);
		EnemyPathProperties.path = path_;
		Enemy.transform.position = path_[0].transform.position;
		//EnemyPathProperties.path = path;									//Get from prefab
		EnemyPathProperties.speed = currentWave[indexcurrentWave].GetComponent<waves>().speedList[indexcurrentenemy];								    //Get from prefab
		EnemyPropierties.life = currentWave[indexcurrentWave].GetComponent<waves>().lifeList[indexcurrentenemy];										//Get from prefab
		/*==================================================================*/
		indexcurrentenemy++;
		count++;
		waveAux = false;
		waveCount--;
	}
    /// <summary>
    /// Create the path points
    /// </summary>
    /// <param name="road"></param>
    /// <returns></returns>
	private Transform[] GeneratePath(string road){
		List<Transform> path_ = new List<Transform>();
		int auxindex=0;
		while(GameObject.Find(road+""+auxindex)){
			path_.Add(GameObject.Find(road+""+auxindex).transform);
			auxindex++;
		}
		Transform[] aux = new Transform[path_.Count+1];
		path_.Add(GameObject.Find(road+"End").transform);
		path_.CopyTo(aux);
		return aux;
	}
	/// <summary>
    /// Get the money relative to the name
    /// </summary>
    /// <param name="go">gameobject</param>
    /// <returns>price</returns>
	public int getPrice(GameObject go){
		int aux_ = 0;
		if(go.name=="KT"||go.name=="KT0"){aux_=KT_price;}
		if(go.name=="AT"||go.name=="AT0"){aux_=AT_price;}
		if(go.name=="MT"||go.name=="MT0"){aux_=MT_price;}
		if(go.name=="Damage"){aux_=KT_Damage_price;}
		if(go.name=="Life"){aux_=KT_Shield_price;}
		if(go.name=="MTFire"){aux_=MT_Fire_price;}
		if(go.name=="Fire"){aux_=AT_Fire_price;}
		if(go.name=="Ratio"){aux_=AT_Ratio_price;}
		if(go.name=="Accuracy"){aux_=AT_Accuracy_price;}
		return aux_;
	}
    /// <summary>
    /// Get the money value from money.text
    /// </summary>
    /// <returns>money as intger</returns>
	public int countMoney(){return int.Parse (money.text);}
    /// <summary>
    /// Add money when kill an enemy
    /// </summary>
    /// <param name="value">Value</param>
	public void addMoney(int value){
		int valueaux = int.Parse (money.text);
		valueaux = valueaux + value;
		money.text = ""+valueaux;
	}
    /// <summary>
    /// Remove money when buying
    /// </summary>
    /// <param name="value"></param>
	public void removeMoney(int value){
		int valueaux = int.Parse (money.text);
		valueaux = valueaux - value;
		money.text = ""+valueaux;
	}
    /// <summary>
    /// Get the number of points ot the path a
    /// </summary>
    /// <returns></returns>
	int getsize(){
		int i = 0;
		while(GameObject.Find("a" + i)){
			i++;
		}
		i++;//end point
		return i;
	}
	/// <summary>
    /// About Loading levels
    /// </summary>
	public void Level1(){
		GameObject.Find("Crossfade").GetComponent<Animator>().SetBool("out",true);
		Invoke("Level1Go",2);
	}
    /// <summary>
    /// About Loading levels
    /// </summary>
	private void Level1Go(){
		if (Application.platform == RuntimePlatform.Android){
            SceneManager.LoadScene("Phone_Example_Scene");
		}else{
            SceneManager.LoadScene("Example_Scene");
		}
	}
    /// <summary>
    /// About Loading levels
    /// </summary>
	public void Level2(){
		if (Application.platform == RuntimePlatform.Android){
            SceneManager.LoadScene("Phone_Example_Scene2");
		}else{
            SceneManager.LoadScene("Example_Scene2");
		}
	}
    /// <summary>
    /// About Loading levels
    /// </summary>
	public void Level3(){
		if (Application.platform == RuntimePlatform.Android){
            SceneManager.LoadScene("Phone_Example_Scene3");
		}else{
            SceneManager.LoadScene("Example_Scene3");
		}
	}
}

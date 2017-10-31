using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour {
	public Scrollbar healthBar;
	public Image mask;
	public Image life;
	public Image frame;
	public int lifeValue;
	public Text[] carriagesText = new Text[5];
	public Text clues;

	void Start()
	{		
		State_Bar(false);
		for(int i=0 ; i<=5 ; i++){
			carriagesText[i].text = "" + GameObject.Find ("ConfigController").GetComponent<ConfigController>().carriageMaxLife;
		}
		clues.text = 0.ToString();
	}

	public void State_Bar(bool state){

		healthBar.enabled = state;
		mask.enabled = state;
		life.enabled = state;
		frame.enabled = state;
	}
	public void resetLife(){

		lifeValue = GameObject.Find ("ConfigController").GetComponent<ConfigController>().characterMaxLife;
	}

	public void dealDamageCharacter(int damage)
	{ //Resta a la vida la entrada damage
		lifeValue = lifeValue - damage;
		if (lifeValue <= 0)
		{
			lifeValue = 0;
			Destroy(this.gameObject);
		}
		healthBar.size = lifeValue / 100f;
	}
	public void dealDamageCarriage(int objective, int damage){
		int actual = 0;

		Int32.TryParse(carriagesText[objective].text, out actual);
		if(actual>damage){
			carriagesText[objective].text= (actual-damage).ToString();
		}else{
			carriagesText[objective].text= "X";
		}
	}
		
}

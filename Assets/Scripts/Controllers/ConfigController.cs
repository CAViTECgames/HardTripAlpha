using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigController : MonoBehaviour {

	public int characterActualLife=0;
	public int characterMaxLife = 100;
	public int carriageMaxLife = 100;
	public int banditAgressiveMaxLife = 100;
	public int wolfMaxLife = 100;
	public int banditNegotiatorMaxLife = 100;	
	public int characterSpeed = 5;
	public int carriageSpeed = 3;
	public int wolfSpeed = 4;
	public int banditAgressiveSpeed= 4;
	public int banditNegotiatorSpeed = 4;

	// Use this for initialization
	void Start () {
		characterActualLife = characterMaxLife;
		characterActualLife = carriageMaxLife;
	}

	public int GetLife(string objective){

		switch (objective) {

		case "character": 
			return characterMaxLife;
		case "carriage":
			return carriageMaxLife;
		case "wolf":
			return wolfMaxLife;
		case "banditAgressive":
			return banditAgressiveMaxLife;
		case "banditNegotiator":
			return banditNegotiatorMaxLife;
		default:
			return 0;
		}
	}

	public int GetSpeed(string objective){

		switch (objective) {

		case "character": 
			return characterSpeed;
		case "carriage":
			return carriageSpeed;
		case "wolf":
			return wolfSpeed;
		case "banditAgresive":
			return banditAgressiveSpeed;
		case "banditNegotiator":
			return banditNegotiatorSpeed;
		default:
			return 0;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigController : MonoBehaviour {

	public static int characterActualLife=0;
	public static int characterMaxLife = 100;
	public static int carriageMaxLife = 100;
	public static int banditAgressiveMaxLife = 100;
	public static int wolfMaxLife = 100;
	public static int banditNegotiatorMaxLife = 100;	
	public static int characterSpeed = 5;
	public static int carriageSpeed = 3;
	public static int wolfSpeed = 4;
	public static int banditAgressiveSpeed= 4;
	public static int banditNegotiatorSpeed = 4;

    // Technical values
    public static int combatUpdateFrames = 5;
    public static float statusFrameScale = 0.1f;

	// Use this for initialization
	void Start () {
		characterActualLife = characterMaxLife;
		characterActualLife = carriageMaxLife;
	}

	public static int GetLife(string objective){

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

	public static int GetSpeed(string objective){

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

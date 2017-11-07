using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

	int phase;
	int side;
	float timer;
	bool inCombat;
	bool startGame;
	public GameObject totalSpawnPoints;
	Transform[] actualSpawnPoints;

	// Use this for initialization
	void Start ()
	{
		SetTimer ();
		inCombat = false;
		startGame = false;
		phase = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (startGame) {
			if (timer == 0) {
				if (!inCombat) {
					spawnEnemy ();
					SetTimer ();
				} else {
					timer = timer + 3000f;				
				}
			} else {
				timer = timer - 1f;
			}
		}
	}


	public void SetSide (int extSide)
	{
		this.side = extSide;
	}

	public void SetPhase (int extPhase)
	{
		this.phase = extPhase;
	}

	private void SetTimer ()
	{
		timer = 30000f;
	}

	public void SetInCombat(bool stateOfCombat){
		inCombat = stateOfCombat;
	}

	public void startingGame(bool stateOfGame){
		startGame = stateOfGame;
	}




	void spawnEnemy ()
	{
		GameObject realSpawnPoint;
		List<GameObject> phaseList = new List<GameObject>();
		List<GameObject> sideList = new List<GameObject>();
		List<GameObject> spawnPoints = new List<GameObject>();
		totalSpawnPoints.GetComponentsInChildren<GameObject>(true, phaseList);
		phaseList[phase].GetComponentsInChildren<GameObject>(true, sideList);
		sideList[side].GetComponentsInChildren<GameObject>(true, spawnPoints);


		if (spawnPoints != null)
		{
			foreach(GameObject child in spawnPoints)
			{
				//TODO - calcular la distancia de ese punto a la caravana

				//realSpawnPoint=mascercano
			}

			//mover enemigos de la zona base aqui

		}

	}

}


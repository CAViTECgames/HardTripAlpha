using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour {
	public Scrollbar healthBar;
	//public Image mask;
	//public Image life;
	//public Image frame;
	public int lifeValue;
	public Text[] carriagesText = new Text[5];
	public Canvas inventoryCanvas;
	public Text clues;
	public Canvas princessDialogue;
	public Canvas clueDescription;
	public Canvas adviseCanvas;

	//put the number of clues to 0, hides the healthbarand set the life of carriages to maximun
	void Start()
	{		
		State_Bar(false);
		for(int i=0 ; i<=5 ; i++){
			carriagesText[i].text = "" + ConfigController.carriageMaxLife;
		}
		clues.text = 0.ToString();
	}

	public void State_Bar(bool state){


		//healthBar.enabled = state;

		List<Image> healthBarChildrens = new List<Image>();
		healthBar.GetComponentsInChildren<Image>(true, healthBarChildrens);

		if (healthBarChildrens != null)
		{
			foreach(Image child in healthBarChildrens)
			{
				child.enabled = state;	
			}
		}
			
		//mask.enabled = state;
		//life.enabled = state;
		//frame.enabled = state;
	}

	//reset character life
	public void resetLife(){

		lifeValue = ConfigController.characterMaxLife;
	}

	//decrement the healthbar for the character
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

	//dechrement de life fot the carriages, should indicate the carriage number
	public void dealDamageCarriage(int objective, int damage){
		int actual = 0;

		Int32.TryParse(carriagesText[objective].text, out actual);
		if(actual>damage){
			carriagesText[objective].text= (actual-damage).ToString();
		}else{
			carriagesText[objective].text= "X";
		}
	}

	//hide or show the bag and the numer of clues
	public void inventory(bool state){
	
		List<Image> bagImage = new List<Image>();
		inventoryCanvas.GetComponentsInChildren<Image>(true, bagImage);

		if (bagImage != null)
		{
			foreach(Image child in bagImage)
			{
				child.enabled = state;	
			}
		}

		List<Text> numberClues = new List<Text>();
		inventoryCanvas.GetComponentsInChildren<Text>(true, numberClues);

		if (numberClues != null)
		{
			foreach(Text child in numberClues)
			{
				child.enabled = state;	
			}
		}
	}


	//hide or chow the image and the princess texts
	public void princessDialog(bool state){


		List<Image> backgroundDialogue = new List<Image>();
		princessDialogue.GetComponentsInChildren<Image>(true, backgroundDialogue);

		if (backgroundDialogue != null)
		{
			foreach(Image child in backgroundDialogue)
			{
				child.enabled = state;	
			}
		}

		List<Text> textDialogue = new List<Text>();
		princessDialogue.GetComponentsInChildren<Text>(true, textDialogue);

		if (textDialogue != null)
		{
			foreach(Text child in textDialogue)
			{
				child.enabled = state;	
			}
		}
	
	
	}

	//hide or show the paper an the text for the clue
	public void clue(bool state){

		List<Image> paper = new List<Image>();
		clueDescription.GetComponentsInChildren<Image>(true, paper);

		if (paper != null)
		{
			foreach(Image child in paper)
			{
				child.enabled = state;	
			}
		}

		List<Text> clueText = new List<Text>();
		clueDescription.GetComponentsInChildren<Text>(true, clueText);

		if (clueText != null)
		{
			foreach(Text child in clueText)
			{
				child.enabled = state;	
			}
		}
	}

	//hide or show the image an the text for advise mensage
	public void advise(bool state){

		List<Image> images = new List<Image>();
		adviseCanvas.GetComponentsInChildren<Image>(true, images);

		if (images != null)
		{
			foreach(Image child in images)
			{
				child.enabled = state;	
			}
		}

		List<Text> adviseText = new List<Text>();
		adviseCanvas.GetComponentsInChildren<Text>(true, adviseText);

		if (adviseText != null)
		{
			foreach(Text child in adviseText)
			{
				child.enabled = state;	
			}
		}
	}
		
}

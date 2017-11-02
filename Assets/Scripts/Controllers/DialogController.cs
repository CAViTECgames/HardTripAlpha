using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

	public Text princessDialog;
	public Text clueText;
	public Text adviseDialog;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator changePrincessDialogue(string phrase){

		int letter = 0;
		princessDialog.text = "";
		while (letter< phrase.Length){
			princessDialog.text += phrase [letter];
			letter += 1;
			yield return new WaitForSeconds (0.02f);

		}
				
	}

	public void cluepopup(string clue){

		clueText.text = clue;		
	}

	public IEnumerator changeAdviseDialogue(string phrase){

		int letter = 0;
		adviseDialog.text = "";
		while (letter< phrase.Length){
			adviseDialog.text += phrase [letter];
			letter += 1;
			yield return new WaitForSeconds (0.02f);

		}

	}
}

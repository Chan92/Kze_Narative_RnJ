using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour{
	[SerializeField]
	private Text buttonTextField;

	public int optionPoints {get; private set; }
	public string buttonText {get; private set;	}
	public string afterText{get; private set; }
	public string unlockRequirements {get; private set;}
	public string nextChapter {get; private set;}

	private void Start() {
		buttonTextField = transform.GetComponentInChildren<Text>();
	}

	public void SetInfo(string btText, string afterDialog, string unlock, string points, string chapterName) {
		if(btText.Contains("[@]")) {
			btText = btText.Replace("[@]", "");
		}

		if(afterDialog.Contains("[@]")) {
			afterDialog = afterDialog.Replace("[@]", "");
		}

		buttonTextField.text = btText;
		afterText			 = afterDialog;
		unlockRequirements	 = unlock;
		optionPoints		 = int.Parse(points);
		nextChapter			 = chapterName;
	}

	public void Clicked() {
		if(Manager.instance.timedOptionTimer != null) {
			//StopCoroutine(Manager.instance.timedOptionTimer);
		}

		Manager.instance.timerOnDirtFix = false;
		Manager.instance.debugTimer.gameObject.SetActive(false);

		StoryReader.instance.currentChapter = nextChapter;
		CheckUnlocks();


		if(afterText != "" && afterText != null) {
			afterText = StoryReader.instance.NameCheck(afterText);
			Manager.instance.dialogBox.text = afterText;
			Manager.instance.SetButtons();
		} else {			
			Manager.instance.NextText(true);
		}
	}

	private void CheckUnlocks() {
		//add new unlocked requierements
		if(!unlockRequirements.Contains("none") && !unlockRequirements.Contains("None")) {
			Manager.instance.acquiredRequirements.Add(unlockRequirements);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;


public class StoryReader:MonoBehaviour {
	public static StoryReader instance;
	private XmlDocument storyDoc;

	public string currentChapter {
		get; set;
	}
	public int currentLineId {
		get; private set;
	}

	private int buttonObjectId = 0;

	private string timeoutAfterText;
	private int timeoutOptionPoints;
	private string timeoutNextChapter;

	private void Awake() {
		instance = this;
		currentChapter = "Chapter1";
		currentLineId = 0;
	}

	public void FindData() {
		/*
		
		//string path = "Assets/Resources/XML/StoryScript.xml";
		string path = "Assets/Resources/XML/TestScript.xml";

		XmlDocument doc = new XmlDocument();
		var contents = "";
		using(StreamReader streamReader = new StreamReader(path)) {
			contents = streamReader.ReadToEnd();
		}
		doc.LoadXml(contents);

		*/

		TextAsset storyFile = Resources.Load<TextAsset>("XML/TestScript");
		//TextAsset storyFile = Resources.Load<TextAsset>("XML/StoryScript");
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(storyFile.text);

		string chapter = currentChapter;
		XmlNodeList nl = doc.GetElementsByTagName(chapter);

		if(nl[0] != null) {
			if(currentLineId <= GetLineCount(nl)) {

				if(currentLineId < GetLineCount(nl)){
					GetXmlLineInfo(nl);
					currentLineId++;
				} else if(currentLineId == GetLineCount(nl)) {
					print("line id = line count");

					for(int i = 0; i < GetButtonCount(nl); i++) {
						print("for loop broken?");
						GetXmlButtonInfo(nl, i);
					}

					Manager.instance.buttonsActive = true;
					currentLineId = 0;
					buttonObjectId = 0;

					if(HasTimedOptions(nl)) {
						Manager.instance.timerOnDirtFix = true;
						StopCoroutine(OptionTimer());
						Manager.instance.timedOptionTimer = StartCoroutine(OptionTimer());
					}



				} else {
					//currentLineId++;
					print("nani??");
				}
			} else {
				Debug.Log("Error?");
			}
		} else {
			Debug.LogError("Missing Chapter.");
		}
	}

	int GetLineCount(XmlNodeList nl) {
		return nl[0].ChildNodes[1].ChildNodes.Count;
	}

	int GetButtonCount(XmlNodeList nl) {
		return nl[0].ChildNodes[2].ChildNodes.Count;
	}

	bool HasTimedOptions(XmlNodeList nl) {
		string str = "";
		str = nl[0].ChildNodes[0].ChildNodes[0].Value;
		
		if(str.Contains("timed") || str.Contains("Timed")) {
			return true;
		}

		return false;
	}

	void GetXmlLineInfo(XmlNodeList nl) {
		string str = "";
		str = nl[0].ChildNodes[1].ChildNodes[currentLineId].ChildNodes[0].Value;
		//str = StringReplace(str);

		if(str != "[@]" && str != null) {
		//	str = CheckExpressions(str);
		//	str = CheckSoundEffects(str);
			Manager.instance.dialogBox.text = str;
		}
	}

	void GetXmlButtonInfo(XmlNodeList nl, int buttonId) {
		string buttonText = "", afterText = "", preReq = "", unlock = "", points = "", nextChapter = "";

		buttonText	= nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[0].ChildNodes[0].ChildNodes[0].Value;
		afterText	= nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[0].ChildNodes[1].ChildNodes[0].Value;
		preReq		= nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[1].ChildNodes[0].Value;
		unlock		= nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[2].ChildNodes[0].Value;
		points		= nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[3].ChildNodes[0].Value;
		nextChapter = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[4].ChildNodes[0].Value;

		//check if pre requirements are needed for this button
		if((preReq.Contains("none") || preReq.Contains("None")) || Manager.instance.acquiredRequirements.Contains(preReq)) {
			//timeout options are invisible, automaticly chosen when timed out
			if(!buttonText.Contains("timeout") && !buttonText.Contains("Timeout")) {

				//aftertext
				if(afterText != null) {
					//afterText = StringReplace(afterText);
				} else {
					afterText = "";
					print("null?");
				}

				//set info & enable button
				Manager.instance.btInfo[buttonObjectId].SetInfo(buttonText, afterText, unlock, points, nextChapter);
				Manager.instance.btInfo[buttonObjectId].gameObject.SetActive(true);
				buttonObjectId++;
			} else {
				timeoutAfterText = afterText;
				timeoutOptionPoints = int.Parse(points);
				timeoutNextChapter = nextChapter;
			}
		}
	}

	private void ChosenTimeoutButton() {
		Manager.instance.debugTimer.gameObject.SetActive(false);
		StoryReader.instance.currentChapter = timeoutNextChapter;

		if(timeoutAfterText != "" && timeoutAfterText != null) {
			Manager.instance.dialogBox.text = timeoutAfterText;
			Manager.instance.SetButtons();
		} else {
			Manager.instance.NextText(true);
		}
	}

	private IEnumerator OptionTimer() {
		Manager.instance.debugTimer.gameObject.SetActive(true);
		float timecounter = 5.5f;

		while(timecounter > 0 && Manager.instance.timerOnDirtFix == true) {
			timecounter -= Time.deltaTime;

			Manager.instance.debugTimer.text = "Timer: " + timecounter.ToString();
			yield return null;

			if(timecounter <= 0) {
				ChosenTimeoutButton();
			}
		}
	}

	/* <placeholder, fix later>
	string StringReplace(string s) {
		s = s.Replace('$', '\n');
		s = s.Replace("[Juliet]", Manager.instance.mainCharName);
		s = s.Replace("[Romeo]", Manager.instance.dateName);
		s = s.Replace("[Tybalt]", Manager.instance.dateName);
		s = s.Replace("[Narator]", Manager.instance.dateName);

		return s;
	}

	public string CheckSoundEffects(string s) {
		//sound effects
		if(s.Contains("[Audio:happy]")) {
			s = s.Replace("[Audio:happy]", "");
		}

		//bgm
		if(s.Contains("[BGM:date]")) {
			s = s.Replace("[BGM:happy]", "");
			//Manager.instance.bgmEffect.PlayLoopBGM("date");
		} 

		return s;
	}
	*/
}
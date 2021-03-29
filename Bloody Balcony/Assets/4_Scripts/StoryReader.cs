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
		//TextAsset storyFile = Resources.Load<TextAsset>("XML/TestScript");
		TextAsset storyFile = Resources.Load<TextAsset>("XML/StoryScript");
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(storyFile.text);

		string chapter = currentChapter;
		XmlNodeList nl = doc.GetElementsByTagName(chapter);

		if(nl[0] != null) {
			if(currentLineId < GetLineCount(nl)) {
				GetXmlLineInfo(nl);
				currentLineId++;

				if(currentLineId == GetLineCount(nl)) {
					for(int i = 0; i < GetButtonCount(nl); i++) {
						GetXmlButtonInfo(nl, i);
					}

					Manager.instance.buttonsActive = true;
					currentLineId = 0;

					if(HasTimedOptions(nl)) {
						Manager.instance.timerOnDirtFix = true;
						StopCoroutine(OptionTimer());
						Manager.instance.timedOptionTimer = StartCoroutine(OptionTimer());
					}
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

		if(!str.Contains("[@]") && str != null) {
			str = NameCheck(str);
			str = CheckBackground(str);
			//	str = CheckExpressions(str);
			//	str = CheckSoundEffects(str);
			Manager.instance.dialogBox.text = str;
		} else if(str.Contains("[@]")) {
			str = NameCheck(str);
			str = CheckBackground(str);
			str = "";
			Manager.instance.dialogBox.text = str;
		}
	}

	void GetXmlButtonInfo(XmlNodeList nl, int buttonId) {
		string buttonText = "", afterText = "", preReq = "", unlock = "", points = "", nextChapter = "", bId = "";

		buttonText = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[0].ChildNodes[0].ChildNodes[0].Value;
		afterText = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[0].ChildNodes[1].ChildNodes[0].Value;
		preReq = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[1].ChildNodes[0].Value;
		unlock = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[2].ChildNodes[0].Value;
		points = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[3].ChildNodes[0].Value;
		nextChapter = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[4].ChildNodes[0].Value;
		bId = nl[0].ChildNodes[2].ChildNodes[buttonId].ChildNodes[5].ChildNodes[0].Value;


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
				buttonObjectId = int.Parse(bId);
				Manager.instance.btInfo[buttonObjectId].SetInfo(buttonText, afterText, unlock, points, nextChapter);
				Manager.instance.btInfo[buttonObjectId].gameObject.SetActive(true);
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

		if(timeoutAfterText == "[@]") {
			timeoutAfterText = "";
		}

		if(timeoutAfterText != "" && timeoutAfterText != null) {
			timeoutAfterText = NameCheck(timeoutAfterText);

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

			Manager.instance.debugTimer.text = "" + timecounter.ToString();
			yield return null;

			if(timecounter <= 0) {
				ChosenTimeoutButton();
			}
		}
	}

	
	public string NameCheck(string s) {
		s = s.Replace('$', '\n');

		if(s.Contains("[J]")) {
			s = s.Replace("[J]", "");
			Manager.instance.julietNametag.SetActive(true);
		} else {
			Manager.instance.julietNametag.SetActive(false);
		}


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
	

	public string CheckBackground(string s) {
		if(!s.Contains("[BG:")) {
			return s;
		}

		if(s.Contains("[BG:Balcony]")) {
			s = s.Replace("[BG:Balcony]", "");
			Manager.instance.ChangeBackgrounds(0);
		}
		if(s.Contains("[BG:Bedroom]")) {
			s = s.Replace("[BG:Bedroom]", "");
			Manager.instance.ChangeBackgrounds(1);
		}
		if(s.Contains("[BG:Hall]")) {
			s = s.Replace("[BG:Hall]", "");
			Manager.instance.ChangeBackgrounds(2);
		}
		if(s.Contains("[BG:Room1]")) {
			s = s.Replace("[BG:Room1]", "");
			Manager.instance.ChangeBackgrounds(3);
		}
		if(s.Contains("[BG:Room2]")) {
			s = s.Replace("[BG:Room2]", "");
			Manager.instance.ChangeBackgrounds(4);
		}
		if(s.Contains("[BG:SecretRoom]")) {
			s = s.Replace("[BG:SecretRoom]", "");
			Manager.instance.ChangeBackgrounds(5);
		}

		return s;
	}
}
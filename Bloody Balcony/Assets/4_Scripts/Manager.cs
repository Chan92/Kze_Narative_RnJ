using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour{
	public static Manager instance;

	public Text debugTimer;

	public ButtonInfo[] btInfo;
	public Text dialogBox;

	public bool buttonsActive = false;

	public List<string> acquiredRequirements = new List<string>();
	public Coroutine timedOptionTimer;
	public bool timerOnDirtFix = false;

	private void Awake() {
		instance = this;
	}

	private void Start() {
		debugTimer.gameObject.SetActive(false);
	}

	private void Update() {
		if(Input.GetButtonDown("Jump") && !buttonsActive) {
			NextText(false);
		}
	}

	public void NextText(bool button) {
		if(button || !buttonsActive) {
			if(!CheckGameOver()) {
				SetButtons();
				StoryReader.instance.FindData();
			}
		}
	}

	public void SetButtons() {
		buttonsActive = false;

		for(int i = 0; i < btInfo.Length; i++) {
			btInfo[i].gameObject.SetActive(false);
		}
	}


	bool CheckGameOver() {
		return false;
	}


	public void StartGame() {
		//startMenu.SetActive(false);
	}

	public void RestartButton() {
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		Resources.UnloadUnusedAssets();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	//test
	public IEnumerator TextOverTime(string text) {
		string[] s = text.Split(' ');

		dialogBox.text = "";

		for(int i = 0; i < s.Length; i++) {
			dialogBox.text += s[i];
			if(i < s.Length - 1) dialogBox.text += "";
			yield return new WaitForSeconds(0);
		}
	}
}

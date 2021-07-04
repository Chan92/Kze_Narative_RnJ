using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomPuzzle : MonoBehaviour {
	static public BedroomPuzzle instance;

	private int searchOptionsCount;
	private int currentSearchCount;

	private void Awake() {
		instance = this;
	}

	public void SetSearchCount(int num) {
		searchOptionsCount = num;
	}

	public string CheckCurrentCount() {
		if(currentSearchCount >= searchOptionsCount) {
			//.. nextchapter = Chapter2Continue;
			return "Chapter2Continue";
		} else {
			currentSearchCount++;
			return "Chapter2c";
		}
	}
}

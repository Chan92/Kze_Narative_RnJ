using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seekable : MonoBehaviour {
	private bool isMouseOver;

	private void OnMouseOver() {
		
	}

	private void OnMouseEnter() {
		isMouseOver = true;
		print("seekable");
		Seeker.instance.ChangeFire(true);
	}


	private void OnMouseExit() {
		isMouseOver = false;
		Seeker.instance.ChangeFire(false);
	}
}

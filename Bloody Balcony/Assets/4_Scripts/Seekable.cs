using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seekable : MonoBehaviour {
	public float cooldown;
	private bool isMouseOver;

	private void OnMouseOver() {
		
	}

	private void OnMouseEnter() {
		isMouseOver = true;
		//print("seekable");
		StartCoroutine(CooldownTimer());
	}

	private void OnMouseExit() {
		isMouseOver = false;
		Seeker.instance.ChangeFire(false);

	}

	private IEnumerator CooldownTimer() {
		yield return new WaitForSeconds(cooldown);

		if(isMouseOver) {
			Seeker.instance.ChangeFire(true);
		}
	}
}

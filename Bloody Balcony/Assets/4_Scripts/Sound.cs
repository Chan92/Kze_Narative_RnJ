using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
	public AudioSource audioObj;
	public Slider soundSlider;

	private void Start() {
		SetSoundValue();
	}

	public void SetSoundValue() {
		audioObj.volume = soundSlider.value;
	}
}

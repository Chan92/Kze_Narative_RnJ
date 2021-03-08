using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour {
	public static Seeker instance;

	public GameObject testobj;

	public ParticleSystem particles;
	[Header("red fire")]
	public Color red1;
	public Color red2;
	[Header("blue fire")]
	public Color blue1;
	public Color blue2;

	private void Awake() {
		instance = this;
	}
	
	public void ChangeFire(bool hoverOn) {
		var currentCol = particles.main;

		ParticleSystem.MinMaxGradient newCol;

		if(hoverOn) {
			newCol = new ParticleSystem.MinMaxGradient(blue1, blue2);
		} else {
			newCol = new ParticleSystem.MinMaxGradient(red1, red2);
		}
		
		newCol.mode = ParticleSystemGradientMode.TwoColors;
		currentCol.startColor = newCol;
	}
}

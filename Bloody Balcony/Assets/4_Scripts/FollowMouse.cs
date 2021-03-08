using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour{
	public bool moveable = true;
	public Vector2 clampOffsetX = new Vector2(-8, 8);
	public Vector2 clampOffsetY = new Vector2(-5, 5);
	public float offsetZ = 0;
	public float moveSpeed = 0.1f;
	
	private Vector3 mousePosition;

	void Update(){
		if(moveable) {
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			mousePosition.z = offsetZ;

			mousePosition.x = Mathf.Clamp(mousePosition.x , clampOffsetX.x, clampOffsetX.y);
			mousePosition.y = Mathf.Clamp(mousePosition.y, clampOffsetY.x, clampOffsetY.y);

			transform.localPosition = mousePosition;
		}
	}
}

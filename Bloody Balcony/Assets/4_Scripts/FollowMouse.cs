using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour{

	private Vector3 mousePosition;
	public float moveSpeed = 0.1f;

	void Update(){
		//if(Input.GetMouseButtonDown(1)) {
		if(true) {
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			mousePosition.z = 0;

			mousePosition.x = Mathf.Clamp(mousePosition.x , -8, 8);
			mousePosition.y = Mathf.Clamp(mousePosition.y, -5, 5);

			transform.localPosition = mousePosition;
			//transform.position = mousePosition;
			//transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
		}
	}

	private void OnMouseDrag() {
		Vector3 testpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		testpos.z = transform.position.z;
		transform.position = testpos;
	}
}

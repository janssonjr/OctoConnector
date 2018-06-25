using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCamera : MonoBehaviour {

	public SpriteRenderer sr;
	public Camera cam;

	// Use this for initialization
	void Start () {
		float worldScreenHeight = cam.orthographicSize * 2;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		transform.localScale = new Vector3(worldScreenWidth / sr.sprite.bounds.size.x, worldScreenHeight/ sr.sprite.bounds.size.y, 1);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

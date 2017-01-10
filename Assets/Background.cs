using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public float parallaxScale;
	
	// Update is called once per frame
	void Update () {

        GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2 (
            transform.position.x / transform.localScale.x,
            transform.position.y / transform.localScale.y) * parallaxScale;
	}
}

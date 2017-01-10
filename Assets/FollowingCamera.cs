using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {
    public GameObject player;

	void Update () {
        // Follow the player
        transform.position = new Vector3(
            player.transform.position.x, 
            player.transform.position.y, 
            this.transform.position.z);
	}
}

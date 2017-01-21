using UnityEngine;

public class Player : Spacecraft {

	void FixedUpdate () {
        ApplyForwardThrust(Input.GetAxis("Vertical"));
        ApplyTorque(Input.GetAxis("Horizontal") * -1);
	}


}

using UnityEngine;

public class Player : Spacecraft {

    //public float mainThrusterPower = 4f;
    //public float angularThrusterPower = .5f;
    //public ParticleSystem mainThrusterParticleSystem;

	void FixedUpdate () {
        ApplyForwardThrust(Input.GetAxis("Vertical"));
        ApplyTorque(Input.GetAxis("Horizontal"));
	}


}

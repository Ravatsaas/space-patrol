using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spacecraft : NavigatableObject {

    public float mainThrusterPower = 4f;
    public float angularThrusterPower = .5f;
    public ParticleSystem mainThrusterParticleSystem;

    private Rigidbody2D rigidbody2d;

    protected virtual void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    protected void ApplyForwardThrust(float acceleration)
    {
        if (acceleration <= 0)
        {
            mainThrusterParticleSystem.Stop();
            return;
        }
            

        rigidbody2d.AddRelativeForce(new Vector2(0, acceleration * mainThrusterPower));

        var psMain = mainThrusterParticleSystem.main;
        psMain.startSpeed = acceleration * mainThrusterPower;

        mainThrusterParticleSystem.Play();
                    
    }

    protected void ApplyTorque(float torque)
    {
        rigidbody2d.AddTorque(torque * angularThrusterPower);
    }

}

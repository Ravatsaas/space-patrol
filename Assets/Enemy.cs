using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Spacecraft {

    private enum EnemyState { Idle, Navigating, Attacking }
    private EnemyState currentState;

    protected override void Start()
    {
        currentState = EnemyState.Attacking;
        base.Start();
    }

    private void Update()
    {
        // Reevaluate which state we're in?
    } 

    void FixedUpdate () {

        switch (currentState)
        {
            case EnemyState.Idle:
                
                break;

            case EnemyState.Navigating:
                
                break;
            case EnemyState.Attacking:
                FollowPlayer();
                break;
        }
	}

    void FollowPlayer()
    {
        var playerPosition = FindObjectOfType<Player>().transform.position;
        Navigate(playerPosition - transform.position);

    }

    void Navigate(Vector3 vector)
    {
        var currentAngle = transform.rotation.eulerAngles.z;
        var targetAngle = Vector3.Angle(Vector3.up, vector);
        var angleDiff = currentAngle - targetAngle;

        var angularVelocity = GetComponent<Rigidbody2D>().angularVelocity; //deg pr. sec

        // adjust angle

        // apply forward thrust if within 5 deg

        float distance = vector.magnitude;
        if (Mathf.Abs(angleDiff) < 5)
        {
            ApplyForwardThrust(1);
        }
        else
            ApplyForwardThrust(0);
        
    }
}

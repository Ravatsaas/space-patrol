using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Spacecraft {

    private float maxSpeed = 10f;
    private float maxAngularSpeed = 20f;
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
        var angleDiff = AngleDiff(currentAngle, targetAngle);
        var angularSpeed = GetComponent<Rigidbody2D>().angularVelocity; //deg pr. sec
        var forwardSpeed = Vector3.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);

        //Debug.LogFormat("ANGLES: enemy: {0:N}, target: {1:N}, diff: {2:N}", currentAngle, targetAngle, angleDiff);

        // face toward target
        var targetAngularSpeed = maxAngularSpeed * (angleDiff > 0 ? -1 : 1);
        // Debug.LogFormat("ROTATION: angular speed: {0:N}, target: {1:N}", angularSpeed, targetAngularSpeed);
        ApplyTorque(Mathf.Clamp(targetAngularSpeed - angularSpeed, -1, 1));

        // apply forward thrust if within 5 deg
        Debug.LogFormat("forwardSpeed: {0:N}", forwardSpeed);
        if (Mathf.Abs(angleDiff) < 5 && forwardSpeed < maxSpeed)
            ApplyForwardThrust(1);
        else
            ApplyForwardThrust(0);       
    }

    /// <summary>
    /// Returns the difference between two angles in the range -180 to 180 degrees
    /// </summary>
    /// <param name="currentAngle"></param>
    /// <param name="targetAngle"></param>
    /// <returns></returns>
    float AngleDiff(float currentAngle, float targetAngle)
    {
        if (currentAngle < 0 || currentAngle > 360) throw new System.ArgumentOutOfRangeException("currentAngle", currentAngle, "Should be between 0 and 360");
        if (targetAngle < 0 || targetAngle > 360) throw new System.ArgumentOutOfRangeException("targetAngle", targetAngle, "Should be between 0 and 360");

        var result = currentAngle - targetAngle;
        if (result > 180)
            result -= 360;
        return result;
    }
}

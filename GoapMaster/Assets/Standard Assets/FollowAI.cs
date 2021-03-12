using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAI : MonoBehaviour {

    private Transform tr = null;
    public Transform target;

    private float maxSpeed = 15; // 最大速度
    private Vector3 accelerated = Vector3.zero; // 加速度

    private Vector3 velocity = new Vector3(16, 0, 30); // 速率
	// Use this for initialization
	void Start () {
        tr = transform;	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
        accelerated = Accelerated();

        velocity += accelerated * Time.deltaTime;

        Debug.LogError(velocity);
        tr.Translate(velocity * Time.deltaTime, Space.World);
    }

    private Vector3 Accelerated()
    {
        Vector3 forceVelocity = Vector3.zero;
        if (target == null)
        {
            return forceVelocity;
        }
        Vector3 desiredVector = (target.position - tr.position).normalized * maxSpeed;

        Debug.LogError(((target.position - tr.position).normalized));

        forceVelocity = desiredVector - velocity;

        return forceVelocity;
    }

}

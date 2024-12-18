using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MLAgent : Agent
{
    [SerializeField] private float forceMovement = 200;
    [SerializeField] private Transform target;

    public bool training = true;
    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        if (!training)
        {
            MaxStep = 0;
        }
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        MoveInitialPosition();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 movimiento = new Vector3(actions.ContinuousActions[0], 0f, actions.ContinuousActions[1]);
        rb.AddForce(movimiento * forceMovement * Time.deltaTime);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 toTarget = target.position - transform.position;
        sensor.AddObservation(toTarget.normalized);
    }

    public  void Heuristic(float[] actionsOut)
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        actionsOut[0] = movement.x;
        actionsOut[1] = movement.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (training)
        {
            if (other.CompareTag("target"))
            {
                AddReward(1f);
            }
            if (other.CompareTag("borders"))
            {
                AddReward(-0.1f);
            }
        }
    }

private void OnTriggerStay(Collider other)
    {
        if (training) 
        {
            if (other.CompareTag("target"))
            {   
                AddReward(0.5f);
            }
            if (other.CompareTag("borders"))
            {
                AddReward(-0.05f);
            }
        }
    }

    private void MoveInitialPosition()
    {
        bool positionFound = false;
        int attempts = 100;
        Vector3 positionPotential = Vector3.zero;

        while (!positionFound && attempts >= 0)
        {
            attempts--;
            positionPotential = new Vector3(transform.parent.position.x + UnityEngine.Random.Range(-3f, 3f), 1f, transform.parent.position.z + UnityEngine.Random.Range(-3f, 3f));

            Collider[] colliders = Physics.OverlapSphere(positionPotential, 0.05f);
            if (colliders.Length == 0)
            {
                transform.position = positionPotential;
                positionFound = true;
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CartAgent : Agent
{
    [SerializeField] private Rigidbody _pole;
    [SerializeField] private float _speed = 10;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;

        _pole.transform.position = new Vector3(0, 2.5f, 0);
        _pole.transform.rotation = Quaternion.identity;
        _pole.angularVelocity = Vector3.zero;
        _pole.velocity = Vector3.zero;
    }

    public override void CollectObservations()
    {
        AddVectorObs(_pole.transform.rotation.eulerAngles.z / 360.0f);
        AddVectorObs(_pole.angularVelocity.z);
        AddVectorObs(transform.position.x);
        AddVectorObs(_rigidbody.velocity.x);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        _rigidbody.AddForce(controlSignal * _speed);

        var poleAngle = _pole.transform.rotation.eulerAngles.z;
        if (Mathf.Abs(180 - poleAngle) < 120 ||
            Mathf.Abs(transform.position.x) > 10)
        {
            Done();
        }
        else
        {
            SetReward(1.0f);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachRandom : PrimativeTask
{
    public override string name { get; }

    public Vector3 loc;

    public ApproachRandom(Transform a, Transform b)
    {
        loc = new Vector3(Random.Range(a.position.x, b.position.x), 1f, Random.Range(a.position.z, b.position.z));
        name = "Approach" + loc.ToString();
    }
    public override bool Prev(State s)
    {
        if (s.random > 0.5){
            return true;
        }
        return false;
    }

    public override void Start(Monster m){
        m.agent.destination = loc;
    }


    public override bool Terminates(Monster m)
    {
        if ((m.agent.destination - m.transform.position).sqrMagnitude < 15 && m.agent.velocity.sqrMagnitude < 0.001)
        {
            m.agent.isStopped = true;
            m.agent.ResetPath();
            return true;
        }
        return false;
    }

    public override void Post(State s)
    {
        s.location = loc;
    }
}
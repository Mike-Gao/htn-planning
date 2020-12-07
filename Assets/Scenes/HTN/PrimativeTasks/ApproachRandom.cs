using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachRandom : PrimativeTask
{
    public override string name { get; }

    public ApproachRandom()
    {
        name = "ApproachRandom";
    }
    public override bool Prev(State s)
    {
        if (s.ObjectInHand is null && s.random < 0.5) {
            return true;
        }
        return false;
    }

    public override void Start(Monster m){
        m.agent.destination = Global.ws.randPos;
    }


    public override bool Terminate(Monster m)
    {
        if ((m.agent.destination - m.transform.position).sqrMagnitude < 30 && m.agent.velocity.sqrMagnitude < 0.001)
        {
            Debug.Log("APPROACHRANDOM - TERMINATES");
            Debug.Log(m.agent.destination);
            m.agent.isStopped = true;
            m.agent.ResetPath();
            return true;
        }
        return false;
    }

    public override void Post(State s)
    {
        s.location = Global.ws.randPos;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachObject : PrimativeTask
{
    GameObject Object;
    public override bool Prev(State s)
    {
        if (Object.tag == "crate" && s.crateCount > 0) {
            s.nearestObject = Object;
            return true;
        } else {
            return false;
        }
    }

    public void Start(Monster m){
        m.agent.destination = Global.ws.target.transform.position;
    }

    public override bool Terminates(Monster m)
    {
        if (m.agent.velocity.sqrMagnitude < 0.001)
        {
            m.agent.isStopped = true;
            m.agent.ResetPath();
            return true;
        }
        return false;
    }

    public void Post(State s)
    {
        s.location = s.target.transform.position;
    }
}

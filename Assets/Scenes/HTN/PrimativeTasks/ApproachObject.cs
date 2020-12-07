using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachObject : PrimativeTask
{
    string objType;
    
    public override string name { get; }

    public ApproachObject(string s)
    {
        name = "Approach" + s;
        objType = s;
    }
    public override bool Prev(State s)
    {
        if (objType == "crate" && s.crateCount > 0 && s.nearestObject.tag == "crate" && Global.ws.random > 0.5 || objType == s.nearestObject.tag) {
            return true;
        } else {
            return false;
        }
    }

    public override void Start(Monster m){
        m.agent.destination = Global.ws.nearestObject.transform.position;
    }


    public override bool Terminate(Monster m)
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
        s.location = s.nearestObject.transform.position;
    }
}

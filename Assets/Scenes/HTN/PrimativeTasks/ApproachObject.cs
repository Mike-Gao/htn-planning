﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachObject : PrimativeTask
{
    string objType;

    public ApproachObject(string s)
    {
        objType = s;
    }
    public override bool Prev(State s)
    {
        if (objType == "crate" && s.crateCount > 0 && s.nearestObject.tag == "crate" || objType == s.nearestObject.tag) {
            return true;
        } else {
            return false;
        }
    }

    public override void Start(Monster m){
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

    public override void Post(State s)
    {
        s.location = s.target.transform.position;
    }
}

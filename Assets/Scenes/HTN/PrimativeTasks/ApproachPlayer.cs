using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachPlayer : PrimativeTask
{


    public override void Post(State s)
    {
        s.playerInRange = true;
        s.location = s.playerLocation;
    }

    public void Start(Monster m) {
        m.agent.destination = Global.ws.playerLocation;
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

}

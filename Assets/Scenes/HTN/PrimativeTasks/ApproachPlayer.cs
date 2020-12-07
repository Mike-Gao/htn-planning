using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachPlayer : PrimativeTask
{

    public override string name => "ApproachPlayer";

    public override void Post(State s)
    {
        s.playerInRange = true;
        s.location = s.playerLocation;
    }

    public override void Start(Monster m) {
        m.agent.destination =  Global.ws.playerLocation - (Global.ws.playerLocation - m.transform.position) / 2;
    }

    public override bool Terminates(Monster m)
    {
        // Shooting range is sqrt(40)
        if ( (m.agent.destination - m.transform.position).sqrMagnitude < 30  && m.agent.velocity.sqrMagnitude < 0.001)
        {
            m.agent.isStopped = true;
            m.agent.ResetPath();
            return true;
        }
        return false;
    }

}

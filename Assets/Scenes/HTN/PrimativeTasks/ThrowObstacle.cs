using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObstacle : PrimativeTask
{
    Vector3 tgt;
    public override string name => "ThrowObstacle";

    public override bool Prev(State s)
    {
        return s.playerInRange && s.ObjectInHand != null;
    }

    public override void Post(State s)
    {
        s.ObjectInHand = null;
    }

    public override void Start(Monster m)
    {
        tgt = new Vector3(m.player.transform.position.x, Global.ws.ObjectInHand.transform.position.y, m.player.transform.position.z);
        Global.ws.ObjectInHand.transform.SetParent(null);
    }

    public override bool Terminates(Monster m) {
        var cur = Global.ws.ObjectInHand.transform.position;

        if (cur.x != tgt.x || cur.z != tgt.z)
        {
            Global.ws.ObjectInHand.transform.position = Vector3.MoveTowards(cur, tgt, 6 * Time.deltaTime);
            return false;
        }

        if (cur.y > 1.38)
        {
            var newtgt = new Vector3(tgt.x, 1, tgt.z);
            Global.ws.ObjectInHand.transform.position = Vector3.MoveTowards(cur, newtgt, 6 * Time.deltaTime);
            return false;
        }

        Global.ws.ObjectInHand = null;
        return true;
    }



    
}

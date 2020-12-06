using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObstacle : PrimativeTask
{
    Vector3 tgt;
    public static readonly float velocity = 6;

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
        tgt = new Vector3(m.player.transform.position.x, m.currentObject.transform.position.y, m.player.transform.position.z);
        m.currentObject.transform.SetParent(null);
    }

    public bool Terminate(Monster m) {
        var cur = m.currentObject.transform.position;

        if (cur.x != tgt.x || cur.z != tgt.z)
        {
            m.currentObject.transform.position = Vector3.MoveTowards(cur, tgt, velocity * Time.deltaTime);
            return false;
        }
        m.currentObject = null;
        return true;
    }



    
}

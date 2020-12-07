using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePickUp : PrimativeTask
{
    string objType;

    public override string name { get;}
    public ObstaclePickUp(string s) {
        this.objType = s;
        name = "PickUp" + s;
    }

    public override bool Prev(State s)
    {
        return s.ObjectInHand == null && (s.nearestObject.transform.position - s.location).sqrMagnitude < 9;
    }

    public override void Post(State s)
    {
        s.ObjectInHand = Global.ws.nearestObject;
    }

    public override void Start(Monster m)
    {
        Global.ws.ObjectInHand = Global.ws.nearestObject;
        Global.ws.ObjectInHand.transform.position = new Vector3(m.transform.position.x, m.transform.position.y + 4f, m.transform.position.z);
        Global.ws.ObjectInHand.transform.SetParent(m.transform);
    }

    public override bool Terminate(Monster m)
    {
        return true;
    }
}

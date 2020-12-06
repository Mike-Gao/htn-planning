using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePickUp : PrimativeTask
{
    string objType;

    public override string name { get;}
    public ObstaclePickUp(string s) {
        this.objType = s;
        name = "pickup" + s;
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
        m.currentObject = Global.ws.target;
        m.currentObject.transform.position = new Vector3(m.transform.position.x, m.transform.position.y + 4f, m.transform.position.z);
        m.currentObject.transform.SetParent(m.transform);
        Global.ws.ObjectInHand.tag = objType;
    }
}

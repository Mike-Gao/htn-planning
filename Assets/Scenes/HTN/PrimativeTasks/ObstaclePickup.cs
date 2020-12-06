using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePickUp : PrimativeTask
{
    GameObject obj;

    public ObstaclePickUp(GameObject obj) {
        this.obj = obj;
    }

    public override bool Prev(State s)
    {
        return s.ObjectInHand == null && (s.target.transform.position - s.location).sqrMagnitude < 9;
    }

    public override void Post(State s)
    {
        s.ObjectInHand = obj;
    }

    public void Start(Monster m)
    {
        m.currentObject = Global.ws.target;
        m.currentObject.transform.position = new Vector3(m.transform.position.x, m.transform.position.y + 4f, m.transform.position.z);
        m.currentObject.transform.SetParent(m.transform);
        Global.ws.ObjectInHand = obj;
    }
}

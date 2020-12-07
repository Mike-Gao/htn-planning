using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObstacle : PrimativeTask
{
    Vector3 tgt;
    public override string name => "ThrowObstacle";

    public override bool Prev(State s)
    {
        //Debug.Log("Prev");
        return s.playerInRange && s.ObjectInHand != null;
    }

    public override void Post(State s)
    {
        //Debug.Log("Post");
        s.ObjectInHand = null;
    }

    public override void Start(Monster m)
    {
        //Debug.Log("Start");
        tgt = new Vector3(m.player.transform.position.x, Global.ws.ObjectInHand.transform.position.y, m.player.transform.position.z);
        Global.ws.ObjectInHand.transform.SetParent(null);
    }

    public override bool Terminate(Monster m) {
        //Debug.Log("Calling Terminate");
        if (Global.ws.ObjectInHand == null) {
            return true;
        }
        
        var cur = Global.ws.ObjectInHand.transform.position;

        if (cur.x != tgt.x || cur.z != tgt.z)
        {
            Global.ws.ObjectInHand.transform.position = Vector3.MoveTowards(cur, tgt, 6 * Time.deltaTime);
            return false;
        }

        if (cur.y > 1)
        {
            Debug.Log("Drop");
            var newtgt = new Vector3(tgt.x, 0f, tgt.z);
            Global.ws.ObjectInHand.transform.position = Vector3.MoveTowards(cur, newtgt, 6 * Time.deltaTime);
            return false;
        }

        // If it's a crate, destroy by any use, so we tag it as "used" thats automatically cleaned by Update()
        if (Global.ws.ObjectInHand.tag == "crate") {
            Global.ws.ObjectInHand.tag = "used";
        }

        Global.ws.ObjectInHand = null;
        return true;
    }



    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : PrimativeTask
{
    float end;

    public override string name => "cooldown";
    public override void Start(Monster m)
    {
        // wait 1 second
        end = Time.time + 1;
    }

    public override bool Terminates(Monster m)
    {
        return Time.time > end;
    }

}

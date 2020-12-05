using System.Collections;
using System.Collections.Generic;

public abstract class PrimativeTask : Task
{


    public virtual void Post(State s) {};
    // return true if it terminates
    public virtual bool Terminates(Monster m) { return true; }
}

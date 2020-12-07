using System.Collections;
using System.Collections.Generic;

public abstract class PrimativeTask : Task
{

    public abstract string name { get; }
    public virtual void Post(State s) {}

    public virtual void Start(Monster m) {}

    // return true if it terminates
    public virtual bool Terminate(Monster m) { return true; }
}

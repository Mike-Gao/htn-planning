using System.Collections;
using System.Collections.Generic;

public abstract class Task
{
    public virtual bool Prev(State s) { return true; }
    public virtual void Post(State s) { }

}

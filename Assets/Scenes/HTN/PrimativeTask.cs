using System.Collections;
using System.Collections.Generic;

public abstract class PrimativeTask : Task
{


    // return true if it terminates
    public virtual bool Terminates() { return true; }
}

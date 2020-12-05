using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Method : CompoundTask
{
    public readonly List<Task> sub_task;
    public Method(List<Task> ts)
    {
        this.sub_task = ts;
    }
    public override bool Prev(State s) {
        return sub_task[0].Prev(s);
    }

    public void AddMethodSubtasksToStack(Stack<Task> tasks) {
        for (int i = sub_task.Count - 1; i >= 0; i--){
            tasks.Push(sub_task[i]);
        }
    }

}

﻿using System.Collections;
using System.Collections.Generic;

public class HTNPlanner
{
    private CompoundTask root;

    private Stack<(Stack<Task> t, List<PrimativeTask> plan, Method m, State state)> history;
    
    public HTNPlanner(CompoundTask  t) {
        this.root = t;
    }

    public List<PrimativeTask> GetPlan() {
        history = new Stack<(Stack<Task> t, List<PrimativeTask> plan, Method m, State state)>();
        List<PrimativeTask> plan = new List<PrimativeTask>();
        State s = Global.ws.Clone();
        Stack<Task> tasks = new Stack<Task>();
        tasks.Push(root);
        Method m = null;
        while (tasks.Count > 0)
        {
            Task t = tasks.Peek();
            if(t is CompoundTask ct){
                m = ct.GetMethod(s, m);
                if (m == null) {
                    (tasks, plan, m, s) = history.Pop();
                } else {
                    history.Push((new Stack<Task>(tasks), new List<PrimativeTask>(plan), m, s.Clone()));
                    tasks.Pop();
                    m.AddMethodSubtasksToStack(tasks);
                }
            } else if (t is PrimativeTask pt) {
                if (t.Prev(s))
                {
                    tasks.Pop();
                    pt.Post(s);
                    plan.Add(pt);
                }
                else 
                {
                    (tasks, plan, m, s) = history.Pop();
                }
            }
        }
        plan.Reverse();
        return plan;
    }
}

using System.Collections;
using System.Collections.Generic;

public class CompoundTask : Task
{

    public readonly List<Method> methods_lst = new List<Method>();

    public Method GetMethod(State s, Method last) {
        bool afterLast = last == null || !methods_lst.Contains(last);
        foreach (var m in methods_lst)
        {
            if (afterLast) {
                if (m.Prev(s)) return m;
            } else {
                if (m == last) afterLast = true;
            }
            
        }
        return null;
    }

    public void AddMethod(List<Task> lst) {
        methods_lst.Add(new Method(lst));
    }

    public override bool Prev(State s) {
        foreach(var m in methods_lst) {
            if (m.Prev(s)) return true;
        }
        return false;
    }
}

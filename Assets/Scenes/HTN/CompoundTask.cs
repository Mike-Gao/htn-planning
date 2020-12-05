using System.Collections;
using System.Collections.Generic;

public class CompoundTask : Task
{
    public readonly string name;

    public readonly List<Method> methods_lst = new List<Method>();
    
    public CompoundTask(string n) {
        this.name = n;
    }

    public string GetName() {
        return this.name;
    }

    public Method GetMethod(State s, Method last) {
        foreach (var m in methods_lst)
        {
            if (m.Prev(s)) return m;
        }
        return null;
    }

    public void AddMethod(List<Method> lst) {
        methods_lst.Add(new Method());
    }
}

using System;
using System.Collections.Generic;

public static class Extentions
{
    // Stack Extention
    public static void Shuffle<T>(this Stack<T> stack)
    {
        List<T> l = new List<T>();
        int n = stack.Count;
        for (int i = 0; i < n; i++)
        {
            l.Add(stack.Pop());
        }
        T value;
        int k;
        while (--n > 1)
        {
            k = Constants.GetRandomNumber(n + 1);
            value = l[k];
            l[k] = l[n];
            l[n] = value;
        }
        foreach (T t in l)
            stack.Push(t);
        l.Clear();
    }
}

public class AnimationStateReference
{
    public bool value;
    public bool running;

    public AnimationStateReference(bool value = false, bool running = false)
	{
        this.value = value;
        this.running = running;
	}

	public void Reset(bool value = false)
	{
        this.value = value;
        running = false;
	}

	/*private Func<T> _get;
    private Action<T> _set;

    public VarRef(Func<T> get,Action<T> set)
	{
        _get = get;
        _set = set;
	}

    public T Value
	{
        get => _get(); 
        set { _set(value); }
	}*/
}
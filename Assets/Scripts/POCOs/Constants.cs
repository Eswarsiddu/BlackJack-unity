
using System.Collections.Generic;
using UnityEngine;
public static class Constants
{

    private static System.Random rng = new System.Random();

    public static Sprite CARDBACKSPRITE;

    public static Transform offsiteCardsParent;


    public static void Shuffle<T>(this Stack<T> stack)
    {
        List<T> l = new List<T>();
        int n = stack.Count;
        for(int i = 0; i < n; i++)
		{
            l.Add(stack.Pop());
		}
        T value;
        int k;
		while (n > 1)
		{
            n--;
            k = rng.Next(n + 1);
            value = l[k];
            l[k] = l[n];
            l[n] = value;
		}
		foreach (T t in l)
		{
            stack.Push(t);
		}
        l.Clear();
    }
}

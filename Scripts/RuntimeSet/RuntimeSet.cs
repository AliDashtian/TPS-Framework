using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> Items = new List<T>();

    public event Action OnItemAdded;
    public event Action OnItemRemoved;
    public event Action OnSetEmptied;

    public void Add(T t)
    {
        if (!Items.Contains(t))
        {
            Items.Add(t);
            OnItemAdded?.Invoke();
        }
    }

    public void Remove(T t)
    {
        if (Items.Contains(t))
        {
            Items.Remove(t);
            OnItemRemoved?.Invoke();
            InvokeIfSetEmptied();
        }
    }

    public bool Contains(T t)
    {
        return Items.Contains(t);
    }

    public int Count() 
    { 
        return Items.Count;
    }

    private void InvokeIfSetEmptied()
    {
        if (Count() <= 0)
        {
            OnSetEmptied?.Invoke();
        }
    }
}

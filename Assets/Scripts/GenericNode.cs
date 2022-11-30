using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericNode<T>
{
    T dato;
    GenericNode<T> node;
    public GenericNode()
    {
        dato = default(T);
        node = null;
    }
    public T Dato { get => dato; set => dato = value; }
    public GenericNode<T> Node { get => node; set => node = value; }
}

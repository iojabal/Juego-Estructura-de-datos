using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Lista<T> : Collection<T>
{
    GenericNode<T> head;
    GenericNode<T> newei;
    int count = 0;
    public void crearNodo(T dato)
    {
        newei = new GenericNode<T>();
        newei.Dato = dato;
        newei.Node = null;
    }

    public void crearLista(T dato)
    {
        GenericNode<T> punt;
        crearNodo(dato);
        if(head == null)
        {
            head = newei;
            count++;
        }
        else
        {
            punt = head;
            while(punt.Node!= null)
            {
                punt = punt.Node;
            }
            punt.Node = newei;
            count++;
        }
    }

    public T search(T d)
    {
        GenericNode<T> punt = head;
        while(punt != null)
        {
            if (punt.Dato.Equals(d))
            {
                return punt.Dato;
            }
            punt= punt.Node; 
        }
        return default(T) ;
    }

    public bool searchB(T d)
    {
        GenericNode<T> punt = head;
        while (punt != null)
        {
            if (punt.Dato.Equals(d))
            {
                return true;
            }
            punt = punt.Node;
        }
        return false;
    }

    public void deleteHead()
    {
        GenericNode<T> punt = head;
        head = head.Node;
        punt.Node = null;
        count--;
    }

    public void deleteNode(T d)
    {
        GenericNode<T> ant = new GenericNode<T>();
        GenericNode<T> punt = head;
        while(punt != null)
        {
            if (head.Dato.Equals(d))
            {
                head = head.Node;
                punt.Node= null;
                count--;
                break;
            }
            else if (punt.Dato.Equals(d))
            {
                ant.Node = punt.Node;
                punt.Node = null;
                count--;
                break;
            }

            else
            {
                ant = punt;
                punt = punt.Node;
            }
        }
    }
    public int Count { get => count; set => count = value; }
}

using System;
using System.Collections;
using System.Collections.Generic;

public class LoopChainTableNode<T>
{
    public T content;
    public LoopChainTableNode<T> next;
    public LoopChainTableNode<T> before;
    public LoopChainTableNode(T Head)
    {
        content = Head;
        before = this;
        next = this;
    }
}
public class LoopChainTable<T>
{
    public LoopChainTableNode<T> Current
    {
        get;set;
    }
    public LoopChainTable(LoopChainTableNode<T> Head)
    {
        Current = Head;
    }
    public LoopChainTable(T Head) : this(new LoopChainTableNode<T>(Head))
    {

    }
    public void Add(LoopChainTableNode<T> newNode)
    {
        newNode.next = Current.next;
        newNode.before = Current;
        Current.next.before = newNode;
        Current.next = newNode;
        Current = newNode;
    }
    public void Add(T content)
    {
        var newNode = new LoopChainTableNode<T>(content);
        Add(newNode);
    }
    public T MoveNext()
    {
        var temp = Current;
        Current = Current.next;
        return temp.content;
    }
    public T MoveBack()
    {
        var temp = Current;
        Current = Current.before;
        return temp.content;
    }
}
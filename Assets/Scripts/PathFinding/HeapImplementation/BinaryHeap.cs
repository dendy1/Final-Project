﻿using System;

public class BinaryHeap<T> where T : HeapItem<T>
{
    private T[] _items;
    private int _currentItemIndex;
    
    public BinaryHeap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    public int Count => _currentItemIndex;

    public bool Contains(T item)
    {
        return Equals(_items[item.HeapIndex], item);
    }

    public void Insert(T item)
    {
        item.HeapIndex = _currentItemIndex;
        _items[_currentItemIndex] = item;
        
        ReCalculateUp(item);

        _currentItemIndex++;
    }

    public T Peek()
    {
        if (Count == 0)
            throw new IndexOutOfRangeException();

        return _items[0];
    }

    public T Extract()
    {
        T extractable = _items[0];
        _currentItemIndex--;

        _items[0] = _items[_currentItemIndex];
        _items[0].HeapIndex = 0;
        
        ReCalculateDown(_items[0]);
        return extractable;
    }

    public void UpdateItem(T item)
    {
        ReCalculateUp(item);
    }

    private void ReCalculateUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        
        while (true)
        {
            T parentItem = _items[parentIndex];
            
            if (item.CompareTo(parentItem) > 0)
                Swap(parentItem, item);
            else 
                break;           
        }
    }

    private void ReCalculateDown(T item)
    {
        while (true)
        {
            int leftChild = item.HeapIndex * 2 + 1, rightChild = item.HeapIndex * 2 + 2;

            if (leftChild < _currentItemIndex)
            {
                var swapIndex = leftChild;

                if (rightChild < _currentItemIndex)
                    if (_items[leftChild].CompareTo(_items[rightChild]) < 0)
                        swapIndex = rightChild;

                if (item.CompareTo(_items[swapIndex]) < 0)
                    Swap(item, _items[swapIndex]);
                else
                    return;
            }
            else
                return;
        }
    }

    private void Swap(int firstIndex, int secondIndex)
    {
        Swap(_items[firstIndex], _items[secondIndex]);
    }

    private void Swap(T itemA, T itemB)
    {
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;

        var itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

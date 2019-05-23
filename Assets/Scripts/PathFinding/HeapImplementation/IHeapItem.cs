using System;

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
    
    int LeftChildIndex { get; }
    int RightChildIndex { get; }
    int ParentIndex { get; }
}

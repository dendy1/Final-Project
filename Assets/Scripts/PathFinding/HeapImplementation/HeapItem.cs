public abstract class HeapItem<T> : IHeapItem<T>
{
    public int HeapIndex { get; set; }
    public int LeftChildIndex => HeapIndex * 2 + 1;
    public int RightChildIndex => HeapIndex * 2 + 2;
    public int ParentIndex => (HeapIndex - 1) / 2;

    public abstract int CompareTo(T other);
}

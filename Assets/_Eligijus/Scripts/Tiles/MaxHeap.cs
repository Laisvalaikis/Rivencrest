using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaxHeap
{

    ChunkData[] _arr;

    // Maximum possible size of
    // the Max Heap.
    int _maxSize;

    // Number of elements in the
    // Max heap currently.
    int _heapSize;

    // Constructor function.
    public MaxHeap(int maxSize)
    {
        this._maxSize = maxSize;
        _arr = new ChunkData[maxSize];
        _heapSize = 0;
    }

    public void MaxHeapify()
    {
        MaxHeapify(0);
    }

    // Heapifies a sub-tree taking the
    // given index as the root.
    void MaxHeapify(int i)
    {
        int l = LChild(i);
        int r = RChild(i);
        int largest = i;
        
        if(l < _heapSize && _arr[l].GetWeight() > _arr[i].GetWeight())
            largest = l;
        if (r < _heapSize && _arr[r].GetWeight() > _arr[largest].GetWeight())
            largest = r;

        if (largest != i)
        {
            ChunkData temp = _arr[i];
            _arr[i] = _arr[largest];
            _arr[largest] = temp;
            MaxHeapify(largest);
        }
    }

    // Returns the index of the parent
    // of the element at ith index.
    private int Parent(int i)
    {
        return (i - 1) / 2;
    }

    // Returns the index of the left child.
    private int LChild(int i)
    {
        return (2 * i + 1);
    }

    // Returns the index of the
    // right child.
    private int RChild(int i)
    {
        return (2 * i + 2);
    }

    // Removes the root which in this
    // case contains the maximum element.
    private ChunkData RemoveMax()
    {
        // Checking whether the heap array
        // is empty or not.
        if (_heapSize <= 0)
            return null;
        if (_heapSize == 1)
        {
            _heapSize--;
            return _arr[0];
        }

        // Storing the maximum element
        // to remove it.
        ChunkData root = _arr[0];
        _arr[0] = _arr[_heapSize - 1];
        _heapSize--;

        // To restore the property
        // of the Max heap.
        MaxHeapify(0);

        return root;
    }

    // Increases value of key at
    // index 'i' to new_val.
    private void IncreaseKey(int i, ChunkData newVal)
    {
        _arr[i] = newVal;
        while (i != 0 && _arr[Parent(i)].GetWeight() < _arr[i].GetWeight())
        {
            ChunkData temp = _arr[i];
            _arr[i] = _arr[Parent(i)];
            _arr[Parent(i)] = temp;
            i = Parent(i);
        }
    }

    // Returns the maximum key
    // (key at root) from max heap.
    public ChunkData GetMax()
    {
        return _arr[0];
    }

    public int CurSize()
    {
        return _heapSize;
    }

    // Deletes a key at given index i.
    public void DeleteKey(int i)
    {
        // It increases the value of the key
        // to infinity and then removes
        // the maximum value.
        ChunkData maxChunkData = new ChunkData(2000, 2000);
        IncreaseKey(i, maxChunkData);
        RemoveMax();
    }

    // Inserts a new key 'x' in the Max Heap.
    public void InsertKey(ChunkData x)
    {
        // To check whether the key
        // can be inserted or not.
        if (_heapSize == _maxSize)
        {
            Debug.Log("\nOverflow: Could not insertKey\n");
            return;
        }

        // The new key is initially
        // inserted at the end.
        _heapSize++;
        int i = _heapSize - 1;
        _arr[i] = x;

        // The max heap property is checked
        // and if violation occurs,
        // it is restored.
        while (i != 0 && _arr[Parent(i)].GetWeight() < _arr[i].GetWeight())
        {
            ChunkData temp = _arr[i];
            _arr[i] = _arr[Parent(i)];
            _arr[Parent(i)].SetHeapIndex(i);
            _arr[Parent(i)] = temp;
            i = Parent(i);
            temp.SetHeapIndex(i);
        }
        x.SetHeapIndex(i);
    }

    public void RebuildHeap(int index)
    {
        int i = index;
        while (i != 0 && _arr[Parent(i)].GetWeight() < _arr[i].GetWeight())
        {
            ChunkData temp = _arr[i];
            _arr[i] = _arr[Parent(i)];
            _arr[Parent(i)].SetHeapIndex(i);
            _arr[Parent(i)] = temp;
            i = Parent(i);
            temp.SetHeapIndex(i);
        }
    }
}
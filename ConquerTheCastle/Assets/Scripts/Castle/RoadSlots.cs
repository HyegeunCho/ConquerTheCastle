using System;
using System.Collections;
using System.Collections.Generic;
using HGPlugins;
using UnityEngine;

public class RoadSlots
{
    public int CurrentSlot { private set; get; }= 0;
    
    private List<Castle> _outSlots = new List<Castle>();

    public int Count => _outSlots.IsNullOrEmpty() ? 0 : _outSlots.Count;

    public bool IsContain(Castle inCastle)
    {
        return _outSlots.Contains(inCastle);
    }
    
    public void Add(Castle inCastle)
    {
        if (IsContain(inCastle)) return;
        _outSlots.Add(inCastle);
    }

    public void Remove(Castle inCastle)
    {
        if (!IsContain(inCastle)) return;
        _outSlots.Remove(inCastle);
    }

    public Castle NextTarget()
    {
        if (Count == 0) return null;
        if (CurrentSlot + 1 > Count) CurrentSlot = CurrentSlot % Count;

        Castle result = _outSlots[CurrentSlot];
        CurrentSlot++;
        
        return result;
    }
}

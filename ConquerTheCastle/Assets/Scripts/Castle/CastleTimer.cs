using System;
using System.Collections;
using System.Collections.Generic;
using HGPlugins;
using HGPlugins.Singleton;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class CastleTimer
{
    private Dictionary<float, Subject<int>> _counterMap;
    private Dictionary<float, int> _counterValueMap;
    private Dictionary<float, IDisposable> _disposeMap;

    private bool _isReady = true;
    
    public IObservable<int> GetObservable(float inValue)
    {
        if (_counterMap == null) _counterMap = new Dictionary<float, Subject<int>>();
        if (!_counterMap.ContainsKey(inValue) || _counterMap[inValue] == null) _counterMap[inValue] = new Subject<int>();
        if (!_counterMap.TryGetValue(inValue, out var subject)) return null;

        return subject.AsObservable().ObserveOnMainThread();
    }

    public IDisposable AddListener(float inValue, Action<int> inAction)
    {
        if (!_isReady) return null;
        
        if (_disposeMap == null) _disposeMap = new Dictionary<float, IDisposable>();
        
        var dispose = GetObservable(inValue).Subscribe(inAction); 
        _disposeMap.Add(inValue, dispose);
        return dispose;
    }

    public void Start(float inValue = 0f)
    {
        _isReady = false;
        

        _isReady = true;
    }

    private void StartInternal(float inValue, Subject<int> inSubject)
    {
        if (_counterValueMap == null) _counterValueMap = new Dictionary<float, int>();
        if (!_counterValueMap.ContainsKey(inValue)) _counterValueMap.Add(inValue, 0);
        Observable.Interval(TimeSpan.FromSeconds(inValue)).ObserveOnMainThread().Subscribe(v =>
        {
            if (_counterValueMap.TryGetValue(inValue, out int value))
            {
                _counterValueMap.Add(inValue, value + 1);
            }
            inSubject.OnNext(_counterValueMap[inValue]);
        });
    }

    public void Pause()
    {
        
    }

    public void Stop()
    {
        
    }

    public void Clear()
    {
        
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using HGPlugins;
using HGPlugins.Singleton;
using UniRx;

public class CastleTimerManager : MonoSingleton<CastleTimerManager>
{
    private Subject<long> _seconds;
    private Subject<long> _half;
    private Subject<long> _third;

    public IObservable<long> OnSeconds => _seconds;
    public IObservable<long> OnHalf => _half;
    public IObservable<long> OnThird => _third;


    public override void Awake()
    {
        base.Awake();

        if (_seconds == null) _seconds = new Subject<long>();
        if (_half == null) _half = new Subject<long>();
        if (_third == null) _third = new Subject<long>();
    }

    private void Start()
    {
        Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(v => _seconds.OnNext(v)).AddTo(this);
        Observable.Interval(TimeSpan.FromMilliseconds(500)).Subscribe(v => _half.OnNext(v)).AddTo(this);
        Observable.Interval(TimeSpan.FromMilliseconds(333)).Subscribe(v => _third.OnNext(v)).AddTo(this);
    }
}

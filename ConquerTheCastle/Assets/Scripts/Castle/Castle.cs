using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private TextMeshPro TXT_CastleHP;
    
    [Range(1, 10)] public float Height;
    public int MaxHp = 60;
    
    [HideInInspector] public ReactiveProperty<int> CurrentHp;

    public float CurrentHeight
    {
        get { return transform.localScale.y; }
        set
        {
            transform.localScale = new Vector3(1, value, 1);
        }
    }

    private void Awake()
    {
        CurrentHp = new ReactiveProperty<int>(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp.ObserveOnMainThread().Subscribe(OnUpdateCastleHp).AddTo(this);

        CastleTimerManager.Instance.OnSeconds.Subscribe(OnTimerSeconds).AddTo(this);
        // CastleTimerManager.Instance.OnHalf.Subscribe(OnTimerHalf).AddTo(this);
        // CastleTimerManager.Instance.OnThird.Subscribe(OnTimerThird).AddTo(this);
    }
    
    private void OnUpdateCastleHp(int inHp)
    {
        TXT_CastleHP.text = $"{CurrentHp}";
    }

    private void OnTimerSeconds(long inCount)
    {
        RegenerateCastleHp();
    }

    private void RegenerateCastleHp()
    {
        if (CurrentHp.Value >= MaxHp) return;
        CurrentHp.Value = CurrentHp.Value + 1;
    }

    private void OnTimerHalf(int inCount)
    {
        
    }

    private void OnTimerThird(int inCount)
    {
        
    }

    void Update()
    {
        UpdateHeight(Height);
    }

    public void UpdateHeight(float inValue)
    {
        if (Math.Abs(CurrentHeight - inValue) < MathConstants.FLOAT_COMPARISION_TOLERANCE) return;
        CurrentHeight = inValue;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using HGPlugins;
using TMPro;
using UniRx;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public const int SLOT3_HP = 30;
    public const int SLOT2_HP = 10;
    public const int SLOT1_HP = 1;
    
    [SerializeField] private TextMeshPro TXT_CastleHP;

    public List<Castle> OutSlots;
    
    
    [Range(1, 10)] public float Height;
    public int MaxHp = 60;
    
    [HideInInspector] public ReactiveProperty<int> CurrentHp;
    
    private int _maxSlots = 1;

    public bool IsMax => (CurrentHp.Value == MaxHp);
    
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
        OutSlots = new List<Castle>();
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
        if (inHp >= SLOT3_HP)
        {
            _maxSlots = 3;
        }
        else if (inHp >= SLOT2_HP)
        {
            _maxSlots = 2;
        }
        else
        {
            _maxSlots = 1;
        }

        string strHp = IsMax ? "MAX" : inHp.ToString();
        TXT_CastleHP.text = $"{_maxSlots}:{strHp}";

        float adder = inHp / 10f;
        UpdateHeight(1f + adder);
    }

    private void OnTimerSeconds(long inCount)
    {
        try
        {
            if (OutSlots.Count == 0)
            {
                RegenerateCastleHp();
            }
        }
        catch (Exception e)
        {
             Debug.LogError($"{e.Message}");
        }
        
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

    public void UpdateHeight(float inValue)
    {
        if (Math.Abs(CurrentHeight - inValue) < MathConstants.FLOAT_COMPARISION_TOLERANCE) return;
        CurrentHeight = inValue;
    }
}

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
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private TriggerDelegator _trigger;
    
    [Range(1, 10)] public float Height;
    public int MaxHp = 60;
    
    [HideInInspector] public ReactiveProperty<int> CurrentHp;
    
    public int ID = 1;
    public Enums.EColor CastleColor = Enums.EColor.Blue;
    
    private RoadSlots _roadSlots;
    
    private int _maxSlots = 1;

    public bool IsAlive
    {
        get
        {
            return CurrentHp.Value > 0 && gameObject.activeSelf;
        }
    }

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
        _roadSlots = new RoadSlots();
        
        _trigger.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp.ObserveOnMainThread().Subscribe(OnUpdateCastleHp).AddTo(this);

        CastleTimerManager.Instance.OnSeconds.Subscribe(OnTimerSeconds).AddTo(this);
        CastleTimerManager.Instance.OnHalf.Subscribe(OnTimerHalf).AddTo(this);
        CastleTimerManager.Instance.OnThird.Subscribe(OnTimerThird).AddTo(this);

        _trigger.DelegateTriggerEnter += OnTriggerEnter;

        SetColor(CastleColor);
    }
    
    public void SetColor(Enums.EColor inColor)
    {
        _renderer.material = ResourceManager.GetMaterial(inColor);
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
        if (_roadSlots.Count == 0)
        {
            RegenerateCastleHp();
        }

        if (CurrentHp.Value >= MaxHp / 2f) return;

        Castle target = _roadSlots.NextTarget();
        if (target == null) return;
        RequestDepart(target);
    }

    private void OnTimerHalf(long inCount)
    {
        if (CurrentHp.Value < MaxHp / 2f) return;
        if (CurrentHp.Value == MaxHp) return;
        
        Castle target = _roadSlots.NextTarget();
        if (target == null) return;
        RequestDepart(target);
    }

    private void OnTimerThird(long inCount)
    {
        if (CurrentHp.Value != MaxHp) return;
        
        Castle target = _roadSlots.NextTarget();
        if (target == null) return;
        RequestDepart(target);
    }
    
    private void RegenerateCastleHp()
    {
        if (CurrentHp.Value >= MaxHp) return;
        CurrentHp.Value = CurrentHp.Value + 1;
    }

    public void UpdateHeight(float inValue)
    {
        if (Math.Abs(CurrentHeight - inValue) < MathConstants.FLOAT_COMPARISION_TOLERANCE) return;
        CurrentHeight = inValue;
    }

    public void RequestDepart(Castle inTarget)
    {
        Soldier soldier = SoldierManager.Instance.GetSoldier(CastleColor, this);
        soldier.MoveTo(inTarget);
    }

    public void ConnectTo(Castle inTarget)
    {
        if (_roadSlots.Count >= _maxSlots) return;
        if (RoadManager.Instance.IsConnected(this, inTarget)) return;
        _roadSlots.Add(inTarget);
        RoadManager.Instance.ConnectCastle(this, inTarget);
        
    }

    public void DisconnectFrom(Castle inTarget)
    {
        if (_roadSlots.Count == 0) return;
        if (!RoadManager.Instance.IsConnected(this, inTarget)) return;
        _roadSlots.Remove(inTarget);
        RoadManager.Instance.DisconnectCastle(this, inTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }
}

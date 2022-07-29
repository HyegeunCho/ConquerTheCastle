using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private TriggerDelegator _trigger;

    public int AttackPoint = 1;
    public int CurrentHp = 1;
    
    public float Speed = 0.5f;
    public Enums.EColor CurrentColor { private set; get; }
    
    public Castle TargetCastle { private set; get; } = null;

    public int HomeCastleID { private set; get; } = 0;

    private void Awake()
    {
        _trigger.Clear();
    }

    private void Start()
    {
        _trigger.DelegateTriggerEnter += OnTriggerEnter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HomeCastleID == 0) return;
        if (other.CompareTag(Tags.CASTLE))
        {
            Castle target = other.transform.parent.GetComponent<Castle>();
            if (target == null) return;

            if (target.ID == HomeCastleID) return;
            // Destroy without die animation
        }
        else if (other.CompareTag(Tags.SOLDIER))
        {
            Soldier target = other.transform.parent.GetComponent<Soldier>();
            if (target == null) return;
            
            // Retrieve opponent's attack point
            // Destroy with die animation
        }
        
        StopAllCoroutines();
        SoldierManager.Instance.ReturnSoldier(this);
    }

    public bool IsAllyWith(Castle inTarget)
    {
        return CurrentColor == inTarget.CastleColor;
    }

    public bool IsAllyWith(Soldier inTarget)
    {
        return CurrentColor == inTarget.CurrentColor;
    }

    public void SetHomeCastle(Castle inCastle)
    {
        HomeCastleID = inCastle.ID;
    }
    
    public void SetColor(Enums.EColor inColor)
    {
        CurrentColor = inColor;
        _renderer.material = ResourceManager.GetMaterial(CurrentColor);
    }

    public void MoveTo(Castle inTarget)
    {
        TargetCastle = inTarget;
    }

    private void Update()
    {
        MoveToTarget(TargetCastle);
    }

    private void MoveToTarget(Castle inTarget)
    {
        if (inTarget == null) return;

        Vector3 direction = inTarget.transform.position - transform.position;
        transform.Translate(direction.normalized * Speed * Time.deltaTime, Space.World);
    }

    private void OnDisable()
    {
        Drop();

    }

    private void Drop()
    {
        TargetCastle = null;
    }
}

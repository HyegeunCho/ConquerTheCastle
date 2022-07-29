using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    

    public float Speed = 0.5f;
    
    public Castle TargetCastle { private set; get; } = null;

    public void SetColor(Enums.EColor inColor)
    {
        _renderer.material = ResourceManager.GetMaterial(inColor);
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

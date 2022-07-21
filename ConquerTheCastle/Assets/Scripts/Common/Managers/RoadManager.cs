using System;
using System.Collections;
using System.Collections.Generic;
using HGPlugins.Singleton;
using Unity.VisualScripting;
using UnityEngine;

public struct RoadInfo : IEquatable<RoadInfo>
{
    public int FromId;
    public int ToId;

    public bool Equals(RoadInfo inInfo)
    {
        return FromId == inInfo.FromId && ToId == inInfo.ToId;
    }

    public static RoadInfo Create(Castle inFrom, Castle inTo)
    {
        return new RoadInfo
        {
            FromId = inFrom.ID, ToId = inTo.ID
        };
    }
}

public class RoadManager : MonoSingleton<RoadManager>
{
    public enum EColor
    {
        Blue = 0,
        Green = 1,
        Purple = 2,
        Red = 3,
        Yellow = 4
    }

    [SerializeField] private Material[] RoadMaterials;


    private Queue<LineRenderer> _linePool;

    private Dictionary<RoadInfo, LineRenderer> _currentRoads;

    public override void Awake()
    {
        base.Awake();
        _linePool = new Queue<LineRenderer>();
        _currentRoads = new Dictionary<RoadInfo, LineRenderer>();
    }

    private Material GetMaterial(EColor inColor)
    {
        return RoadMaterials[(int) inColor];
    }

    private LineRenderer CreateLine(EColor inColor, Vector3 inFrom, Vector3 inTo)
    {
        GameObject go = new GameObject();
        go.transform.SetParent(transform);

        LineRenderer result = go.AddComponent<LineRenderer>();

        UpdateLine(ref result, inColor, inFrom, inTo);
        // result.material.CopyPropertiesFromMaterial(GetMaterial(inColor));
        //
        // result.startWidth = 1f;
        // result.endWidth = 1f;
        //
        // result.SetPositions(new Vector3[] {inFrom, inTo});

        return result;
    }

    private void UpdateLine(ref LineRenderer refLine, EColor inColor, Vector3 inFrom, Vector3 inTo)
    {
        refLine.material = GetMaterial(inColor);
        refLine.startWidth = 1f;
        refLine.endWidth = 1f;
        refLine.SetPositions(new Vector3[] {inFrom, inTo});
    }

    private LineRenderer GetLine(EColor inColor, Vector3 inFrom, Vector3 inTo)
    {
        LineRenderer result = null;
        if (_linePool.Count > 0)
        {
            result = _linePool.Dequeue();
            result.gameObject.SetActive(true);
            UpdateLine(ref result, inColor, inFrom, inTo);
        }
        else
        {
            result = CreateLine(inColor, inFrom, inTo);
        }

        return result;
    }

    private void ReturnLine(LineRenderer inLine)
    {
        inLine.gameObject.SetActive(false);
        _linePool.Enqueue(inLine);
    }

    public void ConnectCastle(Castle inFrom, Castle inTo)
    {
        RoadInfo key = RoadInfo.Create(inFrom, inTo);
        if (IsConnected(key)) return;

        LineRenderer line = GetLine(EColor.Blue, inFrom.transform.position, inTo.transform.position);
        _currentRoads.Add(key, line);
    }

    public void DisconnectCastle(Castle inFrom, Castle inTo)
    {
        RoadInfo key = RoadInfo.Create(inFrom, inTo);
        if (!IsConnected(key)) return;
        
        LineRenderer line = _currentRoads[key];
        if (line == null) return;

        ReturnLine(line);
        _currentRoads.Remove(key);
    }

    public bool IsConnected(Castle inFrom, Castle inTo)
    {
        RoadInfo key = RoadInfo.Create(inFrom, inTo);
        return IsConnected(key);
    }

    public bool IsConnected(RoadInfo inKey)
    {
        return _currentRoads.ContainsKey(inKey);
    }
}
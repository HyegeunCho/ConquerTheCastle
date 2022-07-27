using System;
using System.Collections;
using System.Collections.Generic;
using HGPlugins.Singleton;
using UnityEngine;
using UnityEngine.Rendering;

public class RoadManager : MonoSingleton<RoadManager>
{
    private Material[] _roadMaterials;

    private Queue<LineRenderer> _linePool;

    private Dictionary<RoadInfo, LineRenderer> _currentRoads;

    public override void Awake()
    {
        base.Awake();
        _linePool = new Queue<LineRenderer>();
        _currentRoads = new Dictionary<RoadInfo, LineRenderer>();
        _roadMaterials = new Material[5];
    }

    private Material GetMaterial(Enums.EColor inColor)
    {
        if (_roadMaterials[(int)inColor] == null)
        {
            _roadMaterials[(int)inColor] = Resources.Load<Material>($"Road/Road_{inColor}");
        }
        return _roadMaterials[(int) inColor];
    }

    private LineRenderer CreateLine(Enums.EColor inColor, Vector3 inFrom, Vector3 inTo)
    {
        GameObject go = new GameObject();
        go.transform.SetParent(transform);

        LineRenderer result = go.AddComponent<LineRenderer>();

        UpdateLine(ref result, inColor, inFrom, inTo);

        return result;
    }

    private void UpdateLine(ref LineRenderer refLine, Enums.EColor inColor, Vector3 inFrom, Vector3 inTo)
    {
        refLine.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        
        refLine.alignment = LineAlignment.TransformZ;
        refLine.textureMode = LineTextureMode.Tile;

        refLine.shadowCastingMode = ShadowCastingMode.Off;
        
        refLine.material = GetMaterial(inColor);
        refLine.startWidth = 1f;
        refLine.endWidth = 1f;
        
        inFrom.Set(inFrom.x, 0.1f, inFrom.z);
        inTo.Set(inTo.x, 0.1f, inTo.z);
        
        refLine.SetPositions(new Vector3[] {inFrom, inTo});
    }

    private LineRenderer GetLine(Enums.EColor inColor, Vector3 inFrom, Vector3 inTo)
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

        LineRenderer line = GetLine(Enums.EColor.Blue, inFrom.transform.position, inTo.transform.position);
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    
    private static Material[] _materials = new Material[5];
    

    private static Material GetMaterial(Enums.EColor inColor)
    {
        if (_materials[(int)inColor] == null)
        {
            _materials[(int)inColor] = Resources.Load<Material>($"Soldier/Materials/Soldier_{inColor}");
        }
        return _materials[(int) inColor];
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetColor(Enums.EColor inColor)
    {
        _renderer.material = GetMaterial(inColor);
    }
}

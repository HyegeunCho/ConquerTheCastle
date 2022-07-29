using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
    private static Material[] _materials;
    
    public static Material GetMaterial(Enums.EColor inColor)
    {
        if (_materials == null) _materials = new Material[5];
        if (_materials[(int)inColor] == null)
        {
            _materials[(int)inColor] = Resources.Load<Material>($"Common/Materials/Color_{inColor}");
        }
        return _materials[(int) inColor];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [Range(1, 10)] public float Height;

    public float CurrentHeight
    {
        get { return transform.localScale.y; }
        set
        {
            transform.localScale.Set(1, value, 1);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    
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

using System;
using System.Collections;
using System.Collections.Generic;
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

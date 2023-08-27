using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BSplinePoint
{
    public Vector3 point;
}

public class BSpline : MonoBehaviour
{
    public List<BSplinePoint> bezierPoints;
}

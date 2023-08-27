using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BezierPoint
{
    public Vector3 point;
    public Vector3 tangent;
}

public class Bezier : MonoBehaviour
{
    public List<BezierPoint> bezierPoints;
}

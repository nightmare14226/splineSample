using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HermitePoint
{
    public Vector3 point;
    public Vector3 tangent;
}

public class Hermite : MonoBehaviour
{
    public List<HermitePoint> bezierPoints;
}

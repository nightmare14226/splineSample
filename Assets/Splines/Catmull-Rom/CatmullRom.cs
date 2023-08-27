using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CatmullRomPoint
{
    public Vector3 point;
}

public class CatmullRom : MonoBehaviour
{
    public List<CatmullRomPoint> bezierPoints;
}

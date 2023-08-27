using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BSpline))]
public class BSplineEditor : Editor
{
    Matrix4x4 matrix;
    private static Matrix4x4 CreateMatrix()
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetRow(0, new Vector4(1, 4, 1, 0) / 6f);
        matrix.SetRow(1, new Vector4(-3, 0, 3, 0) / 6f);
        matrix.SetRow(2, new Vector4(3, -6, 3, 0) / 6f);
        matrix.SetRow(3, new Vector4(-1, 3, -3, 1) / 6f);
        return matrix;
    }

    int selectedIndex = 0;
    void OnSceneGUI()
    {
        matrix = CreateMatrix();
        BSpline bezier = target as BSpline;
        if (bezier.bezierPoints == null)
        {
            bezier.bezierPoints = new List<BSplinePoint>();
        }

        if(bezier.bezierPoints.Count == 0)
        {
            bezier.bezierPoints.Add(new BSplinePoint(){
                point = Vector3.zero, 
            });
        }

        for(int i = 0; i < bezier.bezierPoints.Count; i++)
        {
            BSplinePoint? p0 = null;
            if(i > 0)
            {
                p0 = bezier.bezierPoints[i - 1];
            }

            BSplinePoint? p1 = null;
            if(i < bezier.bezierPoints.Count - 1)
            {
                p1 = bezier.bezierPoints[i + 1];
            }

            BSplinePoint? p2 = null;
            if(i < bezier.bezierPoints.Count - 2)
            {
                p2 = bezier.bezierPoints[i + 2];
            }

            var p = bezier.bezierPoints[i];
            if(DrawBezierPoint(ref p, p0, p1, p2, i == selectedIndex))
            {
                selectedIndex = i;
                bezier.bezierPoints[i] = p;
            }
        }

        // scene ui
        Handles.BeginGUI();
        if (GUILayout.Button("添加节点", GUILayout.Width(100)))
        {
            var isLastPoint = selectedIndex == bezier.bezierPoints.Count - 1;
            var selectPoint = bezier.bezierPoints[selectedIndex];
            var nextP = new BSplinePoint(){
                point = selectPoint.point + Vector3.right,
            };
            if(!isLastPoint)
            {
                var nextPoint = bezier.bezierPoints[selectedIndex + 1];
                nextP = new BSplinePoint(){ 
                    point = (nextPoint.point + selectPoint.point) / 2f,
                };
            }

            if(isLastPoint)
            {
                bezier.bezierPoints.Add(nextP);
            }
            else
            {
                bezier.bezierPoints.Insert(selectedIndex + 1, nextP);
            }
        }
        if (GUILayout.Button("删除", GUILayout.Width(100)))
        {
            if(bezier.bezierPoints.Count > 0)
            {
                bezier.bezierPoints.RemoveAt(selectedIndex);
                selectedIndex = 0;
            }
            
        }
        Handles.EndGUI();
    }

    private bool DrawBezierPoint(ref BSplinePoint bezierPoint, BSplinePoint? p0, BSplinePoint? p2, BSplinePoint? p3, bool seletedPoint)
    {
        EditorGUI.BeginChangeCheck();

        Handles.color = seletedPoint ? Color.yellow : Color.white;
        bezierPoint.point = Handles.FreeMoveHandle(bezierPoint.point, Quaternion.identity, 0.1f, Vector3.zero, Handles.CubeHandleCap);

        if(p0 == null && p2 != null)
        {
            Handles.DrawLine(bezierPoint.point, p2.Value.point);
        }
        else if(p0 != null && p2 == null)
        {
            Handles.DrawLine(bezierPoint.point, p0.Value.point);
        }

        if(p0 != null && p2 != null && p3 != null)
        {
            Handles.color = Color.red;
            var points = GetPointsInSpline(matrix, p0.Value.point, bezierPoint.point, p2.Value.point, p3.Value.point, 20);
            Handles.DrawPolyLine(points);

            Handles.color = Color.green;
            Handles.DrawLine(bezierPoint.point, points[0]);
            Handles.DrawLine(p2.Value.point, points.Last());
        }

        return EditorGUI.EndChangeCheck();
    }

    public static Vector3[] GetPointsInSpline(Matrix4x4 splineMatrix, Vector3 p0, Vector3 p0Tangent, Vector3 p1Tangent, Vector3 p1, int n)
    {
        var points = new Vector3[n];
        for (var i = 0; i < n; i++)
        {
            var t = (float)i / (n - 1);
            points[i] = CalLerpPoint(splineMatrix, t, p0, p0Tangent, p1Tangent, p1);
        }
        return points;
    }

    public static Vector3 CalLerpPoint(Matrix4x4 matrix,  float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {   
        Vector4 tVector = new Vector4(1, t, t * t, t * t * t);
        Vector4 result = new Vector4(
            Vector4.Dot(matrix.GetColumn(0), tVector),
            Vector4.Dot(matrix.GetColumn(1), tVector),
            Vector4.Dot(matrix.GetColumn(2), tVector),
            Vector4.Dot(matrix.GetColumn(3), tVector));

            // if(t == 1)
            // {
            //     Debug.LogError(result);
            //     Debug.LogError(matrix.GetColumn(3));
            //     Debug.LogError(matrix.GetRow(0));
            //     Debug.LogError(matrix.GetRow(1));
            //     Debug.LogError(matrix.GetRow(2));
            //     Debug.LogError(matrix.GetRow(3));
            // }
        return result.x * p0 + result.y * p1 + result.z * p2 + result.w * p3;
        
    }
}

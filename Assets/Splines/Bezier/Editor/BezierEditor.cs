using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bezier))]
public class BezierEditor : Editor
{
    Matrix4x4 matrix;
    private static Matrix4x4 CreateMatrix()
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetRow(0, new Vector4(1, 0, 0, 0));
        matrix.SetRow(1, new Vector4(-3, 3, 0, 0));
        matrix.SetRow(2, new Vector4(3, -6, 3, 0));
        matrix.SetRow(3, new Vector4(-1, 3, -3, 1));
        return matrix;
    }

    int selectedIndex = 0;
    void OnSceneGUI()
    {
        matrix = CreateMatrix();
        Bezier bezier = target as Bezier;
        if (bezier.bezierPoints == null)
        {
            bezier.bezierPoints = new List<BezierPoint>();
        }

        if(bezier.bezierPoints.Count == 0)
        {
            bezier.bezierPoints.Add(new BezierPoint(){
                point = Vector3.zero, 
                tangent = Vector3.one,
            });
        }

        for(int i = 0; i < bezier.bezierPoints.Count; i++)
        {
            BezierPoint? nextPoint = null;
            if(i < bezier.bezierPoints.Count - 1)
            {
                nextPoint = bezier.bezierPoints[i + 1];
            }

            var p = bezier.bezierPoints[i];
            if(DrawBezierPoint(ref p, nextPoint, i == selectedIndex))
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
            var nextP = new BezierPoint(){
                point = selectPoint.point + Vector3.right,
                tangent = selectPoint.tangent + Vector3.right
            };
            if(!isLastPoint)
            {
                var nextPoint = bezier.bezierPoints[selectedIndex + 1];
                nextP = new BezierPoint(){ 
                    point = (nextPoint.point + selectPoint.point) / 2f,
                    tangent = (nextPoint.tangent + selectPoint.tangent) / 2f
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

    private bool DrawBezierPoint(ref BezierPoint bezierPoint, BezierPoint? nextPoint, bool seletedPoint)
    {
        EditorGUI.BeginChangeCheck();

        Handles.color = seletedPoint ? Color.yellow : Color.white;
        bezierPoint.point = Handles.FreeMoveHandle(bezierPoint.point, Quaternion.identity, 0.1f, Vector3.zero, Handles.CubeHandleCap);

        Handles.color = Color.green;
        bezierPoint.tangent = Handles.FreeMoveHandle(bezierPoint.tangent, Quaternion.identity, 0.1f, Vector3.zero, Handles.CubeHandleCap);

        Handles.DrawLine(bezierPoint.point, bezierPoint.tangent);

        if(nextPoint != null)
        {
            Handles.color = Color.red;
            var nextTangent = nextPoint.Value.point - (nextPoint.Value.tangent - nextPoint.Value.point);
            var points = GetPointsInSpline(matrix, bezierPoint.point, bezierPoint.tangent, nextTangent, nextPoint.Value.point, 20);
            Handles.DrawPolyLine(points);
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
        return result.x * p0 + result.y * p1 + result.z * p2 + result.w * p3;
        
    }
}

using UnityEngine;
using UnityEditor;

// Generates 20 points that define the bezier curve

[CustomEditor(typeof(DrawBezier))]
public class DrawBezierExample1 : Editor
{
    private Vector3[] points;

    private void OnSceneGUI()
    {
        points = Handles.MakeBezierPoints(
            new Vector3(1.0f,  0.0f,   0.0f),
            new Vector3(-1.0f,  0.0f,   0.0f),
            new Vector3(-1.0f,  0.75f,  0.75f),
            new Vector3(1.0f, -0.75f, -0.75f),
            20);

        Handles.DrawAAPolyLine(points);
    }
}
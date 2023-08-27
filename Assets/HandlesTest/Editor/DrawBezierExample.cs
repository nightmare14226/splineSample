using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierExample))]
public class DrawBezierExample : Editor
{
    public void OnSceneGUI()
    // private void OnSceneViewGUI(SceneView sv)
    {
        BezierExample be = target as BezierExample;

        be.startPoint = Handles.PositionHandle(be.startPoint, Quaternion.identity);
        be.endPoint = Handles.PositionHandle(be.endPoint, Quaternion.identity);
        be.startTangent = Handles.PositionHandle(be.startTangent, Quaternion.identity);
        be.endTangent = Handles.PositionHandle(be.endTangent, Quaternion.identity);

        Handles.DrawBezier(be.startPoint, be.endPoint, be.startTangent, be.endTangent, Color.red, null, 2f);

        Handles.DrawLine(be.startPoint, be.startTangent);
        Handles.DrawLine(be.endPoint, be.endTangent);
    }

    // void OnEnable()
    // {
    //     Debug.Log("OnEnable");
    //     SceneView.onSceneGUIDelegate += OnSceneViewGUI;
    // }

    // void OnDisable()
    // {
    //     Debug.Log("OnDisable");
    //     SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
    // }
}
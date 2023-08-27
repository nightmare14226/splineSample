using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FreeMoveHandleExample)), CanEditMultipleObjects]
public class FreeMoveHandleExampleEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        FreeMoveHandleExample example = (FreeMoveHandleExample)target;

        float size = HandleUtility.GetHandleSize(example.targetPosition) * 0.5f;
        Vector3 snap = Vector3.one * 0.5f;

        EditorGUI.BeginChangeCheck();
        Vector3 newTargetPosition = example.transform.position - new Vector3(1,1,1);
        Quaternion q = example.transform.rotation;
        var s = example.transform.localScale;
        Handles.TransformHandle(ref newTargetPosition, ref q, ref s);
        example.transform.rotation = q;
        example.transform.position = newTargetPosition + new Vector3(1, 1, 1);
        example.transform.localScale = s;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change Look At Target Position");
            example.targetPosition = newTargetPosition;
            example.Update();
        }
    }
}
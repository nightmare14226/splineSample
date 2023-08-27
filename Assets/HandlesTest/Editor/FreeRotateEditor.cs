// Name this script "FreeRotateEditor"
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FreeRotate))]
[CanEditMultipleObjects]
public class FreeRotateEditor : Editor
{
    public void OnSceneGUI()
    {
        FreeRotate t = (target as FreeRotate);

        EditorGUI.BeginChangeCheck();
        Quaternion rot = Handles.FreeRotateHandle(t.rot, Vector3.zero, 2);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Free Rotate");
            t.rot = rot;
            t.Update();
        }
    }
}
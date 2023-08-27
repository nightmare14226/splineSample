// Name this script "DiscHandleEditor"
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DiscHandle))]
[CanEditMultipleObjects]
public class DiscHandleEditor : Editor
{
    public void OnSceneGUI()
    {
        DiscHandle t = (target as DiscHandle);

        EditorGUI.BeginChangeCheck();
        Quaternion rot = Handles.Disc(t.rot, t.transform.position, new Vector3(1, 1, 0), 5, false, 1);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Disc Rotate");
            t.rot = rot;
            t.Update();
        }
    }
}
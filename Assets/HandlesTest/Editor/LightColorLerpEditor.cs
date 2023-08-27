using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightColorLerp)), CanEditMultipleObjects]
public class LightColorLerpEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        LightColorLerp colorLerp = (LightColorLerp)target;

        float size = HandleUtility.GetHandleSize(colorLerp.transform.position) * 5f;
        float snap = 0.1f;

        EditorGUI.BeginChangeCheck();
        float newAmount = Handles.ScaleValueHandle(colorLerp.amount, colorLerp.transform.position, Quaternion.identity, size, Handles.ArrowHandleCap, snap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(colorLerp, "Change Light Color Interpolation");
            colorLerp.amount = newAmount;
            colorLerp.Update();
        }
    }
}
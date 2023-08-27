using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonExample)), CanEditMultipleObjects]
class ButtonExampleEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        ButtonExample buttonExample = (ButtonExample)target;

        Vector3 position = buttonExample.transform.position + Vector3.up * 2f;
        float size = 2f;
        float pickSize = size * 2f;

        if (Handles.Button(position, Quaternion.identity, size, pickSize, Handles.DotHandleCap))
            Debug.Log("The button was pressed!");
    }
}
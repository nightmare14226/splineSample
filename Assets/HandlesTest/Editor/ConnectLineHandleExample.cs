// Draw lines to the connected game objects that a script has.
// If the target object doesn't have any game objects attached
// then it draws a line from the object to (0, 0, 0).

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConnectedObjectsExample))]
class ConnectLineHandleExample : Editor
{
    float dashSize = 4.0f;
    void OnSceneGUI()
    {
        ConnectedObjectsExample connectedObjects = target as ConnectedObjectsExample;
        if (connectedObjects.objs == null)
            return;

        Vector3 center = connectedObjects.transform.position;
        for (int i = 0; i < connectedObjects.objs.Length; i++)
        {
            GameObject connectedObject = connectedObjects.objs[i];
            if (connectedObject)
            {
                Handles.DrawDottedLine(center, connectedObject.transform.position, dashSize);
            }
            else
            {
                Handles.DrawDottedLine(center, Vector3.zero, dashSize);
            }
        }
    }
}
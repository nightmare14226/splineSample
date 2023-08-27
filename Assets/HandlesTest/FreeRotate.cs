// Name this script "FreeRotate"
using UnityEngine;

[ExecuteInEditMode]
public class FreeRotate : MonoBehaviour
{
    public Quaternion rot = Quaternion.identity;
    public void Update()
    {
        transform.rotation = rot;
    }
}
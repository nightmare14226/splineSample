// Name this script "DiscHandle"
using UnityEngine;
[ExecuteInEditMode]
public class DiscHandle : MonoBehaviour
{
    public Quaternion rot = Quaternion.identity;
    public void Update()
    {
        transform.rotation = rot;
    }
}
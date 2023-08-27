using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Light))]
public class LightColorLerp : MonoBehaviour
{
    [SerializeField]
    private Color m_Color1 = Color.red;
    [SerializeField]
    private Color m_Color2 = Color.green;

    public float amount { get { return m_Amount; } set { m_Amount = Mathf.Clamp01(value); } }
    [SerializeField, Range(0f, 1f)]
    private float m_Amount = 1f;

    private Light m_Light;

    protected virtual void OnEnable()
    {
        m_Light = GetComponent<Light>();
    }

    public virtual void Update()
    {
        m_Light.color = Color.Lerp(m_Color1, m_Color2, m_Amount);
    }
}
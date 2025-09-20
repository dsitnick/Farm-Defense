using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public abstract void SetActive(bool active);
    public abstract void UpdateTool();
}

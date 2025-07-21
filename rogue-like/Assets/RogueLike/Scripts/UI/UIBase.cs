using UnityEngine;

public abstract class UIBase<T> : MonoBehaviour where T : class
{
    [Header("Common")]
    [SerializeField] UIType _type;
}

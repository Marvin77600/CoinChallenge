using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    public static T Instance;

    protected virtual void Awake()
    {
        Instance = (T)this;
    }
}
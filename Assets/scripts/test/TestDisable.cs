using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
public class TestDisable : MonoBehaviour
{
    void OnEnable()
    {
        Debug.LogError("Enable");
    }

    void OnDisable()
    {
        Debug.LogError("Disable");
    }
}

#endif
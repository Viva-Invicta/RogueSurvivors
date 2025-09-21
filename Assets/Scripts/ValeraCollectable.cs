using System;
using UnityEngine;

public class ValeraCollectable : MonoBehaviour
{
    public event Action Dead;

    public void Kill()
    {
        Dead?.Invoke();
        Destroy(gameObject);
    }
}

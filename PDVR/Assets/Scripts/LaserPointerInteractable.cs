using UnityEngine;
using UnityEngine.Events;

public class LaserPointerInteractable : MonoBehaviour
{
    [SerializeField] private UnityEvent _activated;

    public void TriggerInteractable() => _activated?.Invoke();
}
using System;
using UnityEngine;
using UnityEngine.Events;

public class NodeButton : Node
{
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private UnityEvent _onReleased;

    public event Action OnPressed;
    public event Action OnReleased;

    public bool IsPressed;

    public void Press()
    {
        IsPressed = true;
        Occupied = true;
        OnPressed?.Invoke();
        _onPressed?.Invoke();
    }

    public void Release()
    {
        IsPressed = false;
        Occupied = false;
        OnReleased?.Invoke();
        _onReleased?.Invoke();
    }
}
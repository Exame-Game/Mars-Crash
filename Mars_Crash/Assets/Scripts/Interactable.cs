using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] internal InteractableStates _currentState;
    [SerializeField] internal InteractableStates _previousState;
    [SerializeField] internal InteractableStates _nextState;

    [SerializeField] internal InteractableStates _defaultState = InteractableStates.Idle;

    protected virtual void Start()
    {
        _currentState = _defaultState;
    }

    protected internal virtual void Transition(InteractableStates nextState)
    {
        if (nextState == _currentState) 
            return;

        OnExitState(_currentState);
        _previousState = _currentState;
        _nextState = nextState;
        _currentState = nextState;
        OnEnterState(nextState);
    }

    public virtual void StateUpdate()
    {
        OnUpdateState(_currentState);
    }

    public virtual void OnPointerDown(Vector3 worldHitPoint) { }
    public virtual void OnPointerDrag(Ray pointerRay) { }
    public virtual void OnPointerReleased() { }
    public virtual void OnPointerUnHover() { }
    public virtual void OnPointerHover() { }

    // Override these in subclasses as needed
    protected virtual void OnEnterState(InteractableStates state) { }
    protected virtual void OnExitState(InteractableStates state) { }
    protected virtual void OnUpdateState(InteractableStates state) { }
}

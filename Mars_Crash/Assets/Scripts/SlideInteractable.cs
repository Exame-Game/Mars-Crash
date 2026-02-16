using DG.Tweening;
using UnityEngine;

public class SlideInteractable : Interactable
{
    [SerializeField] private Vector3 _movementAxis = Vector3.right;
    [SerializeField] private float _slideSpeed = 4f;
    [SerializeField] private DOTweenAnimation _animation;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _hasTarget;

    protected override void Start()
    {
        base.Start();
        _startPosition = transform.position;
    }

    public override void OnPointerDown(Vector3 worldHitPoint)
    {
        _startPosition = transform.position;
        _hasTarget = false;
        if (_animation == null)
            return;

        _animation.DORestart(true);
    }

    public override void OnPointerDrag(Ray pointerRay)
    {
        Plane plane = new Plane(Camera.main.transform.forward * -1f, transform.position);
        if (plane.Raycast(pointerRay, out float enter))
        {
            Vector3 worldPoint = pointerRay.GetPoint(enter);
            Vector3 delta = worldPoint - _startPosition;

            Vector3 worldAxis = transform.TransformDirection(_movementAxis.normalized);
            Vector3 projected = worldAxis * Vector3.Dot(delta, worldAxis);

            _targetPosition = _startPosition + projected;
            _hasTarget = true;
            Transition(InteractableStates.Moving);
        }
    }

    public override void OnPointerReleased()
    {
        if (_hasTarget)
        {
            // Snap to target on release
            transform.position = _targetPosition;
            _hasTarget = false;
        }

        if (_animation == null)
            return;

        _animation.DORestart(true);

        Transition(InteractableStates.Idle);
    }

    protected override void OnEnterState(InteractableStates state)
    {
        if (state == InteractableStates.Moving)
        {
            // Visual feedback
        }
    }

    protected override void OnUpdateState(InteractableStates state)
    {
        if (state == InteractableStates.Moving && _hasTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _slideSpeed * Time.deltaTime);
            if (Vector3.SqrMagnitude(transform.position - _targetPosition) < 0.0001f)
            {
                _hasTarget = false;
                Transition(InteractableStates.Idle);
            }
        }
    }
}

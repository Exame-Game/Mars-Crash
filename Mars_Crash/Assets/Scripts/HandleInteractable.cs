using DG.Tweening;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class HandleInteractable : Interactable
{
    [Header("Rotation Settings")]
    [SerializeField] private Transform _rotationAnchor;
    [SerializeField] private Vector3 _rotationAxis = Vector3.up;
    [SerializeField] private bool _snapToAngles = true;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _snapAngleIncrement = 90f;

    [Header("Optional Animation")]
    [SerializeField] private DOTweenAnimation _grabAnimation;
    [SerializeField] private DOTweenAnimation _lockAnimation;


    public bool _isLocked;

    private Vector3 _currentPointerPosition;

    private bool _isDragging;
    private bool _hasValidPointer;
    private bool _isFirstDragFrame;

    private float _currentRotation;
    private float _targetRotation;
    private float _initialDragAngle;

    [Header("Debug")]
    [SerializeField] private bool _debugDrawPlane = true;
    [SerializeField] private Color _debugPlaneColor = new Color(0f, 1f, 1f, 0.25f);
    [SerializeField] private int _debugPlaneSegments = 36;

    protected override void Start()
    {
        base.Start();

        if (_rotationAnchor == null)
        {
            Debug.LogWarning($"No rotation anchor set for {gameObject.name}. Using self as anchor.");
            _rotationAnchor = transform;
        }

        _currentRotation = 0f;
    }

    public override void OnPointerDown(Vector3 worldHitPoint)
    {
        if (_isLocked)
            return;

        _isDragging = true;
        _hasValidPointer = false;
        _isFirstDragFrame = true;

        if (_grabAnimation != null)
            _grabAnimation.DOPause();

        Transition(InteractableStates.Selected);
    }

    public override void OnPointerDrag(Ray pointerRay)
    {
        if (!_isDragging)
            return;

        Vector3 worldAxis = _rotationAxis.normalized;
        Plane rotationPlane = new Plane(worldAxis, _rotationAnchor.position);

        if (rotationPlane.Raycast(pointerRay, out float enter))
        {
            _currentPointerPosition = pointerRay.GetPoint(enter);
            _hasValidPointer = true;

            Transition(InteractableStates.Moving);
        }
    }

    public override void OnPointerReleased()
    {
        _isDragging = false;
        _hasValidPointer = false;

        if (_snapToAngles)
        {
            Debug.Log($"Snapping from {_currentRotation} to nearest increment of {_snapAngleIncrement}");
            _targetRotation = Mathf.Round(_currentRotation / _snapAngleIncrement) * _snapAngleIncrement;
        }
        else
            _targetRotation = _currentRotation;

        if (_grabAnimation != null)
            _grabAnimation.DORestart(true);

        Transition(InteractableStates.Idle);
    }

    protected override void OnEnterState(InteractableStates state)
    {
        if (state == InteractableStates.Moving)
        {
            // Add visual feedback here
        }
    }

    protected override void OnUpdateState(InteractableStates state)
    {
        if (state == InteractableStates.Moving && _hasValidPointer)
        {
            Vector3 worldAxis = _rotationAxis.normalized;
            Vector3 fromAnchorToPointer = _currentPointerPosition - _rotationAnchor.position;
            fromAnchorToPointer = Vector3.ProjectOnPlane(fromAnchorToPointer, worldAxis);

            if (fromAnchorToPointer.sqrMagnitude > 0.0001f)
            {
                Vector3 referenceDirection = Vector3.ProjectOnPlane(transform.forward, worldAxis);
                if (referenceDirection.sqrMagnitude < 0.0001f)
                    referenceDirection = Vector3.ProjectOnPlane(Vector3.forward, worldAxis);

                float currentPointerAngle = Vector3.SignedAngle(referenceDirection, fromAnchorToPointer, worldAxis);

                if (_isFirstDragFrame)
                {
                    _initialDragAngle = currentPointerAngle - _currentRotation;
                    _isFirstDragFrame = false;
                }

                float targetAngle = currentPointerAngle - _initialDragAngle;

                float rotationDelta = _rotationSpeed * Time.deltaTime;
                _currentRotation = Mathf.MoveTowardsAngle(_currentRotation, targetAngle, rotationDelta);

                ApplyRotation();
            }
        }
        else if (state == InteractableStates.Idle && _snapToAngles)
        {
            Debug.Log($"Snapping from {_currentRotation} to {_targetRotation}");
            if (Mathf.Abs(Mathf.DeltaAngle(_currentRotation, _targetRotation)) > 0.01f)
            {
                Debug.Log($"Current: {_currentRotation}, Target: {_targetRotation}");
                float rotationDelta = _rotationSpeed * Time.deltaTime;
                _currentRotation = Mathf.MoveTowardsAngle(_currentRotation, _targetRotation, rotationDelta);
                ApplyRotation();
            }
        }
    }

    public void LockHandle(bool locked)
    {
        _isLocked = locked;
        if (_lockAnimation != null)
            if (locked)
                _lockAnimation.DORestart(true);
            else
                _lockAnimation.DOPlayBackwards();
    }

    private void ApplyRotation()
    {
        Vector3 worldAxis = _rotationAxis.normalized;

        Vector3 initialDirection = transform.position - _rotationAnchor.position;
        float distance = initialDirection.magnitude;

        Quaternion rotation = Quaternion.AngleAxis(_currentRotation, worldAxis);

        Vector3 baseDirection = Vector3.ProjectOnPlane(initialDirection, worldAxis).normalized;
        if (baseDirection.sqrMagnitude < 0.0001f)
            baseDirection = Vector3.ProjectOnPlane(Vector3.forward, worldAxis).normalized;

        Vector3 rotatedDirection = rotation * baseDirection;
        Vector3 newPosition = _rotationAnchor.position + rotatedDirection * distance;

        transform.position = newPosition;

        transform.rotation = Quaternion.AngleAxis(_currentRotation, worldAxis) * Quaternion.identity;
    }

    private void OnDrawGizmos()
    {
        if (!_debugDrawPlane) 
            return;

        Vector3 worldAxis = _rotationAxis.normalized;
        Transform anchor = _rotationAnchor != null ? _rotationAnchor : transform;
        Vector3 center = anchor.position;

        // Find the furthest child from the anchor projected onto the rotation plane
        float radius = 0f;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child == anchor) 
                continue;

            Vector3 toChild = child.position - center;
            float dist = Vector3.ProjectOnPlane(toChild, worldAxis).magnitude;
            if (dist > radius)
                radius = dist;
        }

        if (radius < 0.0001f) 
            radius = 0.5f; // fallback

        int segments = Mathf.Max(3, _debugPlaneSegments);

        Vector3 u = Vector3.ProjectOnPlane(transform.forward, worldAxis);
        if (u.sqrMagnitude < 0.0001f) 
            u = Vector3.ProjectOnPlane(Vector3.forward, worldAxis);

        u.Normalize();
        Vector3 v = Vector3.Cross(worldAxis, u).normalized;

        Color prevColor = Gizmos.color;

#if UNITY_EDITOR
        Color handleColor = _debugPlaneColor;
        handleColor.a = Mathf.Clamp01(_debugPlaneColor.a);
        Handles.color = handleColor;
        Handles.DrawSolidDisc(center, worldAxis, radius);
#endif

        Gizmos.color = new Color(_debugPlaneColor.r, _debugPlaneColor.g, _debugPlaneColor.b, 1f);
        Vector3 prevPoint = center + u * radius;
        for (int i = 1; i <= segments; i++)
        {
            float angle = (i * Mathf.PI * 2f) / segments;
            Vector3 point = center + (u * Mathf.Cos(angle) + v * Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }

        Gizmos.DrawLine(center - u * radius, center + u * radius);
        Gizmos.DrawLine(center - v * radius, center + v * radius);
        Gizmos.DrawLine(center, center + worldAxis * (radius * 0.5f));

        Gizmos.color = prevColor;
    }
}

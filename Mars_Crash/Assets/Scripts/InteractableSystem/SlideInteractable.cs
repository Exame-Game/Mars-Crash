using DG.Tweening;
using UnityEngine;

public class SlideInteractable : Interactable
{
    [Header("Slide Settings")]
    [SerializeField] private DOTweenAnimation _animation;
    [SerializeField] private Vector3 _movementAxis = Vector3.right;
    [SerializeField] private float _slideSpeed = 4f;

    [Header("Visuals")]
    [SerializeField] private MaterialAssigner _materialAssigner;
    [SerializeField] private Material draggingMaterial;
    [SerializeField] private Material idleMaterial;

    [Header("Constraints")]
    public GameObject[] constraintPoints;
    public bool changeColorWhenDragged;

    private Vector3[] constraints;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    private bool _hasTarget;
    private bool _isDragging;

    protected override void Start()
    {
        base.Start();

        _startPosition = transform.position;
        _materialAssigner.ApplyMaterial(idleMaterial);

        SetConstraints();
    }

    public override void OnPointerDown(Vector3 worldHitPoint)
    {
        _isDragging = true;
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

            float distA = Vector3.Dot(constraints[0] - _startPosition, worldAxis);
            float distB = Vector3.Dot(constraints[1] - _startPosition, worldAxis);
            float currentDist = Vector3.Dot(_targetPosition - _startPosition, worldAxis);

            float clamped = Mathf.Clamp(currentDist, Mathf.Min(distA, distB), Mathf.Max(distA, distB));
            _targetPosition = _startPosition + worldAxis * clamped;

            _hasTarget = true;
            Transition(InteractableStates.Moving);
        }
    }

    public override void OnPointerReleased()
    {
        _isDragging = false;
        if (_hasTarget)
        {
            transform.position = _targetPosition;
            _hasTarget = false;
        }

        if (_animation != null)
            _animation.DORestart(true);

        Transition(InteractableStates.Idle);
    }

    protected override void OnEnterState(InteractableStates state)
    {
        if (state == InteractableStates.Moving)
        {
            if (changeColorWhenDragged)
                _materialAssigner.ApplyMaterial(draggingMaterial);
        }

        if (state == InteractableStates.Idle)
        {
            if (changeColorWhenDragged)
                _materialAssigner.ApplyMaterial(idleMaterial);
        }
    }

    protected override void OnUpdateState(InteractableStates state)
    {
        if (state == InteractableStates.Moving && _hasTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _slideSpeed * Time.deltaTime);
            if (!_isDragging && Vector3.SqrMagnitude(transform.position - _targetPosition) < 0.0001f)
            {
                _hasTarget = false;
                Transition(InteractableStates.Idle);
            }
        }
        else if (state == InteractableStates.Idle && !_hasTarget && !_isDragging)
        {
            Vector3 worldAxis = transform.TransformDirection(_movementAxis.normalized);
            float projectedDistance = Vector3.Dot(_targetPosition - _startPosition, worldAxis);
            float snappedDistance = Mathf.Round(projectedDistance);

            _targetPosition = _startPosition + worldAxis * snappedDistance;
            transform.DOMove(_targetPosition, 0.1f).SetEase(Ease.OutExpo);
        }
    }
    
    private void SetConstraints()
    {
        constraints = new Vector3[constraintPoints.Length];

        for (int i = 0; i < constraintPoints.Length; i++)
            constraints[i] = constraintPoints[i].transform.position;
    }
}

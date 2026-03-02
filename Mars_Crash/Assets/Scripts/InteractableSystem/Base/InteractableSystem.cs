using UnityEngine;

public class InteractableSystem : MonoBehaviour
{
    [SerializeField] private LayerMask interactableMask = ~0;

    private Camera _mainCam;
    private Interactable _selected;
    private Interactable _settling;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        HandlePointer();
        _selected?.StateUpdate();

        if (_settling == null || _settling == _selected)
            return;

        _settling.StateUpdate();
        if (_settling._currentState == InteractableStates.Idle)
            _settling = null;
    }

    private void HandlePointer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, interactableMask))
            {
                Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
                if (interactable != null)
                    Select(interactable, hit.point);
            }
            else
                Deselect();
        }

        if (_selected != null && Input.GetMouseButtonUp(0))
        {
            _selected.OnPointerReleased();
            _settling = _selected;
            Deselect();
        }

        if (_selected != null && Input.GetMouseButton(0))
        {
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            _selected.OnPointerDrag(ray);
        }
    }

    private void Select(Interactable interactable, Vector3 hitPoint)
    {
        if (_selected == interactable) 
            return;

        Deselect(); // Deselect before selecting again
        _selected = interactable;
        _selected.Transition(InteractableStates.Selected);
        _selected.OnPointerDown(hitPoint);
    }

    private void Deselect()
    {
        if (_selected == null) 
            return;

        _selected.Transition(InteractableStates.Idle);
        _selected = null;
    }
}

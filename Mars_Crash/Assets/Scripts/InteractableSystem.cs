using UnityEngine;

public class InteractableSystem : MonoBehaviour
{
    [SerializeField] private LayerMask interactableMask = ~0;

    private Camera _mainCam;
    private Interactable _selected;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        HandlePointer();
        _selected?.StateUpdate();
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
            Deselect();
        }

        if (_selected != null && Input.GetMouseButton(0))
        {
            // Update drag positioni
            //Ray ray = _mainCam.ScreenPointToRay(Input.GetTouch(0).position);
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

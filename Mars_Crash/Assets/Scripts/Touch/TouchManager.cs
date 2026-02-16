using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _offset = new(0, 0.5f, 0);

    private void Update()
    {
        MovePlayerToTouch();
    }

    private void MovePlayerToTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray rayOrigin = _mainCamera.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(rayOrigin, out hit))
                _player.transform.position = hit.point + _offset;
        }
    }
}


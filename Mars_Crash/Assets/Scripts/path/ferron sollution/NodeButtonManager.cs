using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeButtonManager : MonoBehaviour
{
    [SerializeField] private List<NodeButton> buttons;

    [SerializeField] private float _buttonOffset = 0.605f;
    [SerializeField] private float _pressScale = 0.6f;
    [SerializeField] private float _pressDuration = 0.3f;

    public GameObject buttonPrefab;

    private Dictionary<NodeButton, Transform> _buttonTransforms = new Dictionary<NodeButton, Transform>();

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateButtonPressed();
    }

    private void UpdateButtonPressed()
    {
        foreach (NodeButton button in buttons)
        {
            if (button.Occupied && !button.IsPressed)
                button.Press();
            else if (!button.Occupied && button.IsPressed)
                button.Release();
        }
    }

    public void SpawnButton(GameObject buttonPrefab, Transform targetTransform, Quaternion rotation, NodeButton nodeButton)
    {
        Vector3 spawnPosition = targetTransform.position + (rotation * Vector3.up) * _buttonOffset;
        GameObject spawnedButton = Instantiate(buttonPrefab, spawnPosition, rotation, targetTransform);
        _buttonTransforms[nodeButton] = spawnedButton.transform;
    }

    private void Initialize()
    {
        buttons = GetComponentsInChildren<NodeButton>().ToList();
        foreach (NodeButton button in buttons)
        {
            SpawnButton(buttonPrefab, button.transform, GetNodeRotation(button), button);
            button.OnPressed += () => OnButtonPressed(button);
            button.OnReleased += () => OnButtonReleased(button);
        }
    }

    private Quaternion GetNodeRotation(Node node)
    {
        return node.transform.rotation;
    }

    private void OnButtonPressed(NodeButton button)
    {
        Debug.Log($"Button pressed: {button.name}");
        if (!_buttonTransforms.TryGetValue(button, out Transform t)) return;

        t.DOKill();
        t.DOPunchScale(Vector3.one * -_pressScale, _pressDuration, vibrato: 5, elasticity: 0.5f);
    }

    private void OnButtonReleased(NodeButton button)
    {
        Debug.Log($"Button released: {button.name}");
        if (!_buttonTransforms.TryGetValue(button, out Transform t)) return;

        t.DOKill();
        t.DOScale(Vector3.one, _pressDuration * 0.5f).SetEase(Ease.OutBack);
    }
}
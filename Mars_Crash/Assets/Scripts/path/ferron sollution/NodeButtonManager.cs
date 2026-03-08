using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeButtonManager : MonoBehaviour
{
    [SerializeField] private List<NodeButton> buttons;
    [SerializeField] private float _buttonOffset = 0.395f;
    [SerializeField] private DOTweenAnimation _buttonTween;
    public GameObject buttonPrefab;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        foreach (NodeButton button in buttons)
        {
            Node pathNode = button.GetComponentInParent<Node>(true);
            bool occupied = pathNode != null && pathNode != button && pathNode.Occupied;

            if (occupied && !button.IsPressed)
                button.Press();
            else if (!occupied && button.IsPressed)
                button.Release();
        }
    }

    private void OnDestroy()
    {
        foreach (NodeButton button in buttons)
        {
            button.OnPressed -= () => OnButtonPressed(button);
            button.OnReleased -= () => OnButtonReleased(button);
        }
    }

    public void SpawnButton(GameObject buttonPrefab, Transform targetTransform, Quaternion rotation)
    {
        Vector3 spawnPosition = targetTransform.position + (rotation * Vector3.up) * _buttonOffset;
        GameObject spawnedButton = Instantiate(buttonPrefab, spawnPosition, rotation, transform);

        if (_buttonTween != null)
        {
            DOTweenAnimation anim = spawnedButton.GetComponent<DOTweenAnimation>();
            if (anim != null) anim.DORestart();
        }
    }

    private void Initialize()
    {
        buttons = GetComponentsInChildren<NodeButton>().ToList();
        foreach (NodeButton button in buttons)
        {
            SpawnButton(buttonPrefab, button.transform, GetNodeRotation(button));
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
        button.IsPressed = true;
        Debug.Log($"Button pressed: {button.name}");
        DOTweenAnimation anim = GetButtonObject(button);
        if (anim != null) anim.DOPlay();
    }

    private void OnButtonReleased(NodeButton button)
    {
        button.IsPressed = false;
        DOTweenAnimation anim = GetButtonObject(button);
        if (anim != null) anim.DOPlayBackwards();
    }

    private DOTweenAnimation GetButtonObject(NodeButton button)
    {
        // Find the spawned button near this node's position
        foreach (Transform child in transform)
            if (Vector3.Distance(child.position, button.transform.position) < 1f)
                return child.GetComponent<DOTweenAnimation>();

        return null;
    }
}
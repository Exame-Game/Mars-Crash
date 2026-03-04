using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> ConnectedNodes;
    public List<Node> ConectionBlackList;
    public bool Occupied;
}

public class NodeButton : Node
{
    public event Action OnPressed;
    public event Action OnReleased;
    public bool IsPressed;
}

public class NodeButtonManager : MonoBehaviour
{
    [SerializeField] private List<NodeButton> buttons;
    private GameObject buttonPrefab;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (NodeButton button in buttons)
        {
            button.OnPressed += () => OnButtonPressed(button);
            button.OnReleased += () => OnButtonReleased(button);
        }
    }

    private void OnButtonPressed(NodeButton button)
    {
        button.IsPressed = true;
    }

    private void OnButtonReleased(NodeButton button)
    {
        button.IsPressed = false;
    }
    
    private void SpawnButton(GameObject buttonPrefab)
    {
        Instantiate(buttonPrefab, transform.position, Quaternion.identity, transform);
    }
}

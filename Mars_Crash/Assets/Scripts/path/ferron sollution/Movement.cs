using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    #region Variables

    [SerializeField] private UnityEvent _onSplit;
    [SerializeField] private UnityEvent _onMerge;
    [SerializeField] private UnityEvent _onStartMove;
    [SerializeField] private Animator[] _animator;
    
    [SerializeField] private GameObject _otherPlayer;
    [SerializeField] private int _playerIndex;
    [SerializeField] private float _moveSpeed;
    
    [SerializeField] private Button _mergeButton;
    
    private Camera _camera;

    private Node _switchPathNode;
    
    private Queue<Node> _queue = new Queue<Node>(256);
    private HashSet<Node> _visited = new HashSet<Node>();
    private Dictionary<Node, Node> _cameFrom = new Dictionary<Node, Node>();
    
    public Node CurrentNode;
    private Coroutine _movementRoutine;

    public  bool _inControl;
    public bool _isSplit;
    
    #endregion

    private void OnEnable()
    {
        CurrentNode = transform.parent.GetComponent<Node>();

        CurrentNode.Occupied = true;
        
        _inControl = false;
        
        if (_playerIndex == 1)
            _isSplit = true;
    }

    private void Update()
    {
        PointClickMovement();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private List<Node> FindPath(Node startNode, Node targetNode)
    {
        _onStartMove.Invoke();
        
        if (startNode == null || targetNode == null)
            return null;

        if (startNode == targetNode)
            return null;
        
        if (startNode.transform.up != targetNode.transform.up)
            return null;

        _queue.Clear();
        _visited.Clear();
        _cameFrom.Clear();

        _queue.Enqueue(startNode);
        _visited.Add(startNode);

        while (_queue.Count > 0)
        {
            Node current = _queue.Dequeue();

            foreach (Node neighbor in current.ConnectedNodes)
            {
                if (neighbor == null || _visited.Contains(neighbor))
                    continue;

                _visited.Add(neighbor);
                _cameFrom[neighbor] = current;

                if (neighbor == targetNode)
                    return ReconstructPath(targetNode);

                _queue.Enqueue(neighbor);
            }
        }

        return null; // No path found
    }

    private List<Node> ReconstructPath(Node endNode)
    {
        List<Node> path = new List<Node>();

        Node current = endNode;
        path.Add(current);

        while (_cameFrom.TryGetValue(current, out Node parent))
        {
            current = parent;
            path.Add(current);
        }

        path.Reverse();
        return path;
    }

    private Vector3 Flatten(Vector3 worldPos)
    {
        var camPos = _camera.transform.position;
        var camForward = _camera.transform.forward;
        var toPoint = worldPos - camPos;
        return worldPos - Vector3.Dot(toPoint, camForward) * camForward;
    }

    public void SwitchInControl()
    {
        if(!_isSplit) 
            return;
        
        _inControl = !_inControl;
    }

    public void Split()
    {
        if (_playerIndex != 0 || _isSplit) 
            return;
        
        if (CurrentNode.ConnectedNodes.Count <= 0) 
            return;
        
        _otherPlayer.transform.parent = CurrentNode.ConnectedNodes[0].transform;
        _otherPlayer.transform.localPosition = Vector3.zero;
        _otherPlayer.transform.localRotation = Quaternion.identity;
        _otherPlayer.SetActive(true);
        _onSplit.Invoke();
        _isSplit = true;
        _inControl = false;
        _otherPlayer.GetComponent<Movement>()._inControl = true;
    }

    public void Merge()
    {
        _onMerge.Invoke();
        bool canMerge = false;
        for (int i = 0; i < CurrentNode.ConnectedNodes.Count; i++)
        {
            if (CurrentNode.ConnectedNodes[i].Occupied)
            {
                canMerge = true;
                break;
            }
        }
        if (!canMerge)
            return;
        
        Movement otherPlayerMovement = _otherPlayer.GetComponent<Movement>();
        
        otherPlayerMovement.CurrentNode.Occupied = false;
        otherPlayerMovement.CurrentNode = null;
        
        _otherPlayer.SetActive(false); 
        
        _inControl = true;
        _isSplit = false;
    }

    private void PointClickMovement()
    {
        if (_isSplit && !_inControl)
            return;
        
        Vector3 inputPosition;

        // Desktop
        if (Input.GetMouseButtonDown(0))
            inputPosition = Input.mousePosition;
        
        // Mobile
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            inputPosition = Input.GetTouch(0).position;
        
        else return;

        Ray ray = _camera.ScreenPointToRay(inputPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f)) 
            return;
        
        Node targetNode = hit.collider.GetComponent<Node>();

        if (targetNode == null)
            return;

        if (_movementRoutine != null)
        {
            _switchPathNode = targetNode;
            return;
        }
        
        List<Node> nodes = FindPath(transform.parent.GetComponent<Node>(), targetNode);

        if (nodes != null)
            _movementRoutine = StartCoroutine(MoveAlongPath(nodes));
    }

    private void RecalculatePath(Node targetNode)
    {
        List<Node> nodes = FindPath(transform.parent.GetComponent<Node>(), targetNode);
        
        _switchPathNode = null;
        
        if (nodes != null)
            _movementRoutine = StartCoroutine(MoveAlongPath(nodes));
    }
    
    private IEnumerator MoveAlongPath(List<Node> path)
    {
        Debug.Log("start");
        if (_mergeButton.enabled)
            _mergeButton.interactable = false;
        
        CurrentNode = path[0];
        
        transform.SetParent(CurrentNode.transform);
        transform.position = CurrentNode.transform.position;

        foreach (Animator animator in _animator)
        {
            animator.SetTrigger("walk");
        }

        for (int i = 1; i < path.Count; i++)
        {
            Node nextNode = path[i];
            
            _onStartMove.Invoke();
            if (!CurrentNode.ConnectedNodes.Contains(nextNode) || nextNode.Occupied)
            {
                _movementRoutine = null;
                foreach (Animator animator in _animator)
                {
                    animator.SetTrigger("idle");
                }

                if (nextNode.Occupied)
                    if (_mergeButton.enabled)
                        _mergeButton.interactable = true;
                
                yield break;
            }
            
            Vector3 headingA = CurrentNode.transform.position - _camera.transform.position;
            Vector3 headingB = nextNode.transform.position - _camera.transform.position;
            
            float distanceA = Vector3.Dot(headingA, _camera.transform.forward);
            float distanceB = Vector3.Dot(headingB, _camera.transform.forward);

            Vector3 startOffset = Vector3.zero;
            Vector3 endOffset =  Vector3.zero;
            
            if (distanceA < distanceB)
            {
                Vector3 direction = Flatten(nextNode.transform.position) - Flatten(CurrentNode.transform.position);
                endOffset = (CurrentNode.transform.position + direction) - nextNode.transform.position;
            }
            else
            {
                Vector3 direction = Flatten(CurrentNode.transform.position) - Flatten(nextNode.transform.position);
                startOffset = (nextNode.transform.position + direction) - CurrentNode.transform.position;
            }
            

            Transform startPosition = CurrentNode.transform;
            Transform targetPosition = nextNode.transform;
            
            
            float duration = 1/_moveSpeed;
            float elapsed = 0f;
            
            Vector3 lookDirection = nextNode.transform.position - CurrentNode.transform.position;
            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude > 0f)
                transform.rotation = Quaternion.LookRotation(lookDirection);
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                transform.position = Vector3.Lerp(startPosition.position + startOffset, targetPosition.position + endOffset, t);
                yield return null;
            }

            
            transform.position = targetPosition.position;
            
            CurrentNode.Occupied = false;
            nextNode.Occupied = true;
            
            CurrentNode = nextNode;

            transform.SetParent(CurrentNode.transform);

            if (_switchPathNode != null)
            {
                RecalculatePath(_switchPathNode);
                foreach (Animator animator in _animator)
                {
                    animator.SetTrigger("idle");
                }
                yield break;
            }
        }
        Debug.Log("done");
        foreach (Animator animator in _animator)
        {
            animator.SetTrigger("idle");
        }
        _movementRoutine = null;

        bool nextToOtherPlayer = false;
        
        foreach (Node node in CurrentNode.ConnectedNodes)
        {
            if (node.Occupied)
            {
                nextToOtherPlayer = true;
                break;
            }
        }

        if (nextToOtherPlayer)
        {
            if (_mergeButton.enabled)
                _mergeButton.interactable = true;
        }
    }
}
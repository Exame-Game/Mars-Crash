using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float _overlapTolerance;
    [SerializeField] private float _moveSpeed = 3f;
    
    private Camera _camera;
    
    private Queue<Node> _queue = new Queue<Node>(256);
    private HashSet<Node> _visited = new HashSet<Node>();
    private Dictionary<Node, Node> _cameFrom = new Dictionary<Node, Node>();
    
    private Coroutine _movementRoutine;
    private Node _currentNode;

    #endregion
    
    void Update()
    {
        PointClickMovement();
    }

    private void Start()
    {
        _camera = Camera.main;
        RebuildGraph();
    }

    private List<Node> FindPath(Node startNode, Node targetNode)
    {
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
    
    private Vector3 Positive(Vector3 v)
    {
        return new Vector3(math.abs(v.x), math.abs(v.y), math.abs(v.z));
    }

    private Vector3 Flatten(Vector3 worldPos)
    {
        var camPos = _camera.transform.position;
        var camForward = _camera.transform.forward;
        var toPoint = worldPos - camPos;
        return worldPos - Vector3.Dot(toPoint, camForward) * camForward;
    }
    
    private bool IsWalkable(Node n)
    {
        if (n == null) 
            return false;
        
        if (!n.gameObject.activeInHierarchy) 
            return false;
        
        return true;
    }

    private void PointClickMovement()
    {
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
        
        List<Node> nodes = FindPath(transform.parent.GetComponent<Node>(), targetNode);

        if (nodes != null)
            StartCoroutine(MoveAlongPath(nodes));
    }
    
    private void RebuildGraph()
    {
        var nodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        
        foreach (Node n in nodes)
            n.ConnectedNodes.Clear();

        foreach (Node a in nodes)
        {
            if (!IsWalkable(a)) continue;
            Vector3 flatA = Flatten(a.transform.position);

            foreach (Node b in nodes)
            {
                if (a == b || !IsWalkable(b)) 
                    continue;
                
                if (a.transform.up != b.transform.up) 
                    continue;

                if (a.ConectionBlackList.Contains(b) || b.ConectionBlackList.Contains(a)) 
                    continue;

                Vector3 flatB = Flatten(b.transform.position);
                // Vector3 delta = b.transform.position - a.transform.position;

                Vector3 flatDelta = flatB - flatA;
                
                float planarDistance = flatDelta.magnitude;
                
                if (planarDistance > _overlapTolerance) 
                    continue;
                
                Vector3 alignmentCheck = Positive(flatDelta);
                
                alignmentCheck.x = Mathf.Round(alignmentCheck.x);
                alignmentCheck.z = Mathf.Round(alignmentCheck.z);
                alignmentCheck.y = Mathf.Round(alignmentCheck.y);
                
                if (alignmentCheck == Positive(a.transform.up)) 
                    continue;
                
                a.ConnectedNodes.Add(b);
            }
        }
    }
    
    private IEnumerator MoveAlongPath(List<Node> path)
    {
        _currentNode = path[0];
        
        transform.SetParent(_currentNode.transform);
        transform.position = _currentNode.transform.position;

        for (int i = 1; i < path.Count; i++)
        {
            Node nextNode = path[i];
            
            if (!_currentNode.ConnectedNodes.Contains(nextNode))
            {
                _movementRoutine = null;
                yield break;
            }
            
            Vector3 headingA = _currentNode.transform.position - _camera.transform.position;
            Vector3 headingB = nextNode.transform.position - _camera.transform.position;
            
            float distanceA = Vector3.Dot(headingA, _camera.transform.forward);
            float distanceB = Vector3.Dot(headingB, _camera.transform.forward);

            Vector3 startOffset = Vector3.zero;
            Vector3 endOffset =  Vector3.zero;
            
            if (distanceA < distanceB)
            {
                Vector3 direction = Flatten(nextNode.transform.position) - Flatten(_currentNode.transform.position);
                endOffset = (_currentNode.transform.position + direction) - nextNode.transform.position;
            }
            else
            {
                Vector3 direction = Flatten(_currentNode.transform.position) - Flatten(nextNode.transform.position);
                startOffset = (nextNode.transform.position + direction) - _currentNode.transform.position;
            }
            
            _currentNode.Occupied = false;
            nextNode.Occupied = true;

            Transform startPosition = _currentNode.transform;
            Transform targetPosition = nextNode.transform;
            
            
            float duration = 1/_moveSpeed;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                transform.position = Vector3.Lerp(startPosition.position + startOffset, targetPosition.position + endOffset, t);
                yield return null;
            }
            transform.position = targetPosition.position;

            _currentNode = nextNode;

            transform.SetParent(_currentNode.transform);
        }
        _movementRoutine = null;
    }
}
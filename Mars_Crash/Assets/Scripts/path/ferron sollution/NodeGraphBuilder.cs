using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NodeGraphBuilder : MonoBehaviour
{
    [SerializeField] private float _overlapTolerance = 1f;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        RebuildGraph();
    }

    public void RebuildGraph()
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
                Vector3 flatDelta = flatB - flatA;

                float planarDistance = flatDelta.magnitude;

                if (planarDistance > _overlapTolerance)
                    continue;

                Vector3 alignmentCheck = Positive(flatDelta);

                alignmentCheck.x = Mathf.Round(alignmentCheck.x);
                alignmentCheck.y = Mathf.Round(alignmentCheck.y);
                alignmentCheck.z = Mathf.Round(alignmentCheck.z);

                if (alignmentCheck == Positive(a.transform.up))
                    continue;

                a.ConnectedNodes.Add(b);
            }
        }
    }

    private bool IsWalkable(Node n)
    {
        return n != null && n.gameObject.activeInHierarchy;
    }

    private Vector3 Flatten(Vector3 worldPos)
    {
        var camPos = _camera.transform.position;
        var camForward = _camera.transform.forward;
        var toPoint = worldPos - camPos;

        return worldPos - Vector3.Dot(toPoint, camForward) * camForward;
    }

    private Vector3 Positive(Vector3 v)
    {
        return new Vector3(math.abs(v.x), math.abs(v.y), math.abs(v.z));
    }
}
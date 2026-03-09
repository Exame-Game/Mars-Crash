using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> ConnectedNodes;
    public List<Node> ConectionBlackList;
    public bool Occupied;
}

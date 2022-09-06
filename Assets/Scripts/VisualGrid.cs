using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualGrid : MonoBehaviour
{
    public static VisualGrid Instance;
    private List<VisualNode> visualNodes;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Init()
    {
        // foreach (KeyValuePair<Vector3, Node> pair in GridManager.Instance.GetGrid)
        // {
        //     visualNodes.Add(pair.Value.GetComponent<VisualNode>());
        // }
    }

    public void ToggleNodeInRange(Node _startNode, int _range, VisualNodeType _visualType = VisualNodeType.Highlight)
    {
        ToggleNodesVisual(GridManager.Instance.GetNodesInRange(_startNode, _range), _visualType);
    }

    private void ToggleNodesVisual(List<Node> _listNodes, VisualNodeType _visualType)
    {
        foreach (Node node in _listNodes)
        {
            node.visualNode.ToggleNodeVisual(true, _visualType);
        }
    }

    public void ClearAllToggleNodes()
    {
        foreach (KeyValuePair<Vector3, Node> pair in GridManager.Instance.GetGrid)
        {
            pair.Value.visualNode.ToggleNodeVisual(false, VisualNodeType.All);
        }
    }
}

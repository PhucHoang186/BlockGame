using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int height;
    public int weight;
    [SerializeField] Node nodePref;

    public LayerMask BlockLayer;
    public LayerMask EntityLayer;

    public Dictionary<Vector3, Node> Init(Dictionary<Vector3, Node> _grids, float _nodeSize)
    {
        _grids = new Dictionary<Vector3, Node>();
        GenerateGrid(height, weight, _grids, _nodeSize);
        return _grids;
    }

    void GenerateGrid(int _height, int _weight, Dictionary<Vector3, Node> _grids, float _nodeSize)
    {
        for (int i = 0; i < weight; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node newNode = Instantiate(nodePref);
                newNode.transform.parent = transform;
                newNode.transform.position = new Vector3(i, 0f, j) * _nodeSize;
                newNode.Init();
                if (!newNode.isNull)
                {
                    newNode.x = i;
                    newNode.y = j;
                    _grids.Add(new Vector3(i, 0f, j), newNode);
                }
                else
                {
                    Destroy(newNode.gameObject);
                }
            }
        }
    }
}

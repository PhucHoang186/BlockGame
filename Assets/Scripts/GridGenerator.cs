using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    [SerializeField] int height;
    [SerializeField] int weight;
    [SerializeField] Node nodePref;

    public Dictionary<Vector3, Node> GridInit(Dictionary<Vector3, Node> _grids)
    {
        _grids = new Dictionary<Vector3, Node>();
        GenerateGrid(height,weight,_grids);
        return _grids;
    }

    void GenerateGrid(int _height, int _weight,Dictionary<Vector3, Node> _grids)
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _weight; j++)
            {
                Node newNode = Instantiate(nodePref);
                newNode.transform.parent = transform;
                newNode.transform.position = new Vector3(i,0f,j) * newNode.nodeSize;
                _grids.Add(new Vector2(i, j), newNode);
            }
        }
    }
}

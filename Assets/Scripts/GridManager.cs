using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    Dictionary<Vector3, Node> grids;
    [SerializeField] GridGenerator gridGenerator;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void Init()
    {
        gridGenerator.GridInit(grids);
        GameManager.Instance.SwitchState(GameState.PlayerTurn);
    }

    public void SetGridNodeToVisited(GameObject Obj)
    {
        if(grids.ContainsKey(Obj.transform.position))
        {
            // grids[Obj.transform.position].PlaceObjectOnNode(Obj);
        }
    }
    public void ReleaseNodeFromVisited(GameObject Obj)
    {
        if(grids.ContainsKey(Obj.transform.position))
        {
            grids[Obj.transform.position].ReleaseNode(Obj);
        }
    }
}

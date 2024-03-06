
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] Tower towerPrefab;

    gridManager GridManager;
    Pathfinder  finder;
    Vector2Int coordinates = new Vector2Int();

    void Awake(){
        GridManager = FindObjectOfType<gridManager>();
        finder = FindObjectOfType<Pathfinder>();
    }

    void Start(){
        if (GridManager != null){
            coordinates = GridManager.GetCoordinatesFromPosition(transform.position);
            if (!isPlaceable){
                GridManager.BlockNode(coordinates);
            }
        }
    }

    public bool IsPlaceable{get{return isPlaceable;}}

    void OnMouseDown(){{
        if (GridManager.getNode(coordinates).isWalkable && !finder.WillBlockPath(coordinates)){

            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if (isSuccessful){
                GridManager.BlockNode(coordinates);
                finder.NotifyReceiveres();
            }
            
        }
    }}
}

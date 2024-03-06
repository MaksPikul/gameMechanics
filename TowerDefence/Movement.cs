using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] [Range(0f,5f)] float speed = 1f;
    List<Node> path = new List<Node>();
    Enemy enemy;
    gridManager GridManager;
    Pathfinder pathfinder;
    void OnEnable()
    {
        ReturnToStart();
        Recalculate(true);
    }

    

    void Awake(){
        enemy = GetComponent<Enemy>();
        GridManager = FindObjectOfType<gridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Recalculate(bool resetPath){
        Vector2Int coordinates = new Vector2Int();

        if (resetPath){
            coordinates = pathfinder.StartCoordinates;
        }
        else{
            coordinates = GridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(followPath());
    }

    void ReturnToStart(){
        transform.position = GridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void finishPath(){
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    IEnumerator followPath(){
        for(int i=1; i<path.Count; i++){
            
            Vector3 startPos = transform.position;
            Vector3 endPos = GridManager.GetPositionFromCoordinates(path[i].coords);;
            float travelPercent = 0f;

            transform.LookAt(endPos);

            while(travelPercent < 1f){
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        finishPath();
    }
}

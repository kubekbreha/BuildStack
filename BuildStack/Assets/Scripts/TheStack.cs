using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour {

    private const float STACK_MOVING_SPEED = 5.0f;
    private const float BOUNDS_SIZE = 3.8f;
    private const float ERROR_MARGIN = 0.1f;

	private GameObject[] theStack;
    private Vector2 stacksBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);   

    private int stackIndex;
    private int scoreCount = 0;
    private int combo = 0;

    private float tileTransaction = 0.0f;
    private float tileSpeed = 2.0f;
    private float secondaryPossition;

    private Vector3 desiredPosition;
    private Vector3 lastTilePossition;

    private bool isMovingOnX = true;
    private bool gameOver = false;

	private void Start () {
		theStack = new GameObject[transform.childCount ];

		for(int i = 0; i < transform.childCount; i++){
            theStack[i] = transform.GetChild(i).gameObject;
		}

	}


    private void Update(){
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile()){
                SpawnTile();
                scoreCount++;
            }else{
                EndGame();
            }
        }
        MoveTile();

        transform.position = Vector3.Lerp(transform.position, desiredPosition, STACK_MOVING_SPEED * Time.deltaTime);
    }
	


    private void MoveTile(){
        if(gameOver){
            return;
        }

        tileTransaction += Time.deltaTime * tileSpeed;
        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition
                                = new Vector3(Mathf.Sin(tileTransaction) * BOUNDS_SIZE, scoreCount, secondaryPossition);
        }else{
            theStack[stackIndex].transform.localPosition
                                = new Vector3(secondaryPossition, scoreCount, Mathf.Sin(tileTransaction) * BOUNDS_SIZE);
        }
    }


    private void SpawnTile(){
        lastTilePossition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if(stackIndex < 0){
            stackIndex = transform.childCount - 1;
        }

        desiredPosition = (Vector3.down) * scoreCount;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0); 
        theStack[stackIndex].transform.localScale = new Vector3(stacksBounds.x, 1, stacksBounds.y); 
    }

    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;

        if(isMovingOnX){
            float deltaX = lastTilePossition.x - t.position.x;
            if(Mathf.Abs (deltaX) > ERROR_MARGIN){

                //Cuting the tile
                combo = 0;
                stacksBounds.x -= Mathf.Abs(deltaX);
                if(stacksBounds.x < 0){
                    return false;
                }

                float middle = lastTilePossition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(stacksBounds.x, 1, stacksBounds.y);
                t.localPosition = new Vector3(middle - (lastTilePossition.x / 2), scoreCount, lastTilePossition.z);
            }
        }else{
            float deltaZ = lastTilePossition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {

                //Cuting the tile
                combo = 0;
                stacksBounds.y -= Mathf.Abs(deltaZ);
                if (stacksBounds.y < 0)
                {
                    return false;
                }

                float middle = lastTilePossition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(stacksBounds.x, 1, stacksBounds.y);
                t.localPosition = new Vector3(lastTilePossition.x, scoreCount, middle - (lastTilePossition.z / 2));
            }
        }

        secondaryPossition = isMovingOnX ? t.localPosition.x : t.localPosition.z;

        isMovingOnX = !isMovingOnX;
        return true;
    }


    private void EndGame(){
        Debug.Log("You Lose.");
        gameOver = true;
        theStack[stackIndex].AddComponent<Rigidbody>();
    }

}


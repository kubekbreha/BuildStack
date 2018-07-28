using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour {

    private const float STACK_MOVING_SPEED = 5.0f;
    private const float BOUNDS_SIZE = 3.8f;
	private GameObject[] theStack;

    private int stackIndex;
    private int scoreCount = 0;

    private float tileTransaction = 0.0f;
    private float tileSpeed = 2.0f;
    private float secondaryPossition;

    private Vector3 desiredPosition;
    private bool isMovingOnX = true;

	private void Start () {
		theStack = new GameObject[transform.childCount ];

		for(int i = 0; i < transform.childCount; i++){
            theStack[i] = transform.GetChild(i).gameObject;
		}

	}


    private void Update(){
        if (Input.GetMouseButtonDown(0)){
            if (PlaceTile()) { }
            SpawnTile();
            scoreCount++;
        }else{
            EndGame();
        }

        MoveTile();

        transform.position = Vector3.Lerp(transform.position, desiredPosition, STACK_MOVING_SPEED * Time.deltaTime);
    }
	


    private void MoveTile(){
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
        stackIndex--;
        if(stackIndex < 0){
            stackIndex = transform.childCount - 1;
        }

        desiredPosition = (Vector3.down) * scoreCount;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0); 
    }

    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;

        secondaryPossition = isMovingOnX ? t.localPosition.x : t.localPosition.z;

        isMovingOnX = !isMovingOnX;
        return true;
    }


    private void EndGame(){
        
    }

}


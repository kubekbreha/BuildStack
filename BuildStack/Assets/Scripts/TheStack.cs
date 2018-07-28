using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour {


    private const float BOUNDS_SIZE = 3.8f;
	private GameObject[] theStack;

    private int stackIndex;
    private int scoreCount = 0;

    private float tileTransaction = 0.0f;
    private float tileSpeed = 2.0f;

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
    }
	


    private void MoveTile(){
        tileTransaction += Time.deltaTime * tileSpeed;
        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition
                                = new Vector3(Mathf.Sin(tileTransaction) * BOUNDS_SIZE, scoreCount, 0);
        }else{
            theStack[stackIndex].transform.localPosition
                                = new Vector3(0, scoreCount, Mathf.Sin(tileTransaction) * BOUNDS_SIZE);
        }
    }


    private void SpawnTile(){
        stackIndex--;
        if(stackIndex < 0){
            stackIndex = transform.childCount - 1;
        }

        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0); 
    }

    private bool PlaceTile(){
        isMovingOnX = !isMovingOnX;
        return true;
    }


    private void EndGame(){
        
    }

}


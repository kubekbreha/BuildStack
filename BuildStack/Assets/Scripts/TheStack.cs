using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour {

	private GameObject[] theStack;

    private int stackIndex;
    private int scoreCount = 0;

	private void Start () {
		theStack = new GameObject[transform.childCount ];

		for(int i = 0; i < transform.childCount; i++){
            theStack[i] = transform.GetChild(i).gameObject;
		}

	}

	private void Update () {
        if(Input.GetMouseButtonDown(0)){
            spawnTile();
            scoreCount++;
        }

	}

    private void spawnTile(){
        stackIndex--;
        if(stackIndex < 0){
            stackIndex = transform.childCount - 1;
        }

        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0); 

    }

    private void PlaceTile(){

    }

}


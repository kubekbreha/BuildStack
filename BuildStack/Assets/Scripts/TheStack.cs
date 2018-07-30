using UnityEngine;

/*
 * Created by Kubo Brehuv 29.7.2018.
 */
public class TheStack : MonoBehaviour
{
    
    private const float STACK_MOVING_SPEED = 5.0f;
    private const float BOUNDS_SIZE = 3.5f;
    private const float ERROR_MARGIN = 0.15f;
    private const float STACK_BOUNDS_GAIN = 0.25f;
    private const int COMBO_START_GAIN = 3;

    private GameObject[] theStack;
    private Vector2 stacksBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);
    private Vector3 desiredPosition;
    private Vector3 lastTilePossition;

    private int stackIndex;
    private int scoreCount = 0;
    private int combo = 0;

    private float tileTransaction = 0.0f;
    private float tileSpeed = 2.0f;
    private float secondaryPossition;

    private bool isMovingOnX = true;
    private bool gameOver = false;


    private void Start()
    {
        theStack = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i).gameObject;
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                MoveStack();
                scoreCount++;
            }
            else
            {
                EndGame();
            }
        }
        MoveTile();

        //moving whole stack down
        transform.position = Vector3.Lerp(transform.position,
                                          desiredPosition,
                                          STACK_MOVING_SPEED * Time.deltaTime);
    }


    /*  
     * Creating rubble of badly placed tile. 
     */
    private void CreateRubble(Vector3 position, Vector3 scale)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = position;
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody>();
    }


    /*
     * Movimg tile in both axes. Called in update so it's moving all the time.
     */
    private void MoveTile()
    {
        //moving tile while game is not over 
        if (gameOver)
        {
            return;
        }

        //moving tile up and down, using sinus to get walue from -1 to 1 every time tileTransaction value grows
        tileTransaction += Time.deltaTime * tileSpeed;
        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition
                                = new Vector3(Mathf.Sin(tileTransaction) * BOUNDS_SIZE,
                                              scoreCount,
                                              secondaryPossition);
        }
        else
        {
            theStack[stackIndex].transform.localPosition
                                = new Vector3(secondaryPossition,
                                              scoreCount,
                                              Mathf.Sin(tileTransaction) * BOUNDS_SIZE);
        }
    }


    /*
     * Moving stack up. 
     */
    private void MoveStack()
    {
        Debug.Log("Combo: " + combo);

        lastTilePossition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }

        //moving whole stack down
        desiredPosition = (Vector3.down) * scoreCount;

        //setting size and scale of tile which we placing
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
        theStack[stackIndex].transform.localScale = new Vector3(stacksBounds.x, 1, stacksBounds.y);
    }


    /*
     * Placig tile on the top of the stack.
     */
    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;

        if (isMovingOnX)
        {
            float deltaX = lastTilePossition.x - t.position.x;

            //placed not perfectly
            if (Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                //Cuting the tile
                combo = 0;
                stacksBounds.x -= Mathf.Abs(deltaX);
                if (stacksBounds.x < 0)
                {
                    return false;
                }

                float middle = lastTilePossition.x + t.localPosition.x / 2;

                //set cutted tile and spawn rubble
                t.localScale = new Vector3(stacksBounds.x,
                                           1,
                                           stacksBounds.y);

                CreateRubble(new Vector3((t.position.x > 0) ? t.position.x + (t.localScale.x / 2) : t.position.x - (t.localScale.x / 2),
                                         t.position.y,
                                         t.position.z),

                             new Vector3(Mathf.Abs(deltaX),
                                         1,
                                         t.localScale.z));

                t.localPosition = new Vector3(middle - (lastTilePossition.x / 2),
                                              scoreCount,
                                              lastTilePossition.z);
            }
            //placed perfectly
            else
            {
                combo++;
                if (combo > COMBO_START_GAIN)
                {
                    //Setting tile bigger.
                    stacksBounds.x += COMBO_START_GAIN;
                    if (stacksBounds.x > BOUNDS_SIZE)
                        stacksBounds.x = BOUNDS_SIZE;

                    float middle = lastTilePossition.x + t.localPosition.x / 2;
                    t.localScale = new Vector3(stacksBounds.x,
                                               1,
                                               stacksBounds.y);
                   
                    t.localPosition = new Vector3(middle - (lastTilePossition.x / 2),
                                                  scoreCount,
                                                  lastTilePossition.z);
                }
                else
                {
                    //Setting tile same as before.
                    t.localPosition = new Vector3(lastTilePossition.x,
                                                  scoreCount,
                                                  lastTilePossition.z);
                }

            }
        }
        else
        {
            float deltaZ = lastTilePossition.z - t.position.z;
           
            //placed perfectly
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

                //set cutted tile and spawn rubble
                CreateRubble(new Vector3(t.position.x,
                                         t.position.y,
                                         (t.position.z > 0) ? t.position.z + (t.localScale.z / 2) : t.position.z - (t.localScale.z / 2)),
                           
                             new Vector3(t.localScale.x,
                                         1,
                                         Mathf.Abs(deltaZ)));

                t.localPosition = new Vector3(lastTilePossition.x,
                                              scoreCount,
                                              middle - (lastTilePossition.z / 2));
            }
            //placed not perfectly
            else
            {
                combo++;
                if (combo > COMBO_START_GAIN)
                {
                    //Setting tile bigger.
                    if (stacksBounds.y > BOUNDS_SIZE)
                        stacksBounds.y = BOUNDS_SIZE;

                    stacksBounds.y += COMBO_START_GAIN;
                    float middle = lastTilePossition.z + t.localPosition.z / 2;
                    t.localScale = new Vector3(stacksBounds.x,
                                               1,
                                               stacksBounds.y);
                 
                    t.localPosition = new Vector3(lastTilePossition.x,
                                                  scoreCount,
                                                  middle - (lastTilePossition.z / 2));
                }
                else
                {
                    //Setting tile same as before.
                    t.localPosition = new Vector3(lastTilePossition.x,
                                                  scoreCount,
                                                  lastTilePossition.z);
                }

            }
        }

        //new position from where will floating tile flow
        secondaryPossition = isMovingOnX ? t.localPosition.x : t.localPosition.z;

        isMovingOnX = !isMovingOnX;
        return true;
    }


    /*
     * End game function.
     */
    private void EndGame()
    {
        Debug.Log("You Lose.");
        gameOver = true;
        theStack[stackIndex].AddComponent<Rigidbody>();
    }

}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrafficManager : MonoBehaviour
{
    public static TrafficManager Instance { get; set; }
    public GameObject[] Cars;
    public int[] CoordinatesX;
    public int[] CoordinatesY;
    public int currentPos;
    //public int currentY;
    public bool move;
    public int triggerMoves;
    public bool isGameOver;
    private int index;
    //public int curPos;
    public int curDir;
    public int secondsLeft;
    public float carSpeed = 1.5f;
    public string movement;
    public float destinationX;
    public float destinationZ;
    //private Rigidbody rb;
    public Vector3 target;
    public GameObject GameOverPanel;
    public GameObject WinPanel;
    public TextMeshProUGUI Timer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        isGameOver = false;
        move = false;
        triggerMoves = 0;
        currentPos = 0;
        movement = "1";
        CoordinatesX = new int[8]; // cutomize size to suit n-sided grid
        CoordinatesY = new int[8]; // cutomize size to suit n-sided grid
        CoordinatesX[0] = 3;
        CoordinatesY[0] = 3;
        curDir = 0;
        secondsLeft = 180;
        PickNextCarToMove();
        Cars[15].transform.rotation = Quaternion.Euler(0, 90, 0);
        SetNextDestination(CoordinatesX[currentPos], CoordinatesY[currentPos]);
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerMoves > 0 && !isGameOver)
        {
            if (move)
            {
                if (movement[curDir] == '1')
                    Cars[index].transform.rotation = Quaternion.Euler(0, 90, 0);
                else
                    Cars[index].transform.rotation = Quaternion.Euler(0, 0, 0);

                Cars[index].transform.position = Vector3.MoveTowards(Cars[index].transform.position, target, carSpeed * Time.deltaTime);
                if (curDir < movement.Length - currentPos - 1 && Cars[index].transform.position == target)
                {
                    curDir++;
                    if (index != 16 && curDir == 1)  // cutomize size to suit n-sided grid
                        Cars[index].SetActive(false);

                    SetNextDestination(CoordinatesX[currentPos + curDir], CoordinatesY[currentPos + curDir]);
                }
                else if (curDir == movement.Length - currentPos - 1 && Cars[index].transform.position == target)
                {
                    move = false;
                }
            }

            if (!move)
            {
                currentPos++;
                triggerMoves--;
                if (currentPos == CoordinatesX.Length)
                {
                    isGameOver = true;
                    WinPanel.SetActive(true);
                    triggerMoves = 0;
                    Debug.Log("You Win");
                }
                else
                {
                    curDir = 0;
                    //PickNextCarToMove();
                    SetNextDestination(CoordinatesX[currentPos], CoordinatesY[currentPos]);
                }
            }
        }
    }

    IEnumerator CountDown()
    {
        while (secondsLeft > 0 && !isGameOver)
        {
            yield return new WaitForSeconds(1);
            secondsLeft -= 1;
            int seconds = secondsLeft % 60;
            int minutes = secondsLeft / 60;
            Timer.text = "Time Left: " + minutes.ToString() + ":" + seconds.ToString("00");
        }
        isGameOver = true;
        GameOverPanel.SetActive(true);
    }

    public void SetNextDestination(int x, int y)
    {
        index = x * 4 + y;
        //curDir = 0;
        if (curDir == 0)
        {
            destinationX = Cars[index].transform.position.x;
            destinationZ = Cars[index].transform.position.z - 4;
            carSpeed *= 2.5f;
        }
        else if (movement[curDir] == '1')
        {
            carSpeed = 1.5f;
            destinationX = Cars[index].transform.position.x;
            destinationZ = Cars[index].transform.position.z - 1;
        }
        else
        {
            carSpeed = 1.5f;
            destinationZ = Cars[index].transform.position.z;
            destinationX = Cars[index].transform.position.x + 1;
        }
        target = new Vector3(destinationX, Cars[index].transform.position.y, destinationZ);
        Debug.Log(target);
        move = true;
    }

    public void MoveCar()
    {
        triggerMoves++;
    }

    public void PickNextCarToMove()
    {
        int id;
        for (int i = 1; i < 8; i++)
        {
            if (CoordinatesX[i - 1] - 1 < 0 && CoordinatesY[i - 1] - 1 < 0)
            {
                //isGameOver = true;
                movement += "0";
                CoordinatesX[i] = 3;  // cutomize size to suit n-sided grid
                CoordinatesY[i] = 4;  // cutomize size to suit n-sided grid
                id = CoordinatesX[i] * 4 + CoordinatesY[i];
                Cars[id].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (CoordinatesX[i - 1] - 1 < 0)
            {
                movement += "0";
                CoordinatesX[i] = CoordinatesX[i - 1];
                CoordinatesY[i] = CoordinatesY[i - 1] - 1;
                id = CoordinatesX[i] * 4 + CoordinatesY[i];
                Cars[id].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (CoordinatesY[i - 1] - 1 < 0)
            {
                movement += "1";
                CoordinatesY[i] = CoordinatesY[i - 1];
                CoordinatesX[i] = CoordinatesX[i - 1] - 1;
                id = CoordinatesX[i] * 4 + CoordinatesY[i];
                Cars[id].transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                int cur = Random.Range(0, 2);
                if (cur == 0)
                {
                    movement += "0";
                    CoordinatesX[i] = CoordinatesX[i - 1];
                    CoordinatesY[i] = CoordinatesY[i - 1] - 1;
                    id = CoordinatesX[i] * 4 + CoordinatesY[i];
                    Cars[id].transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    movement += "1";
                    CoordinatesY[i] = CoordinatesY[i - 1];
                    CoordinatesX[i] = CoordinatesX[i - 1] - 1;
                    id = CoordinatesX[i] * 4 + CoordinatesY[i];
                    Cars[id].transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }
}

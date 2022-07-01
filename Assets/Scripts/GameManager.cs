using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hitInfo;
    private GlassTube glassTube;

    private int ballSort;
    private int ballReceived;
    private int destinationNumb;
    private bool movementControl;
    private bool deletedObject;
    private bool ballPosControl;

    public GlassTube selectedGlassTube;
    public GlassTube saveGlassTube;
    public GameObject createBall;
    public GameObject createGlassTube;
    public GameObject selectedColorBall;
    public GameObject selectedOutBall;
    public GameObject selectedBallPoint;
    public GameObject targetBallPoint;
    public GameObject newDestination;

    public GameObject Panel;
    public GameObject contnButton;
    public Button againButton;
    public Button ınGameAgainButton;
    public Button quitButton;
    public Button pauseButton;
    public Button continueButton;

    public List<GameObject> glassTubes;
    public List<Material> Colors;
    public List<GameObject> allInsBall;
    public List<GameObject> GlassTubesSpawnPoints;
    public List<int> ballSortList;
    public List<int> glassSortList;

    private void Awake()
    {
        movementControl = false;
        Panel.SetActive(false);
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SpawnGlassTubes();
        SpawnRandomColorBalls();
    }

    private void Start()
    {
        againButton.onClick.AddListener(() => AgainButton());
        ınGameAgainButton.onClick.AddListener(() => AgainButton());
        quitButton.onClick.AddListener(() => QuitButton());
        pauseButton.onClick.AddListener(() => PauseButton());
        continueButton.onClick.AddListener(() => ContinueButton());
    }

    void Update()
    {
        IPressedTheScreen();
        BallMovement(targetBallPoint);
    }

    public void SpawnGlassTubes()
    {
        int randomSpawnCount = Random.Range(3, GlassTubesSpawnPoints.Count + 1);
        for (int h = 0; h < randomSpawnCount; h++)
        {
            GameObject GlassTubes = Instantiate(createGlassTube, GlassTubesSpawnPoints[h].transform.position, GlassTubesSpawnPoints[h].transform.rotation);
            glassTubes.Add(GlassTubes);
        }
    }

    public void SpawnRandomColorBalls()
    {
        for (int a = 0; a < glassTubes.Count - 1; a++)
        {
            glassTube = glassTubes[a].GetComponent<GlassTube>();
            for (int b = 0; b < 4; b++)
            {
                ballSort++;
                int pointNumb = ballSort;
                ballSortList.Add(pointNumb - 1);
                GameObject ball = Instantiate(createBall);
                ball.GetComponent<Renderer>().material = Colors[a];
                allInsBall.Add(ball);
            }
        }

        for (int k = 0; k < glassTubes.Count - 1; k++)
        {
            glassTube = glassTubes[k].GetComponent<GlassTube>();
            for (int d = 0; d <= glassTube.ballPoints.Count - 1; d++)
            {
                int randomSort = Random.Range(0, ballSortList.Count - 1);
                int SortNumb = ballSortList[randomSort];
                glassTube.ballsInsideTheTube.Add(allInsBall[SortNumb]);
                allInsBall[SortNumb].transform.position = glassTube.ballPoints[d].transform.position;
                ballSortList.Remove(SortNumb);
            }
        }
    }

    public void IPressedTheScreen()
    {
        if (Input.GetMouseButtonDown(0) && !movementControl && !ballPosControl)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.CompareTag("GlassTube"))
            {
                selectedGlassTube = hitInfo.collider.gameObject.GetComponent<GlassTube>();
                if (selectedGlassTube.ballsInsideTheTube != null && selectedColorBall == null)
                {
                    if (selectedGlassTube.ballsInsideTheTube.Count - 1 == -1)
                    {
                        print("İçi Boş");
                    }
                    else
                    {
                        for (int g = 0; g < selectedGlassTube.ballsInsideTheTube.Count - 1; g++)
                        {
                            ballReceived++;
                        }
                        saveGlassTube = selectedGlassTube;
                        selectedColorBall = selectedGlassTube.ballsInsideTheTube[ballReceived];
                        targetBallPoint = selectedGlassTube.outBallPoint;
                        selectedBallPoint = selectedGlassTube.ballPoints[ballReceived];
                        selectedGlassTube.ballsInsideTheTube.Remove(selectedGlassTube.ballsInsideTheTube[ballReceived]);
                        movementControl = true;
                        ballReceived = 0;
                    }
                }
                else
                {
                    selectedOutBall = selectedGlassTube.outBallPoint;
                    if (selectedOutBall == saveGlassTube.outBallPoint)
                    {
                        saveGlassTube.ballsInsideTheTube.Add(selectedColorBall);
                        targetBallPoint = selectedBallPoint;
                    }
                    else if (selectedGlassTube.ballsInsideTheTube.Count < 4)
                    {
                        selectedGlassTube.ballsInsideTheTube.Add(selectedColorBall);
                        targetBallPoint = selectedGlassTube.outBallPoint;
                        for (int g = 0; g < selectedGlassTube.ballsInsideTheTube.Count - 1; g++)
                        {
                            destinationNumb++;
                        }
                        newDestination = selectedGlassTube.ballPoints[destinationNumb];
                        ballPosControl = true;
                    }
                    else
                    {
                        saveGlassTube.ballsInsideTheTube.Add(selectedColorBall);
                        targetBallPoint = selectedBallPoint;
                    }
                    movementControl = true;
                    if (!ballPosControl)
                    {
                        deletedObject = true;
                    }
                }
            }
        }

        /*if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.CompareTag("GlassTube"))
            {
                selectedGlassTube = hitInfo.collider.gameObject.GetComponent<GlassTube>();
                if (selectedGlassTube.ballsInsideTheTube != null && selectedColorBall == null)
                {
                    for (int g = 0; g < selectedGlassTube.ballsInsideTheTube.Count - 1; g++)
                    {
                        ballReceived++;
                    }
                    saveGlassTube = selectedGlassTube;
                    selectedColorBall = selectedGlassTube.ballsInsideTheTube[ballReceived];
                    targetBallPoint = selectedGlassTube.outBallPoint;
                    selectedBallPoint = selectedGlassTube.ballPoints[ballReceived];
                    selectedGlassTube.ballsInsideTheTube.Remove(selectedGlassTube.ballsInsideTheTube[ballReceived]);
                    movementControl = true;
                    ballReceived = 0;
                }
            }
        }*/
    }

    public void BallMovement(GameObject targetPos)
    {
        if (movementControl)
        {
            if (targetPos.transform.position != selectedColorBall.transform.position)
            {
                selectedColorBall.transform.position = Vector3.MoveTowards(selectedColorBall.transform.position, targetPos.transform.position, 20 * Time.deltaTime);
            }
            else
            {
                movementControl = false;
                DeletedSelecetBall();
                selectedGlassTube.BallColorControl(true);
            }
        }
        if (ballPosControl && !movementControl)
        {
            selectedColorBall.transform.position = Vector3.MoveTowards(selectedColorBall.transform.position, newDestination.transform.position, 20 * Time.deltaTime);
            if (selectedColorBall.transform.position == newDestination.transform.position)
            {
                destinationNumb = 0;
                ballPosControl = false;
                newDestination = null;
                deletedObject = true;
                DeletedSelecetBall();
                selectedGlassTube.BallColorControl(true);
                CorrectGlassTubes(true);
            }
        }
    }

    public void DeletedSelecetBall()
    {
        if (deletedObject)
        {
            selectedColorBall = null;
            deletedObject = false;
        }
    }

    public void CorrectGlassTubes(bool Check)
    {
        int countedToward = 0;
        for (int tubes = 0; tubes < glassTubes.Count; tubes++)
        {
            if (glassTubes[tubes].GetComponent<GlassTube>().ıAmCorrect)
            {
                countedToward++;
            }
        }
        if (countedToward == glassTubes.Count - 1)
        {
            Panel.SetActive(true);
            contnButton.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            countedToward = 0;
        }
    }

    public void AgainButton()
    {
        SceneManager.LoadScene("InGame");
        Time.timeScale = 1;
    }

    public void PauseButton()
    {
        Panel.SetActive(true);
        Time.timeScale = 0;
        contnButton.SetActive(true);
    }

    public void ContinueButton()
    {
        Panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}

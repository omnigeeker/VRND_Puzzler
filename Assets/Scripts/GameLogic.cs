using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameLogic : MonoBehaviour
{
    public GameObject player;
    public GameObject eventSystem;
    public GameObject startUI, restartUI;
    public GameObject startPoint, restartPoint;

    // How many seconds between the orbs light up during the pattern display.
    public float puzzleSpeed = 1f;

    public GameObject playPoint0, playPoint1, playPoint2, playPoint3;

    // An array to hold the orbs.
    public GameObject[] puzzleSpheres0;
    public GameObject[] puzzleSpheres1;
    public GameObject[] puzzleSpheres2;
    public GameObject[] puzzleSpheres3;

    // Variable for storing the order of the pattern display.
    private int[] puzzleOrder0;
    private int[] puzzleOrder1;
    private int[] puzzleOrder2;
    private int[] puzzleOrder3;

    private List<GameObject[]> puzzleSpheresList = new List<GameObject[]>();
    private List<int[]> puzzleOrderList = new List<int[]>();
    private List<GameObject> playPointList = new List<GameObject>();

    // 当前关卡
    private int curretRoom = 0;

    // Variable for storing the index during the pattern display.
    private int currentDisplayIndex = 0;

    // Variable for storing the index the player is trying to solve.
    private int currentSolveIndex = 0;

    /* Uncomment the line below during 'A Little More Feedback!' lesson.*/
    public GameObject failAudioHolder;

    void Start()
    {
        // Update 'player' to be the camera's parent gameobject, i.e. 'GvrEditorEmulator' instead of the camera itself.
        // Required because GVR resets camera position to 0, 0, 0.
        player = player.transform.parent.gameObject;

        // Move the 'player' to the 'startPoint' position.
        player.transform.position = startPoint.transform.position;

        puzzleSpheresList.Add(puzzleSpheres0);
        puzzleSpheresList.Add(puzzleSpheres1);
        puzzleSpheresList.Add(puzzleSpheres2);
        puzzleSpheresList.Add(puzzleSpheres3);
        playPointList.Add(playPoint0);
        playPointList.Add(playPoint1);
        playPointList.Add(playPoint2);
        playPointList.Add(playPoint3);

        // Set the size of our array to the declared puzzle length.
        puzzleOrder0 = new int[puzzleSpheres0.Length];
        puzzleOrder1 = new int[puzzleSpheres1.Length];
        puzzleOrder2 = new int[puzzleSpheres2.Length];
        puzzleOrder3 = new int[puzzleSpheres3.Length];
        puzzleOrderList.Add(puzzleOrder0);
        puzzleOrderList.Add(puzzleOrder1);
        puzzleOrderList.Add(puzzleOrder2);
        puzzleOrderList.Add(puzzleOrder3);

        // Create a random puzzle sequence.
        GeneratePuzzleSequence();
    }

    // Create a random puzzle sequence.
    public void GeneratePuzzleSequence()
    {
        // Variable for storing a random number.
        int randomInt;

        // Loop as many times as the puzzle length.
        foreach (int[] puzzleOrder in puzzleOrderList)
        {
            for (int i = 0; i < puzzleOrder.Length; i++)
            {
                // Generate a random number.
                randomInt = UnityEngine.Random.Range(0, puzzleOrder.Length);

                // Set the current index to the randomly generated number.
                puzzleOrder[i] = randomInt;
                Debug.Log("puzzleOrder[" + i + "]: " + puzzleOrder[i]);
            }
        }
    }

    // Begin the puzzle sequence.
    public void StartPuzzle()
    {
        // Disable the start UI.
        startUI.SetActive(false);

        // Move the player to the play position.
        iTween.MoveTo(player,
            iTween.Hash(
                "position", playPointList[curretRoom].transform.position,
                "time", 2,
                "easetype", "linear"
            )
        );

        // Call the DisplayPattern() function repeatedly.
        CancelInvoke("DisplayPattern");
        InvokeRepeating("DisplayPattern", 3, puzzleSpeed);

        // Reset the index the player is trying to solving.
        currentSolveIndex = 0;
    }

    // Reset the puzzle sequence.
    public void ResetPuzzle()
    {
        Debug.Log("ResetPuzzle()");
        // Enable the start UI.
        startUI.SetActive(true);

        // Disable the restart UI.
        restartUI.SetActive(false);

        curretRoom = 0;

        // Stop Fireworks
        GameObject[] FireworkSystems = GameObject.FindGameObjectsWithTag("Fireworks");
        foreach (GameObject GO in FireworkSystems)
            GO.GetComponent<ParticleSystem>().Stop();

        // Move the player to the start position.
        player.transform.position = startPoint.transform.position;

        // Create a random puzzle sequence.
        GeneratePuzzleSequence();
    }

    // Disaplay the
    // Called from StartPuzzle() and invoked repeatingly.
    void DisplayPattern()
    {
        // If we haven't reached the end of the display pattern.
        if (currentDisplayIndex < puzzleOrderList[curretRoom].Length)
        {
            Debug.Log("Display index " + currentDisplayIndex + ": Orb index " + puzzleOrderList[curretRoom][currentDisplayIndex]);

            // Disable gaze input while displaying the pattern (prevents player from interacting with the orbs).
            eventSystem.SetActive(false);

            // Light up the orb at the current index.
            puzzleSpheresList[curretRoom][puzzleOrderList[curretRoom][currentDisplayIndex]].GetComponent<LightUp>().PatternLightUp(puzzleSpeed);

            // Move one to the next orb.
            currentDisplayIndex++;
        }
        // If we have reached the end of the display pattern.
        else
        {
            Debug.Log("End of puzzle display");

            // Renable gaze input when finished displaying the pattern (allows player to interacte with the orbs).
            eventSystem.SetActive(true);

            // Reset the index tracking the orb being lit up.
            currentDisplayIndex = 0;

            // Stop the pattern display.
            CancelInvoke();
        }
    }

    // Identify the index of the sphere the player selected.
    // Called from LightUp.PlayerSelection() method (see LightUp.cs script).
    public void PlayerSelection(GameObject sphere)
    {
        // Variable for storing the selected index.
        int selectedIndex = 0;

        // Loop throught the array to find the index of the selected sphere.
        for (int i = 0; i < puzzleSpheresList[curretRoom].Length; i++)
        {
            // If the passed in sphere is the sphere at the index being checked.
            if (puzzleSpheresList[curretRoom][i] == sphere)
            {
                Debug.Log("Looks like we hit sphere: " + i);

                // Update the index of the passed in sphere to be the same as the index being checked.
                selectedIndex = i;
            }
        }

        // Check if the sphere the player selected is correct.
        SolutionCheck(selectedIndex);
    }

    // Check if the sphere the player selected is correct.
    public void SolutionCheck(int playerSelectionIndex)
    {
        Debug.Log("playerSelectionIndex = " + playerSelectionIndex + ", puzzleOrder[currentSolveIndex] = " + puzzleOrderList[curretRoom][currentSolveIndex]);
        // If the sphere the player selected is the correct sphere.
        if (playerSelectionIndex == puzzleOrderList[curretRoom][currentSolveIndex])
        {
            Debug.Log("Correct!  You've solved " + currentSolveIndex + " out of " + puzzleSpheresList[curretRoom].Length);

            // Update the tracker to check the next sphere.
            currentSolveIndex++;

            // If this was the last sphere in the pattern display...
            if (currentSolveIndex >= puzzleSpheresList[curretRoom].Length)
            {
                PuzzleNextRoom();
            }
        }
        // If the sphere the player selected is the incorrect sphere.
        else
        {
            PuzzleFailure();
        }
    }

    public void PuzzleNextRoom()
    {
        curretRoom++;
        if (curretRoom >= playPointList.Count)
            PuzzleSuccess();
        else
        {
            currentSolveIndex = 0;

            StartPuzzle();
        }
    }

    // Do this when the player solves the puzzle.
    public void PuzzleSuccess()
    {
        // Play Fireworks
        GameObject[] FireworkSystems = GameObject.FindGameObjectsWithTag("Fireworks");
        foreach (GameObject GO in FireworkSystems)
            GO.GetComponent<ParticleSystem>().Play();

        // Enable the restart UI.
        restartUI.SetActive(true);

        // Move the player to the restart position.
        iTween.MoveTo(player,
            iTween.Hash(
                "position", restartPoint.transform.position,
                "time", 2,
                "easetype", "linear"
            )
        );
    }

    // Do this when the player selects the wrong sphere.
    public void PuzzleFailure()
    {
        Debug.Log("You failed, resetting puzzle");

        // Get the GVR audio source component on the failAudioHolder and play the audio.
        /* Uncomment the line below during 'A Little More Feedback!' lesson.*/
        try
        {
            failAudioHolder.GetComponent<GvrAudioSource>().Play();
        } catch (Exception exp)
        {
            print(exp.ToString());
        }

        // Reset the index the player is trying to solving.
        currentSolveIndex = 0;

        // Begin the puzzle sequence.
        StartPuzzle();
    }
}
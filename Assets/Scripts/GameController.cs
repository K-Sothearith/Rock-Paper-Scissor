using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The starter TODOs below are kept as slide references and have been completed.
// Rock - Paper - Scissors : starter script
// Follow the slides and complete every TODO.
// Rock - Paper - Scissors : starter script
// Follow the slides and complete every TODO.
// 1 = Rock, 2 = Paper, 3 = Scissors, 4 = Unknown ("?")
public class GameController : MonoBehaviour
{
    [Header("Round Settings")]
    [SerializeField] private int maxRounds = 5;  // maximum number of rounds
    private int currentRound;                   // current round number
    private bool isGameOver;                    // whether the game has ended
    private Button[] choiceButtons;             // choice buttons to disable when game ends

    private UnityEngine.UI.Image imgYou;  // your selected image (rock, paper, scissor)
    private UnityEngine.UI.Image imgCom;  // computer selected image (rock, paper, scissor)
    private UnityEngine.UI.Text txtYou;  // the score you win
    private UnityEngine.UI.Text txtCom;  // the score computer win
    private UnityEngine.UI.Text txtResult;  // the result

    private int cntYou;  // number you win
    private int cntCom;  // number computer win
    private bool initialized;

    private void InitGame()
    {
        // TODO (slides 37-40): find the UI objects by name and get their components.
        // Example:
        // imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
        // Also find: ImgCom, TxtYou, TxtCom, TxtResult_
        imgYou = GameObject.Find("ImgYou")?.GetComponent<UnityEngine.UI.Image>();
        imgCom = GameObject.Find("ImgCom")?.GetComponent<UnityEngine.UI.Image>();
        txtYou = GameObject.Find("TxtYou")?.GetComponent<UnityEngine.UI.Text>();
        txtCom = GameObject.Find("TxtCom")?.GetComponent<UnityEngine.UI.Text>();
        txtResult = GameObject.Find("TxtResult_")?.GetComponent<UnityEngine.UI.Text>();

        // Find choice buttons to manage interactivity
        GameObject choicesObject = GameObject.Find("Choices");
        if (choicesObject != null)
        {
            choiceButtons = choicesObject.GetComponentsInChildren<Button>();
        }
        else
        {
#if UNITY_2023_1_OR_NEWER
            choiceButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);
#else
            choiceButtons = FindObjectsOfType<Button>();
#endif
        }

        initialized = imgYou != null && imgCom != null && txtYou != null &&
                      txtCom != null && txtResult != null;
        if (!initialized)
        {
            Debug.LogError("GameController requires ImgYou and ImgCom with Image components, " +
                           "and TxtYou, TxtCom and TxtResult_ with Legacy Text components.", this);
            return;
        }

        cntYou = 0;
        cntCom = 0;
        currentRound = 0;
        isGameOver = false;

        SetButtonsInteractable(true);
        txtResult.text = "Select the button below";
        SetResult(4, 4);
    }

    private void Start()
    {
        // init the game
        InitGame();
    }

    // Update is called once per frame
    private void Update()
    {
        // exit if press escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Restart game with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            InitGame();
        }
    }

    // Slides 24-31: link this function to the On Click () event of every button.
    // Pass the button itself as the GameObject parameter.
    public void OnButtonClick(GameObject buttonObject)
    {
        if (buttonObject == null)
        {
            return;
        }

        // Stop accepting input if 5 rounds are reached or game is over
        if (isGameOver || currentRound >= maxRounds)
        {
            Debug.Log("Game has ended. Maximum rounds reached.");
            return;
        }

        // TODO (slide 34): get the number from the first character of the button name
        // int you = int.Parse(buttonObject.name.Substring(0, 1));
        // CheckResult(you);
        string buttonName = buttonObject.name;
        Debug.Log(buttonName + " clicked");

        if (buttonName.Length < 2 || buttonName[1] != '_' ||
            buttonName[0] < '1' || buttonName[0] > '3')
        {
            Debug.LogWarning("Choice button names must start with 1_, 2_ or 3_.", buttonObject);
            return;
        }

        if (initialized)
        {
            CheckResult(buttonName[0] - '0');
        }
    }

    private void CheckResult(int yourResult)
    {
        currentRound++;
        Debug.Log($"Round {currentRound}/{maxRounds} - Your result is: " + yourResult);

        // TODO (slide 33): game logic
        // 1. com = random number 1..3  ->  UnityEngine.Random.Range(1, 4)
        // 2. k = you - com
        // 3. k == 0              -> "Draw."
        //    k == 1 or k == -2   -> "You win."       (cntYou++)
        //    otherwise           -> "Computer win."  (cntCom++)
        // 4. call SetResult(yourResult, comResult)
        int comResult = Random.Range(1, 4);
        int difference = yourResult - comResult;
        string roundOutcome = "";

        if (difference == 0)
        {
            roundOutcome = "Draw.";
        }
        else if (difference == 1 || difference == -2)
        {
            roundOutcome = "You win.";
            cntYou++;
        }
        else
        {
            roundOutcome = "Computer win.";
            cntCom++;
        }

        SetResult(yourResult, comResult);

        // Check if the 5th round has been reached to stop the game
        if (currentRound >= maxRounds)
        {
            isGameOver = true;
            SetButtonsInteractable(false);

            string finalWinner;
            if (cntYou > cntCom)
            {
                finalWinner = "Game Over! You win!";
            }
            else if (cntCom > cntYou)
            {
                finalWinner = "Game Over! Computer wins!";
            }
            else
            {
                finalWinner = "Game Over! Draw!";
            }

            txtResult.text = $"Round {currentRound}/{maxRounds}: {roundOutcome} {finalWinner}";
            Debug.Log($"Game stopped after {maxRounds} rounds. {finalWinner}");
        }
        else
        {
            txtResult.text = $"Round {currentRound}/{maxRounds}: {roundOutcome}";
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (choiceButtons != null)
        {
            foreach (Button btn in choiceButtons)
            {
                if (btn != null)
                {
                    btn.interactable = interactable;
                }
            }
        }
    }

    private void SetResult(int you, int com)
    {
        // TODO (slide 41):
        // 1. change imgYou.sprite and imgCom.sprite
        //    Resources.Load("img_" + you, typeof(Sprite)) as Sprite
        // 2. flip the computer image on the x axis: localScale = (-1, 1, 1)
        // 3. update txtYou.text and txtCom.text with the scores
        imgYou.sprite = Resources.Load<Sprite>("img_" + you);
        imgCom.sprite = Resources.Load<Sprite>("img_" + com);
        imgCom.rectTransform.localScale = new Vector3(com == 4 ? 1f : -1f, 1f, 1f);
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();
    }
}

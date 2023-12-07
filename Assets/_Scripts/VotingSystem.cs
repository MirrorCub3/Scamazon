using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Image = UnityEngine.UI.Image;
using UnityEngine.Device;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;

public class VotingSystem : MonoBehaviour
{
    [HideInInspector]
    public bool deactivateVoting;
    [HideInInspector]
    public bool activateVoting;
    [HideInInspector]
    public bool votingReady;

    // Voting Odds //
    private int goodOdds;
    private int badOdds;
    private int randomNumber;

    // Voting System //
    private int voteNumber;
    private bool processVote;
    private bool SkipVoteUpdateCall;
    private bool voteSwitched;
    private bool selectedOption;
    private bool pickedGood;
    private bool pickedBad;
    private bool goodOption;
    private bool badOption;

    // Voting Table //
    private float setRotationDuration;
    private float rotationSpeed;
    private bool activateButtons;

    // Boss //
    private bool letBossLeave;

    [Header("Voting System")]
    [Tooltip("delay (in seconds) before the table flips over to the voting side")]
    [SerializeField] private int buttonActivateDelay;
    [Tooltip("delay (in seconds) before the table flips back over to the packing side")]
    [SerializeField] private int voteDeactivateDelay;
    [Tooltip("time (in seconds) it takes for the vote to process before displaying the actual selection on screen")]
    [SerializeField] private float voteProcessingTime;
    [SerializeField] private Slider voteProcessingSlider;
    [SerializeField] private GameObject voteTitle;          /// remove later if the vote title is in the vote screen
    [SerializeField] private string[] voteTitles;           /// remove later if the vote title is in the vote screen
    [SerializeField] private MachineType[] machineTypes;  /// order of which the machines are going to be swapped out 
    [SerializeField] private int machineIndex;
    [SerializeField] private MachineManager machineManager;
    [SerializeField] private Sprite[] voteScreens;
    [SerializeField, TextArea(1, 10)] private string[] badFactText;
    [SerializeField, TextArea(1, 10)] private string[] goodFactText;

    [Header("Voting Table")]
    [Tooltip("time (in seconds) it takes for the table to flip over to the voting side")]
    [SerializeField] private float rotationDuration;
    //[SerializeField] private GameObject coffee;
    ///[SerializeField] private GameObject pen;
    [SerializeField] private GameObject plasticTableTop;
    [SerializeField] private GameObject paperTableTop;
    private GameObject table;
    //private TableCollision tableCollision;

    [Header("Monitor Screens")]
    [SerializeField] private GameObject blankScreen;
    [SerializeField] private GameObject voteScreen;
    [SerializeField] private GameObject processingScreen;
    [SerializeField] private GameObject goodScreen;
    [SerializeField] private GameObject badScreen;

    [Header("DialogueSystem")]
    [SerializeField]
    private PromptManager promptManager;

    [Header("Boss Navigation")]
    [SerializeField] private GameObject boss;
    [Tooltip("delay (in seconds) before the boss leaves after the vote is processed")]
    [SerializeField] private float bossLeaveDelay;
    private Boss_Navigation bossNav;

    [Header("Emissions Meter")]
    [SerializeField] private EmissionsMeter emissionsMeter;

    [Header("Box Sizing")]
    [SerializeField]
    private List<Box> boxes;
    private bool boxSizeChanged = false;

    [Header("Conveyors")]
    [SerializeField]
    private List<Conveyor> conveyors;
    private bool conveyorSpeedChanged = false;

    private bool SkipVoteProcess;

    void Start()
    {
        voteSwitched = false;
        selectedOption = false;
        goodOption = false;
        badOption = false;
        voteNumber = 0;
        goodOdds = 50;
        badOdds = 50;
        rotationSpeed = 180 / rotationDuration;
        setRotationDuration = rotationDuration;
        voteProcessingSlider.value = 0;
        voteProcessingSlider.maxValue = voteProcessingTime;
        SkipVoteProcess = false;

        machineIndex = 0;
        table = plasticTableTop;
        //paperTableTop.SetActive(false);
        //tableCollision = table.GetComponentInChildren<TableCollision>();
        bossNav = boss.GetComponent<Boss_Navigation>();

        letBossLeave = false;

        boxSizeChanged = false;
        conveyorSpeedChanged = false;
    }

    void Update()
    {
        // TEMPORARY TESTING KEYBINDS //
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (votingReady == true)
            {
                Debug.Log("PLAYER PICKED GOOD OPTION");

                PlayerPickedGoodOption();
            }
        }

        // TEMPORARY TESTING KEYBINDS //
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (votingReady == true)
            {
                Debug.Log("PLAYER PICKED BAD OPTION");

                PlayerPickedBadOption();
            }
        }

        // TEMPORARY TESTING KEYBINDS //
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            activateVoting = true;
        }

        // TEMPORARY TESTING KEYBINDS //
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            deactivateVoting = true;
        }

        if (activateVoting == true)
        {
            ActivateVoting();
        }

        if (deactivateVoting == true)
        {
            DeactivateVoting();
        }

        if (activateButtons == true)
        {
            ActivateButtons();
        }

        if (processVote == true && !SkipVoteProcess)
        {
            ProcessVote();
            SkipVoteProcess = true;
        }

        if (letBossLeave == true)
        {
            bossNav.moveBackward();
            letBossLeave = false;
        }

        if (voteNumber == 1 && goodOption == true && selectedOption == true)
        {
            paperTableTop.SetActive(true);
            //machineManager.MoveMachineSwap(machineTypes[0]);
            table = paperTableTop;
        }
        else if (voteNumber == 2 && !boxSizeChanged && goodOption == true && selectedOption == true)
        {
            foreach (Box b in boxes)
                b.SetEndScale();
            boxSizeChanged = true;
        }
        else if (voteNumber == 4 && !conveyorSpeedChanged && goodOption == true && selectedOption == true)
        {
            foreach (Conveyor c in conveyors)
                c.slowDownConveyor();
        }
        
        MonitorScreenManager();
    }

    public void ActivateVoting()
    {
        if (voteSwitched == false)
        {
            voteScreen.GetComponent<Image>().sprite = voteScreens[voteNumber];
            voteTitle.GetComponent<TextMeshProUGUI>().text = voteTitles[voteNumber];
            goodScreen.GetComponent<TextMeshProUGUI>().text = goodFactText[voteNumber];
            badScreen.GetComponent<TextMeshProUGUI>().text = badFactText[voteNumber];

            StartCoroutine("DelayButtonActivate");

            voteSwitched = true;
            promptManager.Play();
        }
    }

    IEnumerator DelayButtonActivate()
    {
        float delay = promptManager.GetOpeningPromptLength();
        yield return new WaitForSeconds(delay);

        activateButtons = true;
    }

    public void ActivateButtons()
    {
        if (rotationDuration > 0)
        {
            rotationDuration -= Time.deltaTime;

            //if (tableCollision.coffeeOn == true)
            //{
            //    coffee.GetComponent<Rigidbody>().isKinematic = true;
            //    coffee.transform.RotateAround(table.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
            //}

            // rotates table on x-axis at specified degrees per second
            table.transform.RotateAround(table.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
        }
        else
        {
            votingReady = true;
            activateVoting = false;
            activateButtons = false;
            rotationDuration = setRotationDuration;
            table.transform.eulerAngles = new Vector3(180, 0, 0);
        }
    }

    public void PlayerPickedGoodOption()
    {
        randomNumber = Random.Range(0, 100);
        processVote = true;
        pickedGood = true;
        votingReady = false;

        StartCoroutine("DelayDeactivate");
        promptManager.PlayGood();
    }

    public void PlayerPickedBadOption()
    {
        randomNumber = Random.Range(0, 100);
        processVote = true;
        pickedBad = true;
        votingReady = false;

        StartCoroutine("DelayDeactivate");
        promptManager.PlayBad();
    }

    IEnumerator DelayDeactivate()
    {
        yield return new WaitForSeconds(voteDeactivateDelay);

        deactivateVoting = true;
    }

    public void DeactivateVoting()
    {
        if (rotationDuration > 0)
        {
            rotationDuration -= Time.deltaTime;

            //if (tableCollision.coffeeOn == true)
            //{
            //    coffee.GetComponent<Rigidbody>().isKinematic = true;
            //    coffee.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
            //}

            // rotates table on x-axis at specified degrees per second
            table.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
        }
        else
        {
            deactivateVoting = false;
            rotationDuration = setRotationDuration;
            table.transform.eulerAngles = new Vector3(0, 0, 0);

            //coffee.GetComponent<Rigidbody>().isKinematic = false;

            voteNumber++;
            voteSwitched = false;
        }
    }

    public void ProcessVote()
    {
        selectedOption = false;

        StartCoroutine("ProcessingVote");
    }

    IEnumerator ProcessingVote()
    {
        float time = 0;
        voteProcessingTime = promptManager.GetCurrNodeLength(); // gets length of current line to time prompt processing
        voteProcessingSlider.maxValue = voteProcessingTime;

        while (time < voteProcessingTime)
        {
            voteProcessingSlider.value = Mathf.Lerp(0, voteProcessingTime, time / voteProcessingTime);
            time += Time.deltaTime;
            yield return null;
        }


        /*while (voteProcessingSlider.value <= voteProcessingTime)
        {
            voteProcessingSlider.value += Time.deltaTime;
            yield return null;
        }*/

        //yield return new WaitForSeconds(voteProcessingTime);

        processVote = false;
        SkipVoteProcess = false;
        voteProcessingSlider.value = 0;
        PickVotingOption();
    }

    public void PickVotingOption()
    {
        if (goodOdds >= badOdds && selectedOption == false)
        {
            if (randomNumber <= goodOdds)
            {
                goodOption = true;
            }
            else if (randomNumber > goodOdds)
            {
                badOption = true;
            }

            selectedOption = true;
            //StartCoroutine("DelayBossLeave");
        }

        if (badOdds > goodOdds && selectedOption == false)
        {
            if (randomNumber <= badOdds)
            {
                badOption = true;
            }
            else if (randomNumber > badOdds)
            {
                goodOption = true;
            }

            selectedOption = true;
            //StartCoroutine("DelayBossLeave");
        }

        if (voteNumber > 0)
        {
            if (pickedGood == true)
            {
                goodOdds += 10;
                badOdds -= 10;
                pickedGood = false;
            }
            else if (pickedBad == true)
            {
                badOdds += 10;
                goodOdds -= 10;
                pickedBad = false;
            }
        }

        if (goodOption == true)
        {
            emissionsMeter.UpdateEmissionsMeter(-10);
            machineManager.MoveMachineSwap(machineTypes[machineIndex]);
            print("SWAPPING OUT MACHINE");
            promptManager.PlayGood();
        }

        if (badOption == true)
        {
            emissionsMeter.UpdateEmissionsMeter(10);
            promptManager.PlayBad();
        }
        StartCoroutine("DelayBossLeave");
        ++machineIndex;
        //promptManager.NextPrompt();
    }

    IEnumerator DelayBossLeave()
    {
        bossLeaveDelay = promptManager.GetCurrNodeLength();
        yield return new WaitForSeconds(bossLeaveDelay);

        letBossLeave = true;

        promptManager.NextPrompt();
    }

    public void MonitorScreenManager()
    {
        if (processVote == true)
        {
            processingScreen.SetActive(true);
            voteTitle.SetActive(false);
            voteScreen.SetActive(false);
        }
        else
        {
            processingScreen.SetActive(false);
        }

        if (goodOption == true && selectedOption == true)
        {
            blankScreen.SetActive(true);
            goodScreen.SetActive(true);
            badScreen.SetActive(false);

            Debug.Log("GOOD OPTION SELECTED");
            goodOption = false;
        }
        else if (badOption == true && selectedOption == true)
        {
            blankScreen.SetActive(true);
            badScreen.SetActive(true);
            goodScreen.SetActive(false);

            Debug.Log("BAD OPTION SELECTED");
            badOption = false;
        }

        if (activateVoting == true)
        {
            voteTitle.SetActive(true);
            voteScreen.SetActive(true);
            blankScreen.SetActive(false);
            goodScreen.SetActive(false);
            badScreen.SetActive(false);
        }
    }
}

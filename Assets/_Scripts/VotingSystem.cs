using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Image = UnityEngine.UI.Image;

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
    [SerializeField] private int voteProcessingTime;
    [SerializeField] private Slider voteProcessingSlider;
    [SerializeField] private GameObject voteTitle;          /// remove later if the vote title is in the vote screen
    [SerializeField] private string[] voteTitles;           /// remove later if the vote title is in the vote screen
    [SerializeField] private Sprite[] voteScreens;
    
    [Header("Voting Table")]
    [Tooltip("time (in seconds) it takes for the table to flip over to the voting side")]
    [SerializeField] private float rotationDuration;
    [SerializeField] private GameObject coffee;
    ///[SerializeField] private GameObject pen;
    [SerializeField] private GameObject plasticTableTop;
    [SerializeField] private GameObject paperTableTop;
    private GameObject table;
    private TableCollision tableCollision;

    [Header("Monitor Screens")]
    [SerializeField] private GameObject blankScreen;
    [SerializeField] private GameObject voteScreen;
    [SerializeField] private GameObject processingScreen;
    [SerializeField] private GameObject goodScreen;
    [SerializeField] private GameObject badScreen;

    [Header("Boss Nav")]
    [SerializeField] private GameObject boss;
    [Tooltip("delay (in seconds) before the boss leaves after the vote is processed")]
    [SerializeField] private int bossLeaveDelay;
    private Boss_Navigation bossNav;


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

        table = plasticTableTop;
        paperTableTop.SetActive(false);
        tableCollision = table.GetComponentInChildren<TableCollision>();
        bossNav = boss.GetComponent<Boss_Navigation>();

        letBossLeave = false;
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

        if (processVote == true)
        {
            ProcessVote();
        }

        if (letBossLeave == true)
        {
            bossNav.moveBackward();
            letBossLeave = false;
        }

        if (voteNumber == 1 && goodOption == true && selectedOption == true)
        {
            // paperTableTop.SetActive(true)
            // switch out plastic table with paper table (make it rise from the floor)
            // set "table = paperTable" after the table is done rising from the floor
        }

        MonitorScreenManager();
    }

    public void ActivateVoting()
    {
        if (voteSwitched == false)
        {
            voteScreen.GetComponent<Image>().sprite = voteScreens[voteNumber];
            voteTitle.GetComponent<TextMeshProUGUI>().text = voteTitles[voteNumber];

            StartCoroutine("DelayButtonActivate");

            voteSwitched = true;
        }
    }

    IEnumerator DelayButtonActivate()
    {
        yield return new WaitForSeconds(buttonActivateDelay);

        activateButtons = true;
    }

    public void ActivateButtons()
    {
        if (rotationDuration > 0)
        {
            rotationDuration -= Time.deltaTime;

            if (tableCollision.coffeeOn == true)
            {
                coffee.GetComponent<Rigidbody>().isKinematic = true;
                coffee.transform.RotateAround(table.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
            }

            ///if (tableCollision.penOn == true)
            {
                ///pen.GetComponent<Rigidbody>().isKinematic = true;
                ///pen.transform.RotateAround(table.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
            }

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
    }

    public void PlayerPickedBadOption()
    {
        randomNumber = Random.Range(0, 100);
        processVote = true;
        pickedBad = true;
        votingReady = false;

        StartCoroutine("DelayDeactivate");
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

            if (tableCollision.coffeeOn == true)
            {
                coffee.GetComponent<Rigidbody>().isKinematic = true;
                coffee.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
            }

            ///if (tableCollision.penOn == true)
            {
                ///pen.GetComponent<Rigidbody>().isKinematic = true;
                ///pen.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
            }

            // rotates table on x-axis at specified degrees per second
            table.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
        }
        else
        {
            deactivateVoting = false;
            rotationDuration = setRotationDuration;
            table.transform.eulerAngles = new Vector3(0, 0, 0);

            coffee.GetComponent<Rigidbody>().isKinematic = false;
            ///pen.GetComponent<Rigidbody>().isKinematic = false;

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
        if (voteProcessingSlider.value <= voteProcessingTime)
        {
            voteProcessingSlider.value += Time.deltaTime;
        }

        yield return new WaitForSeconds(voteProcessingTime);

        processVote = false;
        voteProcessingSlider.value = 0;
        PickVotingOption();
    }

    public void PickVotingOption()
    {
        if (goodOdds >= badOdds && selectedOption == false)
        {
            if (randomNumber < goodOdds)
            {
                goodOption = true;
            }
            else if (randomNumber >= goodOdds)
            {
                badOption = true;
            }

            selectedOption = true;
            StartCoroutine("DelayBossLeave");
        }

        if (badOdds > goodOdds && selectedOption == false)
        {
            if (randomNumber < badOdds)
            {
                badOption = true;
            }
            else if (randomNumber >= badOdds)
            {
                goodOption = true;
            }

            selectedOption = true;
            StartCoroutine("DelayBossLeave");
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
    }

    IEnumerator DelayBossLeave()
    {
        yield return new WaitForSeconds(bossLeaveDelay);

        letBossLeave = true;
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

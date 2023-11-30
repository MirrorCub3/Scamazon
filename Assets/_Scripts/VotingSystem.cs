using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Image = UnityEngine.UI.Image;

public class VotingSystem : MonoBehaviour
{
    private int voteNumber;
    private float setRotationDuration;
    private float rotationSpeed;
    private bool deactivateVoting;
    public bool activateVoting;
    private bool activateButtons;          /// change to true when vote needs to be activated (might need to make public)
    private bool processVote;
    private bool voteSwitched;
    private bool pickedGood;
    private bool pickedBad;
    private bool selectedOption;
    private bool goodOption;
    private bool badOption;

    [Header("Voting System")]
    [Tooltip("delay (in seconds) before the table flips over to the button side")]
    [SerializeField] private int buttonActivateDelay;
    [Tooltip("delay (in seconds) before the table flips back over to the packing side")]
    [SerializeField] private int voteDeactivateDelay;
    [Tooltip("time (in seconds) it takes for the vote to process before displaying the actual selection on screen")]
    [SerializeField] private int voteProcessingTime;
    [SerializeField] private Slider voteProcessingSlider;
    [SerializeField] private GameObject voteTitle;          /// remove later if the vote title is in the vote screen
    [SerializeField] private string[] voteTitles;           /// remove later if the vote title is in the vote screen
    [SerializeField] private Sprite[] voteScreens;
    [SerializeField] private ButtonVR[] buttonScripts;

    // REMOVE FROM INSPECTOR LATER //
    [Header("Voting Odds")]
    [SerializeField] private int goodOdds;
    [SerializeField] private int badOdds;
    [SerializeField] private int randomNumber;

    [Header("Voting Table")]
    [Tooltip("time (in seconds) it takes for the table to flip over to the voting side")]
    [SerializeField] private float rotationDuration;
    [SerializeField] private GameObject coffee;
    [SerializeField] private GameObject pen;
    [SerializeField] private GameObject table;
    [SerializeField] private TableCollision tableCollision;

    [Header("Monitor Screens")]
    [SerializeField] private GameObject blankScreen;
    [SerializeField] private GameObject voteScreen;
    [SerializeField] private GameObject processingScreen;
    [SerializeField] private GameObject goodScreen;
    [SerializeField] private GameObject badScreen;


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

        coffee = GameObject.Find("Coffee");
        pen = GameObject.Find("Pen");
        tableCollision = table.GetComponent<TableCollision>();
    }

    void Update()
    {
        // TEMPORARY TESTING KEYBINDS //
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("PLAYER PICKED GOOD OPTION");

            PlayerPickedGoodOption();
        }

        // TEMPORARY TESTING KEYBINDS //
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("PLAYER PICKED BAD OPTION");

            PlayerPickedBadOption();
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

    public void ActivateButtons()
    {
        if (rotationDuration > 0)
        {
            rotationDuration -= Time.deltaTime;

            // rotates objects on x-axis at specified degrees per second
            if (tableCollision.coffeeOn == true)
            {
                coffee.GetComponent<Rigidbody>().isKinematic = true;
                coffee.transform.RotateAround(table.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
            }

            if (tableCollision.penOn == true)
            {
                pen.GetComponent<Rigidbody>().isKinematic = true;
                pen.transform.RotateAround(table.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
            }

            table.transform.RotateAround(table.transform.position, Vector3.right, Time.deltaTime * rotationSpeed);
        }
        else
        {
            activateVoting = false;
            activateButtons = false;
            rotationDuration = setRotationDuration;
            table.transform.eulerAngles = new Vector3(180, 0, 0);
        }
    }

    public void DeactivateVoting()
    {
        if (rotationDuration > 0)
        {
            rotationDuration -= Time.deltaTime;

            // rotates objects on x-axis at specified degrees per second
            if (tableCollision.coffeeOn == true)
            {
                coffee.GetComponent<Rigidbody>().isKinematic = true;
                coffee.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
            }

            if (tableCollision.penOn == true)
            {
                pen.GetComponent<Rigidbody>().isKinematic = true;
                pen.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
            }

            table.transform.RotateAround(table.transform.position, Vector3.left, Time.deltaTime * rotationSpeed);
        }
        else
        {
            deactivateVoting = false;
            rotationDuration = setRotationDuration;
            table.transform.eulerAngles = new Vector3(0, 0, 0);

            coffee.GetComponent<Rigidbody>().isKinematic = false;
            pen.GetComponent<Rigidbody>().isKinematic = false;

            voteNumber++;
            voteSwitched = false;

            foreach (ButtonVR scripts in buttonScripts)
            {
                scripts.enabled = true;
            }
        }
    }

    public void PlayerPickedGoodOption()
    {
        foreach (ButtonVR scripts in buttonScripts)
        {
            scripts.enabled = false;
        }

        randomNumber = Random.Range(0, 100);
        processVote = true;
        pickedGood = true;

        StartCoroutine("DelayDeactivate");
    }

    public void PlayerPickedBadOption()
    {
        foreach (ButtonVR scripts in buttonScripts)
        {
            scripts.enabled = false;
        }

        randomNumber = Random.Range(0, 100);
        processVote = true;
        pickedBad = true;

        StartCoroutine("DelayDeactivate");
    }

    IEnumerator DelayButtonActivate()
    {
        yield return new WaitForSeconds(buttonActivateDelay);

        activateButtons = true;
    }

    IEnumerator DelayDeactivate()
    {
        yield return new WaitForSeconds(voteDeactivateDelay);

        deactivateVoting = true;
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

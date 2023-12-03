using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count_Manager : MonoBehaviour
{
    [SerializeField] private Text textObj;
    [Tooltip("The number of packages that must be packed before the boss shows up.")]
    [SerializeField] private int packQuota;
    private static int packageQuota;
    private static int packageCount;
    //private static int totalPackaged;
    private static bool gameDone;
    private static bool bossHere;

    public delegate void BossAppearance();
    public static BossAppearance bossAppears;

    private void Start()
    {
        bossAppears += bossIsHere;

        packageQuota = packQuota;
        bossHere = false;
        resetCount();
        //totalPackaged = 0;
        gameDone = false;
    }

    private void Update()
    {
        if (!gameDone)
            textObj.text = (packageCount.ToString() + "/" + packageQuota.ToString());
        else
            textObj.text = packageCount.ToString();
    }

    public static void incrementCount()
    {
        if(/*!bossHere &&*/ packageCount < packageQuota) // UNCOMMENT once the boss part of the gameloop is complete
            packageCount++;
        //totalPackaged++;

        if (!gameDone && packageCount >= packageQuota) {
            bossAppears();
            resetCount(); // REMOVE once the boss part of the gameloop is complete
        }
    }

    public static void resetCount()
    {
        packageCount = 0;
        bossHere = false;
    }

    public void gameIsDone()
    {
        resetCount();
        gameDone = true;
    }

    public void bossIsHere()
    {
        bossHere = true;
    }
}

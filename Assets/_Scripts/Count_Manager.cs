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

    public delegate void BossAppearance();
    public static BossAppearance bossAppears;

    private void Start()
    {
        bossAppears += testDelegate;
        bossAppears += testDelegate2;
        packageQuota = packQuota;
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
        packageCount++;
        //totalPackaged++;

        if (!gameDone && packageCount >= packageQuota) {
            bossAppears();
            resetCount(); // TBD to move based on the delegate action thing
        }
    }

    public static void resetCount()
    {
        packageCount = 0;
    }

    public void gameIsDone()
    {
        resetCount();
        gameDone = true;
    }

    public void testDelegate()
    {
        print("delegate");
    }
    public void testDelegate2()
    {
        print("works");
    }
}

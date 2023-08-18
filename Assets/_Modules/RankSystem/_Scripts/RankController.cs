using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

#pragma warning disable 0414
#pragma warning disable 0649
public class RankController : MonoBehaviour
{
    [VerticalGroup("UI", PaddingTop = 5)]
    [FoldoutGroup("UI/UI References")]
    [SerializeField] GameObject ShowProgressPanel;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] Slider ProgressBar;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] GameObject itemPrefab;  
    [FoldoutGroup("UI/UI References")]
    [SerializeField] GameObject LevelUpPanel;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] GameObject ProgressPanel;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] GameObject UnlockedItems;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] Transform UnlockedItemsParent;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] TMP_Text CurrentLevel;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] TMP_Text NextLevel;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] TMP_Text XPIncrease;
    [FoldoutGroup("UI/UI References")]
    [SerializeField] GameObject XPObject;

    [Indent]

    [VerticalGroup("Sounds", PaddingTop = 5)]
    [FoldoutGroup("Sounds/Sounds")]
    [SerializeField] AudioClip LevelUpSound;
    [FoldoutGroup("Sounds/Sounds")]
    [SerializeField] AudioClip Progress;
    [FoldoutGroup("Sounds/Sounds")]
    [SerializeField] AudioSource source;


    [VerticalGroup("Rank", PaddingTop = 5)]
    [FoldoutGroup("Rank/Rank Information")]
    [SerializeField] RankInformation rankData;
    [VerticalGroup("Debug", PaddingTop = 5)]
    [FoldoutGroup("Debug/Debug Info")]
    [SerializeField] Rank currentRank;
    [FoldoutGroup("Debug/Debug Info")]
    [SerializeField] Rank nextRank;
    [FoldoutGroup("Debug/Debug Info")]
    [SerializeField] int currentLevel;
    [FoldoutGroup("Debug/Debug Info")]
    [SerializeField] int currentLevelXP;
    
    void Start()
    {
        OnGameStart();
        source = GetComponent<AudioSource>();
    }

    public static RankController Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    /// <summary>
    /// Initialization of RankController, need to call in start of the game
    /// </summary>
    public void OnGameStart()
    {
        currentLevelXP = 0;
        currentLevel = -1;
        ShowProgress(0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ShowProgress(30);
        }
    }

    /// <summary>
    /// Checks if the XP requirement for next level is complete
    /// </summary>
    /// <param name="xp"></param>
    /// <returns></returns>
    public bool CheckRank(int xp)
    {
        if (xp >= nextRank.requiredXP)
            return true;
        else
            return false;
    }


    /// <summary>
    /// Increases the current progress and checks if it's a level Up
    /// </summary>
    /// <param name="xp">the xp gained after every task</param>
    public void ShowProgress(int xp)
    {
        Debug.Log("currentLevelXP"+ xp);
        //currentLevelXP += xp;
        //ProgressBar.value = currentLevelXP;
        ShowProgressPanel.SetActive(true);
        ShowLevelUpUI(false);
        source.clip = Progress;
        source.Play();
        XPObject.gameObject.SetActive(true);
        XPIncrease.GetComponent<TextMeshProUGUI>().DOCounter(currentLevelXP, currentLevelXP + xp, 2).OnComplete(() =>
        {
            XPObject.transform.DOPunchScale(new Vector3(1, 1, 1), 1, 1, 0).OnComplete(() =>
            {
                XPObject.gameObject.SetActive(false);
            });
        });
        
       

        StartCoroutine(IncreaseProgressBar(xp));
        
    }

    IEnumerator IncreaseProgressBar(int xp)
    {
        Debug.Log(xp);
        int i = currentLevelXP;
        int totalIncrease = currentLevelXP + xp;
        Debug.Log("Total Increase" + totalIncrease);
        while (i<=totalIncrease)
        {           
            yield return new WaitForSeconds(0.01f);
            
            
            ProgressBar.value = i++;
        }
        currentLevelXP += xp;
        if (CheckRank(currentLevelXP))
        {
            LevelUp();
        }
    }
    /// <summary>
    /// Increases the level and Upgrades the Progress Bar
    /// </summary>
    public void LevelUp()
    {
        source.clip = LevelUpSound;
        source.Play();
        if(currentLevelXP!=0)
            ShowLevelUpUI(true);
        currentLevelXP = currentLevelXP - nextRank.requiredXP;
        currentLevel++;
        CurrentLevel.text = currentLevel.ToString();
        NextLevel.text = (currentLevel+1).ToString();
        rankData.rankInfo.rankList[currentLevel+1].requiredXP = GetNextLevelRequiredXP();
        currentRank = nextRank;
        nextRank = rankData.rankInfo.rankList[currentLevel+1];
        SetProgressBarValue(nextRank.requiredXP);

        //Destroy Previous Objects
        if (UnlockedItemsParent.childCount != 0)
        {
            foreach (Transform child in UnlockedItemsParent)
                Destroy(child.gameObject);
        }

       //Instantiate Objects 
        if(rankData.rankInfo.rankList[currentLevel].unlockedObjects.Count !=0)
        {
            foreach (var obj in rankData.rankInfo.rankList[currentLevel].unlockedObjects)
            { 
                GameObject item_obj = Instantiate(itemPrefab, UnlockedItemsParent);
                Item item = obj;
                item_obj.GetComponentInChildren<Image>().sprite = GetImage(item.ItemName);
                item_obj.GetComponentInChildren<TMP_Text>().text = item.ItemName;
                
            }
        }
    }


    /// <summary>
    /// Calculates the Next Level XP Requirement
    /// </summary>
    /// <returns>returns XP required for Next Level </returns>
    public int GetNextLevelRequiredXP()
    {
        int xpLevelUp = 5 * (currentLevel ^ 2) + (50 * currentLevel) + 100 - 0;
        return xpLevelUp;
        
    }
    /// <summary>
    /// Resets the Progress for Next Level
    /// </summary>
    /// <param name="requiredXP">XP required for next level</param>
    void SetProgressBarValue(int requiredXP)
    {
        ProgressBar.minValue = 0;
        ProgressBar.maxValue = requiredXP;        
        ProgressBar.value = currentLevelXP;
    }

    public void ShowLevelUpUI(bool show)
    {
        LevelUpPanel.SetActive(show);
        if(show)
        {
            UnlockedItems.SetActive(true);
            UnlockedItems.GetComponent<RectTransform>().DOPunchScale(new Vector3(-1, -1, -1), 1, 1, 1);
        }
        else
        {
            UnlockedItems.SetActive(false);
            /*if (UnlockedItems.activeSelf)
            {
                UnlockedItems.GetComponent<RectTransform>().DOAnchorPosY(-73, 1, true).OnComplete(() =>
                {
                    UnlockedItems.SetActive(false);
                });
            }*/
        }
    }

    public void ClosePanel()
    {
        ShowProgressPanel.SetActive(false);
    }
    public Sprite GetImage(string name)
    {
        return Resources.Load<Sprite>("Images/" + name);
    }
}

[System.Serializable]
public class Rank
{

    public List<Item> unlockedObjects;
    public int requiredXP;
}

[System.Serializable]
public class Item
{
    // Start is called before the first frame update
    public string ItemName;
    public int id;


    public Item()
    {

    }
    public void setItem(Item item)
    {
        ItemName = item.ItemName;
        id = item.id;
    }
    public virtual void PrintAttribute()
    {

        Debug.Log("Item Name:" + ItemName);
    }


}



using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI coinsText;
    
    private int coinsCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Coins"))
        {
            PlayerPrefsSaveData.SaveInt("Coins", 0);
        }
        else
        {
            coinsCount = PlayerPrefsLoadData.LoadInt("Coins");
        }
        ShowCoins();
    }
    public void AddCoins(int coinsAdd)
    {
        coinsCount += coinsAdd;
        PlayerPrefsSaveData.SaveInt("Coins", coinsCount);
        ShowCoins();
    }
    public void SubstractCoins(int coinsSubstract)
    {
        coinsCount -= coinsSubstract;
        PlayerPrefsSaveData.SaveInt("Coins", coinsCount);
        ShowCoins();
    }
    private void ShowCoins()
    {
        coinsText.text = coinsCount.ToString();
    }
  
}

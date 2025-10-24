using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelFortuneTimer : MonoBehaviour
{
    [SerializeField] private Button spinButton;

    [SerializeField] private TextMeshProUGUI timerText;
    
    private const string TimerKey = "WheelTimer";
    
    private const float TimerDuration = 5 * 60f;
    
    private float remainingTime;
    
    private bool isTimerRunning = false;

    private void Start()
    {
        spinButton.interactable = true;
        timerText.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey(TimerKey))
        {
            string lastSpinStr = PlayerPrefsLoadData.LoadString(TimerKey);
            if (DateTime.TryParse(lastSpinStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out DateTime lastSpinTime))
            {
                float elapsed = (float)(DateTime.UtcNow - lastSpinTime).TotalSeconds;
                remainingTime = TimerDuration - elapsed;

                if (remainingTime > 0)
                {
                    ActivateTimer();
                }
                else
                {
                    TimerFinished();
                }
            }
            else
            {
                PlayerPrefsSaveData.DeleteKey(TimerKey);
            }
        }
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (remainingTime <= 0)
        {
            TimerFinished();
        }
    }

    public void StartTimer()
    {
        remainingTime = TimerDuration;
        ActivateTimer();

        PlayerPrefsSaveData.SaveString(TimerKey, DateTime.UtcNow.ToString());
    }

    private void ActivateTimer()
    {
        isTimerRunning = true;
        spinButton.interactable = false;
        timerText.gameObject.SetActive(true);
        UpdateTimerUI();
    }

    private void TimerFinished()
    {
        isTimerRunning = false;
        spinButton.interactable = true;
        timerText.gameObject.SetActive(false);
        PlayerPrefsSaveData.DeleteKey(TimerKey);
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(Mathf.Max(remainingTime, 0) / 60);
        int seconds = Mathf.FloorToInt(Mathf.Max(remainingTime, 0) % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}

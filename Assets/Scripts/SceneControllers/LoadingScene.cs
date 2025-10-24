using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    private async void Start()
    {
        await InitializeModulesAsync();
        await LoadSceneAsync();
    }

    private void SetProgress(float value)
    {
        value = Mathf.Clamp01(value);
        if (progressBar != null)
        {
            progressBar.fillAmount = value;
        }

        if (progressText != null)
        {
            progressText.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }
    private async Task InitializeModulesAsync()
    {
        MonoBehaviour[] allBehaviours = FindObjectsOfType<MonoBehaviour>(true);
        List<IInitialize> modules = new List<IInitialize>();
        foreach (var mb in allBehaviours)
        {
            if (mb is IInitialize init)
            {
                modules.Add(init);
            }
        }

        int totalModules = modules.Count;
        int completedModules = 0;

        foreach (var module in modules)
        {
            await module.InitializeAsync();
            completedModules++;
            float progress = completedModules / (float)(totalModules + 1);
            SetProgress(progress);
        }
    }

    private async Task LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Minigame");
        asyncLoad.allowSceneActivation = false;

        int totalModules = FindObjectsOfType<MonoBehaviour>(true).Length;
        int completedModules = totalModules;

        while (!asyncLoad.isDone)
        {
            float sceneProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float progress = (completedModules + sceneProgress) / (totalModules + 1);
            SetProgress(progress);

            if (asyncLoad.progress >= 0.9f)
            {
                SetProgress(1f);
                asyncLoad.allowSceneActivation = true;
            }
            await Task.Yield();
        }
    }
}

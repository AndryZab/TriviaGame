using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    public static UImanager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ActivateCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.enabled = false;
        canvasGroup.blocksRaycasts = true;
    }
    public void DeactivateCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.enabled = true;
        canvasGroup.blocksRaycasts = false;
    }
}

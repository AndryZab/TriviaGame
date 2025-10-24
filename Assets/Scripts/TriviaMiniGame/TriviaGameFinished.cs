using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaGameFinished : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelVictory;
    [SerializeField] private CanvasGroup panelLosing;

    [SerializeField] private Animator panelVictoryAnimator;
    [SerializeField] private Animator panelLosingAnimator;
    private void OnEnable()
    {
        EventManager.OnVictory += Victory;
        EventManager.OnLosing += Losing;

    }
    private void OnDisable()
    {
        EventManager.OnVictory -= Victory;
        EventManager.OnLosing -= Losing;
    }

    private void Victory()
    {
        panelVictory.enabled = false;
        panelVictory.blocksRaycasts = true;
        panelVictoryAnimator.enabled = true;
    }
    private void Losing()
    {
        panelLosing.enabled = false;
        panelLosing.blocksRaycasts = true;
        panelLosingAnimator.enabled = true;
    }
}

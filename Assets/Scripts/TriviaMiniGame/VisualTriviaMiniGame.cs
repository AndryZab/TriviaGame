using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class VisualTriviaMiniGame : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private string animTriggerNameNextQuestion;
    [SerializeField] private string animTriggerNameGameEnd;

    private Animator animator;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 15f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(animTriggerNameNextQuestion);
    }
    private void TriggerAnim(int remainingQuestion)
    {
        if (remainingQuestion <= 0)
        {
            animator.SetTrigger(animTriggerNameGameEnd);
        }
        else
        {
            animator.SetTrigger(animTriggerNameNextQuestion);
        }
    }
    public void AnimateButtonCorrectAnswer(Button button, int remainingQuestion)
    {
        AnimateButton(button, remainingQuestion, Color.green, useScale: true);
    }

    public void AnimateButtonIncorrectAnswer(Button button, int remainingQuestion)
    {
        AnimateButton(button, remainingQuestion, Color.red, useShake: true);
    }
    public void AnimateButton(Button button, int remainingQuestion, Color targetColor, bool useShake = false, bool useScale = false)
    {
        if (!button) return;

        RectTransform rect = button.GetComponent<RectTransform>();
        Image img = button.GetComponent<Image>();

        if (rect == null || img == null) return;

        rect.DOKill(true);
        img.DOKill(true);

        if (useScale)
        {
            rect.DOScale(1.1f, shakeDuration / 2).SetLoops(2, LoopType.Yoyo);
            rect.DORotate(new Vector3(0, 0, shakeStrength / 2), shakeDuration / 4)
                .SetLoops(2, LoopType.Yoyo);
        }

        if (useShake)
        {
            rect.DOShakeAnchorPos(
                shakeDuration,
                new Vector2(shakeStrength, shakeStrength),
                shakeVibrato,
                shakeRandomness,
                false
            );

            rect.DORotate(new Vector3(0, 0, shakeStrength), shakeDuration / 4)
                .SetLoops(2, LoopType.Yoyo);
        }

        img.DOColor(targetColor, shakeDuration / 2)
           .OnComplete(() =>
           {
               img.DOColor(Color.white, shakeDuration / 2)
                  .OnComplete(() => TriggerAnim(remainingQuestion));
           });
    }

}

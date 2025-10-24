using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriviaMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject raycastBlocker;

    [Header("Texts Setting")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    
    [SerializeField] private TriviaData triviaData;
    [SerializeField] private string key = "AnswersCount";

    private List<Question> remainingQuestions;
    
    private Question currentQuestion;
    private VisualTriviaMiniGame visualTriviaMiniGame;
    public int currentCorrectCount { get; private set; } = 0;

    private int requiredCorrectAnswers = 0;

    private void Start()
    {
        visualTriviaMiniGame = GetComponent<VisualTriviaMiniGame>();

        FirebaseLoadData.Instance.GetIntKey(count =>
        {
            requiredCorrectAnswers = count;
            Debug.Log($"{key} = {requiredCorrectAnswers}");
        }, key);

        remainingQuestions = new List<Question>(triviaData.questions);
        ShuffleList(remainingQuestions);

    }

    public void ShowRandomQuestion() // called in event animation
    {
        if (remainingQuestions.Count == 0) return;

        currentQuestion = remainingQuestions[0];
        remainingQuestions.RemoveAt(0);

        questionText.text = currentQuestion.questionText;

        if (currentQuestion.answers.Length != answerTexts.Length) return;

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = currentQuestion.answers[i].answerText;
        }
    }

    public void CheckAnswer(int answerIndex)
    {
        if (answerIndex < 0 || answerIndex >= currentQuestion.answers.Length) return;

        GameObject clickedObj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Button clickedButton = clickedObj?.GetComponent<Button>();

        if (clickedButton != null)
        {
            bool isCorrect = currentQuestion.answers[answerIndex].isCorrect;

            if (isCorrect)
            {
                currentCorrectCount++;
                visualTriviaMiniGame.AnimateButtonCorrectAnswer(clickedButton, remainingQuestions.Count);
            }
            else
            {
                visualTriviaMiniGame.AnimateButtonIncorrectAnswer(clickedButton, remainingQuestions.Count);
            }
        }
        
        EnableRaycastBlocker();
        CheckGameEnd();
    }
    private void CheckGameEnd()
    {
        if (remainingQuestions.Count == 0)
        {
            int totalQuestions = triviaData.questions.Length;

            if (currentCorrectCount >= requiredCorrectAnswers && currentCorrectCount <= totalQuestions)
            {
                EventManager.EventOnVictory();
            }
            else
            {
                EventManager.EventOnLosing();
            }
        }
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void DisableRaycastBlocker()
    {
        raycastBlocker.SetActive(false);
    }
    public void EnableRaycastBlocker()
    {
        raycastBlocker.SetActive(true);
    }
}

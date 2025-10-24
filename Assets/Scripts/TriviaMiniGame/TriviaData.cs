using UnityEngine;

[System.Serializable]
public class Answer
{
    public string answerText;
    public bool isCorrect;
}

[System.Serializable]
public class Question
{
    public string questionText;
    public Answer[] answers;
}

[CreateAssetMenu(fileName = "NewTriviaData", menuName = "Trivia/QuestionData")]
public class TriviaData : ScriptableObject
{
    [Header("All Questions")]
    public Question[] questions;
}

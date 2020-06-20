using System;
using System.Collections.Generic;


[Serializable]
public class Question
{
    public string questionText;
    public List<string> possibleAnswers;
    public int correctAnswer;
    public int selectedAnswer = -1;
    public bool selectedToReview = false;
    public int questionNum;

    public override string ToString()
    {
        string decoration = selectedToReview ? "*" : "";
        return $"{decoration}Question {questionNum}{decoration}";
    }
}
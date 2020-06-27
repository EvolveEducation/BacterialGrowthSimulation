using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizHandler : MonoBehaviour
{
    private Quiz currentQuiz { get; set; }
    private TMP_Dropdown questionDropdown;
    private int currentQuestionNum { get; set; }
    private List<int> questionsToReview;

    public GameObject answerPanel;
    public GameObject possibleAnswerButtonTemplate;
    public GameObject questionTitle;
    public GameObject nextBtn;
    public GameObject backBtn;
    public GameObject scoreTxt;
    public GameObject quizTitleTxt;
    public GameObject browseBtn;
    public GameObject questionInput;
    public GameObject markForReviewBtn;
    public GameObject confirmTitle;
    public GameObject unansweredQuestionsTxt;
    public GameObject flaggedQuestionsTxt;
    public GameObject confirmBackBtn;
    public GameObject confirmBtn;

    // Start is called before the first frame update
    void Start()
    {
        SetQuizActive(false);
        SetResultActive(false);
        SetInitialActive(true);
        SetConfirmActive(false);

        BrowseButton browseBtnComponent = browseBtn.GetComponent<BrowseButton>();
        browseBtnComponent.onLoad.AddListener(OnLoadQuiz);

        Button nextBtnBtn = nextBtn.GetComponent<Button>();
        nextBtnBtn.onClick.AddListener(OnNextClicked);

        Button backBtnBtn = backBtn.GetComponent<Button>();
        backBtnBtn.onClick.AddListener(OnBackClicked);

        Button markReviewBtn = markForReviewBtn.GetComponent<Button>();
        markReviewBtn.onClick.AddListener(OnMarkForReviewClicked);

        Button confirmSubmitBtn = confirmBtn.GetComponent<Button>();
        confirmSubmitBtn.onClick.AddListener(OnConfirmBtnClicked);

        Button confirmSubmitBackBtn = confirmBackBtn.GetComponent<Button>();
        confirmSubmitBackBtn.onClick.AddListener(OnConfirmBackClicked);

        questionDropdown = questionInput.GetComponent<TMP_Dropdown>();
        questionDropdown.onValueChanged.AddListener(QuestionSelectValueChanged);

    }

    void SetQuizActive(bool active)
    {
        questionTitle.SetActive(active);
        nextBtn.SetActive(active);
        backBtn.SetActive(active);
        answerPanel.SetActive(active);
        questionInput.SetActive(active);
        markForReviewBtn.SetActive(active);
    }

    void SetResultActive(bool active)
    {
        scoreTxt.SetActive(active);
    }

    void SetInitialActive(bool active)
    {
        quizTitleTxt.SetActive(active);
        browseBtn.SetActive(active);
    }

    void SetConfirmActive(bool active)
    {
        confirmTitle.SetActive(active);
        unansweredQuestionsTxt.SetActive(active);
        flaggedQuestionsTxt.SetActive(active);
        confirmBackBtn.SetActive(active);
        confirmBtn.SetActive(active);
    }

    void OnLoadQuiz()
    {
        BrowseButton browseBtnComponent = browseBtn.GetComponent<BrowseButton>();
        byte[] decodedBytes = Convert.FromBase64String(browseBtnComponent.json);
        string decodedStr = Encoding.UTF8.GetString(decodedBytes);
        currentQuiz = JsonUtility.FromJson<Quiz>(decodedStr);

        int i = 1;
        foreach (Question q in currentQuiz.questions)
        {
            q.questionNum = i++;
        }

        RenderQuestionsList();

        SetInitialActive(false);
        SetQuizActive(true);
        DisplayCurrentQuestion();
    }

    void RenderQuestionsList()
    {
        questionDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (Question q in currentQuiz.questions)
        {
            options.Add(q.ToString());
        }
        questionDropdown.AddOptions(options);

        questionDropdown.value = currentQuestionNum;
    }

    void DisplayCurrentQuestion()
    {
        questionDropdown.value = currentQuestionNum;
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        CheckQuizQuestionCount(currentQuestion);

        questionTitle.GetComponent<TextMeshProUGUI>().text = currentQuestion.questionText;

        for (int i = 0; i < currentQuestion.possibleAnswers.Count; i++)
        {
            GameObject obj = answerPanel.transform.GetChild(i).gameObject;
            obj.GetComponentInChildren<Text>().text = currentQuestion.possibleAnswers[i];
        }

        if (currentQuestion.selectedAnswer != -1)
        {
            SetAnswerSelected(currentQuestion.selectedAnswer);
        }
        CheckLastQuestion();
        UpdateMarkForReviewBtn();
    }

    void SetAnswerSelected(int answerIndex, bool selected = true)
    {
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        if (selected && currentQuestion.selectedAnswer != -1)
        {
            SetAnswerSelected(currentQuestion.selectedAnswer, false);
        }
        if (answerIndex < 0)
        {
            return;
        }
        Image img = answerPanel.transform.GetChild(answerIndex).GetComponent<Image>();
        if (selected)
        {
            img.color = new Color(255, 255, 0, 255);
        }
        else
        {
            img.color = new Color(255, 255, 255, 255);
        }
    }

    void CheckQuizQuestionCount(Question currentQuestion)
    {
        int childCount = answerPanel.transform.childCount;
        int questionCount = currentQuestion.possibleAnswers.Count;
        int diff = childCount - questionCount;
        if (diff != 0)
        {
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    Destroy(answerPanel.transform.GetChild(i).gameObject);
                }
            }
            else
            {
                diff *= -1;
                for (int i = 0; i < diff; i++)
                {
                    int questionNum = i;
                    GameObject gb = Instantiate(possibleAnswerButtonTemplate, answerPanel.transform);
                    gb.SetActive(true);
                    Button btn = gb.GetComponent<Button>();
                    btn.onClick.AddListener(() => OnAnswerClicked(questionNum));
                }
            }
        }
    }

    void OnAnswerClicked(int answerIndex)
    {
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        SetAnswerSelected(answerIndex);
        currentQuestion.selectedAnswer = answerIndex;
    }

    void OnNextClicked()
    {
        if (CheckLastQuestion())
        {
            int unanswered = 0;
            int flagged = 0;
            foreach (Question q in currentQuiz.questions)
            {
                if (q.selectedAnswer == -1)
                {
                    unanswered++;
                }
                if (q.selectedToReview)
                {
                    flagged++;
                }
            }
            unansweredQuestionsTxt.GetComponent<TextMeshProUGUI>().text = $"You have {unanswered} unanswered questions.";
            flaggedQuestionsTxt.GetComponent<TextMeshProUGUI>().text = $"You have {flagged} questions flagged for review.";
            SetQuizActive(false);
            SetConfirmActive(true);
            return;
        }
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        SetAnswerSelected(currentQuestion.selectedAnswer, false);
        currentQuestionNum++;
        DisplayCurrentQuestion();
    }

    int CalculateScore()
    {
        int score = 0;
        for (int i = 0; i < currentQuiz.questions.Count; i++)
        {
            Question currentQuestion = currentQuiz.questions[i];
            if (currentQuestion.selectedAnswer == currentQuestion.correctAnswer)
            {
                score++;
            }
        }
        return score;
    }

    bool CheckLastQuestion()
    {
        if (currentQuestionNum == currentQuiz.questions.Count - 1)
        {
            nextBtn.GetComponentInChildren<TMP_Text>().text = "Submit";
            return true;
        }
        else
        {
            nextBtn.GetComponentInChildren<TMP_Text>().text = "Next";
            return false;
        }
    }

    void OnBackClicked()
    {
        if (currentQuestionNum <= 0)
        {
            return;
        }
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        SetAnswerSelected(currentQuestion.selectedAnswer, false);
        currentQuestionNum--;
        DisplayCurrentQuestion();
    }

    void QuestionSelectValueChanged(int newValue)
    {
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        SetAnswerSelected(currentQuestion.selectedAnswer, false);
        currentQuestionNum = newValue;
        DisplayCurrentQuestion();
    }

    void OnMarkForReviewClicked()
    {
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        currentQuestion.selectedToReview = !currentQuestion.selectedToReview;
        UpdateMarkForReviewBtn();
        RenderQuestionsList();
    }

    void UpdateMarkForReviewBtn()
    {
        Question currentQuestion = currentQuiz.questions[currentQuestionNum];
        TextMeshProUGUI text = markForReviewBtn.GetComponentInChildren<TextMeshProUGUI>();
        text.text = currentQuestion.selectedToReview ? "Unflag for Review" : "Flag for Review";
    }

    void OnConfirmBtnClicked()
    {
        int score = CalculateScore();
        scoreTxt.GetComponent<TextMeshProUGUI>().text = $"You scored {score}/{currentQuiz.questions.Count}";
        SetConfirmActive(false);
        SetResultActive(true);
    }

    void OnConfirmBackClicked()
    {
        SetConfirmActive(false);
        SetQuizActive(true);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TestSystem : MonoBehaviour
{
    public ExamData examData;
    [System.Serializable] 
    private struct ButtonUI {
        public Button button;
        public TextMeshProUGUI textUI;
    }
    [SerializeField] private List<ButtonUI> answersUI;
    public TextMeshProUGUI questionText;

    private int curQuestionInd = 0;
    private int numCorrect = 0;
    private int total_questions =0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        total_questions = examData.questions.Count;
        DisplayNextQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayNextQuestion() {
        ExamData.ExamQuestion cur_q = examData.questions[curQuestionInd];
        questionText.text = cur_q.question;
        List<string> shuffled_answers = new List<string>(cur_q.answers);
        for (int i = 0; i < shuffled_answers.Count; i++) {
            int ind = Random.Range(i, shuffled_answers.Count);
            string temp = shuffled_answers[i];
            shuffled_answers[i] = shuffled_answers[ind];
            shuffled_answers[ind] = temp;
        }
        for (int i = 0; i < answersUI.Count; i++) {
            string answer = shuffled_answers[i];
            answersUI[i].textUI.text = answer;
            answersUI[i].button.onClick.RemoveAllListeners();
            answersUI[i].button.onClick.AddListener(() => SubmitAnswer(answer));
        }
    }
    public void SubmitAnswer(string selected_answer) {
        if (curQuestionInd > total_questions) return;
        if (selected_answer == examData.questions[curQuestionInd].correct_answer) numCorrect+=1;
        if (curQuestionInd+1 == total_questions) {
            questionText.text = "PUT EXAM IN BIN ON TEACHER'S DESK";
            for (int i = 0; i < answersUI.Count; i++) {
                answersUI[i].button.interactable = false;
                answersUI[i].textUI.text = "PUT EXAM IN BIN ON TEACHER'S DESK";
            }
        } else {
            curQuestionInd+=1;
            DisplayNextQuestion();
        }
    }
    public bool GradeExam() {
        return numCorrect == total_questions;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class TestSystem : MonoBehaviour
{
    public struct ExamQuestion {
        public string question;
        public string[] answers;
        public string correct_answer;
    }
    public List<ExamQuestion> questions = new List<ExamQuestion>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "newTest", menuName = "test/ExamData")]
public class ExamData : ScriptableObject
{
    [System.Serializable]
    public struct ExamQuestion {
        public string question;
        public string[] answers;
        public string correct_answer;
    }
    public List<ExamQuestion> questions = new List<ExamQuestion>();
}

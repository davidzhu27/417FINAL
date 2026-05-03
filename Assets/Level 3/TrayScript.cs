using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrayScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boardText;
    private bool triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other) {
        if (triggered) return;
        TestSystem exam_sys = other.gameObject.GetComponent<TestSystem>();
        if (exam_sys != null) {
            if (exam_sys.GradeExam()) {
                triggered = true;
                boardText.text = "You Pass!\n Congrats on surviving a day at school!";
            } else {
                boardText.text = "You Fail!\n Good luck on the next attempt!";
                StartCoroutine(FailLevel());
                
            }
        }
    }
    public IEnumerator FailLevel() {
       yield return new WaitForSecondsRealtime(2);
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}

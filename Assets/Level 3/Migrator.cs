using UnityEngine;
using UnityEditor; // Required for AssetDatabase and MenuItems

public class DataMigrator 
{
    // This creates a new menu at the top of Unity called "Tools"
    [MenuItem("Tools/Migrate Exam Data")]
    public static void Migrate()
    {
        // 1. Create the instance
        ExamData newExam = ScriptableObject.CreateInstance<ExamData>();

        // 2. Add your existing hard-coded questions
        newExam.questions.Add(new ExamData.ExamQuestion {question = "Why does Oculus recommend against traditional HUDs in VR games?", 
            answers = new string[] {"Traditional HUDs are too resource intensive for stereoscopic VR headsets", 
            "HUDs block peripheral vision, leading to less situational awareness", 
            "HUD occlusion interferes with binocular disparity depth cues",
            "HUDs cannot be rendered in stereoscopic 3D and will always appear flat"}, 
            correct_answer = "HUD occlusion interferes with binocular disparity depth cues"});
        // ... repeat for your other questions ...
        newExam.questions.Add(new ExamData.ExamQuestion {question = "Vection is often described as a major contributor to simulator sickness. Define vection.",
            answers = new string[] {"Physical sensation of dizziness caused by rapid head movements in VR", 
            "Mismatch between refresh rate of display and rendering frame rate",
            "Illusory perception of self-motion caused only by visual input",
            "Eye strain due to accomodation-convergence conflict in stereoscopic display"}, 
            correct_answer = "Illusory perception of self-motion caused only by visual input"});
        newExam.questions.Add(new ExamData.ExamQuestion{question = "Why are instantaneous accelerations preferred over gradual ones in VR?", 
            answers = new string[] {"Instantaneous accelerations are below the human perception threshold, so users will not see them",
            "Any period of acceleration creates sensory conflict between vision and vestibular systems, so a shorter duration reduces conflict",
            "Gradual accelerations cause the vestibular-occular reflex to malfunction, while instantaneous ones do not", 
            "Instantaneous accelerations allows the game engine's predictive tracking to compensate effects more effectively"},
            correct_answer = "Any period of acceleration creates sensory conflict between vision and vestibular systems, so a shorter duration reduces conflict"});

        // 3. Save it as a permanent asset file
        AssetDatabase.CreateAsset(newExam, "Assets/Level 3/VR_Exam_Data.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(); // Makes the file show up immediately
        
        Debug.Log("Success! Created Assets/Level 3/VR_Exam_Data.asset");
    }
}
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChairScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NpcSit(SelectEnterEventArgs args) {
        GameObject npc = args.interactableObject.transform.gameObject;
        npc.GetComponent<Person>().Sit();
    }
}

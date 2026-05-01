using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Required for VR Interaction

namespace SojaExiles
{
    // Adding this ensures the object always has an AudioSource
    [RequireComponent(typeof(AudioSource))]
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open = false;
        
        [Header("Audio Settings")]
        public AudioSource audioSource;
        public AudioClip doorSound; // Assign your creak sound here

        private bool isProcessing = false;
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

        void Start()
        {
            if (openandclose == null) openandclose = GetComponent<Animator>();
            if (audioSource == null) audioSource = GetComponent<AudioSource>();

            // Setup XR Interaction
            interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            if (interactable != null)
            {
                // This listens for the OpenXR "Select" trigger (Grip or Trigger button)
                interactable.selectEntered.AddListener(OnDoorClicked);
            }
        }

        private void OnDestroy()
        {
            // Clean up listener to prevent memory leaks
            if (interactable != null)
                interactable.selectEntered.RemoveListener(OnDoorClicked);
        }

        // This replaces the OnMouseOver logic for VR
        private void OnDoorClicked(SelectEnterEventArgs args)
        {
            if (isProcessing) return;

            if (!open)
            {
                StartCoroutine(opening());
            }
            else
            {
                StartCoroutine(closing());
            }
        }

        IEnumerator opening()
        {
            isProcessing = true;
            
            // Play Sound
            if (doorSound != null) audioSource.PlayOneShot(doorSound);
            
            openandclose.Play("Opening");
            open = true;
            
            yield return new WaitForSeconds(0.5f);
            isProcessing = false;
        }

        IEnumerator closing()
        {
            isProcessing = true;

            // Play Sound
            if (doorSound != null) audioSource.PlayOneShot(doorSound);

            openandclose.Play("Closing");
            open = false;

            yield return new WaitForSeconds(0.5f);
            isProcessing = false;
        }
    }
}
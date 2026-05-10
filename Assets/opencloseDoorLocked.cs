using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Required for VR Interaction

namespace SojaExiles
{
    // Adding this ensures the object always has an AudioSource
    [RequireComponent(typeof(AudioSource))]
    public class opencloseDoorLocked : MonoBehaviour
    {
        public Animator openandclose;
        public bool open = false;
        public InventoryManager manager;

        [Header("Lock Settings")]
        public bool isLocked = true;

        [Header("Audio Clips")]
        public AudioClip lockedSound;   // when player DOESN'T have key
        public AudioClip unlockSound;   // when player DOES have key

        [Header("Key Settings")]
        public string keyTag = "Key"; // tag your key object as "Key"
        
        [Header("Audio Settings")]
        public AudioSource audioSource;

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

            // Check if interactor is holding a key
            bool hasKey = false;

            var interactorObject = args.interactorObject;

            // Check if it's a direct interactor (hand grab)
            var directInteractor = interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor;
            if (manager != null)
            {
                if(manager.hotSwapSlots[manager.selectedHotSwapIndex] != null && manager.hotSwapSlots[manager.selectedHotSwapIndex].itemName == "Key")
                    hasKey = true;

                else    
                    hasKey = false;
            }

            // 🚪 LOCK LOGIC
            if (isLocked)
            {
                if (!hasKey)
                {
                    // ❌ No key → play locked sound
                    if (lockedSound != null)
                        audioSource.PlayOneShot(lockedSound);

                    return;
                }
                else
                {
                    // 🔓 Has key → unlock + play sound
                    isLocked = false;

                    if (unlockSound != null)
                        audioSource.PlayOneShot(unlockSound);
                }
            }

    // Normal open/close behavior
                StartCoroutine(open ? closing() : opening());
            }

        IEnumerator opening()
        {
            isProcessing = true;
            
            // Play Sound
            if (unlockSound != null) audioSource.PlayOneShot(unlockSound);
            
            openandclose.Play("Opening");
            open = true;
            
            yield return new WaitForSeconds(0.5f);
            isProcessing = false;
        }

        IEnumerator closing()
        {
            isProcessing = true;

            // Play Sound
            if (unlockSound != null) audioSource.PlayOneShot(unlockSound);

            openandclose.Play("Closing");
            open = false;

            yield return new WaitForSeconds(0.5f);
            isProcessing = false;
        }
    }
}
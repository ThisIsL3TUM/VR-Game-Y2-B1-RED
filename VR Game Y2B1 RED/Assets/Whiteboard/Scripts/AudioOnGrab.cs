using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabClip;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();//getting grab script
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnGrab);//once grabbed call function
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnGrab);//preventing buggy behaviour/scene clean up
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (audioSource != null && grabClip != null)//checks to prevent errors
        {
            audioSource.PlayOneShot(grabClip);//play sound
        }
    }
}
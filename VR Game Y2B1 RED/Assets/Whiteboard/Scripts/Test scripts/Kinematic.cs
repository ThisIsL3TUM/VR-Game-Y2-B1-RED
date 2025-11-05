using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Kinematic : MonoBehaviour
{
    Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        
        StartCoroutine(MakeNonKinematicNextFrame());//coroutine to override XR toolkit’s auto-setting
    }

    private System.Collections.IEnumerator MakeNonKinematicNextFrame()
    {
        yield return null; //wait a frame
        rb.isKinematic = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false;//turning off kinematic
    }
}

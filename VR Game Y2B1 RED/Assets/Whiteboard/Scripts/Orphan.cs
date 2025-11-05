using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Orphan : MonoBehaviour
{
    Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        rb = GetComponent<Rigidbody>();//getting rigidbody
    }
    void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();//getting the xrgrab script
        
        grabInteractable.selectExited.AddListener(OnRelease);//upon releasing call function
    }

    

    private void OnRelease(SelectExitEventArgs args)
    {
        rb.transform.parent = null;//make the object lose its parent
    }
}

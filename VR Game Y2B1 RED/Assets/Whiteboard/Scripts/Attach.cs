using UnityEngine;
using UnityEngine.InputSystem;
public class DirtCarry : MonoBehaviour
{
    private GameObject carriedDirt = null; // dirt currently attached
    public Transform carryPoint;           // empty child object on hoe where dirt should attach
    public InputActionReference customButton;
    public bool Attached = false;

    //in start we get our input action for dropping dirt
    private void Start()
    {
        customButton.action.started += Drop;//subcribing our input to the function
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If hoe hits dirt and we are not carrying anything yet
        if (carriedDirt == null && collision.gameObject.CompareTag("Dirt"))
        {
            PickUp(collision.gameObject);
        }
    }

    void PickUp(GameObject dirt)
    {
        Attached = true;
        carriedDirt = dirt;

        // Disable physics while carried
        Rigidbody rb = dirt.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Attach to hoe's carry point
        //and make sure the picked up dirt doesnt get deformed
        dirt.transform.SetParent(carryPoint, true);
        dirt.transform.localPosition = Vector3.zero;
        dirt.transform.localRotation = Quaternion.identity;
        
    }

    void Drop(InputAction.CallbackContext context)
    {
        if (carriedDirt != null)
        {
            
            // Re-enable physics
            Rigidbody rb = carriedDirt.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            // Detach
            carriedDirt.transform.SetParent(null);
            carriedDirt = null;
            Attached = false;
        }
    }

    private void Update()
    {
        
        

    }
}
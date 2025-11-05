using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Create : MonoBehaviour
{
    public int stock = 10;
    [SerializeField] private GameObject Item;
    [SerializeField] private GameObject SpawnPos;
    public List<GameObject> ListItem = new List<GameObject>();
    [SerializeField] private string targetTag = "LettuceSeed";
    public InputActionReference customButton;
    public TMP_Text questText;

    [SerializeField] private bool pressed = false;
    private bool isHovering = false;
    private XRBaseInteractable interactable;

    [Header("Highlight Settings")]
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private Color highlightColor = Color.cyan;
    private Color originalColor;
    private Material matInstance;
    
    
    [SerializeField] private int Cost;
    public Orders orders;

    void Start()
    {
        UpdateQuestText();//change the text in our object
        customButton.action.started += Drop;//subcribing drop to teh custom button

        
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
        {
            interactable = gameObject.AddComponent<XRSimpleInteractable>();//giving the object simple iteractible script so we can get hover events
        }

        
        interactable.hoverEntered.AddListener(OnHoverEnter);//subcribing hover to our function
        interactable.hoverExited.AddListener(OnHoverExit);//getting when we leave hover

        
        

        if (objectRenderer != null)
        {
            matInstance = objectRenderer.material;//getting renderer
            originalColor = matInstance.color;//remembering original color
        }
    }
    //switching our bool plus calling highlight function
    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        isHovering = true;
        Highlight(true);
        Debug.Log("Hovering over seed crate");
    }
    //opposite of OnHoverEnter triggered upon exiting
    private void OnHoverExit(HoverExitEventArgs args)
    {
        isHovering = false;
        Highlight(false);
        Debug.Log("Stopped hovering");
    }

    private void Highlight(bool active)
    {
        if (matInstance != null)//just making sure we actually have the variable set
        {
            matInstance.color = active ? highlightColor : originalColor;//below a longer way of making the same thing(at first i was confused by how this line works too when i was searching for more optimal ways of switching colors)

            /*if (active)
    matInstance.color = highlightColor;
else
    matInstance.color = originalColor;
            */
        }
    }

    //function to change our pressed variable
    void Drop(InputAction.CallbackContext context)
    {
        pressed = !pressed;//change pressed to the opposite of what it is rn
        Debug.Log("boolean is " + pressed);
    }

    //buy function
    private void CheckBuyInput()
    {
        if (pressed == true)//checking if we pressed desired button
        {
            pressed = false;
            orders.Money -= Cost;//count away cost from our money
            stock++;//add one stock
            UpdateQuestText();//change text of our stock
            orders.UpdateQuestText();//we update the UI of the amount of money by calling this function
        }
    }

    void Update()
    {
        if (isHovering && orders.Money >= Cost)//checking if we can afford the item and are hovering(could have just added pressed bool in here idk why i didnt)
            
            CheckBuyInput();
    }


    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !ListItem.Contains(other.gameObject))//checking if the item that we took from the collider is the correct one and adding it to a list to avoid duping
        {
            ListItem.Add(other.gameObject);
            if (stock != 0)
                StartCoroutine(RestockTime());//starting the restock process through a routine to avoid glitchy behaviour 
        }
    }


    void UpdateQuestText()
    {
        questText.text = $"<b>Remaining:</b>\n<b>{stock}</b>\n<b>Buy More for ${Cost}</b>";//text update of the stock amount
    }

    private System.Collections.IEnumerator RestockTime()
    {
        Vector3 spawnPos = SpawnPos.transform.position;//getting spawn position for our item
        yield return new WaitForSeconds(2);//making sure to wait to avoid it spawning within the item we are taking away and getting flung
        Instantiate(Item, spawnPos, SpawnPos.transform.rotation);//spawn the item
        stock--;//lower stock
        UpdateQuestText();//update stock text
    }
}
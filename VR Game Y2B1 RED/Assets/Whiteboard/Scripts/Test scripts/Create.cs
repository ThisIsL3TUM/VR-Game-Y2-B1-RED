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
        UpdateQuestText();
        customButton.action.started += Drop;

        interactable = GetComponent<XRBaseInteractable>();
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
        {
            interactable = gameObject.AddComponent<XRSimpleInteractable>();
        }

        // Subscribe to hover events
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);

        
        

        if (objectRenderer != null)
        {
            matInstance = objectRenderer.material; // create an instance for this object
            originalColor = matInstance.color;
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        isHovering = true;
        Highlight(true);
        Debug.Log("Hovering over seed crate");
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        isHovering = false;
        Highlight(false);
        Debug.Log("Stopped hovering");
    }

    private void Highlight(bool active)
    {
        if (matInstance != null)
        {
            matInstance.color = active ? highlightColor : originalColor;
        }
    }

    void Drop(InputAction.CallbackContext context)
    {
        pressed = !pressed;
        Debug.Log("boolean is " + pressed);
    }

    private void CheckBuyInput()
    {
        if (pressed == true)
        {
            pressed = false;
            orders.Money -= Cost;
            stock++;
            UpdateQuestText();
            orders.UpdateQuestText();
        }
    }

    void Update()
    {
        if (isHovering && orders.Money >= Cost)
            
            CheckBuyInput();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !ListItem.Contains(other.gameObject))
        {
            ListItem.Add(other.gameObject);
            if (stock != 0)
                StartCoroutine(RestockTime());
        }
    }

    void UpdateQuestText()
    {
        questText.text = $"<b>Remaining:</b>\n<b>{stock}</b>\n<b>Buy More for ${Cost}</b>";
    }

    private System.Collections.IEnumerator RestockTime()
    {
        Vector3 spawnPos = SpawnPos.transform.position;
        yield return new WaitForSeconds(2);
        Instantiate(Item, spawnPos, SpawnPos.transform.rotation);
        stock--;
        UpdateQuestText();
    }
}
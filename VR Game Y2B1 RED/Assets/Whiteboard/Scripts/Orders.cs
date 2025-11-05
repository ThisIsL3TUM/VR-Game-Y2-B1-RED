using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class Orders : MonoBehaviour
{
    public CollisionTracker collisionTracker;
    public List<GameObject> Carrot = new List<GameObject>();
    public List<GameObject> Tomato = new List<GameObject>();
    public List<GameObject> Lettuce = new List<GameObject>();

    public int Money = 20;//keeps track our money linked to UI
    //individual crop pizes
    public int carrotPrice = 25;
    public int tomatoPrice = 25;
    public int lettucePrice = 25;
    //Order required amount
    public int requiredCarrots;
    public int requiredTomatoes;
    public int requiredLettuces;

    public TMP_Text moneyText;//UI
    public TMP_Text questText;//UI
    public int windowNumber;//Also within UI

    private GameObject drugged;//plays a part in determing what drug was used in crop
    //place holders before UI
    public float sus = 0;
    public float addic = 0;
    public float susMax = 100;
    public float addicMax = 100;
    public float susMin = 0;
    public float addicMin = 0;

    //Win con triggers
    public bool winning = false;
    public bool losing = false;


    public bool SoldToCustomer = false;//Variavble to communicate with the NPC pathing

    //audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soldClip;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip Lose;

    public Suspicion_manager suspicion_Manager;//Script handling UI work made by teammate

    //UI which we switch on and off
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject SuspicionScreen;

    //link to NPCs
    public NPCRouting NPCRouting;
    void Start()
    {
        GenerateNewOrder();//making sure we have an order form the begining
    }

    void Update()
    {
        CheckOrder();//making sure to keep running a check
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //could have been done in one line through OR but the line would be too long
        if (other.CompareTag("GrownCarrot") && !Carrot.Contains(other.gameObject))//checking if its desired veggie and if its in the list
            Carrot.Add(other.gameObject);//add to list
        else if (other.CompareTag("TomatoG") && !Tomato.Contains(other.gameObject))//checking if its desired veggie and if its in the list
            Tomato.Add(other.gameObject);
        else if (other.CompareTag("Lettuce") && !Lettuce.Contains(other.gameObject))//checking if its desired veggie and if its in the list
            Lettuce.Add(other.gameObject);
        UpdateQuestText();//UI update
    }

    void GenerateNewOrder()
    {
        //setting random ranges
        requiredCarrots = Random.Range(1, 3);
        requiredTomatoes = Random.Range(4, 8);
        requiredLettuces = Random.Range(1, 2);

        if (requiredCarrots + requiredTomatoes + requiredLettuces == 0)//a check to assure we dont have an empty order rn not needed if ranges change
            GenerateNewOrder();//regen order

        Debug.Log($"New Order: {requiredCarrots} Carrots, {requiredTomatoes} Tomatoes, {requiredLettuces} Lettuces");//for testing before UI was in

        UpdateQuestText();//update text
    }

    //updating text with most recent values
    public void UpdateQuestText()
    {
        questText.text =
            $"<b>Customer {windowNumber}</b>\n" +
            $"<b>Current Order:</b>\n" +
            $"Carrots: {Carrot.Count}/{requiredCarrots}\n" +
            $"Tomatoes: {Tomato.Count}/{requiredTomatoes}\n" +
            $"Lettuce: {Lettuce.Count}/{requiredLettuces}";
        moneyText.text = $"<b>Money ${Money}</b>\n";


    }

    void CheckOrder()
    {
        if (Carrot.Count >= requiredCarrots &&
            Tomato.Count >= requiredTomatoes &&
            Lettuce.Count >= requiredLettuces && NPCRouting.waitingForPlayer)//checking if everything is fullfilled before selling
        {
            Sold();//sold start
            GenerateNewOrder();//make new order
            UpdateQuestText();//text update which actually might be redundant since it gets triggered within generate order
        }
    }

    public void Sold()
    {
        Money += carrotPrice * Carrot.Count + tomatoPrice * Tomato.Count + lettucePrice * Lettuce.Count;//updating money
        audioSource.PlayOneShot(soldClip);//audio
        SoldToCustomer = true;//switching var for the NPC to walk away

        EvaluateDrugs(Carrot);//putting wanted veggie list into crops list and starting function
        EvaluateDrugs(Tomato);//putting wanted veggie list into crops list
        EvaluateDrugs(Lettuce);//putting wanted veggie list into crops list


        foreach (var l in Lettuce) Destroy(l);//destroy everything within list
        foreach (var t in Tomato) Destroy(t);//destroy everything within list
        foreach (var c in Carrot) Destroy(c);//destroy everything within list

        Lettuce.Clear();//clear list
        Tomato.Clear();//clear list
        Carrot.Clear();//clear list
        collisionTracker.collidingObjects.Clear();//clear collision tracker

        //testing stuff before UI
        Debug.Log("Money: " + Money);
        Debug.Log("Suspicion: " + sus);
        Debug.Log("Addictivness: " + addic);

        WinCon();//checking if we fullfilled winning condition with this order completion
    }

    //checking what drugs the veggies have
    void EvaluateDrugs(List<GameObject> crops)
    {
        foreach (var crop in crops)//checking every variable within crops list
        {
            if (crop == null) continue;

            Drugged drug = crop.GetComponent<Drugged>();//Find the Drugged component attached to the gameObject, and store it within new var drug

            if (drug.drug1 == false && drug.drug2 == false && drug.drug3 == false)//check if we have no drugs
            {
                
                suspicion_Manager.GetSuspicion(0f);//call function responsible for UI updates attached to diff script made by teammate
                suspicion_Manager.GetAddiction(1f);//call function responsible for UI updates attached to diff script made by teammate
                Debug.Log("Yes" + suspicion_Manager.suspicionAmount);//debugging since soemthing wasnt working
                //sus -= 10;test stuff before UI
                //addic -= 5;
            }
            else if (drug.drug1 == true)//do we have drug1
            {
                suspicion_Manager.GetSuspicion(12f);//call function responsible for UI updates attached to diff script made by teammate
                suspicion_Manager.GetAddiction(10f);//call function responsible for UI updates attached to diff script made by teammate

                //sus += 10; 
                //addic += 10;
            }
            else if (drug.drug2 == true)//do we have drug2
            {
                suspicion_Manager.GetSuspicion(15f);//call function responsible for UI updates attached to diff script made by teammate
                suspicion_Manager.GetAddiction(15f);//call function responsible for UI updates attached to diff script made by teammate
                //sus += 20;
                //addic += 25;
            }
            else if (drug.drug3 == true)//do we have drug3
            {
                suspicion_Manager.GetSuspicion(25f);//call function responsible for UI updates attached to diff script made by teammate
                suspicion_Manager.GetAddiction(20f);//call function responsible for UI updates attached to diff script made by teammate
                //sus += 50;
                //addic += 40;
            }
        }
    }

    

    //checking for winning
    public void WinCon()
    {
        if (suspicion_Manager.addictionAmount >= 95)//if additction is desired level triggger win
        {
            winning = true;
            Debug.Log("winnin");
        }
        else if (suspicion_Manager.suspicionAmount >= 95)//if suspicion is too high trigge loss
        {
            losing = true;
            Debug.Log("losin");
        }
        //idk why i made this in this way i could have just had it withinthe previos if(brain died at 2am)
        if (losing == true)
        {
            SuspicionScreen.SetActive(true);//enabling teh UI elements
            LoseScreen.SetActive(true);//enabling teh UI elements
            WinScreen.SetActive(false);//disable UI which already is disabled so idk why its here(brain died at 2am)
            audioSource.PlayOneShot(Lose);//audio
            Debug.Log("U lost");
        }
        else if (winning == true)
        {
            SuspicionScreen.SetActive(true);//enabling teh UI elements
            LoseScreen.SetActive(false);//disable UI which already is disabled so idk why its here(brain died at 2am)
            WinScreen.SetActive(true);//enabling teh UI elements
            audioSource.PlayOneShot(Win);//audio
            Debug.Log("U won");
        }
    }
}
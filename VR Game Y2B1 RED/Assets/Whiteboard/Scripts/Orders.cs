using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class Orders : MonoBehaviour
{
    public CollisionTracker collisionTracker;
    public List<GameObject> Carrot = new List<GameObject>();
    public List<GameObject> Tomato = new List<GameObject>();
    public List<GameObject> Lettuce = new List<GameObject>();

    public int Money = 20;
    public int carrotPrice = 25;
    public int tomatoPrice = 25;
    public int lettucePrice = 25;

    public int requiredCarrots;
    public int requiredTomatoes;
    public int requiredLettuces;

    public TMP_Text moneyText;
    public TMP_Text questText;
    public int windowNumber;

    private GameObject drugged;
    public float sus = 0;
    public float addic = 0;
    public float susMax = 100;
    public float addicMax = 100;
    public float susMin = 0;
    public float addicMin = 0;
    public bool winning = false;
    public bool losing = false;
    public bool SoldToCustomer = false;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soldClip;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip Lose;

    public Suspicion_manager suspicion_Manager;

    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject SuspicionScreen;

    void Start()
    {
        GenerateNewOrder();
    }

    void Update()
    {
        CheckOrder();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("GrownCarrot") && !Carrot.Contains(other.gameObject))
            Carrot.Add(other.gameObject);
        else if (other.CompareTag("Tomato") && !Tomato.Contains(other.gameObject))
            Tomato.Add(other.gameObject);
        else if (other.CompareTag("Lettuce") && !Lettuce.Contains(other.gameObject))
            Lettuce.Add(other.gameObject);
        UpdateQuestText();
    }

    void GenerateNewOrder()
    {
        requiredCarrots = Random.Range(1, 3);
        requiredTomatoes = Random.Range(4, 8);
        requiredLettuces = Random.Range(1, 2);

        if (requiredCarrots + requiredTomatoes + requiredLettuces == 0)
            GenerateNewOrder();

        Debug.Log($"New Order: {requiredCarrots} Carrots, {requiredTomatoes} Tomatoes, {requiredLettuces} Lettuces");

        UpdateQuestText();
    }

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
            Lettuce.Count >= requiredLettuces)
        {
            Sold();
            GenerateNewOrder();
            UpdateQuestText();
        }
    }
    void EvaluateDrugs(List<GameObject> crops)
    {
        foreach (var crop in crops)
        {
            if (crop == null) continue;

            Drugged drug = crop.GetComponent<Drugged>();

            if (drug.drug1 == false && drug.drug2 == false && drug.drug3 == false)
            {
                
                suspicion_Manager.GetSuspicion(1f);
                suspicion_Manager.GetAddiction(1f);
                Debug.Log("Yes" + suspicion_Manager.suspicionAmount);
                //sus -= 10;
                //addic -= 5;
            }
            else if (drug.drug1 == true)
            {
                suspicion_Manager.GetSuspicion(10f);
                suspicion_Manager.GetAddiction(10f);

                //sus += 10;
                //addic += 10;
            }
            else if (drug.drug2 == true)
            {
                suspicion_Manager.GetSuspicion(20f);
                suspicion_Manager.GetAddiction(20f);
                //sus += 20;
                //addic += 25;
            }
            else if (drug.drug3 == true)
            {
                suspicion_Manager.GetSuspicion(30f);
                suspicion_Manager.GetAddiction(30f);
                //sus += 50;
                //addic += 40;
            }
        }
    }

    public void Sold()
    {
        Money += carrotPrice * Carrot.Count + tomatoPrice * Tomato.Count + lettucePrice * Lettuce.Count;
        audioSource.PlayOneShot(soldClip);
        SoldToCustomer = true;

        EvaluateDrugs(Carrot);
        EvaluateDrugs(Tomato);
        EvaluateDrugs(Lettuce);


        foreach (var l in Lettuce) Destroy(l);
        foreach (var t in Tomato) Destroy(t);
        foreach (var c in Carrot) Destroy(c);

        Lettuce.Clear();
        Tomato.Clear();
        Carrot.Clear();
        collisionTracker.collidingObjects.Clear();


        Debug.Log("Money: " + Money);
        Debug.Log("Suspicion: " + sus);
        Debug.Log("Addictivness: " + addic);

        WinCon();
    }


    public void WinCon()
    {
        if (losing == true)
        {
            SuspicionScreen.SetActive(true);
            LoseScreen.SetActive(true);
            WinScreen.SetActive(false);
            audioSource.PlayOneShot(Lose);
            Debug.Log("U lost");
        }
        else if(winning == true)
        {
            SuspicionScreen.SetActive(true);
            LoseScreen.SetActive(false);
            WinScreen.SetActive(true);
            audioSource.PlayOneShot(Win);
            Debug.Log("U won");
        }
    }
}
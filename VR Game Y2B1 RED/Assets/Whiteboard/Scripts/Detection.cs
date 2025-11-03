using System;
using System.Collections.Generic;
using UnityEngine;


public class Detection : MonoBehaviour
{
    public List<GameObject> Ingredient = new List<GameObject>();
    [SerializeField]
    public string[] Compare = new string[0];
    bool Speed = false;
    bool Power = false;
    bool Something = false;
    [SerializeField]
    CollisionTracker collisionTracker;
    [SerializeField]
    public GameObject spawn;
    [SerializeField]
    public GameObject[] Product;
    [SerializeField]
    public ParticleSystem Particle;
    [SerializeField]
    public GameObject syringe;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionTracker.GetCollisionCount() == 2)
        {
            Merge();
        }else return;
    }


    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < Compare.Length; i++)
        {
            if (collision.collider.CompareTag(Compare[i]))
            {
                Decide(Compare[i], true); // set true when entering

                if (!Ingredient.Contains(collision.gameObject))
                {
                    Ingredient.Add(collision.gameObject);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < Compare.Length; i++)
        {
            if (collision.collider.CompareTag(Compare[i]))
            {
                Decide(Compare[i], false); // set false when exiting

                if (Ingredient.Contains(collision.gameObject))
                {
                    Ingredient.Remove(collision.gameObject);
                }
            }
            
        }
    }

    void Decide(string tag, bool state)
    {
        switch (tag)
        {
            case "Speed":
                Speed = state;
                break;
            case "Power":
                Power = state;
                break;
            case "Something":
                Something = state;
                break;
        }
    }

    void Merge()
    {
        Vector3 spawnPos = (spawn.transform.position);
        if (Speed == true && Power == true || Power == true && Speed == true) 
        {
            Instantiate(Particle, spawnPos, Quaternion.identity);
            syringe = Instantiate(Product[0], spawnPos, Quaternion.identity);
            if (syringe != null)
            {
                syringe.GetComponent<Injection>().dose1 = true;
            }
                Reset();
        }
            else if (Speed == true && Something == true || Something == true && Speed == true)
        {
            Instantiate(Particle, spawnPos, Quaternion.identity);
            syringe = Instantiate(Product[1], spawnPos, Quaternion.identity);
            if (syringe != null)
            {
                syringe.GetComponent<Injection>().dose2 = true;
            }
            Reset();
        }
            else if (Power == true && Something == true || Something == true && Power == true)
        {
            Instantiate(Particle, spawnPos, Quaternion.identity);
            syringe = Instantiate(Product[2], spawnPos, Quaternion.identity);
            if (syringe != null)
            {
                syringe.GetComponent<Injection>().dose3 = true;
            }
            Reset();
        }
        
    }

    private void Reset()
    {
        audioSource.PlayOneShot(grabClip);
        Speed = false;
        Power = false;
        Something = false;

        collisionTracker.collidingObjects.Clear();
        for (int i = 0; i < Ingredient.Count; i++)
        {
            Destroy(Ingredient[i]);  
        }
        Ingredient.Clear();
        Debug.Log("ingredient count" + Ingredient.Count);
        
    }
}

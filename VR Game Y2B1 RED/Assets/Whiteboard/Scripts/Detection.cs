using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.VFX;


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
    [SerializeField]
    private VisualEffect vfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //making sure we stop the vfx from playing
    void Start()
    {
        vfx.Stop();
    }

    // checking for collision each frame if condition is met we begin the merge
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
            if (collision.collider.CompareTag(Compare[i])) //we check if the tag is found within our list
            {
                Decide(Compare[i], true); // set true in teh switch statement

                if (!Ingredient.Contains(collision.gameObject)) // we make sure to not double up on ingredients
                {
                    Ingredient.Add(collision.gameObject);
                }
            }
        }
    }

    //removing the object upon exiting 
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

    //our switch
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

    //merge function

    void Merge()
    {
        Vector3 spawnPos = (spawn.transform.position); //setting spawn position for our merged object
        if (Speed == true && Power == true || Power == true && Speed == true) //checking for correct crafting recipe
        {
            vfx.Play(); //playing vfx
            syringe = Instantiate(Product[0], spawnPos, Quaternion.identity);//instantieting our prefab
            if (syringe != null)
            {
                syringe.GetComponent<Injection>().dose1 = true; //setting the syringes dosage according to the craft
            }
                Reset(); //calling our reset
        }
            else if (Speed == true && Something == true || Something == true && Speed == true)
        {
            vfx.Play();
            syringe = Instantiate(Product[1], spawnPos, Quaternion.identity);
            if (syringe != null)
            {
                syringe.GetComponent<Injection>().dose2 = true;
            }
            Reset();
        }
            else if (Power == true && Something == true || Something == true && Power == true)
        {
            vfx.Play();
            syringe = Instantiate(Product[2], spawnPos, Quaternion.identity);
            if (syringe != null)
            {
                syringe.GetComponent<Injection>().dose3 = true;
            }
            Reset();
        }

        

}
    //just a delay to let the vfx paly
    private System.Collections.IEnumerator VfxStop()
    {
        yield return new WaitForSeconds(1);
        vfx.Stop();
    }


    //reset functions switching off variables and clearing list calling our vfx stop
    private void Reset()
    {
        StartCoroutine(VfxStop());
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

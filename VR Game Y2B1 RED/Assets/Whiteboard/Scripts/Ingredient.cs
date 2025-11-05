using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.OpenXR.Input;
using static UnityEngine.ParticleSystem;

public class Ingredient : MonoBehaviour
{
    public List<GameObject> Thingie = new List<GameObject>();
    [SerializeField]
    public bool[] vegetables;
    [SerializeField]
    CollisionTracker collisionTracker;
    public bool dirt = false;
    [SerializeField]
    public GameObject spawn;
    [SerializeField]
    public GameObject[] Product;
    [SerializeField]
    public ParticleSystem Particle;
    private GameObject crop;
    public bool currentlyGrowing = false;
    private float joseph;
    
    

    private void Update()
    {
        if (collisionTracker.GetCollisionCount() == 2 && currentlyGrowing == false)//using the collision detection scripts function we check if we have the wanter number of objects
        {
            StartGrow();//start function
        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Dirt"))//checking if its anything other then dirt
        {
            
            Planted(other.tag, true);//putting the object into our switch 
            if (!other.CompareTag("HoedDirt") && !other.CompareTag("Hoe") && !other.CompareTag("Player") && !other.CompareTag("wCan"))//additional checks to avoid bugs
            {
                if (!Thingie.Contains(other.gameObject))
                {
                    Thingie.Add(other.gameObject);
                    
                }
            }
                
            
        }
        /* unnecessary stuff
        if (other.CompareTag("HoedDirt"))
        {
            other.gameObject.transform.rotation = spawn.transform.rotation;
            other.gameObject.transform.position = spawn.transform.position;
            //Debug.Log("object moves");
        }

        if (other.CompareTag("Dirt"))
        {
            other.gameObject.transform.rotation = spawn.transform.rotation;
            other.gameObject.transform.position = spawn.transform.position;
            //Debug.Log("object moves");
        }
    
        if (other.CompareTag("Carrot"))
        {
            other.gameObject.transform.localScale = spawn.transform.localScale;
            other.gameObject.transform.position = spawn.transform.position;
        }
    */
    }
    //the oppositte of enter just removing thing upon leaving
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Dirt"))
        {
            
            Planted(other.tag, false);
            if (!other.CompareTag("Hoe") && !other.CompareTag("HoedDirt"))
            {
                if (Thingie.Contains(other.gameObject))
                {
                    Thingie.Remove(other.gameObject);
                    //Debug.Log("Skibidi");
                }
            }
            
        }
    }

    //simple switch
    private void Planted(string tag, bool state)
    {
        
        switch (tag)
        {
            case "Carrot":
                vegetables[0] = state;
                Debug.Log("Carrot" + vegetables[0]);
                break;
            case "Tomato":
                vegetables[1] = state;
                Debug.Log("Carrot" + vegetables[1]);
                break;
            case "LettuceSeed":
                vegetables[2] = state;
                break;
            
               
            

        }
    }
    
    private void StartGrow()
    {
       
        Vector3 spawnPos = (spawn.transform.position);//setting spawn position
        for(int i = 0; i < vegetables.Length; i++)//going through the whole array
        {
            if (vegetables[i] == true && dirt == true || dirt == true && vegetables[i] == true)//checking for viable combos
            {
                
                crop = Instantiate(Product[i], spawnPos, spawn.transform.rotation);//create crop

                joseph = Product[i].GetComponentInChildren<Growth>().growthTime;//getting growth time of the created crop
                currentlyGrowing = true;
                StartCoroutine(Occupied());//coroutine start


                Reset();
            }
        }
        
        
    }

    //making sure we cant plant a seed in the same patch for the period of growth
    public System.Collections.IEnumerator Occupied()
    {
        yield return new WaitForSeconds(joseph);
        currentlyGrowing = false;
    }
    //reset function setting all important variables to false and clearing out the lists
    private void Reset() 
    {
        
        for(int i = 0;  i < vegetables.Length; i++)
        {
            vegetables[i] = false;
        }
        
        
        collisionTracker.collidingObjects.RemoveAll(obj => obj != collisionTracker.keepThisOne);
        for (int i = 0; i < Thingie.Count; i++)
        {
            Destroy(Thingie[i]);
        }
        Thingie.Clear();
        //Debug.Log("ingredient count" + Thingie.Count);


    }



}

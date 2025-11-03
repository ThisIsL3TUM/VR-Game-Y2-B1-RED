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
        if (collisionTracker.GetCollisionCount() == 2 && currentlyGrowing == false)
        {
            StartGrow();
        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Dirt"))
        {
            
            Planted(other.tag, true);
            if (!other.CompareTag("HoedDirt") && !other.CompareTag("Hoe") && !other.CompareTag("Player") && !other.CompareTag("wCan"))
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
       
        Vector3 spawnPos = (spawn.transform.position);
        for(int i = 0; i < vegetables.Length; i++)
        {
            if (vegetables[i] == true && dirt == true || dirt == true && vegetables[i] == true)
            {
                
                crop = Instantiate(Product[i], spawnPos, spawn.transform.rotation);

                joseph = Product[i].GetComponentInChildren<Growth>().growthTime;
                currentlyGrowing = true;
                StartCoroutine(Occupied());


                Reset();
            }
        }
        
        
    }


    public System.Collections.IEnumerator Occupied()
    {
        yield return new WaitForSeconds(joseph + joseph/2);
        currentlyGrowing = false;
    }

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

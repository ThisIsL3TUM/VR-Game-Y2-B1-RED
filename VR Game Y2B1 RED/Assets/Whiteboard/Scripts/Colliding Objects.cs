using UnityEngine;
using System.Collections.Generic;

public class CollisionTracker : MonoBehaviour
{
    public GameObject keepThisOne;
    public List<GameObject> collidingObjects = new List<GameObject>();

    //Detecting entering objects
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))//safety meassure after accidentally deleting the player upon reseting values
        {
            if (other.CompareTag("HoedDirt"))//checking for our special item
            {
                keepThisOne = other.gameObject;//making sure we always keep this item and never whipe it important for Ingredient script
                if (!collidingObjects.Contains(other.gameObject))
                {
                    collidingObjects.Add(keepThisOne);
                }
            }
            if (!collidingObjects.Contains(other.gameObject))
            {
                collidingObjects.Add(other.gameObject);//simply adding object into our list
            }
        }
        

        Debug.Log("Currently colliding with: " + collidingObjects.Count);
    }
    //just removing objects from the list upon exiting
    private void OnTriggerExit(Collider other)
    {
        if (collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Remove(other.gameObject);
        }

        Debug.Log("Currently colliding with: " + collidingObjects.Count);
    }

   
    //function to make getting collision count with other scripts easier
    public int GetCollisionCount()
    {
        return collidingObjects.Count;
    }
}
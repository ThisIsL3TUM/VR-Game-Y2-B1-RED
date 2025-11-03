using System;
using System.Threading;

using UnityEngine;
using UnityEngine.InputSystem;

public class Injection : MonoBehaviour
{
    
    public bool dose1 = false;
    public bool dose2 = false;
    public bool dose3 = false;
    public int durability = 0;
    public GameObject injection;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabClip;
    public ParticleSystem waterEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrownCarrot")|| other.CompareTag("Tomato") || other.CompareTag("Lettuce"))
            
        {
                injection = other.gameObject;
                if (dose1 == true && injection.GetComponent<Drugged>().drugged == false) 
                { 
                injection.GetComponent<Drugged>().drug1 = true; 
                injection.GetComponent<Drugged>().amDrugged();
                waterEffect.Play();
                audioSource.PlayOneShot(grabClip);
                Debug.Log("drugged");
                
                durability--;
                }
                else if (dose2 == true && injection.GetComponent<Drugged>().drugged == false)
                { 
                injection.GetComponent<Drugged>().drug2 = true;
                injection.GetComponent<Drugged>().amDrugged();
                waterEffect.Play();
                audioSource.PlayOneShot(grabClip);
                Debug.Log("drugged");
                
                durability--;
                }
                else if (dose3 == true && injection.GetComponent<Drugged>().drugged == false)
                { 
                injection.GetComponent<Drugged>().drug3 = true;
                injection.GetComponent<Drugged>().amDrugged();
                waterEffect.Play();
                audioSource.PlayOneShot(grabClip);
                Debug.Log("drugged");
                
                durability--;
                }
                if(durability == 0) {Destroy(this.gameObject);}
        }
    }
}

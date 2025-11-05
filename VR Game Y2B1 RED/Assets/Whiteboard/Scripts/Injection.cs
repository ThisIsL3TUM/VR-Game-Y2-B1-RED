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
    [SerializeField] private AudioClip voiceSource;
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
        if (other.CompareTag("GrownCarrot")|| other.CompareTag("TomatoG") || other.CompareTag("Lettuce"))//checking if the object within trigger is one of our veggies
            
        {
                injection = other.gameObject;//just to shorten the lines
                if (dose1 == true && injection.GetComponent<Drugged>().drugged == false) //get what drug this object is carrying and transfering plus a check so we cant drug the same veggie multiple times
                { 
                injection.GetComponent<Drugged>().drug1 = true; //set the veggies drug on
                injection.GetComponent<Drugged>().amDrugged();//star the veggies am drugged function
                waterEffect.Play();//play corresponding vfx its not a water vfx but the naming is special
                audioSource.PlayOneShot(grabClip);//play proper audio
                StartCoroutine(Sound());//play second audio after delay
                Debug.Log("drugged");
                
                durability--;//decrease durability
                }
                else if (dose2 == true && injection.GetComponent<Drugged>().drugged == false)
                { 
                injection.GetComponent<Drugged>().drug2 = true;
                injection.GetComponent<Drugged>().amDrugged();
                waterEffect.Play();
                audioSource.PlayOneShot(grabClip);
                StartCoroutine(Sound());
                Debug.Log("drugged");
                
                durability--;
                }
                else if (dose3 == true && injection.GetComponent<Drugged>().drugged == false)
                { 
                injection.GetComponent<Drugged>().drug3 = true;
                injection.GetComponent<Drugged>().amDrugged();
                waterEffect.Play();
                audioSource.PlayOneShot(grabClip);
                StartCoroutine(Sound());

                Debug.Log("drugged");
                
                durability--;
                }
                if(durability == 0) {Destroy(this.gameObject);}//destroy self upon 0 durability
        }
    }

    private System.Collections.IEnumerator Sound()
    {
        yield return new WaitForSeconds(2);//wait 2 secs
        audioSource.PlayOneShot(voiceSource);//play second sound
    }
}

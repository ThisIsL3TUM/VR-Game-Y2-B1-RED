using UnityEngine;

public class Drugged : MonoBehaviour
{
    public bool drug1 = false;
    public bool drug2 = false;
    public bool drug3 = false;
    public bool drugged = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void amDrugged()
    {
        if (drug1 == true || drug2 == true || drug3 == true) { drugged = true; Debug.Log("Already got my dose"); }//just check if it has any drug
        //its done through a function bcs we wanted to also make some visual changes to the crop itself like the material changing/glowing but didnt do it in the end
    }
}

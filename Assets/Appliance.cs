using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : MonoBehaviour
{

    public bool mode;
    public GameObject controlled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        controlled.SetActive(mode);
    }

    public void flipState()
    {
        mode = !mode;
        Debug.Log("Something 1");
    }

    public bool ReadState()
    {
        return mode;
    }

    public void WriteState(bool state)
    {
        mode = state;
        controlled.SetActive(state);
    }
}

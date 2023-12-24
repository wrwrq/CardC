using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour
{
    string n;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void matchName(string name)
    {
        transform.Find(name).gameObject.SetActive(true);
        n = name;
    }

    void nonmatchName(string name)
    {
        transform.Find(name).gameObject.SetActive(false);
    }
    public void ClickName()
    {
        gameObject.SetActive(false);
        nonmatchName(n);
        Time.timeScale = 1f;
    }

    //I think, be better to create card with a name
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour
{
    string n;
    bool setClick = false;
    float clickcount = 0;
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
        Debug.Log(name);
        transform.Find(name).gameObject.SetActive(false);
    }
    public void ClickName()
    {
        clickcount ++;
        gameObject.SetActive(false);
        nonmatchName(n);
        Time.timeScale = 1f;
        Debug.Log(clickcount);
    }

    public bool LastCard(float c)
    {
        if (c -1 == clickcount)
        {
            setClick = true;
            return setClick;
        }
        else return false;
    }





    //I think, be better to create card with a name
}

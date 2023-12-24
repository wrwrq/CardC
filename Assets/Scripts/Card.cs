using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    //-----------------------------------------------------------------------------------open, close card parameter
    public bool isSelect;

    [Header("Card State")]
    
    public GameObject front;
    public GameObject back;
    public string cardName;
    //-----------------------------------------------------------------------------------open, close card parameter


    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
    }

    public void openCard()
    {
        GetComponent<Animator>().SetBool("isClick", true);
        transform.Find("Back").gameObject.SetActive(true);
        transform.Find("Front").gameObject.SetActive(false);
        if (GameManager.I.firstCard == null)
        {
            GameManager.I.firstCard = gameObject;
        }
        else
        {
            GameManager.I.secondCard = gameObject;
            GameManager.I.Match();
        }
    }

    public void closeCard()
    {
        GetComponent<Animator>().SetBool("isClick", false);
        transform.Find("Back").gameObject.SetActive(false);
        transform.Find("Front").gameObject.SetActive(true);
    }

    public void closeCardInvoke()
    {
        Invoke("closeCard", 1f);
    }

    //------------------------------------------------------------------------------------------------Test Code
    //-----------------------------------------------------------------------------------open, close card
    public void OpenCard()
    {
        if (!isSelect)
        {
            if (GameManager.I.inChecking)
            {
                CardFlip(front, back);
                GameManager.I.secondCard = gameObject;
                GameManager.I.Match2();

            }
            else
            {
                CardFlip(front, back);
                GameManager.I.inChecking = true;
                GameManager.I.firstCard = gameObject;
            }
        }
      
    }

    public void CloseCard()
    {
        CardFlip(back, front);
        isSelect = false;
    }


    public void CardFlip(GameObject front, GameObject back)
    {
        //audioSource.PlayOneShot(clip); //card flip sound
        StartCoroutine(CardFlipCo(front, back));
    }

    IEnumerator CardFlipCo(GameObject front, GameObject back)
    {
        for (int i = 0; i < 90; i += 2)
        {
            back.transform.rotation = Quaternion.Euler(0, i, 0);
            yield return null;
        }
        back.SetActive(false);
        front.SetActive(true);
        for (int i = 90; 0 < i; i -= 2)
        {
            front.transform.rotation = Quaternion.Euler(0, i, 0);
            yield return null;
        }
    }

    //-----------------------------------------------------------------------------------open, close card
    //------------------------------------------------------------------------------------------------Test Code

    
   
}

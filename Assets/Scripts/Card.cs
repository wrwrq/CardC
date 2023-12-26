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
    public GameObject frontImage;
    public GameObject back;
    public AudioClip flip;
    public AudioSource audioSource;
    public string cardName;

    public float x;
    public float y;

    float hsvV = 1;



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
        audioSource.PlayOneShot(flip);
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



    public void SetCoordAndName(float _x, float _y, string _cardName)
    {
        x = _x;
        y = _y;
        cardName = _cardName;
    }


    //-----------------------------------------------------------------------------------Change BackCard Color
    public void DarkenColor()
    {
        hsvV -= 0.1f;
        Color newColor = Color.HSVToRGB(0, 0, hsvV);

        back.GetComponent<SpriteRenderer>().color = newColor;
    }
    //-----------------------------------------------------------------------------------Change BackCard Color


    //------------------------------------------------------------------------------------------------Test Code
    //-----------------------------------------------------------------------------------open, close card
    public void OpenCard()
    {
        if (!isSelect && !GameManager.I.fullCard)
        {
            if (GameManager.I.inChecking)
            {
                isSelect = true;
                CardFlip(front, back);
                GameManager.I.secondCard = gameObject;
                GameManager.I.Match2();

            }
            else
            {
                isSelect = true;
                CardFlip(front, back);
                GameManager.I.inChecking = true;
                GameManager.I.firstCard = gameObject;
            }
        }
      
    }

    public void CloseCard()
    {
        DarkenColor();
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

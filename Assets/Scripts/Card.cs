using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : MonoBehaviour
{
    
    public bool isSelect; //카드 선택 됐는지 확인하는

    [Header("Card State")]
    public GameObject front;
    public GameObject frontImage;
    public GameObject back;
    //카드 위치
    public float x;
    public float y;
    public float angle;
    float hsvV = 1;

    [Header("Card Audio")]
    public AudioClip flip;
    public AudioSource audioSource;
    public string cardName;

    [Header("Effect")]
    public float rotationSpeed = 300f; // 회전 속도
    public float maxRadius; // 주변을 도는 최대 반지름
    public float orbitSpeed = 5f; // 센터를 중심으로 주변을 도는 속도
    private float currentRadius; // 현재 반지름

    void Start()
    {
        currentRadius = maxRadius;
        StartCoroutine(RotationCo()); //등장
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

    //-----------------------------------------------------------------------------------open, close card
    public void OpenCard()
    {
        if (!isSelect && !GameManager.I.fullCard && GameManager.I.gameState == GameState.Start)
        {
            audioSource.PlayOneShot(flip);
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







    //-----------------------------------------------------------------------------------Start Cart Effect

    IEnumerator RotationCo()
    {
        while (currentRadius > 0)
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, Time.deltaTime * rotationSpeed);
            transform.position = GetPosition();
            currentRadius -= Time.deltaTime * 1.5f;
            currentRadius = Mathf.Clamp(currentRadius, 0, maxRadius);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);

        StartCoroutine(CardOriginalLocationMoveCo());
    }

    Vector2 GetPosition()
    {
        angle += Time.deltaTime * orbitSpeed;
        float x = Mathf.Cos(angle) * currentRadius;
        float y = Mathf.Sin(angle) * currentRadius;
        return new Vector2(x, y);
    }


    IEnumerator CardOriginalLocationMoveCo()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), percent);
            yield return null;
        }


        if (GameManager.I.AllCheckCardOriginalPosition())
        {
            GameManager.I.AllOpenCard();
        }

    }
    //-----------------------------------------------------------------------------------Start Cart Effect




}

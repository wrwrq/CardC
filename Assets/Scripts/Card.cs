using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Animator animator;
    public GameObject front;
    public GameObject back;
    public Transform cardStartPot;

    [Header("State")]
    public float x; //카드의 위치
    public float y;
    public float hsvV = 1; //카드색 명도
    public bool select; 
    public float angle; //카드 등장 효과 사용
    public string cardName; 

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clip;

    //---------------------
    [Header("Effect")]
    public float rotationSpeed = 300f; // 회전 속도
    public float maxRadius; // 주변을 도는 최대 반지름
    public float orbitSpeed = 5f; // 센터를 중심으로 주변을 도는 속도
    private float currentRadius; // 현재 반지름


    private void Awake()
    {
        animator = transform.Find("Back").GetComponent<Animator>();
        currentRadius = maxRadius;
    }

    private void Start()
    {
        //StartCoroutine(RotationCo()); // 게임 시작 카드 등장 효과
    }



    //public void OpenCard()
    //{

    //    if (GameManager_T4.I.matchCardList.Count < 2 && !select && GameManager_T4.I.gameState == GameState.Start)
    //    {
    //        select = true;
    //        CardFlip(front, back);

    //        audioSource.PlayOneShot(clip);

    //        GameManager_T4.I.AddMatchList(gameObject);
    //    }
    //}

    public void CloseCard()
    {
        StartCoroutine(CardFlipCo(back, front));
        DarkenColor();
        select = false;
    }


    //--------------------------------------------------------------------------CardFlip


    public void CardFlip(GameObject front, GameObject back)
    {
        StartCoroutine(CardFlipCo(front, back));
    }

    IEnumerator CardFlipCo(GameObject front, GameObject back)
    {
        for (int i = 0; i < 90; i++)
        {
            back.transform.rotation = Quaternion.Euler(0, i, 0);
            yield return null;
        }

        back.SetActive(false);
        front.SetActive(true);
        for (int i = 90; 0 < i; i--)
        {
            front.transform.rotation = Quaternion.Euler(0, i, 0);
            yield return null;
        }
    }

    //--------------------------------------------------------------------------Card Color
    public void DarkenColor()
    {
        hsvV -= 0.1f;
        Color newColor = Color.HSVToRGB(0, 0, hsvV);

        back.GetComponent<SpriteRenderer>().color = newColor;
    }



    //---------------------Start Card Effect


    public void SetCoordAndName(float _x, float _y, string _cardName)
    {
        x = _x;
        y = _y;
        cardName = _cardName;
    }


    // 게임 시작 카드 등장 효과
    IEnumerator RotationCo()
    {
        while (currentRadius > 0)
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, Time.deltaTime * rotationSpeed);
            transform.position = GetPosition();
            currentRadius -= Time.deltaTime;
            currentRadius = Mathf.Clamp(currentRadius, 0, maxRadius);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);

        StartCoroutine(CardOriginalLocationMoveCo());
    }
    // 게임 시작 카드 등장 효과
    Vector2 GetPosition()
    {
        angle += Time.deltaTime * orbitSpeed;
        float x = Mathf.Cos(angle) * currentRadius;
        float y = Mathf.Sin(angle) * currentRadius;
        return new Vector2(x, y);
    }

    // 게임 시작 카드 등장 효과
    IEnumerator CardOriginalLocationMoveCo()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), percent);
            yield return null;
        }

        //모든 카드들이 제자리에 왔는지 확인
        //if (GameManager_T4.I.AllCheckCardOriginalPosition())
        //{
        //    GameManager_T4.I.AllOpenCard();
        //}

    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;



public enum GameState
{
    Ready,
    Start,
    GameOver
}


public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("UI")]
    public Text tryText;


    [Header("Game State")]
    GameState gameState;
    bool isSingleCardSelect;
    public int boardSize;
    public bool inChecking;

    public int matchCount; //Clear conditions

    public int tryPoint; //count, flip card


    [Header("Card")]
    int[] prefebIdxs;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject nameCard;
    public GameObject failCard;

    public List<Sprite> cardImages = new List<Sprite>();
    
    //public List<GameObject> matchCardList = new List<GameObject>(); //card match list, Take two cards, compare the two cards

    Queue<int> queue = new Queue<int>();


    [Header("Time")]
    public Text timeText;
    float gameTime;   // I think we'd better run out of time.
    float setTime; //only one card flip, count down parameter
    public int limitTime;
    public int penaltyTime; // choose not matched card, deduction time


    void Awake()
    {
        I = this;
    }


    private void Update()
    {
        gameTime += Time.deltaTime;
        timeText.text = gameTime.ToString("N2");
        if (gameTime >= limitTime)
        {
            EndGame();
        }
        if (firstCard != null && secondCard == null)
        {
            setTime += Time.deltaTime;
            if (setTime >= 5)
            {
                //ī�� �ݴ� �Լ� �ֱ�
                firstCard = null;
                setTime = 0;
            }
        }

    }


    void Shuffle()
    {
        for (int i = 0; i < boardSize * boardSize; i++)
        {
            prefebIdxs[i] = (i / 2) % cardImages.Count;
        }

        for (int i = 0; i < boardSize * boardSize; i++)
        {
            int randomIdx = Random.Range(i, boardSize * boardSize);
            int term = prefebIdxs[randomIdx];
            prefebIdxs[randomIdx] = prefebIdxs[i];
            prefebIdxs[i] = term;
        }

        queue = new Queue<int>(prefebIdxs);
    }

    public void Match()
    {
        tryPoint++;

        //�ι�° ī�� ������ �ʱ�ȭ
        setTime = 0;
        if (firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name == secondCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name)
        {
            Destroy(firstCard);
            Destroy(secondCard);

            //�Ұ� ������ ���� ���߱�
            Time.timeScale = 0;

            //�̸� ǥ��
            nameCard.SetActive(true);
            nameCard.GetComponent<Introduction>().matchName(firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name);
        }
        else if (firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name != secondCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name)
        {
            gameTime += penaltyTime;
            failCard.SetActive(true);
            FailCardInvoke();
        }
        firstCard = null;
        secondCard = null;
    }

    void EndGame()
    {
        tryText.gameObject.SetActive(true);
        tryText.text += tryPoint;
        Time.timeScale = 0;
        //SceneManager.LoadScene("");
    }

    void FailCard()
    {
        failCard.SetActive(false);
    }
    void FailCardInvoke()
    {
        Invoke("Failcard", 1f);
    }




    //-----------------------------------------------------------------------------------------------------------Test Code

    //--------------------------------------------------------------------------------card matching
    public void Match2()
    {
        setTime = 0;

        string firstName = firstCard.GetComponent<Card>().front.GetComponent<SpriteRenderer>().sprite.name;
        string secondName = secondCard.GetComponent<Card>().front.GetComponent<SpriteRenderer>().sprite.name;

        if (firstName == secondName)
        {
            Debug.Log("Matched!");

            Destroy(firstCard);
            Destroy(secondCard);
        }
        else
        {
            Debug.Log("Not matched!");
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();

            gameTime -= penaltyTime;
        }

        matchCardReset();
    }

    void matchCardReset()
    {
        firstCard = null;
        secondCard = null;
        inChecking = false;
    }

    //--------------------------------------------------------------------------------card matching
    //--------------------------------------------------------------------------------Time
    void RunTime()
    {
        if(gameState == GameState.Start)
        {
            gameTime -= Time.deltaTime;
            timeText.text = gameTime.ToString("N2");

            if (gameTime <= 0)
            {
                gameState = GameState.GameOver;
                Debug.Log("Game Over!");
            }

            if(firstCard != null && secondCard == null && !isSingleCardSelect)
            {
                StartCoroutine(SingleCardTimeRunCo());
            }

        }
    }

    IEnumerator SingleCardTimeRunCo()
    {
        isSingleCardSelect = true;
        float time = setTime;
        while(secondCard == null)
        {
            if (time <= 0)
            {
                firstCard.GetComponent<Card>().CloseCard();
                matchCardReset();
                break;
            }
            setTime -= Time.deltaTime;
            Debug.Log(setTime);
            yield return null;
        }

        isSingleCardSelect = false;
    }
    //--------------------------------------------------------------------------------Time
    //-----------------------------------------------------------------------------------------------------------Test Code

}

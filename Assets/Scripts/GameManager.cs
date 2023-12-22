using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    [Header("Game State")]
    public int boardSize;

    [Header("Card")]
    int[] prefebIdxs;

    public List<Sprite> cardImages = new List<Sprite>();
    Queue<int> queue = new Queue<int>();

    static GameManager I;
    GameObject firstCard;
    GameObject secondCard;
    public GameObject nameCard;
    public GameObject failCard;
    float gameTime;
    float setTime;
    public int LimitTime;
    public int PenaltyTime;
    private void Update()
    {
        gameTime += Time.deltaTime;
        if (gameTime >= LimitTime)
        {
            EndGame();
        }
        if(firstCard != null && secondCard == null)
        {
            setTime += Time.deltaTime;
            if(setTime >= 5)
            {
                //카드 닫는 함수 넣기
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
    void Match()
    {
        if (firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name == secondCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name)
        {
            Destroy(firstCard);
            Destroy(secondCard);

            //소개 보여줄 때는 멈추기
            Time.timeScale = 0;

            //이름 표시
            nameCard.SetActive(true);
            nameCard.GetComponent<Introduction>().matchName(firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name);
        }
        else if (firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name != secondCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name)
        {
            gameTime += PenaltyTime;
            failCard.SetActive(true);
            FailCardInvoke();
        }
        firstCard = null;
        secondCard = null;
    }
    void EndGame()
    {
        Time.timeScale = 0;
        //SceneManager.LoadScene("");
    }

    void Awake()
    {
        I = this;
    }

    void FailCard()
    {
        failCard.SetActive(false);
    }
    void FailCardInvoke()
    {
        Invoke("Failcard", 1f);
    }
}

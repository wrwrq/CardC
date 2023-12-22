using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    [Header("Game State")]
    public int boardSize;

    [Header("Card")]
    int[] prefebIdxs;

    public List<Sprite> cardImages = new List<Sprite>();
    Queue<int> queue = new Queue<int>();

    [Header("UI")]
    public Text tryText;
    int tryPoint;

    [Header("Card")]
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject nameCard;
    public GameObject failCard;

    [Header("Time")]
    public Text timeText;
    float gameTime;
    float setTime;
    public int LimitTime;
    public int PenaltyTime;


    public static GameManager I;
   
    



    void Awake()
    {
        I = this;
    }



    private void Update()
    {
        gameTime += Time.deltaTime;
        timeText.text = gameTime.ToString("N2");
        if (gameTime >= LimitTime)
        {
            EndGame();
        }
        if(firstCard != null && secondCard == null)
        {
            setTime += Time.deltaTime;
            if(setTime >= 5)
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
            gameTime += PenaltyTime;
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
}

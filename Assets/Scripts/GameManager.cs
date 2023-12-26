using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
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
    public Text PenaltyText;

    [Header("Game State")]
    public GameState gameState;// 게임 상태(준비, 시작, 게임오버)
    bool isSingleCardSelect; //카드 하나만 선택했을때
    public Transform board;
    //board Size
    public int boardSizeX;
    public int boardSizeY;
    int gameLevel;//   minLevel = 1   maxLevel = 3

    public int matchCount; //게임 클리어 조건,
    public int tryPoint; //카드를 몇번 뒤집었는지 확인하는 변수

    public bool inChecking;
    public bool fullCard;
    public float timeTheCardIsOpen;

    [Header("Card")]
    int[] prefebIdxs;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject nameCard;
    public GameObject failCard;
    public GameObject endPanel; // score board
    public GameObject card; //card Prefeb
    public Transform cardStartPot;
    //Card Size
    public float cardSizeX;
    public float cardSizeY;
    [Range(0, 1)]
    public float outLinePercent;
    //Card image
    public List<Sprite> cardImages = new List<Sprite>();
    public List<GameObject> cardPack = new List<GameObject>();
    Queue<int> queue = new Queue<int>();



    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip success;
    public AudioClip fail;
    public AudioClip bgm;

    [Header("Time")]
    public Text timeText;
    public GameObject countTime;
    public Text countTimeText;

    [Header("Score")]
    public Text timeScoreTxt;
    public Text matchScoreTxt;
    public Text failScoreTxt;
    public Text totalScoreTxt;
    private int timeScore = 0;
    private int matchScore = 0;
    private int failScore = 0;
    private int totalScore = 0;

    public float gameTime;   // I think we'd better run out of time.
    public float setTime; //only one card flip, count down parameter
    public int penaltyTime;


    void Awake()
    {
        I = this;
    }

    private void Start()
    {
        gameLevel = PlayerPrefs.GetInt("stage");
        audioSource.clip = bgm;
        audioSource.Play();
        GeneratorBoard();
        gameState = GameState.Start;
    }


    private void Update()
    {
        if (gameState == GameState.Start)
        {
            RunTime();

        }
    }

    IEnumerator PenaltyUi()
    {
        Text temp = Instantiate(PenaltyText);
        temp.transform.SetParent(GameObject.Find("Time/TimeText").transform);
        temp.text = "-" + penaltyTime.ToString();
        yield return new WaitForSeconds(0.5f);
        Destroy(temp.gameObject);
    }

    public void Match()
    {
        tryPoint++;

        if (firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name == secondCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name)
        {
            Destroy(firstCard);
            Destroy(secondCard);


            Time.timeScale = 0;


            nameCard.SetActive(true);
            nameCard.GetComponent<Introduction>().matchName(firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name);
        }
        else if (firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name != secondCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name)
        {
            gameTime += penaltyTime;
            failCard.SetActive(true);
            FailCardInvoke();
            failScore--; // Card Matched fail -1 point
            failScoreTxt.text = "Fail Score: " + failScore.ToString();
        }
        firstCard = null;
        secondCard = null;

        TotalScore();// totalscore
    }

    public void TotalScore()
    {
        // timeScore, matchScore, failScore ++ total
        totalScore = timeScore + matchScore + failScore;
        totalScoreTxt.text = "Total Score: " + totalScore.ToString();
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

    //--------------------------------------------------------------------------------Board
    void GeneratorBoard() //보드 생성
    {
        if (GameObject.Find("Board"))
        {
            DestroyImmediate(GameObject.Find("Board"));
        }
        board = new GameObject("Board").transform;

        prefebIdxs = new int[boardSizeX * boardSizeY];

        Shuffle();

        StartCoroutine(CreateNewCard());

    }
    void Clear()
    {
        gameLevel++;
        gameLevel = Mathf.Clamp(gameLevel, 0, 3);
        PlayerPrefs.SetInt("Unlock", gameLevel);

        //새로운 보드 생성(보드스테이트 클래스 생성, 보드스테이트[] 만들기
        //로컬에서 게임 랩가져와 보드스테이[] 에서 변수 가져와 제네레이터 보드에 넣어서 실행
    }

    void Shuffle() //카드 섞기
    {
        for (int i = 0; i < prefebIdxs.Length; i++)
        {
            prefebIdxs[i] = (i / 2) % cardImages.Count;
        }

        for (int i = 0; i < prefebIdxs.Length; i++)
        {
            int randomIdx = Random.Range(i, prefebIdxs.Length);
            int term = prefebIdxs[randomIdx];
            prefebIdxs[randomIdx] = prefebIdxs[i];
            prefebIdxs[i] = term;
        }

        queue = new Queue<int>(prefebIdxs);
    }

    IEnumerator CreateNewCard() //카드 생성
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < boardSizeX; i++)
        {
            for (int j = 0; j < boardSizeY; j++)
            {
                int idx = queue.Dequeue();
                string name = cardImages[idx].name.Remove(cardImages[idx].name.Length - 1);

                float x = (-boardSizeX / 2f + i + .5f) * cardSizeX;
                float y = (-boardSizeY / 2f + j + .5f) * cardSizeY;
                //GameObject newCard = Instantiate(card, cardStartPot.position, Quaternion.identity);
                GameObject newCard = Instantiate(card, new Vector3(x, y, 0), Quaternion.identity);

                newCard.GetComponent<Card>().SetCoordAndName(x, y, name);

                newCard.name = "Card";

                //newCard.GetComponent<Card>().front.GetComponent<SpriteRenderer>().sprite = cardImages[idx];
                newCard.GetComponent<Card>().frontImage.GetComponent<Image>().sprite = cardImages[idx];

                newCard.transform.localScale = newCard.transform.localScale * (1 - outLinePercent);

                newCard.transform.parent = board;
                cardPack.Add(newCard);

                yield return new WaitForSeconds(.05f);
            }
        }

    }

    //--------------------------------------------------------------------------------Board


    //-----------------------------------------------------------------------------------------------------------Test Code

    //--------------------------------------------------------------------------------card matching

    public void Match2() //카드 매칭
    {
        StartCoroutine(Match2Co());
    }

    IEnumerator Match2Co()
    {
        fullCard = true;
        countTime.SetActive(false);

        yield return new WaitForSeconds(0.8f);
        string firstName = firstCard.GetComponent<Card>().frontImage.GetComponent<Image>().sprite.name;
        string secondName = secondCard.GetComponent<Card>().frontImage.GetComponent<Image>().sprite.name;

        if (firstName == secondName)
        {
            audioSource.PlayOneShot(success);
            Debug.Log("Matched!");

            Destroy(firstCard);
            Destroy(secondCard);

            Time.timeScale = 0f;
            nameCard.SetActive(true);
            nameCard.GetComponent<Introduction>().matchName(firstName);
        }
        else
        {
            audioSource.PlayOneShot(fail);
            Debug.Log("Not matched!");
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();

            failCard.SetActive(true);
            Invoke("FailCard", 1f);
            StartCoroutine(PenaltyUi());
            gameTime -= penaltyTime;
        }
        matchCardReset();
    }

    void matchCardReset() //초기/
    {
        firstCard = null;
        secondCard = null;
        inChecking = false;
        fullCard = false;
    }

    //--------------------------------------------------------------------------------card matching
    //--------------------------------------------------------------------------------Time
    void RunTime()//타이머
    {
        if (gameState == GameState.Start)
        {
            gameTime -= Time.deltaTime;
            timeText.text = gameTime.ToString("N2");

            if (gameTime <= 0)   //게임 종료
            {
                gameState = GameState.GameOver;
                timeText.text = "0.00";
                Debug.Log("Game Over!");
            }

            if (firstCard != null && secondCard == null && !isSingleCardSelect)
            {
                StartCoroutine(SingleCardTimeRunCo());
            }

            // 시간이 얼마 안 남았을 때 깜빡거리는 효과
            if (gameTime <= 10f) // 필요에 따라 조절
            {
                FlashTimeText();
            }
            timeScoreTxt.text = timeScore.ToString();  // timescore update
            timeScore = Mathf.RoundToInt(gameTime); // 1point per second
        }
    }

    IEnumerator SingleCardTimeRunCo() //카드 하나만 골랐을때 타이머
    {
        isSingleCardSelect = true;
        float time = setTime;
        countTime.SetActive(true);

        while (secondCard == null)
        {
            countTimeText.text = time.ToString("N0");
            if (time <= 0)
            {
                firstCard.GetComponent<Card>().CloseCard();
                matchCardReset();
                countTime.SetActive(false);
                break;
            }
            time -= Time.deltaTime;
            yield return null;
        }

        isSingleCardSelect = false;
    }

    public void gameOver()
    {
        Time.timeScale = 0.0f;
        endPanel.SetActive(true);
    }

    void FlashTimeText()
    {
        float flashSpeed = 1f; // 깜빡거림 속도 조절
        float lerpValue = Mathf.PingPong(Time.time * flashSpeed, 1f);
        Color originalColor = timeText.color;
        Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, lerpValue);

        timeText.color = flashColor;
    }
    //--------------------------------------------------------------------------------Time
    //-----------------------------------------------------------------------------------------------------------Test Code












    //-----------------------------------------------------------------------------------Start Cart Effect
    public bool AllCheckCardOriginalPosition()
    {
        for (int i = 0; i < cardPack.Count; i++)
        {
            float x = cardPack[i].GetComponent<Card>().x;
            float y = cardPack[i].GetComponent<Card>().y;

            if (cardPack[i].transform.position != new Vector3(x, y, 0))
            {
                return false;
            }
        }

        return true;

    }

    public void AllOpenCard()
    {
        StartCoroutine(AllCardOpenCo());
    }


    IEnumerator AllCardOpenCo()
    {
        for (int i = 0; i < cardPack.Count; i++)
        {
            GameObject front = cardPack[i].GetComponent<Card>().front;
            GameObject back = cardPack[i].GetComponent<Card>().back;

            cardPack[i].GetComponent<Card>().CardFlip(front, back);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(timeTheCardIsOpen);

        for (int i = 0; i < cardPack.Count; i++)
        {
            GameObject front = cardPack[i].GetComponent<Card>().front;
            GameObject back = cardPack[i].GetComponent<Card>().back;

            cardPack[i].GetComponent<Card>().CardFlip(back, front);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.5f);

        gameState = GameState.Start;

    }
}
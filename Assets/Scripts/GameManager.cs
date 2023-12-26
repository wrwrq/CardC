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
    public Text PenaltyText;

    [Header("Game State")]
    GameState gameState;
    bool isSingleCardSelect;
    public Transform board;
    public int boardSizeX;
    public int boardSizeY;
    int gameLevel;
    public int matchCount; //Clear conditions

    public int tryPoint; //count, flip card

    public bool inChecking;
    public bool fullCard;


    [Header("Card")]
    int[] prefebIdxs;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject nameCard;
    public GameObject failCard;

    public GameObject card; //card Prefeb
    public Transform cardStartPot;
    public float cardSizeX;
    public float cardSizeY;
    [Range(0,1)]
    public float outLinePercent;
    public List<Sprite> cardImages = new List<Sprite>();
    public List<GameObject> cardPack = new List<GameObject>();
    Queue<int> queue = new Queue<int>();
    int[] prefebsIdx;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip success;
    public AudioClip fail;
    public AudioClip bgm;

    [Header("Time")]
    public Text timeText;

    float gameTime= 30f;   // I think we'd better run out of time.
    float setTime = 5; //only one card flip, count down parameter
    public int limitTime;
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
        gameTime += Time.deltaTime;
        timeText.text = gameTime.ToString("N2");
        //if (gameTime >= limitTime)
        //{
        //    EndGame();
        //}

        //if (firstCard != null && secondCard == null)
        //{
        //    setTime += Time.deltaTime;
        //    if (setTime >= 5)
        //    {
        //        
        //        firstCard = null;
        //        setTime = 0;
        //    }
        //}
        //RunTime();
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

        setTime = 0;
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

    //--------------------------------------------------------------------------------Board
    void GeneratorBoard()
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


    void Shuffle()
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

    IEnumerator CreateNewCard()
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
                GameObject newCard = Instantiate(card, new Vector3(x,y,0), Quaternion.identity);

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

    public void Match2()
    {
        StartCoroutine(Match2Co());
    }

   IEnumerator Match2Co()
    {
        fullCard = true;
        setTime = 5;

        yield return new WaitForSeconds(0.8f);
        string firstName = firstCard.GetComponent<Card>().frontImage.GetComponent<Image>().sprite.name;
        string secondName = secondCard.GetComponent<Card>().frontImage.GetComponent<Image>().sprite.name;

        if (firstName == secondName)
        {
            audioSource.PlayOneShot(success);
            Debug.Log("Matched!");

            Destroy(firstCard);
            Destroy(secondCard);

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
            FailCardInvoke();
            StartCoroutine(PenaltyUi());
            gameTime -= penaltyTime;
        }
        matchCardReset();
    }

    void matchCardReset()
    {
        firstCard = null;
        secondCard = null;
        inChecking = false;
        fullCard = false;
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

            // 시간이 얼마 안 남았을 때 깜빡거리는 효과
            if (gameTime <= 10f) // 필요에 따라 조절
            {
                FlashTimeText();
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

}

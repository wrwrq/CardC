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
    public Text PenaltyText;

    [Header("Game State")]
    //게임 난이도
    //public GameLevel gameLevelState;
    public GameState gameState;// 게임 상태(준비, 시작, 게임오버)
    bool isSingleCardSelect; //카드 하나만 선택했을때
    public Transform board;
    //게임 필수 정보
    public int boardSizeX;
    public int boardSizeY;
    public float timeTheCardIsOpen;
    public float setTime; //only one card flip, count down parameter
    public float gameTime;
    public int penaltyTime;

    bool setEndpanel;

    public string gameStageName;// 게임 스테이지(씬 이름) 이름
    public int gameLevel;//   minLevel = 1   maxLevel = 3
    Color originalColor = Color.black;


    public int matchCount; //게임 클리어 조건,

    public bool inChecking;
    public bool fullCard;
    

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
    public float initialVolume;
    public float targetVolume;


    [Header("Time")]
    public Text timeText;
    public GameObject countTime;
    public Text countTimeText;
    bool timeIsRunningOug;

    [Header("Score")]
    public Text timeScoreTxt;
    public Text matchScoreTxt;
    public Text failScoreTxt;
    public Text totalScoreTxt;
    public Text maxScoreTxt;
    private int timeScore = 0;
    private int matchScore = 0;
    private int failScore = 0;
    private int totalScore = 0;
    private int maxScore;

    [Header("Effect")]
    public GameObject matchEffectParticle;

    void Awake()
    {
        I = this;
    }

    private void Start()
    {
        //PlayerPrefs 
        // stage -> 게임 랩
        // "Stage" + gameLevel -> 게임 점수
        gameLevel = PlayerPrefs.GetInt("stage");
        gameStageName = "Stage" + gameLevel;

        audioSource.clip = bgm;
        audioSource.Play();
        GeneratorBoard();

        //초기화
        PlayerPrefs.SetFloat("maxScore", 0f);
        maxScoreTxt.text = "0";
        Time.timeScale = 1;
        initialVolume = 0.1f; // 배경 음악의 초기 볼륨으로 설정
        audioSource.volume = initialVolume;
        audioSource.pitch = 1;
        targetVolume = 0.2f; // 10초 남았을 때의 대상 볼륨으로 설정
        gameState = GameState.Ready;
        timeText.text = gameTime.ToString("N2");
        timeIsRunningOug = false;
        setEndpanel = false;
    }


    private void Update()
    {
        if (gameState == GameState.Start)
        {
            RunTime();
            SingleCardTimeRunCo();
        }
        if (gameState == GameState.GameOver)
        {
            TotalScore(); // 실패시 점수 호출
        }
    }

    IEnumerator Penaltyui()
    {
        Text temp = Instantiate(PenaltyText);
        temp.transform.SetParent(GameObject.Find("Time/TimeText").transform);
        temp.text = "-" + penaltyTime.ToString();
        yield return new WaitForSeconds(0.5f);
        Destroy(temp.gameObject);
    }

    public void Match()
    {
        setTime = 0;
        if (firstCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name == secondCard.transform.Find("Back").GetComponent<SpriteRenderer>().sprite.name)
        {
            RunTime();
        }
    }


    //--------------------------------------------------------------------------------폐기 예정
    //void EndGame()
    //{
    //    tryText.gameObject.SetActive(true);
    //    tryText.text += tryPoint;
    //    Time.timeScale = 0;
    //    //SceneManager.LoadScene("");
    //}


    //void SetGameState(GameLevel _gamelevel)
    //{
    //    if (endPanel.activeSelf)
    //    {
    //        endPanel.SetActive(false);
    //    }
    
    //    boardSizeX = _gamelevel.boardSizeX;
    //    boardSizeY = _gamelevel.boardSizeY;
    //    timeTheCardIsOpen = _gamelevel.timeTheCardIsOpen;
    //    setTime = _gamelevel.setTime;
    //    gameTime = _gamelevel.gameTime;
    //    penaltyTime = _gamelevel.penaltyTime;

    //}


    //shortest record
    void SetPlayerPrefs()
    {
        string currentStageName = "Stage" + gameLevel;
        int localDataTotalScore = PlayerPrefs.GetInt(currentStageName);
        if (localDataTotalScore < totalScore)
        {
            PlayerPrefs.SetInt(currentStageName, totalScore);
            SetStagemaxScore();
        }
    }
    void SetStagemaxScore()
    {
        string stagemaxScoreKey = "StagemaxScore" + gameLevel;
        int stagemaxScore = PlayerPrefs.GetInt(stagemaxScoreKey, 0);

        if (totalScore > stagemaxScore)
        {
            PlayerPrefs.SetInt(stagemaxScoreKey, totalScore);
        }
    }

    //--------------------------------------------------------------------------------폐기 예정정

    //--------------------------------------------------------------------------------Board
    public void GeneratorBoard() //보드 생성
    {
        //폐기예정
        //게임 난이도 설정
        //SetGameState(gameLevels[gameLevel - 1]);

        //if (GameObject.Find("Board"))
        //{
        //    countTime.SetActive(false);
        //    endPanel.SetActive(false);
        //    isSingleCardSelect = false;
        //    DestroyImmediate(GameObject.Find("Board"));
        //    cardPack.Clear();
        //    matchCardReset();
        //    matchCount = 0;
        //    timeText.color = originalColor;
        //    gameTime = gameLevelState.gameTime;
        //}
        //폐기예정정

        board = new GameObject("Board").transform;

        prefebIdxs = new int[boardSizeX * boardSizeY];

        Shuffle();

        StartCoroutine(CreateNewCard());

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
        Setendpanel();

        if (firstName == secondName)
        {
            matchCount++;
            audioSource.PlayOneShot(success);

            //particle effect
            Destroy(Instantiate(matchEffectParticle, firstCard.transform.position, Quaternion.identity), 1f);
            Destroy(Instantiate(matchEffectParticle, secondCard.transform.position, Quaternion.identity), 1f);

            Debug.Log("Matched!");
            //카드 매칭시 카드 제거
            Destroy(firstCard);
            Destroy(secondCard);
            //카드 점수 추가
            matchScore += 10; // Card match successful, 10 point
            matchScoreTxt.text = matchScore.ToString();
            //자기소개 오브젝트 활성화
            Time.timeScale = 0f;
            nameCard.SetActive(true);
            nameCard.GetComponent<Introduction>().matchName(firstName);
        }
        else
        {
            audioSource.PlayOneShot(fail);
            Debug.Log("Not matched!");
            //카드 다시 원위치
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
            failCard.SetActive(true);
            //카드 매칭 실패 효과
            Invoke("FailCard", 1f);
            StartCoroutine(PenaltyUi());
            //매칭 실패 시간 패널티
            gameTime -= penaltyTime;
            //카드 점수
            failScore--; // Card matched failed, -1 point
            failScoreTxt.text = failScore.ToString();
        }

        if(matchCount == boardSizeX * boardSizeY/2)
        {
            
            if(setEndpanel == true)
            {
                Debug.Log("판넬 등장 전");
                yield return new WaitForSeconds(0.001f);
                Debug.Log("판넬 등장 후");
                OnDisable();
            }
            
            Clear();
        }
        //모든 카드 맞췄을 경우 조건 추가
        TotalScore();
        matchCardReset();
       
    }

    void Clear() //게임 클리어 
    {
        gameLevel++;
        Debug.Log(gameLevel);
        gameLevel = Mathf.Clamp(gameLevel, 0, 3);
        PlayerPrefs.SetInt("Unlock", gameLevel);
    }

    //--------------카드 매칭 실패
    void FailCard()
    {
        failCard.SetActive(false);
    }

    IEnumerator PenaltyUi()
    {
        Text temp = Instantiate(PenaltyText);
        temp.transform.SetParent(GameObject.Find("Timer/TimeText").transform);
        temp.text = "-" + penaltyTime.ToString();
        yield return new WaitForSeconds(0.5f);
        Destroy(temp.gameObject);
    }
    //--------------카드 매칭 실패


    void matchCardReset() //초기화
    {
        firstCard = null;
        secondCard = null;
        inChecking = false;
        fullCard = false;
    }

    public void TotalScore()
    {
        // timeScore, matchScore, failScore ++ total
        totalScore = timeScore + matchScore + failScore;
        totalScoreTxt.text = totalScore.ToString() + "점";

        if (PlayerPrefs.HasKey("maxScore") == false)
        {
            PlayerPrefs.SetFloat("maxScore", totalScore);
        }
        else
        {
            if (PlayerPrefs.GetFloat("maxScore") < totalScore)
            {
                PlayerPrefs.SetFloat("maxScore", totalScore);
            }
        }
        maxScoreTxt.text = PlayerPrefs.GetFloat("maxScore").ToString("N0");
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
                endPanel.SetActive(true); // game end, Score board call
                TotalScore(); // 시간 다 떨어져도 점수를 계산
                audioSource.pitch = 1f;
                audioSource.volume = 0.1f;
            }

            // 시간이 얼마 안 남았을 때 깜빡거리는 효과
            else if (gameTime <= 10f) // 필요에 따라 조절
            {
                if (!timeIsRunningOug)
                {
                    timeIsRunningOug = true;
                    StartCoroutine(SoundPitchUpCo());
                }

                if (audioSource.volume < targetVolume)
                {

                    audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, 1.0f - (gameTime / 10f));
                }
                FlashTimeText();

            }


            if (firstCard != null && secondCard == null && !isSingleCardSelect)
            {
                StartCoroutine(SingleCardTimeRunCo());
            }

           
            timeScoreTxt.text = timeScore.ToString();  // timescore update
            timeScore = Mathf.RoundToInt(gameTime); // 1point per second
        }
    }

    IEnumerator SoundPitchUpCo()
    {
        while(gameState == GameState.Start)
        {
            audioSource.pitch += 0.01f;
            yield return new WaitForSeconds(.5f);
        }
        timeIsRunningOug = false;
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
                //카드 틀림, 패널티
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

    void FlashTimeText()
    {
        float flashSpeed = 1f; // 깜빡거림 속도 조절
        float lerpValue = Mathf.PingPong(Time.time * flashSpeed, 1f);
        Color changeColor = originalColor;
        Color flashColor = new Color(changeColor.r, changeColor.g, changeColor.b, lerpValue);

        timeText.color = flashColor;
    }
    //--------------------------------------------------------------------------------Time


    //-----------------------------------------------------------------------------------Start Cart Effect
    public bool AllCheckCardOriginalPosition() //카드가 자기위치에 있는지 확인하는 메서드
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

    public void AllOpenCard()// 모든 카드 오픈
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

    //-----------------------------------------------------------------------------------Start Cart Effect


    //-----------------------------------------------------------------------------------게임 난이도
    //[System.Serializable]
    //public class GameLevel
    //{
    //    public int boardSizeX;
    //    public int boardSizeY;
    //    public float timeTheCardIsOpen;
    //    public float setTime;
    //    public float gameTime;
    //    public int penaltyTime;

    //}

    //마지막 nameCard 사라지고 endPanel 등장
    void Setendpanel()
    {
        if (nameCard.GetComponent<Introduction>().LastCard(boardSizeX * boardSizeY / 2) == true)
        {
            setEndpanel = true;
            Debug.Log("작동");
        }
    }

    void OnDisable()
    {
        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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


    public Text tryText;
    GameObject firstCard;
    GameObject secondCard;
    int tryPoint;
    float gameTime;
    public int LimitTime;
    public int PenaltyTime;
    private void Update()
    {
        gameTime += Time.deltaTime;
        if (gameTime >= LimitTime)
        {
            EndGame();
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
        tryPoint++;
        if (firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name == secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name)
        {

        }
        else if (firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name != secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name)
        {
            gameTime += PenaltyTime;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Transform cardHolder;



    [Header("State")]
    public float boardSize;

    [Header("Card")]
    public GameObject cardPrefeb;
    public float cardSizeX;
    public float cardSizeY;
    [Range(0,1)]
    public float outLinePercent;

    [Space(10)]
    public List<Sprite> cardImages = new List<Sprite>();



    private void Start()
    {
        GeneratorBoard();
    }


    void GeneratorBoard()
    {
      for(int i = 0; i< boardSize; i++)
        {
            for(int j = 0; j< boardSize; j++)
            {
                float x = (-boardSize / 2 + i + 0.5f) * cardSizeX;
                float y = (-boardSize / 2 + j + 0.5f) * cardSizeY;
                GameObject newCard = Instantiate(cardPrefeb, new Vector3(x, y, 0), Quaternion.identity);
                newCard.transform.localScale = newCard.transform.localScale * (1 - outLinePercent);

                newCard.transform.SetParent(cardHolder);

            }
        }  
    }
}

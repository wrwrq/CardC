using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Game State")]
    public int boardSize;

    [Header("Card")]
    int[] prefebIdxs;

    public List<Sprite> cardImages = new List<Sprite>();
    Queue<int> queue = new Queue<int>();

    //void Mix(int[] mix)
    //{//               �������� ���� �Լ��Դϴ�. int�迭�� �־��ּ���
    //    System.Random ran = new System.Random();
    //    int temp;
    //    int randomIndex;
    //    for (int i = 0; i < mix.Length; i++)
    //    {
    //        randomIndex = ran.Next(0, mix.Length);
    //        temp = mix[i];
    //        mix[i] = mix[randomIndex];
    //        mix[randomIndex] = temp;
    //    }
    //}
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
}

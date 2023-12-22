using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject firstCard;
    public GameObject secondCard;

    float t = 60f;
    float settime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t -= Time.deltaTime;
        //텍스트 UI 만들어서 집어 넣어야하는 곳
        
        //첫번째 카드 선택 후 5초 카운트 하는 부분, 초기화는 매치되는 함수 만들 때 넣을 예정
        if (firstCard != null && secondCard == null)
        {
            settime += Time.deltaTime;
            if (settime >= 5)
            {
                //카드 원래대로 만드는 함수 넣어두기
                firstCard = null;
                settime = 0;
            }
        }
    }

    void Mix(int[] mix)
    {//               무작위로 섞는 함수입니다. int배열을 넣어주세요
        System.Random ran = new System.Random();
        int temp;
        int randomIndex;
        for (int i = 0; i < mix.Length; i++)
        {
            randomIndex = ran.Next(0, mix.Length);
            temp = mix[i];
            mix[i] = mix[randomIndex];
            mix[randomIndex] = temp;
        }
    }
}

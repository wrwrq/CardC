using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

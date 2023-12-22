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
        //�ؽ�Ʈ UI ���� ���� �־���ϴ� ��
        
        //ù��° ī�� ���� �� 5�� ī��Ʈ �ϴ� �κ�, �ʱ�ȭ�� ��ġ�Ǵ� �Լ� ���� �� ���� ����
        if (firstCard != null && secondCard == null)
        {
            settime += Time.deltaTime;
            if (settime >= 5)
            {
                //ī�� ������� ����� �Լ� �־�α�
                firstCard = null;
                settime = 0;
            }
        }
    }

    void Mix(int[] mix)
    {//               �������� ���� �Լ��Դϴ�. int�迭�� �־��ּ���
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

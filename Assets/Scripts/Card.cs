using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip flip; // ī�� ������ ����
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
    }

    public void openCard()
    {

        GetComponent<Animator>().SetBool("isClick", true);

        audioSource.PlayOneShot(flip); // openCard() �Լ� �۵��� flip ���� 1ȸ ���

        transform.Find("Back").gameObject.SetActive(true);
        transform.Find("Front").gameObject.SetActive(false);
        if (GameManager.I.firstCard == null)
        {
            GameManager.I.firstCard = gameObject;
        }
        else
        {
            GameManager.I.secondCard = gameObject;
            GameManager.I.Match();
        }
    }

    public void closeCard()
    {
        GetComponent<Animator>().SetBool("isClick", false);
        transform.Find("Back").gameObject.SetActive(false);
        transform.Find("Front").gameObject.SetActive(true);
    }

    public void closeCardInvoke()
    {
        Invoke("closeCard", 1f);
    }
}

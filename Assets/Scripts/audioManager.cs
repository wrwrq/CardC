using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioManager : MonoBehaviour
{

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); // AudioSource ������Ʈ ���� (start ���带 �츮�� ����)
    }

    // Update is called once per frame
    void Update()
    {

    }

}

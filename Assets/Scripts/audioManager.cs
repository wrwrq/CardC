using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioManager : MonoBehaviour
{
    public AudioClip start; // ���� ��ư ����
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(start); // AudioSource ������Ʈ ('start' sound) ����
    }

    // Update is called once per frame
    void Update()
    {

    }
}

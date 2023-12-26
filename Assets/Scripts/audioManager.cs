using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioManager : MonoBehaviour
{
    public AudioClip start; // 시작 버튼 사운드
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(start); // AudioSource 오브젝트 ('start' sound) 보존
    }

    // Update is called once per frame
    void Update()
    {

    }
}

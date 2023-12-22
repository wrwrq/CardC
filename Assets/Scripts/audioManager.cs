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
        DontDestroyOnLoad(gameObject); // AudioSource 오브젝트 보존 (start 사운드를 살리기 위해)
    }

    // Update is called once per frame
    void Update()
    {

    }

}

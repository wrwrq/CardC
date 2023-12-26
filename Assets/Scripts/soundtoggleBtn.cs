using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class soundtoggleBtn : MonoBehaviour
{
    public Sprite[] sprites;
    public bool isClicked;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("soundtoggleBtn").GetComponent<Image>().sprite = sprites[0]; // 시작할때 ON 이미지
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SoundToggle() // 클릭시 volume, Image sprite control
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;

        isClicked = !isClicked;
        if (isClicked) 
        {
            GameObject.Find("soundtoggleBtn").GetComponent<Image>().sprite = sprites[0];
        }
        else
        {
            GameObject.Find("soundtoggleBtn").GetComponent<Image>().sprite = sprites[1];
        }
    }
}

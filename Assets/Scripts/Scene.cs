using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    void Start()
    {
        int unlock = PlayerPrefs.GetInt("Unlock", 1);
        for (int i = 0; i < unlock; i++)
        {
            Transform childTrans = this.transform.GetChild(i);
            childTrans.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>("unlock");
            childTrans.GetComponent<Button>().onClick.AddListener(childTrans.GetComponent<StartGame>().StartButton);
            childTrans.GetComponentInChildren<Text>().text = (1 + i).ToString();
        }
        for (int i = unlock; i < 12; i++)
        {
            Transform childTrans = this.transform.GetChild(i);
            childTrans.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>("lock");
        }
    }
}

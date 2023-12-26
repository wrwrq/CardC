using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
	public void StartButton()
	{
		SceneManager.LoadScene("MainScene");
		PlayerPrefs.SetInt("stage",Int32.Parse(GetComponentInChildren<Text>().text));
	}
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public void StartButton()
	{
		SceneManager.LoadScene("MainScene");
		PlayerPrefs.SetInt("stage", Int32.Parse(GetComponentInChildren<Text>().text));
	}

	public void RetryButton()
	{
		SceneManager.LoadScene("MainScene");
	}

	public void StageButton()
	{

	}

	public void NextButton()
	{

	}


	public GameObject SettingCanvas;
	public void SettingButton()
	{
        SettingCanvas.SetActive(true);
	}
	public void ReSetGame()
	{
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene("StageSelection");
	}
	public void CloseMenu()
	{
        SettingCanvas.SetActive(false);
	}
}

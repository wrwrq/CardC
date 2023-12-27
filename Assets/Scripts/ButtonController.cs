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
		SceneManager.LoadScene("Stage" + GetComponentInChildren<Text>().text);
		PlayerPrefs.SetInt("stage", Int32.Parse(GetComponentInChildren<Text>().text));
	}

	public void RetryButton()
	{
		//SceneManager.LoadScene("MainScene");
		GameManager.I.GeneratorBoard();
	}

	public void StageButton()
	{
        SceneManager.LoadScene("StageSelection");
	}

    public void NextButton()
	{
		int newGameLevel = PlayerPrefs.GetInt("Unlock");
		GameManager.I.gameLevel = newGameLevel;
        GameManager.I.GeneratorBoard();
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

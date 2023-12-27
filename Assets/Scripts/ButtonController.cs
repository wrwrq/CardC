using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public CanvasController controller;
	public void StartButton()
	{
		SceneManager.LoadScene("Stage" + GetComponentInChildren<Text>().text);
	}

	public void RetryButton()
	{
		SceneManager.LoadScene(GameManager.I.gameStageName);

	}

	public void StageButton()
	{
        SceneManager.LoadScene("StageSelection");
		controller.ToggleObjects();
	}

    public void NextButton()
	{
		string stage = "Stage" + GameManager.I.gameLevel;
		PlayerPrefs.SetInt("stage", GameManager.I.gameLevel);
		SceneManager.LoadScene(stage);
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

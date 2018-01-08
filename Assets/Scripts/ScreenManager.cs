// ************************************************************************ 
// File Name:   ScreenManager.cs 
// Purpose:    	Transfers between scenes
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Class: ScreenManager
// ************************************************************************
public class ScreenManager : MonoBehaviour
{
	void Start()
	{
		LoadScreen("MainMenu");
	}
	
	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	public void LoadScreen(string screenName)
	{
		StartCoroutine(LoadSceneAsync(screenName));
	}
	
	
	// ********************************************************************
	// Function:	LoadScene()
	// Purpose:		Loads the supplied scene
	// ********************************************************************
	public IEnumerator LoadSceneAsync(string sceneName)
	{
		// Load loading screen
		yield return Application.LoadLevelAsync("LoadingScreen");
		
		// !!! unload old screen (automatic)
		
		float endTime = Time.time;
		
		// Load level async
		yield return Application.LoadLevelAdditiveAsync(sceneName);
		
		while (Time.time < endTime)
			yield return null;
		
		// Play music or perform other misc tasks

		// !!! unload loading screen
		LoadingSceneManager.UnloadLoadingScene();
	}


}

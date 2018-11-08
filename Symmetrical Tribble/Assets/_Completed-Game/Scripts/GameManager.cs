using UnityEngine;
﻿using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public float levelStartDelay = 2f;

    private int level = 1;
    private Text levelText;
    private GameObject levelImage; // To hide the level while it's being built
    private BoardManager boardScript;
    private bool doingSetup = true;

    void Awake() {
      //Check if instance already exists
          if (instance == null)

              //if not, set instance to this
              instance = this;

          //If instance already exists and it's not this:
          else if (instance != this)

              //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
              Destroy(gameObject);

          //Sets this to not be destroyed when reloading scene
          DontDestroyOnLoad(gameObject);

          //Get a component reference to the attached BoardManager script
          boardScript = GetComponent<BoardManager>();

          //Call the InitGame function to initialize the first level
          initGame();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        level++;
        initGame();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void initGame() {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Stage " + level;

        levelImage.SetActive(true);

        Invoke("hideLevelImage", levelStartDelay);

        boardScript.setupScene(level);
    }

    void hideLevelImage() {
        levelImage.SetActive(false);


        doingSetup = false;
    }

    public void gameOver() {
        levelText.text = "You were killed after " + level + " stages.";
        levelImage.SetActive(true);

        enabled = false;
    }

    // Update is called once per frame
    void Update() {

    }
}

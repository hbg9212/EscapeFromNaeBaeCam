using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    public static SelectManager instance;

    public bool rangedPlayer { get; private set; }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void SelectRangedPlayer() {
        rangedPlayer = true;
        SceneManager.LoadScene("BattelTest2");
    }

    public void SelectMeleePlayer() {
        rangedPlayer = false;
        SceneManager.LoadScene("BattelTest2");
    }

    public void DestroyThis() {
        Destroy(gameObject);
    }
}

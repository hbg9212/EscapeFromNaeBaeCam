using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    public void SelectRangedPlayer() {
        SceneManager.LoadScene("RangedScene");
    }

    public void SelectMeleePlayer() {
        SceneManager.LoadScene("MeleeScene");
    }

    public void StartScene() {
        SceneManager.LoadScene("SelectScene");
    }
}

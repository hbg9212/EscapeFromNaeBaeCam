using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private HealthSystem playerHealthSystem;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    private AudioClip gameAudio;

    public Transform Player { get; private set; }
    [SerializeField] private string playerTag = "Player";

    private Transform dropPosition;

    private void Awake()
    {
        instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;
        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDeath += GameOver;

        //SoundManager.instance.
        gameOverMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CharacterSelect() {
        SceneManager.LoadScene("SelectScene");
    }

    public void GameOver() {
        StopAllCoroutines();
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }
}
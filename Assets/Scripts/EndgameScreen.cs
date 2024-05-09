using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndgameScreen : MonoBehaviour, IGameEventListener<GameEndInfo>
{
    public GameEndEvent gameEndEvent;
    public GameObject endGameScreen;
    public Camera endGameCamera;

    private void OnEnable()
    {
        gameEndEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEndEvent.UnregisterListener(this);
    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        endGameScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        endGameCamera.gameObject.SetActive(false);
    }

    public void OnEventRaised(GameEndInfo arg)
    {
        endGameCamera.gameObject.SetActive(true);
        endGameScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}

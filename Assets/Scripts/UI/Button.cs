using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [Header("Sound Variable")]
    public AudioClip buttonSound;

    public void GoToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
        }
    }

    public void GoToOptions()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Options);
        }
    }

    public void GoToCredits()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Credits);
        }
    }

    public void BeginGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Pause);
        }
    }

    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Restart);
        }
    }

    public void GoBackToPreviousState()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.Instance.previousState);
        }
    }
}

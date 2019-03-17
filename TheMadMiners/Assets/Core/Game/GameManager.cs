using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnGameStart();
public delegate void OnGameEnd();

public class GameManager : MonoBehaviour
{
    public event OnGameStart GameHasBeenStarted;
    public event OnGameEnd GameHasBeenEnded;

    public static GameManager singleton;

    #region Variables

    [Header("Editor")]
    public WorldEditor worldEditor;
    public GameObject GameUI;
    public GameObject EditorUI;
    public GameObject PlayButton;
    public GameObject StopButton;
    public bool isEditor = true;

    [Header("Player")]
    public GameObject PlayerPrefab;

    public bool doublejump = false;
    public bool walljump = false;
    public bool wallstick = false;
    public bool wallslide = false;
    public bool cornergrab = false;
    public bool dash = false;

    #endregion

    [HideInInspector]
    public GameObject player;

    private void Start()
    {
        singleton = this;
    }

    public void PlayGame()
    {
        if (worldEditor.WorldName != "")
        {
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("Beginning");
            GameObject endPoint = GameObject.FindGameObjectWithTag("Ending");

            if (spawnPoint != null)
            {
                if (endPoint != null)
                {
                    worldEditor.SaveWorld(false);
                    EditorUI.SetActive(false);
                    PlayButton.SetActive(false);
                    GameUI.SetActive(true);
                    GameUIManager.singleton.RestHearts();
                    worldEditor.Cursor.SetActive(false);
                    worldEditor.MainCamera.GetComponent<CameraOrbiting>().enabled = false;
                    worldEditor.MainCamera.GetComponent<SmoothCamera2D>().enabled = true;
                    worldEditor.enabled = false;
                    endPoint.GetComponent<SpriteRenderer>().color = Color.clear;
                    spawnPoint.GetComponent<SpriteRenderer>().color = Color.clear;

                    if (isEditor)
                    {
                        StopButton.SetActive(true);
                    }
                    player = Instantiate(PlayerPrefab, spawnPoint.transform.position, Quaternion.identity);

                    //Check Delegate if isn't null
                    if(GameHasBeenStarted != null)
                    {
                        GameHasBeenStarted();
                    }

                    //PlayerProperties
                    if (doublejump)
                    {
                        player.GetComponent<PlatformerMotor2D>().numOfAirJumps = 1;
                    }
                    if (walljump)
                    {
                        player.GetComponent<PlatformerMotor2D>().enableWallJumps = true;
                    }
                    if (wallstick)
                    {
                        player.GetComponent<PlatformerMotor2D>().enableWallSticks = true;
                    }
                    if (wallslide)
                    {
                        player.GetComponent<PlatformerMotor2D>().enableWallSlides = true;
                    }
                    if (cornergrab)
                    {
                        player.GetComponent<PlatformerMotor2D>().enableCornerGrabs = true;
                    }
                    if (dash)
                    {
                        player.GetComponent<PlatformerMotor2D>().enableDashes = true;
                    }

                    Fade.singleton.FadeIn(0f);
                    Fade.singleton.FadeOut(6f);

                    spawnPoint.GetComponent<Beginning>().PlayAnimation();
                }
                else
                {
                    Notifications.singleton.GetNotification("No Ending!", Color.yellow);
                }
            }
            else
            {
                Notifications.singleton.GetNotification("No Beginning!", Color.yellow);
            }
        }
        else
        {
            Notifications.singleton.GetNotification("No Name!", Color.yellow);
        }
    }

    public void EndingGame(bool isNotDead)
    {
        if (player != null)
        {
            Destroy(player);
        }

        if (isNotDead)
        {
           
        }
        else
        {
         
        }
    }

    public void StopGame()
    {
        if (player != null)
        {
            Destroy(player);
        }

        //Check Delegate if isn't null
        if (GameHasBeenEnded != null)
        {
            GameHasBeenEnded();
        }

        worldEditor.enabled = true;
        worldEditor.MainCamera.GetComponent<SmoothCamera2D>().enabled = false;
        worldEditor.MainCamera.GetComponent<CameraOrbiting>().enabled = true;
        worldEditor.Cursor.SetActive(true);
        StopButton.SetActive(false);
        GameUI.SetActive(false);
        PlayButton.SetActive(true);
        EditorUI.SetActive(true);
        worldEditor.LoadWorld(true);
        Fade.singleton.FadeOut(1f);
    }
}

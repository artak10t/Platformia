using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour {
    public static PlayerProperties singleton;

    public Toggle DoubleJump;
    public Toggle WallJump;
    public Toggle WallStick;
    public Toggle WallSlide;
    public Toggle CornerGrab;
    public Toggle Dash;

    private void Start()
    {
        singleton = this;
    }


    public void SetDoubleJump(bool status)
    {
        DoubleJump.isOn = status;
    }

    public void SetWallJump(bool status)
    {
        WallJump.isOn = status;
    }

    public void SetWallStick(bool status)
    {
        WallStick.isOn = status;
    }

    public void SetWallSlide(bool status)
    {
        WallSlide.isOn = status;
    }

    public void SetCornerGrab(bool status)
    {
        CornerGrab.isOn = status;
    }

    public void SetDash(bool status)
    {
        Dash.isOn = status;
    }
}

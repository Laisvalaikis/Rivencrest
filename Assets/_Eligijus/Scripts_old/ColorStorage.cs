using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStorage
{
    static public Color TeamColor(string teamName)
    {
        Color teamColor = Color.white;//grey?
        if (teamName == "Magenta")
        {
            teamColor = new Color(157 / 255f, 18 / 255f, 255 / 255f, 244);
        }
        else if (teamName == "Red")
        {
            teamColor = new Color(255 / 255f, 18 / 255f, 33 / 255f, 244);
        }
        else if (teamName == "Green")
        {
            teamColor = new Color(18 / 255f, 255 / 255f, 83 / 255f, 244);
        }
        else if (teamName == "Yellow")
        {
            teamColor = new Color(255 / 255f, 201 / 255f, 18 / 255f, 244);
        }
        else if (teamName == "Blue")
        {
            teamColor = new Color(49 / 255f, 61 / 255f, 255 / 255f, 244);
        }
        else if (teamName == "Malachite")
        {
            teamColor = new Color(0 / 255f, 226 / 255f, 131 / 255f, 244);
        }
        return teamColor;
    }
}

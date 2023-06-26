using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public Color AttackHighlight = new Color(255, 69, 69);
    public Color AttackHighlightHover = new Color(255, 227, 0);
    public Color MovementHighlight = new Color(130, 255, 95);
    public Color MovementHighlightHover = new Color(255, 227, 0);
    public Color OtherHighlight = new Color(146, 212, 255);
    public Color InspectionHighlight = new Color(113, 113, 113);
    public List<TeamSprites> teamSprites;

    public void SetPortraitBoxSprites(GameObject portraitBox, string teamName)
    {
        foreach(TeamSprites x in teamSprites)
        {
            if(x.teamName == teamName)
            {
                portraitBox.GetComponent<TeamInformation>().ChangeBoxSprites(x.portraitBoxMain, x.portraitBoxExtension, x.CharacterButton);
            }
        }
    }

    [System.Serializable]
    public class TeamSprites
    {
        public string teamName;
        public Sprite portraitBoxMain;
        public Sprite portraitBoxExtension;
        public Sprite CharacterButton;
    }
}

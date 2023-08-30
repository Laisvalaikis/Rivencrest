using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class UndoAction
    {
        public Vector3 coordinates;
        public GameObject character;
        public int movementPoints;
        public bool available;

        public UndoAction(Vector3 coordinates, GameObject character, int movementPoints)
        {
            this.coordinates = coordinates;
            this.character = character;
            this.movementPoints = movementPoints;
            available = true;
        }

        public UndoAction() { }

        public void MoveBack()
        {
            GameObject.Find("GameInformation").GetComponent<GameInformation>().DeselectTeam(character);
            GameObject.Find("GameInformation").GetComponent<GameInformation>().FocusSelectedCharacter(character);
            character.transform.position = coordinates + new Vector3(0f, 0f, -1f);
            // character.GetComponent<GridMovement>().AvailableMovementPoints += movementPoints;
            GameObject.Find("GameInformation").GetComponent<GameInformation>().undoAction.available = false;
        }
    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private RaycastHit2D raycast;
    public LayerMask blockingLayer;
    private GameObject PortalButton;
    public GameObject OtherPortalExit;

    void Start()
    {
        PortalButton = GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("PortalButton").gameObject;
    }
    void Update()
    {
        if (CheckIfSpecificTag(transform.gameObject, 0, 0, blockingLayer, "Player") && GetSpecificGroundTile(transform.gameObject, 0, 0, blockingLayer) == GameObject.Find("GameInformation").GetComponent<GameInformation>().SelectedCharacter
            && !GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.activeSelf)
        {
            PortalButton.GetComponent<PortalButton>().ActivePortal = gameObject;
            PortalButton.SetActive(true);
        }
        else if(PortalButton.GetComponent<PortalButton>().ActivePortal == gameObject)
        {
                PortalButton.GetComponent<PortalButton>().ActivePortal = null;
        }
        
    }
    public void Teleport()
    {
        if (CheckIfSpecificTag(OtherPortalExit, 0, 0, blockingLayer, "Player") || !CheckIfSpecificLayer(OtherPortalExit, 0, 0, blockingLayer))
        {
            if (CheckIfSpecificTag(OtherPortalExit, 0, 0, blockingLayer, "Player"))
            {
                GetSpecificGroundTile(OtherPortalExit, 0, 0, blockingLayer).transform.position = transform.position + new Vector3(0f, 0f, -2f);
            }
            GetSpecificGroundTile(transform.gameObject, 0, 0, blockingLayer).transform.position = OtherPortalExit.transform.position + new Vector3(0f, 0f, -2f);
            StartCoroutine(ExecuteAfterTime(0.001f, () =>
            {
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().EnableMovementAction();
            }));
            GameObject.Find("GameInformation").GetComponent<GameInformation>().FocusSelectedCharacter(GameObject.Find("GameInformation").GetComponent<GameInformation>().SelectedCharacter);
            gameObject.transform.Find("Visuals").GetComponent<Animator>().SetTrigger("teleport");
            OtherPortalExit.transform.Find("Visuals").GetComponent<Animator>().SetTrigger("teleport");
        }
    }
    protected GameObject GetSpecificGroundTile(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        return raycast.transform.gameObject;
    }
    protected bool CheckIfSpecificTag(GameObject tile, int x, int y, LayerMask chosenLayer, string tagName)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        else if (raycast.transform.CompareTag(tagName))
        {
            return true;
        }
        return false;
    }
    protected bool CheckIfSpecificLayer(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        return true;
    }
    IEnumerator ExecuteAfterTime(float time, System.Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
}

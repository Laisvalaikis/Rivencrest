using UnityEngine;

public class CreateFog : BaseAction
{
    [SerializeField] private GameObject fogPrefab;
    private bool isFogActive = true;
    private int i = 0;
    
    public override void OnTurnStart()//reikes veliau tvarkyt kai bus animacijos ir fog of war
    {
        if (isFogActive)
        {
            i++;
            if (i >= 2)
            {
                Destroy(fogPrefab);
                isFogActive = false;
                i = 0;
            }
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        spawnedCharacter = Instantiate(fogPrefab, position + new Vector3(0f,-0.5f) , Quaternion.identity);
        FinishAbility();
        isFogActive = true;
        i = 0;
    }
}

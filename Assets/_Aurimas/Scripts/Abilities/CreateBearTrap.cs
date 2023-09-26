using UnityEngine;

public class CreateBearTrap : BaseAction
{
    //reikia bearTrap sukurti kaip ir praeitame projekte
    [SerializeField] private GameObject bearTrapPrefab;
    private BearTrap _bearTrap;
    private int i = 0;

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        
        spawnedCharacter = Instantiate(bearTrapPrefab, position, Quaternion.identity);
        //_bearTrap.creator = gameObject;
        i = 0;
        FinishAbility();
    }
    
    public override void OnTurnStart()
    {
        if (spawnedCharacter != null)
        {
            i++;
            if (i >= 2)
            {
                Destroy(spawnedCharacter);
                i = 0;
            }
        }
    }
}

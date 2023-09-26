using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private PlayerTeams playerTeams;
    private TeamsList _teams;
    private Team _currentTeam;

    private void Start()
    {
        _teams = playerTeams.allCharacterList;
        if (_teams.teams.Count > 0)
        {
            _currentTeam = _teams.teams[0];
        }
    }

    bool IsGameOver()
    {
        return false;
    }

    private void ExecuteAllAbilitiesOnTurnEnd(Team team)
    {
        List<UsedAbility> usedAbilities = team.usedAbilities;
        for (int i = 0; i < usedAbilities.Count; i++) 
        {
            BaseAction usedAbility = usedAbilities[i].Ability;
            usedAbility.OnTurnEnd();
            if(usedAbility.turnsSinceCast >= usedAbility.turnLifetime)
                usedAbilities.RemoveAt(i);
            //ateity nuzoomint camera ten, kur vyksta abiličiokas
            //Kol runnina animacija, neexecutinti sekančio abiličioko
        }
    }

    public void AddUsedAbility(UsedAbility usedAbility)
    {
        _currentTeam.usedAbilities.Add(usedAbility);
    }
    
    public Team GetCurrentTeam()
    {
        return _currentTeam;
    }
    
    private void ExecuteAllAbilitiesOnTurnStart(Team team)
    {
        List<UsedAbility> usedAbilities = team.usedAbilities;
        for (int i = 0; i < usedAbilities.Count; i++) 
        {
            BaseAction usedAbility = usedAbilities[i].Ability;
            usedAbility.OnTurnStart();
            //ateity nuzoomint camera ten, kur vyksta abiličiokas
            //Kol runnina animacija, neexecutinti sekančio abiličioko
        }
    }

    public void EndTurn()
    {
        //todo: splash screen
        //execute on end
        //change team and wrap around list
        //if all teams did their thing
        //then execute on start
        ExecuteAllAbilitiesOnTurnEnd(_currentTeam);
        int currentTeamIndex = _teams.teams.IndexOf(_currentTeam);
        currentTeamIndex = (currentTeamIndex + 1) % _teams.teams.Count;
        _currentTeam = _teams.teams[currentTeamIndex];
        ExecuteAllAbilitiesOnTurnStart(_currentTeam);
    }
}

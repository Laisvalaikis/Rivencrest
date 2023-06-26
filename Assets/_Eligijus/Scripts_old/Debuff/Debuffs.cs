using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Debuffs
{
    private List<Debuff> debuffs = new List<Debuff>();

    public bool Contains(string debuffName)
    {    //    
        /*foreach(Debuff d in debuffs)
        {
            if(d.debuffName == debuffName)
            {
                return true;
            }
        }*/
        //bool debuffToFind = debuffs.Find(x => x.debuffName == debuffName) != null;
        return debuffs.Find(x => x.debuffName == debuffName) != null;
    }
    public Debuff Get(string debuffName)
    {
        return debuffs.Find(x => x.debuffName == debuffName);
    }
    public void Add(Debuff debuff)
    {
        if (Contains(debuff.debuffName))
        {
            Get(debuff.debuffName).causedBy.Add(debuff.causedBy[0]); //Ideda pirma elementa, nes implyinam kad tik vienas ir atsiustas
            Get(debuff.debuffName).animationTag = debuff.animationTag; //Debatable
        }
        else
        {
            debuffs.Add(debuff);
        }
    }
    public void Remove(string debuffName)
    {
        debuffs.RemoveAll(x => x.debuffName == debuffName);
    }
}

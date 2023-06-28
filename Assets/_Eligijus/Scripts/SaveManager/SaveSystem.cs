using UnityEngine;
using System.IO;
using System;

public static class SaveSystem
{
    private static string currentSlot = Application.persistentDataPath + "/currentSlot.log";
    private static string[] slots = { Application.persistentDataPath + "/slot1.log",
        Application.persistentDataPath + "/slot2.log", Application.persistentDataPath + "/slot3.log" };
    private static string globalStatistics = Application.persistentDataPath + "/globalstats.log";
    private static string[] slotStatistics = { Application.persistentDataPath + "/slot1stats.log",
        Application.persistentDataPath + "/slot2stats.log", Application.persistentDataPath + "/slot3stats.log" };

    public static void SaveTownData(TownData data, int slotIndex = -1)
    {
        if(slotIndex == -1)
        {
            slotIndex = GetCurrentSlot();
            if(slotIndex == -1)
            {
                throw new Exception("Problem retrieving current slot");
            }
        }
        else if(slotIndex < 0 || slotIndex > 2)
        {
            throw new Exception("Invalid slot index");
        }
        SaveCurrentSlot(slotIndex);
        LocalSaveSystem.Save(data, slots[slotIndex]);
    }

    public static TownData LoadTownData(int slotIndex = -1)
    {
        if(slotIndex == -1)
        {
            slotIndex = GetCurrentSlot();
        }
        if (slotIndex == -1)
        {
            throw new Exception("Problem retrieving current slot");
        }
        else
        {
            if (File.Exists(slots[slotIndex]))
            {
                TownData data = LocalSaveSystem.Load<TownData>(slots[slotIndex]);
                if(data == null)
                {
                    Debug.Log("Wrong file type in " + slots[slotIndex]);
                }
                return data;
            }
            else
            {
                // Debug.Log("Save file not found in " + slots[slotIndex]);
                return null;
            }
        }
    }

    public static void SaveCurrentSlot(int slotIndex)
    {
        if(slotIndex < 0 || slotIndex > 2)
        {
            throw new Exception("Invalid slot index");
        }
        LocalSaveSystem.Save(new CurrentSlot(slotIndex), currentSlot);
    }

    public static int GetCurrentSlot()
    {
        if (File.Exists(currentSlot))
        {
            CurrentSlot slot = LocalSaveSystem.Load<CurrentSlot>(currentSlot);
            if (slot != null)
            {
                if (slot.currentSlot < 0 || slot.currentSlot > 2)
                {
                    throw new Exception("Invalid slot index");
                }
                else return slot.currentSlot;
            }
            else
            {
                Debug.Log("Wrong file type in " + currentSlot);
                return -1;
            }
        }
        else
        {
            //Debug.Log("Save file not found in " + currentSlot);
            return -1;
        }
    }

    public static void SaveStatistics(Statistics data, bool global = false)
    {
        if(global)
        {
            LocalSaveSystem.Save(data, globalStatistics);
            return;
        }
        int slotIndex = GetCurrentSlot();
        if (slotIndex == -1)
        {
            throw new Exception("Problem retrieving current slot");
        }
        LocalSaveSystem.Save(data, slotStatistics[slotIndex]);
        //if (LoadStatistics(true) != null)
        //{
        //    LocalSaveSystem.Save(LoadStatistics(true).Add(data), globalStatistics);
        //}
        //else
        //{
        //    LocalSaveSystem.Save(data, globalStatistics);
        //}
    }

    public static Statistics LoadStatistics(bool global = false)
    {
        if(global)
        {
            return LocalSaveSystem.Load<Statistics>(globalStatistics);
        }
        return LocalSaveSystem.Load<Statistics>(slotStatistics[GetCurrentSlot()]);
    }

    public static bool DoesSaveFileExist(int slotIndex = -1)
    {
        return LoadTownData(slotIndex) != null;
    }

    public static void DeleteSlot(int slotIndex)
    {
        if(slotIndex < 0 || slotIndex > 2)
        {
            throw new Exception("WTF VAPSIE");
        }
        if(File.Exists(slots[slotIndex]))
        {
            File.Delete(slots[slotIndex]);
        }
        if (File.Exists(slotStatistics[slotIndex]))
        {
            File.Delete(slotStatistics[slotIndex]);
        }
    }

    public static void ClearGameData()
    {
        for(int i = 0; i < 3; i++)
        {
            if (File.Exists(slots[i]))
            {
                File.Delete(slots[i]);
            }
            if (File.Exists(slotStatistics[i]))
            {
                File.Delete(slotStatistics[i]);
            }
        }
        if (File.Exists(globalStatistics))
        {
            File.Delete(globalStatistics);
        }
        if (File.Exists(currentSlot))
        {
            File.Delete(currentSlot);
        }
    }
}

[Serializable]
public class CurrentSlot
{
    public int currentSlot;

    public CurrentSlot(int currentSlot)
    {
        this.currentSlot = currentSlot;
    }
}

using System.Collections.Generic;

[System.Serializable]
public class SaveDataWrapper
{
    public SaveDataWrapper(List<BuildingSaveData> buildings)
    {
        this.buildings = buildings;
    }
    public List<BuildingSaveData> buildings;
}

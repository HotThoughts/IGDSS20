using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    #region Enums
    public enum TileWeights {
        Water = 30,
        Sand = 2,
        Grass = 1,
        Forest = 2,
        Stone = 1,
        Mountain = 3
    }
    #endregion
    
    public void GeneratePathMap(Tile _t)
    {
        this.PotentialFields(_t);
    }

    private void PotentialFields(Tile _t)
    {
        
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public static class NavigationManager
{
    // Dictionary of path weights
    public static Dictionary<Tile.TileTypes, int> TileWeights = new Dictionary<Tile.TileTypes, int>
    {
        { Tile.TileTypes.Water,     30},
        { Tile.TileTypes.Sand,      2},
        { Tile.TileTypes.Grass,     1},
        { Tile.TileTypes.Forest,    2},
        { Tile.TileTypes.Stone,     1},
        { Tile.TileTypes.Mountain,  3}
    };
    // Generates path map for a placed buildung
    public static Dictionary<Tile, float> GeneratePathMap(Tile _t)
    {
        return ComputePotentialField(_t);
    }
    // Implements Potential Fields algorithm
    private static Dictionary<Tile, float> ComputePotentialField(Tile t)
    {
        // Using the idea of Breadth First Search
        Queue<Tile> computingQueue = new Queue<Tile>();
        Dictionary<Tile, float> map = new Dictionary<Tile, float> { { t, 0 } };
        // Enqueue all neighbours of tile
        t._neighbourTiles.ForEach(tile => computingQueue.Enqueue(tile));

        while (computingQueue.Count != 0)
        {
            Tile currentTile = computingQueue.Dequeue();
            float minPtal = Int32.MaxValue;
            currentTile._neighbourTiles.ForEach(tile => {
                // We first compute tiles, closest to current tile (BFS)
                if (!computingQueue.Contains(tile) && !map.ContainsKey(tile))
                {
                    computingQueue.Enqueue(tile);
                    return;
                }
                // Find Neighbour Tile with known and minimum potential
                if (map.ContainsKey(tile))
                {
                    minPtal = minPtal > map[tile] ? map[tile] : minPtal;
                    // If tile contains building, weight increases
                    if (tile._building != null)
                        minPtal += 1;
                }
            });
            map[currentTile] = minPtal + TileWeights[currentTile._type];
        }

        return map;
    }

}

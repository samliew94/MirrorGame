using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SpawnInteractablesStrategy
{
    private readonly Random random = RNG.GetInstance().Random();
    /// <summary>
    /// <para>Hazard can only spawn at tile index 3 and above.</para>
    /// <para>50-50 chance to spawn a hazard.</para>
    /// <para>Every hazard spawned guarantees that no other hazards are within it's 9 adjacent cells.</para>
    /// <para></para>
    /// </summary>
    public SpawnInteractablesStrategy(int width, int height, List<Tile> tiles)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x < 3)
                    continue;

                // 50% chance to spawn
                var chance = random.Next(0, 100);

                if (chance < 49) // don't spawn
                    continue;

                var tile = tiles.FirstOrDefault(t => t.x == x && t.y == y);

                var possibleInteractables = new TileCode[] { TileCode.HAZARD };
                var tileCode = possibleInteractables.OrderBy(x => RNG.GetInstance().Random().Next()).First();

                if (tileCode == TileCode.HAZARD)
                {
                    bool anyAdjacentTilesOccupied = tile.GetAdjacentTiles().Any(t =>
                                                         t.GetTileCode() == TileCode.CAMP ||
                                                         t.GetTileCode() == TileCode.HAZARD);

                    if (anyAdjacentTilesOccupied) // one or more of the tiles are occupied by another interactable
                        continue;
                }

                //tile.SetTileCode(tileCode);
            }
        }
    }
    
}
public class SpawnGoalStrategy
{
    private readonly Random random = RNG.GetInstance().Random();

    public SpawnGoalStrategy(int width, int height, List<Tile> tiles)
    {
        int goalY = random.Next(2, height - 2);
        int goalX = width - 1;
        int UB = goalY + 2;
        int LB = goalY - 2;
        for (int x = goalX - 2; x < goalX + 1; x++, UB--, LB++) // loop 3 times
        {
            tiles.Where(t => t.x == x && t.y <= UB && t.y >= LB).ToList().ForEach(t => t.SetTileCode(TileCode.WALKABLE));
            tiles.Where(t => t.x == x && t.y > UB && t.GetTileCode() == TileCode.WALKABLE).ToList().ForEach(t => t.SetTileCode(TileCode.BLOCKED));
            tiles.Where(t => t.x == x && t.y < LB && t.GetTileCode() == TileCode.WALKABLE).ToList().ForEach(t => t.SetTileCode(TileCode.BLOCKED));

            if (x == goalX)
                tiles.First(t => t.x == goalX && t.y == goalY).SetTileCode(TileCode.GOAL);
        }
    }
}

public class SpawnCampStrategy
{
    private readonly Random random = RNG.GetInstance().Random();

    public SpawnCampStrategy(int width, int height, List<Tile> tiles)
    {
        // assume width is 20
        var segment = (width - 5) / 2; // 2nd camp base at 14
        var campXs = new int[] { (segment * 1) + random.Next(-1, 2), (segment * 2) + random.Next(-1, 2) }; // 1st camp = 6/7/8 ; 2nd camp = 13/14/15

        for (int i = 0; i < campXs.Length; i++)
        {
            int campX = campXs[i];
            int campY = random.Next(0, height + 1);

            var tile = tiles.FirstOrDefault(t => t.x == campX && t.y == campY);

            if (tile == null)
                return;

            tile.SetTileCode(TileCode.CAMP);
        }
    }
}


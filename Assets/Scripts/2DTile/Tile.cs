public enum Sides
{
    None = -1,
    Top, //  0
    Left, // 1
    Right, // 2 
    Bottom, // 3
}

// 개별 타일 데이터를 담는 클래스
public class Tile
{
    public int id;
    public Tile[] adjacents = new Tile[4];
    public int autoTileId;
    public int fowTileId;

    public bool isVisited = false;

    // 이동 불가 가중치 (int.MaxValue = 이동 불가)
    public bool CanMove => Weight != int.MaxValue;



  // TileTypes. Grass(15)=1, Tree(16)=2, Hills(17)=4, Mountains(18)=MAX(통과 불가), Towns(19)=1, Castle(20)=1, Monster(21)=1.

    public static readonly int[] tableWeight =
    {
        //가중치가 int.MaxValue면 이동불가 처리
        int.MaxValue,
        1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
        2,4, int.MaxValue, 1,1,1,
    };

    // 타일의 종류에 따라 가중치 달라짐 (tableWeight에서 가져옴)
    public int Weight => tableWeight[autoTileId];

    public Tile previousTile = null;

    public void ClearPreviousTile()
    {
        previousTile = null;
    }

    public void UpdateAutoTileId()
    {
        autoTileId = 0;
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] != null && adjacents[i].autoTileId != (int)TileTypes.Empty)
            {
                autoTileId |= 1 << i;
            }
        }
    }

    public void UpdateFowTileId()
    {
        fowTileId = 0;
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] == null || !adjacents[i].isVisited)
            {
                fowTileId |= 1 << i;
            }
        }
    } 

    public void RemoveAdjacents(Tile tile)
    {
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] == null)
                continue;

            if (adjacents[i].id == tile.id)
            {
                adjacents[i] = null;
                UpdateAutoTileId();
                break;
            }
        }
    }

    public void ClearAdjacents()
    {
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] == null)
                continue;

            adjacents[i].RemoveAdjacents(this);
            adjacents[i] = null;
        }
        UpdateAutoTileId();
    }
}

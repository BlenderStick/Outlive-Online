using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    public Tilemap GridRender;
    public Tilemap GridReference;
    public TileBase ReferenceTileBlock;
    public TileBase ReferenceTileScenaBlock;
    
    public TileBase RenderTileVoid;
    public TileBase RenderTileOpen;
    public TileBase RenderTileBlock;
    public TileBase RenderTileScenaBlock;
    public BoundsInt Area;
    public List<GridBlock> Grids;
    private List<Vector3Int> gridVectors;

    // public System.Collections.SortedList<string, GameObject> d;

    #region Unity Methods

    private void Awake() 
    {
        gridVectors = new List<Vector3Int>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // GridBlock[] grids = GetComponents<GridBlock>();
        // Grids.AddRange(GetComponents<GridBlock>());
        foreach (GridBlock g in Grids)
        {
            CalculeGrid(g);
        }
        // Tilemap.RefreshTile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Manager Methods
        
        public Vector3Int[] CalculeGrid(GridBlock grid)
        {
            Vector3Int[] vects = new Vector3Int[grid.blocks.Length];
            int i = 0;
            foreach (Vector3Int v in grid.getBlocks())
            {
                Vector3Int newV = new Vector3Int(v.x, v.z, 1);
                GridReference.SetTile(newV, ReferenceTileBlock);
                vects[i] = v;
                i++;
            }
            return vects;
        }
        public IEnumerable<Vector2Int> Get2DMask()
        {
            return new Outlive.Collections.GridMask(Grids);
        }

        public void Calcule()
        {
            GridReference.ClearAllTiles();

            foreach (GridBlock g in Grids)
            {
                gridVectors.AddRange(CalculeGrid(g));
            }
        }

        public IReadOnlyCollection<Vector3Int> GetGridPoints()
        {
            return gridVectors.AsReadOnly();
        }

        public void PaintRenderGrid()
        {
            // TileReference.
            Vector3Int oldSize = Area.size;
            Vector3Int oldPos = Area.position;
            BoundsInt areaInt = new BoundsInt(new Vector3Int(oldPos.x, oldPos.z, z:1), new Vector3Int(x:oldSize.x, y:oldSize.z, z:1));
            // areaInt.yMax = 0;
            // areaInt.yMin = 0;
// int cont = 0;
            if(GridRender)
            {
                foreach (Vector3Int v in areaInt.allPositionsWithin)
                {
                    TileBase r = GridReference.GetTile(v);
                    // Debug.Log(v);
                    // cont++;

                    if (r == null)
                    {
                        GridRender.SetTile(v, RenderTileVoid);
                        continue;
                    }
                    if (r == ReferenceTileBlock)
                    {
                        GridRender.SetTile(v, RenderTileBlock);
                        continue;
                    }
                    if (r == ReferenceTileScenaBlock)
                    {
                        GridRender.SetTile(v, RenderTileScenaBlock);
                        continue;
                    }
                    // GetTilesBlock(area: Area, GridReference);
                    // GridRender.SetTile(v, RenderTileBlock);
                }
            }
        }

    #endregion

    #region Static Methods

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap){
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int count = 0;

            foreach (Vector3Int v in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(v.x, y:0, v.z);
                array[count] = tilemap.GetTile(pos);
                count ++;
            }

        return array;
    }



    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Outlive.Collections
{
    public class GridMask : IEnumerable<Vector2Int>
    {
        IEnumerable<GridBlock> grids;
        public GridMask(IEnumerable<GridBlock> grids)
        {
            this.grids = grids;
        }
        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return new Outlive.Collections.Enumerator.EnumeratorGridMask(grids.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Outlive.Collections.Enumerator.EnumeratorGridMask(grids.GetEnumerator());
        }
    }

    public class MixMask : IEnumerable<Vector2Int>
    {

        private IEnumerable<Vector2Int> mask1;
        private IEnumerable<Vector2Int> mask2;
        public MixMask(IEnumerable<Vector2Int> mask1, IEnumerable<Vector2Int> mask2)
        {
            this.mask1 = mask1;
            this.mask2 = mask2;
            if( mask1 == null || mask2 == null)
                throw new System.ArgumentNullException("As mascaras não podem ser nulas");
        }
        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return new Outlive.Collections.Enumerator.EnumeratorMixMask(mask1.GetEnumerator(), mask2.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Outlive.Collections.Enumerator.EnumeratorMixMask(mask1.GetEnumerator(), mask2.GetEnumerator());
        }
    }
}

namespace Outlive.Collections.Enumerator
{
    public class EnumeratorGridMask : IEnumerator<Vector2Int>
    {
        IEnumerator<GridBlock> grid;
        IEnumerator<Vector3Int> currentEnum;
        Vector2Int current;
        public EnumeratorGridMask(IEnumerator<GridBlock> grid)
        {
            this.grid = grid;
        }
        public Vector2Int Current 
        {
            get 
            {
                return current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return current;
            }
        }

        public void Dispose()
        {
            grid.Dispose();
            grid = null;
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            while (currentEnum == null || !currentEnum.MoveNext())
            {
                if (!grid.MoveNext())
                {
                    return false;
                }
                else
                {
                    if(currentEnum != null)
                        currentEnum.Dispose();

                    IEnumerable<Vector3Int> blocks = grid.Current.blocks;
                    currentEnum = blocks.GetEnumerator();
                }
            }

            Vector3Int v3 = currentEnum.Current;
            current = new Vector2Int(v3.x, v3.z);
            return true;
        }

        public void Reset()
        {
            grid.Reset();
        }
    }

    public class EnumeratorMixMask : IEnumerator<Vector2Int>
    {

        IEnumerator<Vector2Int> mask1;
        IEnumerator<Vector2Int> mask2;
        int count = 0;
        Vector2Int current;

        public EnumeratorMixMask(IEnumerator<Vector2Int> mask1, IEnumerator<Vector2Int> mask2)
        {
            this.mask1 = mask1;
            this.mask2 = mask2;
        }
        public Vector2Int Current {
            get
            {
                return current;
            }
        }

        object IEnumerator.Current {
            get
            {
                return current;
            }
        }

        public void Dispose()
        {
            mask1.Dispose();
            mask2.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            if(count == 0)
            {
                if(!mask1.MoveNext())
                    count = 1;
                else
                    current = mask1.Current;
            }
            if (count == 1)
            {
                if(!mask2.MoveNext())
                    return false;
                else
                    current = mask2.Current;
            }
            return true;
        }

        public void Reset()
        {
            mask1.Reset();
            mask2.Reset();
            count = 0;
        }
    }
}

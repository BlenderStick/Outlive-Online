using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Human;
using Outlive.Human.Generic;
using UnityEngine;

namespace Outlive.Human.Construcoes
{
    public class HumanConstructable : BasicConstructable
    {

        private GameObject gameObj;
        private Vector3 center;
        private GridManager grid;
        public HumanConstructable(GameObject gameObject) : this(gameObject, gameObject.transform.position)
        { }
        public HumanConstructable(GameObject gameObject, Vector3 constructionCenter): base(4)
        {
            gameObj = gameObject;
            center = constructionCenter;
        }

        public void VerifyConstructors()
        {
            lock (constructorsLock)
            {
                foreach (IConstructorHandler c in constructors)
                {
                Vector3 constructorTransform = c.transform.position;
                Vector2 constructorPosition = new Vector2(constructorTransform.x, constructorTransform.z);
                Vector2 constructionCenter = new Vector2(this.constructionCenter.x, this.constructionCenter.z);
                int index = 0;
                foreach (Vector2 v in positionsToConstruct)
                    {
                        if ((v + constructionCenter - constructorPosition).sqrMagnitude < 0.1f * 0.1f) //Fazemos 0.1² pois sqrMagnitude retorna a distância ao quadrado
                        {
                            Debug.Log("Está dentro");
                            if (constructorsBuilding[index] == null)
                                constructorsBuilding[index] = c;
                            break;
                        }
                        index++;
                    }
                }
                foreach (IConstructorHandler c in constructorsBuilding)
                {
                    if (c != null)
                    {
                        constructors.Remove(c);
                    }
                }
            }
        }



        public GridManager gridManager
        {
            set
            {
                grid = value;
            }
            get
            {
                return grid;
            }
        }
        protected override Vector3 constructionCenter
        {
            get
            {
                return center;
            }
        }

        protected override GameObject gameObject
        {
            get
            {
                return gameObj;
            }
        }

        protected override IEnumerable<Vector2Int> ObstaclePositionsMask()
        {
            return grid.Get2DMask();
        }

        protected override void OnNotExist()
        {
            Debug.Log("Está para ser destruido");
        }
    }
}
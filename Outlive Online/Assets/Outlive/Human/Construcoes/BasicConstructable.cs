using System.Xml.Linq;
using System.Reflection.Emit;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Human.Generic;
using UnityEngine;

namespace Outlive.Human
{
    public abstract class BasicConstructable : IConstructableHandler
    {

        private int futureConstructorsCount;
        public int maxConstructors;
        private bool done;

        public BasicConstructable(int numberOfConstructors)
        {
            constructors = new List<IConstructorHandler>();
            constructorsLock = new System.Object();
            constructorsBuilding = new IConstructorHandler[numberOfConstructors];
            positionsToConstruct = new Vector2[numberOfConstructors];
            maxConstructors = numberOfConstructors;
        }

        #region ConstructorContable
                
            public int futureConstructors {
                get {
                    return futureConstructorsCount;
                }
            }

            public void AddFutureConstructor()
            {
                futureConstructorsCount++;
            }

            public void SubtractFutureConstructor()
            {
                futureConstructorsCount--;
            }

            ///<summary>
            ///Verifica se a construção está em sua condição de existência, se a condição não for atendida, a construção se autodestruirá.
            ///</summary>
            private void VerifyExistsCondition()
            {
                if (futureConstructorsCount > 0 || constructors.Count > 0)
                    return;
                foreach (IConstructorHandler c in constructorsBuilding)
                {
                    if (c != null)
                        return;
                }
                OnNotExist();
            }

            protected abstract void OnNotExist();
        #endregion

        #region ConstructorSetup

            protected Vector2[] positionsToConstruct;///Posições que os construtores podem ocupar
            
            protected IConstructorHandler[] constructorsBuilding;///Construtores que estão construindo, o indice de cada construtor corresponde à sua posição em positionsToConstruct
            
            protected IList<IConstructorHandler> constructors;///Construtores que ainda não chegaram a uma posição para construir
            protected System.Object constructorsLock;
            public bool ConstructorTryToBuild(IConstructorHandler constructor)
            {
                if (done)
                    return false;

                if (constructorsBuilding.Contains(constructor))
                    return true;

                if (constructors.Contains(constructor))
                    return true;
                    
                bool havePosition = false;
                foreach (IConstructorHandler c in constructorsBuilding)
                {
                    if(c == null)
                    {
                        havePosition = true;
                        break;
                    }
                }
                if(havePosition)
                {
                    lock(constructorsLock)
                    {
                        constructors.Add(constructor);
                    }
                    CalculateConstructorsPath();
                }

                return havePosition;
            }

            public void ConstructorNotTryToBuild(IConstructorHandler constructor)
            {
                bool haveConstructorBuilding = false;
                bool haveConstructors = false;

                if(constructorsBuilding.Contains(constructor))
                {
                    haveConstructorBuilding = true;
                    
                    int index = 0;
                    foreach (IConstructorHandler c in constructorsBuilding)
                    {
                        if(c == constructor)
                        {
                            constructorsBuilding[index] = null;
                            return;
                        }
                        index++;
                    }
                }
                if(constructors.Contains(constructor))
                {
                    haveConstructors = true;
                    lock(constructorsLock)
                    {
                        constructors.Remove(constructor);
                    }
                }

                if (haveConstructorBuilding && haveConstructors)
                {
                    CalculateConstructorsPath();
                }
            }

            ///<summary>
            ///Este método deve ser chamado sempre após ConstructorTryToBuild ou ConstructorNotTryToBuild.
            ///<para>
            ///Calcula o caminho e as posições que os construtores deverão chegar para começar a construir.
            ///</para>
            ///</summary>
            public void CalculateConstructorsPath()
            {
                Vector3 centroDaConstrucao = constructionCenter;
                Vector3[] locaisDisponiveis = new Vector3[maxConstructors]; //locais não ocupados por um construtor
                int locaisDisponiveisCount = 0; //Numero de locais disponíveis, utilize este valor ao invés de locaisDisponiveis.Count

                int foreachIndex = 0;
                //Considerando que já há construtores em ação, calcula quais posições ainda estão disponíveis
                foreach (IConstructorHandler c in constructorsBuilding)
                {
                    if (c == null)
                    {
                        // locaisDisponiveis[locaisDisponiveisCount] = 
                        Vector3 v = new Vector3(
                            x: positionsToConstruct[foreachIndex].x + centroDaConstrucao.x, 
                            y: centroDaConstrucao.y + 3f, 
                            z: positionsToConstruct[foreachIndex].y + centroDaConstrucao.z);
                        Ray r = new Ray(v, direction: Vector3.down);
                        RaycastHit h;
                        if(Physics.Raycast(r, out h, Mathf.Infinity, 1 << LayerMask.GetMask("Background")))
                        {
                            locaisDisponiveis[locaisDisponiveisCount] = h.point;
                            Debug.Log(locaisDisponiveis[locaisDisponiveisCount]);
                        }
                        else
                        {
                            v.y = centroDaConstrucao.y;
                            locaisDisponiveis[locaisDisponiveisCount] = v;
                        }
                        locaisDisponiveisCount++;
                            
                    }
                    foreachIndex++;
                }
                lock(constructorsLock) //protegemos a lista constructors para evitar conflito
                {
                    //O programa calcula quais são os construtores mais próximos para tentar construir
                    //Assumimos aqui que a lista de construtores que pretende construir não contém nenhum construtor que já está construindo

                    IConstructorHandler[] construtoresPorDistancia = ConstrutoresMaisPerto(centroDaConstrucao, constructors, constructors.Count);
                    
                    if (constructors.Count <= locaisDisponiveisCount)//Essa estrutura executa se tiver lugares suficientes para os construtores que querem construir
                    {
                        //Posição central do objeto
                        int index = 0;
                        foreach (IConstructorHandler c in construtoresPorDistancia)
                        {
                            c.SetPositionToConstruct(locaisDisponiveis[index++], this);
                        }
                    }
                    else //Essa estrutura executa se não tiver lugares suficientes para todos os construtores
                    {
                        IEnumerable<Vector2Int> mask = ObstaclePositionsMask();
                        Vector3[] pointsAround = OutliveUtilites.From2DTo3DCoordinates(
                            OutliveUtilites.CalculatePointsAround(
                                center: new Vector2(centroDaConstrucao.x, centroDaConstrucao.z), 
                                pointRadius: 0.5f, 
                                numberOfPoints: constructors.Count - locaisDisponiveisCount, 
                                mask: mask, 
                                preferredAngle: 0f));
                        int iLocaisDisponiveis = 0;
                        int iPointsAround = 0;
                        foreach (IConstructorHandler c in construtoresPorDistancia)
                        {
                            if (iLocaisDisponiveis < locaisDisponiveisCount)
                                c.SetPositionToConstruct(locaisDisponiveis[iLocaisDisponiveis++], this);
                            else
                                c.SetPositionToConstruct(pointsAround[iPointsAround++], this);
                        }
                    }
                }
                
                

            }
            
            public bool VerifyConstructor(IConstructorHandler constructor)
            {
                Vector3 constructorTransform = constructor.transform.position;
                Vector2 constructorPosition = new Vector2(constructorTransform.x, constructorTransform.z);
                Vector2 constructionCenter = new Vector2(this.constructionCenter.x, this.constructionCenter.z);
                foreach (Vector2 v in positionsToConstruct)
                {
                    if ((v + constructionCenter - constructorPosition).sqrMagnitude < 0.1f * 0.1f) //Fazemos 0.1² pois sqrMagnitude retorna a distância ao quadrado
                    {
                        return true;
                    }
                }
                return false;
            }

        #endregion

        #region Necessary
            protected abstract GameObject gameObject { 
                get;
            }
            protected abstract Vector3 constructionCenter { get; }
            protected abstract IEnumerable<Vector2Int> ObstaclePositionsMask();
            public Vector2[] locaisParaConstruir
            {
                get
                {
                    return positionsToConstruct;
                }
                set
                {
                    positionsToConstruct = value;
                }
            }

        #endregion
        
        #region Properties
            public int countConstructorsBuilding
            {
                get
                {
                    return constructorsBuilding.Count(OutliveUtilites.CheckIsNotNull);
                }
            }

            public bool isDone
            {
                set
                {
                    done = value;
                }
                get
                {
                    return done;
                }
            }
            
        #endregion

        private static IConstructorHandler[] ConstrutoresMaisPerto(Vector3 target, IEnumerable<IConstructorHandler> constructors, int ConstructorsCount)
        {
            IConstructorHandler[] construtoresPorDistancia = new IConstructorHandler[ConstructorsCount];
            float[] distancias = new float[ConstructorsCount]; //Armazena as distancias entre cada 
            int distanceIndex = 0;
            foreach (IConstructorHandler c in constructors)
            {
                distancias[distanceIndex++] = c.SqrDistancePathTo(target);
            }
            int i = 0;
            foreach (IConstructorHandler current in constructors)
            {
                IConstructorHandler menosDistante = current;
                float distanciaMenosDistante = distancias[i];

                int testCount = 0;
                foreach (IConstructorHandler c in constructors)
                {
                    if (!construtoresPorDistancia.Contains(c) && !construtoresPorDistancia.Contains(menosDistante) && c != menosDistante)
                    {
                        float distanciaCurrent = distancias[testCount];
                        if(distanciaCurrent < distanciaMenosDistante)
                        {
                            menosDistante = c;
                            distanciaMenosDistante = distanciaCurrent;
                        }
                    }
                    testCount++;
                }
                construtoresPorDistancia[i] = menosDistante;
                i++;
            }

            return construtoresPorDistancia;
        }
    }
}
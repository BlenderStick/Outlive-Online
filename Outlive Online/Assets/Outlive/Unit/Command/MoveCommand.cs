using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Unit.Command
{
    public class MoveCommand : ICommand
    {

        Vector3 coordinates;


        public MoveCommand(float x, float y, float z) : this(new Vector3(x, y, z))
        {

        }

        public MoveCommand(Vector3 coordinates)
        {
            this.coordinates = coordinates;
        }

        public Vector3 getCoordinates()
        {
            return coordinates;
        }

        public object alvo
        {
            get
            {
                return coordinates;
            }
        }

        ///<summary>
        ///Calcula uma série de pontos em volta de <paramref name="target"/> até um máximo de <paramref name="numberOfPoints"/>
        ///</summary>
        ///<param name="target"> Ponto inicial para calcular o círculo </param>
        ///<param name="mask"> Pontos que não podem ser preenchidos</param>
        ///<param name="lenghtMask"> Comprimento da matriz <paramref name="mask"/></param>
        public static Vector3Int[] GenerateCircle(Vector3 target, Vector3Int[] mask, int lenghtMask, int numberOfPoints)
        {

            if (numberOfPoints == 0 || numberOfPoints == 1)
                return new Vector3Int[] { Vector3Int.RoundToInt(target) };
            int maskLength = Mathf.Min(mask.Length, lenghtMask);

            Vector3Int lastVisinho = Vector3Int.RoundToInt(target);

            Vector3Int[] finalPosition = new Vector3Int[numberOfPoints];
            Vector3Int[] pointsAndMask = new Vector3Int[numberOfPoints + maskLength];
            System.Array.Copy(mask, pointsAndMask, maskLength);

            finalPosition[0] = lastVisinho;

            //Calcula as posições em círculo
            for (int i = 0; i < numberOfPoints; i++)
            {

                Vector3Int[] visinhos = getVisinhos(lastVisinho);


                Vector3Int visinhoMaisProximo = Vector3Int.RoundToInt(minimumDistance(visinhos, pointsAndMask, i + maskLength, target));

                finalPosition[i] = visinhoMaisProximo;
                pointsAndMask[i + maskLength] = visinhoMaisProximo;


                lastVisinho = visinhoMaisProximo;


            }

            return finalPosition;
        }
        public static Vector3Int[] points(Vector3[] startPoints, Vector3Int[] mask, int maskLenght, Vector3 target)
        {
            int length = startPoints.Length;
            Vector3 midPoint = getMidPoint(startPoints);

            Vector3Int[] finalPosition;

            //Calcula as posições em círculo
            if (mask != null)
            {
                finalPosition = GenerateCircle(target, mask, maskLenght, startPoints.Length);
            }
            else
            {
                finalPosition = GenerateCircle(target, new Vector3Int[0], 0, startPoints.Length);
            }


            // Vector3 offset = target - midPoint;
            Vector3Int[] reorganizedVectors = new Vector3Int[length];

            Vector3Int[] priorityIndex = new Vector3Int[length];
            //Reorganiza as posições de destino

            for (int i = 0; i < length; i++)
            {
                priorityIndex[i] = Vector3Int.RoundToInt(minimumDistance(finalPosition, priorityIndex, i, target));
                // reorganizedVectors[i] = minimumDistance(finalPosition, reorganizedVectors, i, )
                // Vector3 vec = finalPosition[i] + 
            }

            for (int i = 0; i < length; i++)
            {
                reorganizedVectors[i] = Vector3Int.RoundToInt(minimumDistance(finalPosition, reorganizedVectors, i, priorityIndex[length - 1 - i]));
                // targetsOccuped
            }

            return reorganizedVectors;
        }

        private static Vector3 getMidPoint(Vector3[] vectors)
        {
            float x = 0f, y = 0f;

            foreach (Vector3 v in vectors)
            {
                x += v.x;
                y += v.y;
            }
            x = x / vectors.Length;
            y = y / vectors.Length;
            return new Vector3(x, y);
        }

        private static bool containVector(IList<Vector3Int> vectors, int vectorsCount, Vector3 compare)
        {
            for (int i = 0; i < vectorsCount; i++)
            {
                if (Mathf.Abs(vectors[i].x - compare.x) == 0 &&
                Mathf.Abs(vectors[i].y - compare.y) == 0 &&
                Mathf.Abs(vectors[i].z - compare.z) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        // private static Vector3 minimumDistance(Vector3[] vectors, Vector3 compare, Vector3 offset)
        // {
        //     Vector3 minimum = vectors[0];
        //     float distance = 0;
        //     bool isFirstLoop = true;

        //     foreach (Vector3 v in vectors)
        //     {
        //         if(isFirstLoop)
        //         {
        //             minimum = v;
        //             distance = (compare)
        //         }
        //     }


        //     return minimum;
        // }

        ///<summary>
        ///Calcula a distância entre um Vector3 e uma lista de Vector3 para determinar qual é o ponto mais próximo
        ///<para>
        ///Esse método considera uma lista de Vector3 que devem ser desconsiderados da lista principal, se um dos pontos da 
        ///lista principal estiver em <paramref name="maskVectors"/>, esse ponto não será calculado
        ///</para>
        ///</summary>
        ///<param name="vectors"> Vetores para calcular a distância com o target</param>
        ///<param name="maskVectors"> Vetores que não serão considerados na lista <paramref name="vectors"/></param>
        ///<param name="sizeOfExcludeVectors"> Indica qual o indice máximo da lista de <paramref name="maskVectors"/> que será analisado</param>
        ///<param name="target"> Vector3 alvo da verificação da distância dos <paramref name="vectors"/></param>
        ///<returns>
        ///Vector3 mais próximo de <paramref name="target"/> que não está contido em <paramref name="maskVectors"/>
        ///</returns>
        private static Vector3 minimumDistance(Vector3Int[] vectors, Vector3Int[] maskVectors, int sizeOfExcludeVectors, Vector3 target)
        {
            if (vectors.Length == 0)
                return target;

            bool isFirstLoop = true;
            Vector3 min = Vector3.zero;
            float distance = 0f;
            foreach (Vector3 v in vectors)
            {
                if (maskVectors != null && !containVector(maskVectors, sizeOfExcludeVectors, v))
                {
                    float newDistance = (v - target).sqrMagnitude;
                    if (isFirstLoop)
                    {
                        min = v;
                        distance = newDistance;
                        isFirstLoop = false;
                    }
                    else
                        if (newDistance < distance)
                    {
                        min = v;
                        distance = newDistance;
                    }

                }

            }
            return min;
        }

        private static Vector3Int[] getVisinhos(Vector3 target)
        {
            Vector3Int[] visinhos = new Vector3Int[8];
            int count = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (target.x != x && target.y != z)
                    {
                        visinhos[count] = Vector3Int.RoundToInt(new Vector3(target.x + x, target.y, target.z + z));
                        count++;
                    }

                }
            }
            return visinhos;
        }

        public static Vector3Int[] points(IList<UnitBehaviour> units, Vector3Int[] mask, int maskLenght, Vector3 target)
        {
            Vector3[] vectors = new Vector3[units.Count];
            for (int i = 0; i < units.Count; i++)
            {
                vectors[i] = units[i].transform.position;
            }
            return points(vectors, mask, maskLenght, target);
        }


    }
}
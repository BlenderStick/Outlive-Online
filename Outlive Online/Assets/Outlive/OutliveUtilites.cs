using System.Timers;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Outlive
{
    public static class OutliveUtilites
    {

        public static HashSet<Vector2Int> CalculePointsAroundGrid(Vector2Int offset, int radius)
        {
            HashSet<Vector2Int> hash = new HashSet<Vector2Int>();
            hash.Add(offset);
            for (int i = 1; i < radius; i++)
            {
                CalculeDiagonal(ref hash, offset, i);
            }
            return hash;
        }

        public static HashSet<Vector2Int> CalculePointsAroundGrid(Vector2Int offset, int numberOfPoints, Func<Vector2Int, bool> containInMask)
        {
            HashSet<Vector2Int> points = new HashSet<Vector2Int>();

            if (numberOfPoints >= 1 && (containInMask == null || !containInMask.Invoke(offset)))
                points.Add(offset);


            int radius = 1;
            while (points.Count < numberOfPoints)
            {
                HashSet<Vector2Int> set = CalculeDiagonal(offset, radius++, numberOfPoints - points.Count, containInMask);
                points.UnionWith(set);
            }

            return points;
        }

        public static void CalculeDiagonal(ref HashSet<Vector2Int> hash, Vector2Int offset, int radius)
        {
            Vector2Int retoDirection = new Vector2Int(-1, 1);
            Vector2Int retoStart = new Vector2Int(1, 0);

            Vector2Int rasoStart = new Vector2Int(0, 1);
            Vector2Int rasoDirection = Vector2Int.one;

            PutDiagonal(hash, retoStart * radius + offset, retoDirection * -1, 0, radius);
            PutDiagonal(hash, (retoStart * radius * -1) + offset, retoDirection, 0, radius);
            PutDiagonal(hash, rasoStart * radius + offset, rasoDirection, 0, radius);
            PutDiagonal(hash, (rasoStart * radius * -1) + offset, rasoDirection * -1, 0, radius);
        }

        public static HashSet<Vector2Int> CalculeDiagonal(Vector2Int offset, int radius, int maxPoints, Func<Vector2Int, bool> containInMask)
        {
            HashSet<Vector2Int> set = new HashSet<Vector2Int>();
            if (maxPoints <= 0)
                return set;
            

            Vector2Int retoDirection = new Vector2Int(-1, 1);
            Vector2Int retoStart = new Vector2Int(1, 0);

            Vector2Int rasoStart = new Vector2Int(0, 1);
            Vector2Int rasoDirection = Vector2Int.one;

            if (!PutDiagonal(set, retoStart * radius + offset, retoDirection * -1, 1, radius, maxPoints, containInMask))
            {
                if (!PutDiagonal(set, (retoStart * radius * -1) + offset, retoDirection, 1, radius, maxPoints, containInMask))
                {
                    if (!PutDiagonal(set, rasoStart * radius + offset, rasoDirection, 1, radius, maxPoints, containInMask))
                    {
                        if (!PutDiagonal(set, (rasoStart * radius * -1) + offset, rasoDirection * -1, 1, radius, maxPoints, containInMask))
                        {
                            if (!PutDiagonal(set, retoStart * radius + offset, retoDirection * -1, 0, 1, maxPoints, containInMask))
                            {
                                if (!PutDiagonal(set, (retoStart * radius * -1) + offset, retoDirection, 0, 1, maxPoints, containInMask))
                                {
                                    if (!PutDiagonal(set, rasoStart * radius + offset, rasoDirection, 0, 1, maxPoints, containInMask))
                                    {
                                        PutDiagonal(set, (rasoStart * radius * -1) + offset, rasoDirection * -1, 0, 1, maxPoints, containInMask);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return set;
        }

        public static Vector3 Project(float high, LayerMask mask, Vector2Int v)
        {
            Vector3 vect = new Vector3(v.x, high, v.y);
            RaycastHit hit;
            if (Physics.Raycast(vect, Vector3.down, out hit, Mathf.Infinity, mask))
            {
                return hit.point;
            }
            return new Vector3(v.x, 0, v.y);
        }

        ///<summary>Adiciona uma série de pontos a um hashset a partir da posição start, na direção e comprimento definidos<para>
        ///Se a contagem de itens do hashset chegar a maxPoints, a contagem para e retorna true</para>
        ///Os pontos não podem intersectar a função containInMask</summary>
        public static bool PutDiagonal(HashSet<Vector2Int> set, Vector2Int offset, Vector2Int normalizedDirection, int start, int lenght, int maxPoints, Func<Vector2Int, bool> containInMask)
        {
            if (set.Count >= maxPoints)
                return true;

            for (int i = start; i < lenght; i++)
            {
                Vector2Int vect = offset - normalizedDirection * i;
                if (containInMask == null || !containInMask.Invoke(vect))
                {
                    set.Add(vect);
                    if (set.Count >= maxPoints)
                        return true;
                }
            }
            return false;
        }
        public static void PutDiagonal(HashSet<Vector2Int> set, Vector2Int offset, Vector2Int normalizedDirection, int start, int lenght)
        {
            for (int i = start; i < lenght; i++)
            {
                Vector2Int vect = offset - normalizedDirection * i;
                    set.Add(vect);
            }
        }


        ///<summary>
        ///Calcula um conjunto de pontos ao redor de center, esses pontos não respeitam a grade, por isso tendem a formar um circulo próximo ao perfeito.
        ///</summary>
        ///<param name="pointRadius"> angulo em radiano </param>
        public static Vector2[] CalculatePointsAround(Vector2 center, float pointRadius, int numberOfPoints, Func<Vector2, bool> ContainInMask, float preferredAngle)
        {
            if (pointRadius <= 0)
                throw new ArgumentOutOfRangeException("pointRadius não pode ser 0 ou negativo");

            if (numberOfPoints == 0)
                return new Vector2[0];

            Vector2[] vects = new Vector2[numberOfPoints];

            int camada = 0;
            int index = 0;
            
            //Verifica se o centro está insidindo em algum ponto da mascara
            if(!ContainInMask(center))
            {
                vects[index] = center;
                camada++;
                index++;
            }

            float camadaRaio = camada * 2;

            while (index < numberOfPoints)
            {
                float meiaCircunferencia = camadaRaio * Mathf.PI;

                int maxPoints = (int) (meiaCircunferencia);
                if(maxPoints > numberOfPoints - index)
                    maxPoints = numberOfPoints - index;

                float angleStep = Mathf.PI * 2 / (float) maxPoints;

                float standAngle = preferredAngle;
                for (int i = 0; i < maxPoints; i++)
                {
                    float x = Mathf.Cos(standAngle) * camadaRaio + center.x;
                    float y = Mathf.Sin(standAngle) * camadaRaio + center.y;
                    Vector2 v = new Vector2(x, y);
                    standAngle += angleStep;

                    if(index < numberOfPoints)
                    {
                        if(!ContainInMask(v))
                        {
                            vects[index] = v;
                            index++;
                        }
                    }
                    else
                        break;
                }
                camada++;
                camadaRaio = camada * 2 * pointRadius;
            }


            return vects;
        }

        ///<summary>
        ///Calcula um conjunto de pontos ao redor de center, esses pontos não respeitam a grade, por isso tendem a formar um circulo próximo ao perfeito.
        ///</summary>
        ///<param name="pointRadius"> angulo em radiano </param>
        public static Vector2[] CalculatePointsAround(Vector2 center, float pointRadius, int numberOfPoints, float preferredAngle)
        {
            return CalculatePointsAround(center, pointRadius, numberOfPoints, (v) => false, preferredAngle);
        }
        public static Vector2[] CalculatePointsAround(Vector2 center, float pointRadius, int numberOfPoints)
        {
            return CalculatePointsAround(center, pointRadius, numberOfPoints, (v) => false, 0f);
        }

        public static bool IntersectaAMascara(Vector2 vect, float radius, IEnumerable<Vector2Int> mask, int maskLenght)
        {
            if (mask == null)
                return false;

            int count = 0;

            foreach (Vector2Int v in mask)
            {
                if(count >= maskLenght)
                    return false;

                Vector2 maskV = new Vector2((float) v.x, (float) v.y);
                Vector2 magnitude = vect - maskV;
                magnitude = new Vector2(Mathf.Abs(magnitude.x), Mathf.Abs(magnitude.y));
                float angle = Mathf.Atan2(magnitude.y, magnitude.x);
                if(angle < 0)
                    angle = Mathf.PI + angle;

                if (magnitude.x < 0.5f)
                {
                    if (magnitude.y < 0.5f + radius)
                        return true;
                }
                else if (magnitude.y < 0.5f)
                {
                    if (magnitude.x < 0.5f + radius)
                        return true;
                }
                else
                {
                    if (((new Vector2(0.5f, 0.5f)) - magnitude).sqrMagnitude < radius * radius)
                    {
                        return true;
                    }
                }
                count++;
            }
            return false;
        }
        public static bool IntersectaAMascara(Vector2 vect, float radius, IEnumerable<Vector2Int> mask)
        {
            if (mask == null)
                return false;

            foreach (Vector2Int v in mask)
            {

                Vector2 maskV = new Vector2((float) v.x, (float) v.y);
                Vector2 magnitude = vect - maskV;
                magnitude = new Vector2(Mathf.Abs(magnitude.x), Mathf.Abs(magnitude.y));
                float angle = Mathf.Atan2(magnitude.y, magnitude.x);
                if(angle < 0)
                    angle = Mathf.PI + angle;

                if (magnitude.x < 0.5f)
                {
                    if (magnitude.y < 0.5f + radius)
                        return true;
                }
                else if (magnitude.y < 0.5f)
                {
                    if (magnitude.x < 0.5f + radius)
                        return true;
                }
                else
                {
                    if (((new Vector2(0.5f, 0.5f)) - magnitude).sqrMagnitude < radius * radius)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Vector3[] From2DTo3DCoordinates(ICollection<Vector2> vects)
        {
            Vector3[] newVects = new Vector3[vects.Count];
            int index = 0;
            foreach (Vector2 v in vects)
            {
                newVects[index] = new Vector3(v.x, 0, v.y);
                index++;
            }
            return newVects;
        }
        public static Vector3[] From2DTo3DCoordinates(ICollection<Vector2Int> vects)
        {
            Vector3[] newVects = new Vector3[vects.Count];
            int index = 0;
            foreach (Vector2Int v in vects)
            {
                newVects[index] = new Vector3(v.x, 0, v.y);
                index++;
            }
            return newVects;
        }
        public static Vector3 From2DTo3DCoordinates(Vector2 vect) => new Vector3(vect.x, 0, vect.y);
        public static Vector2 From3DTo2DCoordinates(Vector3 vect) => new Vector2(vect.x, vect.z);

        /// <summary>FIX-ME</summary>
        public static T[] SortByFurthest<T>(Vector2 target, IEnumerable<T> itensToOrganize, Func<Vector2, T, float> sqrtMagnitude)
        {
            
            if (itensToOrganize == null)
                return new T[0];

            SortedList<float, HashSet<T>> sorted = new SortedList<float, HashSet<T>>();

            int lenght = 0;

            foreach (var item in itensToOrganize)
            {
                float magnitude = -sqrtMagnitude(target, item);
                if (!sorted.ContainsKey(magnitude))
                    sorted.Add(magnitude, new HashSet<T>());
                sorted[magnitude].Add(item);
                lenght++;
            }
            
            int index = 0;
            T[] result = new T[lenght];
            foreach (var item in sorted)
                foreach (var i2 in item.Value)
                result[index++] = i2;
                
                    

            return result;
        }

        public static Vector2Int PointToGrid(Vector2 point)
        {
            new HashSet<Vector2Int>().ToArray();
            return new Vector2Int(Decimal.ToInt32(Decimal.Round(new Decimal(point.x))), Decimal.ToInt32(Decimal.Round(new Decimal(point.y))));
        }

        public static HashSet<T> Difference<T> (IEnumerable<T> arg1, IEnumerable<T> arg2)
        {
            HashSet<T> result = new HashSet<T>();
            if (arg1 == arg2)
                return result;
                
            if (arg1 == null || arg2 == null)
            {
                if (arg1 != null || arg2 != null)
                    result.UnionWith(arg1 != null? arg1 : arg2);
                return result;
            }

            foreach (var item in arg1)
                if (!arg2.Contains(item))
                    result.Add(item);
            foreach (var item in arg2)
                if (!arg1.Contains(item))
                    result.Add(item);
            
            return result;
        }

        public static Vector2 To2D(this Vector3 vector) => new Vector2(vector.x, vector.z);

        public enum Conversion
        {
            XZtoXY,
            
        }

    }
}


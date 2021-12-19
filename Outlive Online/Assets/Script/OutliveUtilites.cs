using System.Collections.Generic;
using UnityEngine;

namespace Outlive
{
    public static class OutliveUtilites
    {
        ///<summary>
        ///Calcula quais são as posições possíveis ao redor de <paramref name="target"/> evitando as posições contidas em <paramref name="mask"/>
        ///<para>Se mask for nulo, calcula todas as posições ao redor de <paramref name="target"/></para>
        ///</summary>
        ///<param name="target"> Posição centro do circulo </param>
        ///<param name="numberOfPoints"> Numero de pontos a ser calculado </param>
        ///<param name="mask"> Coleção de pontos a serem evitados pelo algoritmo </param>
        ///<param name="lenghtMask"> Comprimento da máscara, caso não queira incluir todos os pontos </param>
        public static Vector2Int[] CalculatePointsAroundInGrid(Vector2Int target, int numberOfPoints, ICollection<Vector2Int> mask, int lenghtMask)
        {
            Vector2Int[] vects = new Vector2Int[numberOfPoints];

            int totalPontos = 0;

            ///O algoritmo se basea na verificação por camada em torno de target
            int camada = 1;

            ///Verifica se a posição target não está na mascara
            if (!ContemNaMascara(target, mask, lenghtMask))
                vects[totalPontos++] = target;

            ///Executa a busca pelos pontos em cada camada do círculo
            while (totalPontos < numberOfPoints)
            {
                ///Verifica o topo, base, direita e esquerda
                Vector2Int top = new Vector2Int(target.x, target.y + camada);
                Vector2Int bottom = new Vector2Int(target.x, target.y - camada);
                Vector2Int right = new Vector2Int(target.x + camada, target.y);
                Vector2Int left = new Vector2Int(target.x - camada, target.y);

                ///Verifica se estão na máscara
                if (!ContemNaMascara(top, mask, lenghtMask))
                    vects[totalPontos++] = top;
                if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido

                if (!ContemNaMascara(bottom, mask, lenghtMask))
                    vects[totalPontos++] = bottom;
                if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido

                if (!ContemNaMascara(right, mask, lenghtMask))
                    vects[totalPontos++] = right;
                if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido

                if (!ContemNaMascara(left, mask, lenghtMask))
                    vects[totalPontos++] = left;
                if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido


                ///Calcula a posição de cada vizinho
                for (int i = 1; i < camada; i++)
                {
                    //Calcula a posição para cada quadrante
                    Vector2Int topRight = new Vector2Int(target.x + i, target.y + camada - i);
                    Vector2Int bottomRight = new Vector2Int(target.x + i, target.y - camada + i);
                    Vector2Int topLeft = new Vector2Int(target.x - i, target.y + camada - i);
                    Vector2Int bottomLeft = new Vector2Int(target.x - i, target.y - camada + i);


                    ///Verifica se estão na máscara
                    if (!ContemNaMascara(topRight, mask, lenghtMask))
                        vects[totalPontos++] = topRight;
                    if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido

                    if (!ContemNaMascara(bottomRight, mask, lenghtMask))
                        vects[totalPontos++] = bottomRight;
                    if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido

                    if (!ContemNaMascara(topLeft, mask, lenghtMask))
                        vects[totalPontos++] = topLeft;
                    if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido

                    if (!ContemNaMascara(bottomLeft, mask, lenghtMask))
                        vects[totalPontos++] = bottomLeft;
                    if(totalPontos == numberOfPoints) break; //Verifica se o limite de pontos não foi atingido
                }
                
                camada++;
            }

            return vects;
        }
        ///<summary>
        ///Calcula quais são as posições possíveis ao redor de <paramref name="target"/> evitando as posições contidas em <paramref name="mask"/>
        ///<para>Se mask for nulo, calcula todas as posições ao redor de <paramref name="target"/></para>
        ///</summary>
        ///<param name="target"> Posição centro do circulo </param>
        ///<param name="numberOfPoints"> Numero de pontos a ser calculado </param>
        ///<param name="mask"> Coleção de pontos a serem evitados pelo algoritmo </param>
        public static Vector2Int[] CalculatePointsAroundInGrid(Vector2Int target, int numberOfPoints, ICollection<Vector2Int> mask)
        {
            return CalculatePointsAroundInGrid(target, numberOfPoints, mask, mask.Count);
        }
        ///<summary>
        ///Calcula quais são as posições possíveis ao redor de <paramref name="target"/>
        ///</summary>
        ///<param name="target"> Posição centro do circulo </param>
        ///<param name="numberOfPoints"> Numero de pontos a ser calculado </param>
        public static Vector2Int[] CalculatePointsAroundInGrid(Vector2Int target, int numberOfPoints)
        {
            return CalculatePointsAroundInGrid(target, numberOfPoints, new Vector2Int[0], 0);
        }

        ///<summary>
        ///Calcula um conjunto de pontos ao redor de center, esses pontos não respeitam a grade, por isso tendem a formar um circulo próximo ao perfeito.
        ///</summary>
        ///<param name="pointRadius"> angulo em radiano </param>
        public static Vector2[] CalculatePointsAround(Vector2 center, float pointRadius, int numberOfPoints, IEnumerable<Vector2Int> mask, int lenghtMask, float preferredAngle)
        {

            Vector2[] vects = new Vector2[numberOfPoints];

            int camada = 0;
            int index = 0;
            
            //Verifica se o centro está insidindo em algum ponto da mascara
            if(!IntersectaAMascara(center, pointRadius, mask, lenghtMask))
            {
                vects[index] = center;
                camada++;
                index++;
            }

            float camadaRaio = camada * 2 * pointRadius;

            while (index < numberOfPoints)
            {
                float meiaCircunferencia = camadaRaio * Mathf.PI;

                int maxPoints = (int) (meiaCircunferencia / pointRadius);
                if(maxPoints > numberOfPoints - index)
                    maxPoints = numberOfPoints - index;

                float angleStep = Mathf.PI * 2 / (float) maxPoints;

                float standAngle = preferredAngle;
                Debug.Log(camada);
                for (int i = 0; i < maxPoints; i++)
                {
                    float x = Mathf.Cos(standAngle) * camadaRaio + center.x;
                    float y = Mathf.Sin(standAngle) * camadaRaio + center.y;
                    Vector2 v = new Vector2(x, y);
                    standAngle += angleStep;

                    if(index < numberOfPoints)
                    {
                        if(!IntersectaAMascara(v, pointRadius, mask, lenghtMask))
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
        public static Vector2[] CalculatePointsAround(Vector2 center, float pointRadius, int numberOfPoints, IEnumerable<Vector2Int> mask, float preferredAngle)
        {
            

            Vector2[] vects = new Vector2[numberOfPoints];

            int camada = 0;
            int index = 0;
            
            //Verifica se o centro está insidindo em algum ponto da mascara
            if(!IntersectaAMascara(center, pointRadius, mask))
            {
                vects[index] = center;
                camada++;
                index++;
            }

            float camadaRaio = camada * 2 * pointRadius;

            while (index < numberOfPoints)
            {
                float meiaCircunferencia = camadaRaio * Mathf.PI;

                int maxPoints = (int) (meiaCircunferencia / pointRadius);
                if(maxPoints > numberOfPoints - index)
                    maxPoints = numberOfPoints - index;

                float angleStep = Mathf.PI * 2 / (float) maxPoints;

                float standAngle = preferredAngle;
                // Debug.Log(camada);
                for (int i = 0; i < maxPoints; i++)
                {
                    float x = Mathf.Cos(standAngle) * camadaRaio + center.x;
                    float y = Mathf.Sin(standAngle) * camadaRaio + center.y;
                    Vector2 v = new Vector2(x, y);
                    standAngle += angleStep;

                    if(index < numberOfPoints)
                    {
                        if(!IntersectaAMascara(v, pointRadius, mask))
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
            return CalculatePointsAround(center, pointRadius, numberOfPoints, null, 0, preferredAngle);
        }
        public static Vector2[] CalculatePointsAround(Vector2 center, float pointRadius, int numberOfPoints)
        {
            return CalculatePointsAround(center, pointRadius, numberOfPoints, null, 0, 0f);
        }
        public static bool ContemNaMascara(Vector2Int vect, IEnumerable<Vector2Int> mask, int maskLenght)
        {
            if (mask != null && maskLenght > 0)
            {
                int i = 0;
                foreach (Vector2Int v in mask)
                {
                    if (vect.Equals(v))
                        return true;

                    if(i >= maskLenght)
                        break;

                    i++;
                }
            }
            return false;
        }

        public static bool ContemNaMascara<T> (T obj, IEnumerable<T> mask, int maskLenght)
        {
            if (mask != null && maskLenght > 0)
            {
                int i = 0;
                foreach (T v in mask)
                {
                    if (obj.Equals(v))
                        return true;

                    if(i >= maskLenght)
                        break;

                    i++;
                }
            }
            return false;
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

        ///<summary>
        ///Reorganiza <paramref name="toReorde"/> do ponto mais distante para o menos distante de <paramref name="reference"/>
        ///</summary>
        ///<param name="reference"> Posições de referência para o calculo</param>
        ///<param name="toReorde"> Posições que serão reordenadas na lista</param>
        public static ICollection<Vector2Int> ReorganizarPeloMaisDistante(ICollection<Vector2Int> reference, IList<Vector2Int> toReorde)
        {
            if(reference == null || toReorde == null)
                throw new System.ArgumentNullException();
            if(reference.Count > 0 && toReorde.Count >= reference.Count)
            {
                Vector2Int[] final = new Vector2Int[reference.Count];

                int count = 0;
                Vector2Int current = toReorde[0];
                // alvoEnum.

                foreach (Vector2Int v in reference)
                {
                    Vector2Int minVect = toReorde[count];
                    int minDist = (v - minVect).sqrMagnitude;

                    for (int i = count; i < toReorde.Count; i++)
                    {
                        Vector2Int cVect = toReorde[i];
                        int cDist = (cVect - v).sqrMagnitude;
                        if (cDist < minDist)
                        {
                            minVect = cVect;
                            minDist = cDist;
                        }
                    }

                    count ++;
                }

                return final;
            }
            return reference;
            
        }

        public static bool CheckIsNull(object obj)
        {
            return obj == null;
        }
        public static bool CheckIsNotNull(object obj)
        {
            return obj != null;
        }
    }
}


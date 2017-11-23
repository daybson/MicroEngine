using MicroEngine.src.Collision.Colliders;
using MicroEngine.src.Collision.Helper;
using MicroEngine.src.Math2D;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroEngine.src.Collision
{
    public static class CollisionDetector
    {
        #region =========================== A U X I L I A R  =========================== 

        private static float _diagonalScreen = 1132;


        public static void SetResolution(Vector2u size)
        {
            _diagonalScreen = (float)Math.Sqrt(size.X * size.X + size.Y * size.Y);
        }


        private static CollisionInfo GetCollisionInfoFrom(SegmentCollider a, SegmentCollider b)
        {
            Vector2f candidate1 = new Vector2f();
            Vector2f candidate2 = new Vector2f();
            CollisionInfo info;

            if ((a.Point2 - a.Point1).Magnitude() != (b.Point2 - b.Point1).Magnitude())
            {
                if (CollidesWith(a.Point1, b, out info))
                    candidate1 = a.Point1;
                if (CollidesWith(a.Point2, b, out info))
                    candidate2 = a.Point2;

                if (CollidesWith(b.Point1, a, out info))
                    candidate1 = b.Point1;
                if (CollidesWith(b.Point2, a, out info))
                    candidate2 = b.Point2;

                Vector2f contact = candidate1.MagnitudeRaw() < candidate2.MagnitudeRaw() ? candidate1 : candidate2;
                Vector2f range = candidate2 - candidate1;
                return new CollisionInfo(contact, range.Normalize(), range.Magnitude());
            }
            else
                return new CollisionInfo(a.Point1, (a.Point2 - a.Point1).Normalize(), (a.Point2 - a.Point1).Magnitude());
        }


        public static SupportStruct FindSupportPoint(this RectangleCollider self, Vector2f dir, Vector2f ptOnEdge)
        {
            var tmpSupport = new SupportStruct(GameMath.MIN_VALUE, float.MinValue);

            for (var i = 0; i < self.Vertex.Length; i++)
            {
                var projection = (self.Vertex[i] - ptOnEdge).Dot(dir);

                if (projection > 0 && projection > tmpSupport.Distance)
                {
                    tmpSupport.Point = self.Vertex[i];
                    tmpSupport.Distance = projection;
                }
            }

            return tmpSupport;
        }


        public static bool FindAxisLeastPenetration(this RectangleCollider self, RectangleCollider other, ref CollisionInfo info)
        {
            float bestDistance = float.MaxValue;
            for (int i = 0; i < self.FaceNormal.Length; i++)
            {
                //acha o suporte em B, o ponto com maior distancia na aresta I
                var tmpSupport = other.FindSupportPoint(-self.FaceNormal[i], self.Vertex[i]);

                if (tmpSupport.Distance == float.MinValue)
                {
                    info = null;
                    return false;
                }

                //pega o ponto de suporte com menor profundidade
                if (tmpSupport.Distance < bestDistance)
                {
                    bestDistance = tmpSupport.Distance;
                    info = new CollisionInfo(tmpSupport.Point + self.FaceNormal[i] * tmpSupport.Distance,
                                             self.FaceNormal[i],
                                             tmpSupport.Distance);
                }
            }
            return true;
        }


        public static Vector2f GetIntersectionPoint(SegmentCollider a, SegmentCollider b)
        {
            var intersection = GameMath.INF_NEGATIVE;
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = a.Point2.X - a.Point1.X;
            s1_y = a.Point2.Y - a.Point1.Y;
            s2_x = b.Point2.X - b.Point1.X;
            s2_y = b.Point2.Y - b.Point1.Y;

            float s, t;
            s = (-s1_y * (a.Point1.X - b.Point1.X) + s1_x * (a.Point1.Y - b.Point1.Y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (a.Point1.Y - b.Point1.Y) - s2_y * (a.Point1.X - b.Point1.X)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected
                intersection.X = a.Point1.X + (t * s1_x) + 0.1f;
                intersection.Y = a.Point1.Y + (t * s1_y) + 0.1f;
                return intersection;
            }

            return intersection; // No collision
        }

        #endregion


        #region =========================== E A R L Y   E X I T   ======================== 


        /// <summary>
        /// Test if rectangle is completle lateral at a given axis
        /// </summary>        
        public static bool EarlyExit(ref RectangleCollider r, ref LineCollider l)
        {
            Vector2f n = l.Direction.Rotate90Deg();

            Vector2f c1 = r.Vertex[0];
            Vector2f c2 = r.Vertex[1];
            Vector2f c3 = r.Vertex[2];
            Vector2f c4 = r.Vertex[3];

            var xc1 = c1 - l.Base;
            var xc2 = c2 - l.Base;
            var xc3 = c3 - l.Base;
            var xc4 = c4 - l.Base;

            float dp1 = n.Dot(xc1);
            float dp2 = n.Dot(xc2);
            float dp3 = n.Dot(xc3);
            float dp4 = n.Dot(xc4);

            //se algum dos vertices está a menos de 90deg da base
            return !(dp1 * dp2 <= 0 || dp2 * dp3 <= 0 || dp3 * dp4 <= 0);
        }


        public static bool EarlyExitAABBxAABBPhysicsCookbook(RectangleCollider r1, RectangleCollider r2)
        {
            if (!((r1.MinX <= r2.MaxX && r1.MaxX >= r2.MinX) &&
                 (r1.MinY <= r2.MaxY && r1.MaxY >= r2.MinY)))
                return true;

            return false;
        }


        public static bool EarlyExitOBBxOBBPhysicsCookbook(ref RectangleCollider r1, ref RectangleCollider r2)
        {
            var boundR1 = new RectangleCollider(r1.MaxX - r1.MinX, r1.MaxY - r1.MinY, new Vector2f());
            var boundR2 = new RectangleCollider(r2.MaxX - r2.MinX, r2.MaxY - r2.MinY, new Vector2f());

            return EarlyExitAABBxAABBPhysicsCookbook(boundR1, boundR2);
        }


        public static bool EarlyExitPointxAABB(ref Vector2f p, ref RectangleCollider r)
        {
            return !
                (r.MinX <= p.X &&
                 r.MaxY >= p.Y &&
                 p.X <= r.MaxX &&
                 p.Y >= r.MinY);
        }


        public static bool EarlyExitCirclexAABB_OBB(ref CircleCollider c, ref RectangleCollider r)
        {
            //vetor de diagonal do centro a um vertice 
            var hipot = r.Center - r.Vertex[0];

            //se a distancia do circulo ao retangulo for maior que a magnitude da hipotenusa somada ao raio do circulo, não há colisão
            //TODO: IMPRECISO! A distancia pode ser menor e ainda assim nao haver colisao
            return (c.Center - r.Center).Magnitude() > hipot.Magnitude() + c.Radius;
        }

        #endregion



        #region =========================== D E T E C T I O N  =========================== 

        public static bool CollidesWith(RectangleCollider ra, RectangleCollider rb, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            if (EarlyExitAABBxAABBPhysicsCookbook(ra, rb))
                return false;

            //resolve para AABB x AABB
            if (ra.Rotation % 90 == 0 && rb.Rotation % 90 == 0)
            {
                var diffXRa1 = ra.MaxX - rb.MinX;
                var diffXRa2 = rb.MaxX - ra.MinX;
                var diffYRa1 = ra.MaxY - rb.MinY;
                var diffYRa2 = rb.MaxY - ra.MinY;

                Vector2f cx;
                Vector2f nx;
                Vector2f cy;
                Vector2f ny;

                float mtdx;
                float mtdy;

                //Calcula diferença para caso de X1
                if (diffXRa1 < diffXRa2)
                {
                    nx = new Vector2f(-1, 0);
                    mtdx = diffXRa1;
                    cx = new Vector2f(ra.MaxX, ra.Center.Y);
                }
                //Calcula diferença para caso de X2
                else
                {
                    nx = new Vector2f(-1, 0);
                    mtdx = diffXRa2;
                    cx = new Vector2f(rb.MaxX, rb.Center.Y);
                }

                //Calcula diferença para caso de Y1
                if (diffYRa1 < diffYRa2)
                {
                    ny = new Vector2f(0, -1);
                    mtdy = diffYRa1;
                    cy = new Vector2f(ra.Center.X, ra.MaxY);
                }
                //Calcula diferença para caso de Y2
                else
                {
                    ny = new Vector2f(0, -1);
                    mtdy = diffYRa2;
                    cy = new Vector2f(rb.Center.X, rb.MaxY);
                }

                //Decide se deve usar o mtd de Y caso seja menor que o de X
                if (mtdy < mtdx)
                {
                    collisionInfo = new CollisionInfo(cy, ny, mtdy);
                    return true;
                }

                collisionInfo = new CollisionInfo(cx, nx, mtdx);
                return true;
            }


            //Tenta early exit para bound obb
            //if (EarlyExitOBBxOBBPhysicsCookbook(ref ra, ref rb))
            //  return false;

            CollisionInfo infoSelf = null;
            CollisionInfo infoOther = null;
            bool status1 = ra.FindAxisLeastPenetration(rb, ref infoSelf);
            bool status2 = false;

            if (status1)
            {
                status2 = rb.FindAxisLeastPenetration(ra, ref infoOther);
                if (status2)
                {
                    if (infoSelf.MTD < infoOther.MTD)
                    {
                        var depthVec = infoSelf.Normal * infoSelf.MTD;
                        collisionInfo = new CollisionInfo(infoSelf.Start - depthVec, infoSelf.Normal, infoSelf.MTD);
                    }
                    else
                        collisionInfo = new CollisionInfo(infoOther.Start, infoOther.Normal * -1, infoOther.MTD);
                }
            }
            return status1 && status2;
        }

        public static bool CollidesWith(RectangleCollider r, LineCollider l, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            if (EarlyExit(ref r, ref l))
                return false;

            SegmentCollider sr = new SegmentCollider(l.GetPointOnLine(-_diagonalScreen), l.GetPointOnLine(_diagonalScreen));
            return CollidesWith(r, sr, out collisionInfo);
        }

        public static bool GamePhysicsCookbook(RectangleCollider r, LineCollider l, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            Vector2f n = l.Direction.Rotate90Deg();
            if (EarlyExit(ref r, ref l))
                return false;

            //testa primeiro para ângulos retos - AABB
            //if (r.Rotation == 0 || Math.Abs(r.Rotation) == 90 || Math.Abs(r.Rotation) == 180 || Math.Abs(r.Rotation) == 270 || Math.Abs(r.Rotation) == 360)
            if (Math.Abs(r.Rotation) % 90 == 0)
            {
                //encontra a intersecção do raio contra os 2 slabs (lajes) que formam o retângulo
                //soma com um valor de limiar 0.0001 para evitar divisão por zero
                float t1 = (r.MinX - l.Base.X) / (l.Direction.X + 0.00001f);
                float t2 = (r.MaxX - l.Base.X) / (l.Direction.X + 0.00001f);
                float t3 = (r.MinY - l.Base.Y) / (l.Direction.Y + 0.00001f);
                float t4 = (r.MaxY - l.Base.Y) / (l.Direction.Y + 0.00001f);

                //máximo menor valor
                float tmin = Math.Max(Math.Min(t1, t2), Math.Min(t3, t4));

                //mínimo maior valor
                float tmax = Math.Min(Math.Max(t1, t2), Math.Max(t3, t4));

                if (tmin > tmax)
                    return false;

                var min = l.GetPointOnLine(tmin);
                var max = l.GetPointOnLine(tmax);
                var c = max - min;
                collisionInfo = new CollisionInfo(min, c.Normalize(), c.Magnitude());

                return true;
            }
            else
            {
                //Converte o retângulo para AABB na origem do plano
                RectangleCollider localRect = new RectangleCollider(r.Width, r.Height, new Vector2f());

                //Converte a base da linha para local space do retângulo 
                Vector2f localBase;
                localBase = l.Base - r.Center;
                localBase = localBase.Rotate(-r.Rotation);

                //Converte a direção da linha para local space do retângulo 
                Vector2f localDir;
                localDir = l.SecondPoint - r.Center;
                localDir = localDir.Rotate(-r.Rotation);

                //Cria a linha local
                LineCollider localLine = new LineCollider(localBase, localDir);

                //Resolve a colisão como Line x AABB
                if (GamePhysicsCookbook(localRect, localLine, out collisionInfo))
                {
                    //O ponto de contato retornado do teste estará no espalo local do retângulo
                    //Converte-se o ponto local para espaço global
                    Vector2f pointWorld = collisionInfo.Start + r.Center;
                    pointWorld = pointWorld.Rotate(r.Center, r.Rotation);

                    collisionInfo = new CollisionInfo(pointWorld, l.Direction.Normalize(), collisionInfo.MTD);

                    return true;
                }
            }

            return false;
        }

        public static bool CollidesWith(RectangleCollider r, SegmentCollider s, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            //Early exit: se é lateral ao segmento, não há colisão
            LineCollider l = new LineCollider(s.Point1, s.Point2);
            if (EarlyExit(ref r, ref l))
                return false;


            //Só é possível que o segmento colida no máximo em duas arestas do retângulo
            Vector2f[] contacts = new Vector2f[2];
            short indexContacts = 0;

            //Testa segmento x aresta do retângulo
            for (short i = 0; i < r.Vertex.Length; i++)
            {
                var e = new SegmentCollider(r.Vertex[i], r.Vertex[(i + 1) % r.Vertex.Length]);

                //encontra um ponto de intersecção entre os segmentos, caso exista
                var c = GetIntersectionPoint(s, e);
                if (c != GameMath.INF_NEGATIVE)
                {
                    contacts[indexContacts++] = c;
                    //encontrado o máximo possível de colisões, termina o loop (early exit)
                    if (indexContacts == 2)
                        break;
                }
            }

            Vector2f d;
            //Caso exista somente uma colisão, encontra o ponto dentro do retângulo
            if (indexContacts == 1)
            {
                //Compara a distância bruta entre o retângulo e cada ponto, evitando raiz quadrada, 
                //e usa o ponto mais próximo do centro, ou seja, o que está dentro do retângulo
                Vector2f inside = s.Point1.RawDistanceBetween(r.Center) < s.Point2.RawDistanceBetween(r.Center) ? s.Point1 : s.Point2;

                //só existe um ponto de contato: a normal é de dentro do retângulo para fora
                d = inside - contacts[0];
                collisionInfo = new CollisionInfo(contacts[0], d.Normalize(), d.Magnitude());
            }
            //Como o early exit já verifica a impossibilidade de colisão, só resta o caso de haver 2 pontos de colisão
            else if (indexContacts == 2)
            {
                d = contacts[1] - contacts[0];
                int c = 0;

                collisionInfo = new CollisionInfo(contacts[c], d.Normalize(), d.Magnitude());
            }

            return true;
        }

        public static bool CollidesWith(SegmentCollider sa, SegmentCollider sb, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;
            var axisA = new LineCollider(sa.Point1, sa.Point2);
            if (axisA.IsOnOneSide(sb))
                return false;

            var axisB = new LineCollider(sb.Point1, sb.Point2);
            if (axisB.IsOnOneSide(sa))
                return false;

            if (axisA.Direction.IsParallelTo(axisB.Direction))
            {
                Range rangeA = sa.ProjectOnto(axisA.Direction);
                Range rangeB = sb.ProjectOnto(axisA.Direction);

                if (rangeA.IsOverlapping(rangeB))
                {
                    collisionInfo = GetCollisionInfoFrom(sa, sb);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                Vector2f contact = GetIntersectionPoint(sa, sb);
                Vector2f intersection = contact - sa.Point1;

                if ((contact - sa.Point2).Magnitude() < intersection.Magnitude())
                    intersection = contact - sa.Point2;

                if ((contact - sb.Point1).Magnitude() < intersection.Magnitude())
                    intersection = contact - sb.Point1;

                if ((contact - sb.Point2).Magnitude() < intersection.Magnitude())
                    intersection = contact - sb.Point2;

                collisionInfo = new CollisionInfo(contact, -intersection.Normalize(), intersection.Magnitude());
                return true;
            }
        }

        public static bool CollidesWith(SegmentCollider s, LineCollider l, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            //se está completamenete do outro lado, early exit
            if (l.IsOnOneSide(s))
                return false;

            Vector2f d = s.Point2 - s.Point1;
            //Se é paralelo (estão sobrepostos), o comprimento da colisão é a magnitude do segmento (o próprio segmento)
            if (d.IsParallelTo(l.Direction))
                collisionInfo = new CollisionInfo(s.Point1, d.Normalize(), d.Magnitude());
            else
            {
                //converte a linha em segmento e obtém o ponto de contato
                Vector2f contact = GetIntersectionPoint(s, new SegmentCollider(l.GetPointOnLine(-_diagonalScreen), l.GetPointOnLine(_diagonalScreen)));

                //o comprimento da colisão é a menor distância entre o ponto de contato e um dos pontos do segmento
                float distP1 = contact.DistanceBetween(s.Point1);
                float distP2 = contact.DistanceBetween(s.Point2);

                //determina qual o ponto da menor distância e usa a normal nessa direção
                if (distP1 < distP2)
                    collisionInfo = new CollisionInfo(contact, (s.Point1 - contact).Normalize(), distP1);
                else
                    collisionInfo = new CollisionInfo(contact, (s.Point2 - contact).Normalize(), distP2);
            }
            return true;
        }

        public static bool CollidesWith(LineCollider la, LineCollider lb, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            if (la.Direction.IsParallelTo(lb.Direction))
            //TODO:
            //if(la.Slope == lb.Slope)
            {
                if (la.IsEquivalent(lb))
                {
                    collisionInfo = new CollisionInfo(la.Base, -la.Direction.Normalize(), _diagonalScreen);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                var p = GetIntersectionPoint(new SegmentCollider(la.GetPointOnLine(-_diagonalScreen), la.GetPointOnLine(_diagonalScreen)),
                                             new SegmentCollider(lb.GetPointOnLine(-_diagonalScreen), lb.GetPointOnLine(_diagonalScreen)));
                collisionInfo = new CollisionInfo(p, la.Direction.Normalize(), _diagonalScreen);
                return true;
            }
        }

        public static bool CollidesWith(CircleCollider ca, CircleCollider cb, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;
            Vector2f from1to2 = cb.Center - ca.Center;
            float radiusSum = ca.Radius + cb.Radius;
            float distance = from1to2.Magnitude();

            if (distance >= radiusSum)
                return false;

            if (distance != 0)
            {
                //sobrepondo em posições diferentes
                var normalFrom2To1 = (-1 * from1to2).Normalize();
                var radius2 = normalFrom2To1 * cb.Radius;
                collisionInfo = new CollisionInfo(cb.Center + radius2, from1to2.Normalize(), radiusSum - distance);
                return true;
            }
            else
            {
                //sobrepondo em posições iguais
                if (ca.Radius > cb.Radius)
                    collisionInfo = new CollisionInfo(ca.Center + new Vector2f(0, ca.Radius), new Vector2f(0, -1), radiusSum);
                else
                    collisionInfo = new CollisionInfo(cb.Center + new Vector2f(0, cb.Radius), new Vector2f(0, -1), radiusSum);

                return true;
            }
        }

        public static bool CollidesWith(CircleCollider c, RectangleCollider r, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            if (EarlyExitCirclexAABB_OBB(ref c, ref r))
                return false;

            bool inside = true;
            float bestDistance = float.MinValue;
            int nearestEdge = 0;
            Vector2f v;
            int i;
            Vector2f circ2Pos = new Vector2f();
            float projection;


            //descobre a aresta mais próxima do centro do círculo
            for (i = 0; i < r.Vertex.Length; i++)
            {
                //face mais proxima do centro do circulo
                circ2Pos = c.Center;
                v = circ2Pos - r.Vertex[i];
                projection = v.Dot(r.FaceNormal[i]);

                //+ projection = centro está fora do retangulo
                if (projection > 0)
                {
                    bestDistance = projection;
                    nearestEdge = i;
                    inside = false;
                    break;
                }

                if (projection > bestDistance)
                {
                    bestDistance = projection;
                    nearestEdge = i;
                }
            }

            Vector2f normal;
            Vector2f radiusVec;
            float dis;

            //centro do círculo está fora do retângulo
            if (!inside)
            {
                //se está na região R1
                var v1 = circ2Pos - r.Vertex[nearestEdge];
                var v2 = r.Vertex[(nearestEdge + 1) % r.Vertex.Length] - r.Vertex[nearestEdge];

                var dot = v1.Dot(v2);

                //região R1
                if (dot < 0)
                {
                    dis = v1.Magnitude();
                    if (dis > c.Radius)
                        return false;

                    normal = v1.Normalize();
                    radiusVec = normal * (-c.Radius);
                    collisionInfo = new CollisionInfo(circ2Pos + radiusVec, normal, c.Radius - dis);
                }
                else //região R2
                {
                    v1 = circ2Pos - r.Vertex[(nearestEdge + 1) % r.Vertex.Length];
                    v2 = v2 * -1;
                    dot = v1.Dot(v2);

                    if (dot < 0)
                    {
                        dis = v1.Magnitude();
                        if (dis > c.Radius)
                            return false;

                        normal = v1.Normalize();
                        radiusVec = normal * (-c.Radius);
                        collisionInfo = new CollisionInfo(circ2Pos + radiusVec, normal, c.Radius - dis);
                    }
                    else
                    {
                        if (bestDistance < c.Radius)
                        {
                            radiusVec = r.FaceNormal[nearestEdge] * c.Radius;
                            collisionInfo = new CollisionInfo(circ2Pos - radiusVec, r.FaceNormal[nearestEdge], c.Radius - bestDistance);
                        }
                        else
                            return false;
                    }
                }
            }
            //centro do círculo está dentro do retângulo
            else
            {
                radiusVec = r.FaceNormal[nearestEdge] * c.Radius;
                collisionInfo = new CollisionInfo(circ2Pos - radiusVec, r.FaceNormal[nearestEdge], c.Radius - bestDistance);
            }
            return true;
        }

        public static bool CollidesWith(CircleCollider c, Vector2f p, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;
            var distance = c.Center - p;
            if (distance.Magnitude() <= c.Radius)
            {

                var edgePoint = new Vector2f(p.X, p.Y + (p.Y - c.Radius));

                if (distance.Equals(new Vector2f()))
                    distance = edgePoint - c.Center;

                collisionInfo = new CollisionInfo(p, -distance.Normalize(), c.Radius - c.Center.DistanceBetween(p));
                return true;
            }
            return false;
        }

        public static bool CollidesWith(CircleCollider c, SegmentCollider seg, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            bool cp1 = false;
            bool cp2 = false;
            Vector2f d = seg.Point2 - seg.Point1;

            float cx = c.Center.X;
            float cy = c.Center.Y;
            float radius = c.Radius;
            Vector2f point1 = seg.Point1;
            Vector2f point2 = seg.Point2;
            Vector2f intersection1;
            Vector2f intersection2;

            cp1 = CollidesWith(seg.Point1, c, out collisionInfo);
            cp2 = CollidesWith(seg.Point2, c, out collisionInfo);

            if (cp1 || cp2)
            {
                //http://csharphelper.com/blog/2014/09/determine-where-a-line-intersects-a-circle-in-c/
                float dx, dy, A, B, C, det, t;
                dx = point2.X - point1.X;
                dy = point2.Y - point1.Y;

                A = dx * dx + dy * dy;
                B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
                C = (point1.X - cx) * (point1.X - cx) + (point1.Y - cy) * (point1.Y - cy) - radius * radius;

                det = B * B - 4 * A * C;

                // Two solutions.
                if (cp1)
                {
                    t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                    intersection1 = new Vector2f(point1.X + t * dx, point1.Y + t * dy);
                    collisionInfo = new CollisionInfo(intersection1, (seg.Point1 - intersection1).Normalize(), (intersection1 - seg.Point1).Magnitude());
                    return true;
                }
                else if (cp2)
                {
                    t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                    intersection2 = new Vector2f(point1.X + t * dx, point1.Y + t * dy);
                    collisionInfo = new CollisionInfo(intersection2, (seg.Point2 - intersection2).Normalize(), (seg.Point2 - intersection2).Magnitude());
                    return true;
                }
            }

            Vector2f lc1;
            Vector2f p1;
            Vector2f nearest1;
            //se o segmento atravessa o círculo
            d = seg.Point2 - seg.Point1;
            lc1 = c.Center - seg.Point1;
            p1 = lc1.ProjectOnto(d);
            nearest1 = seg.Point1 + p1;

            if (CollidesWith(c, nearest1, out collisionInfo) &&
                p1.Magnitude() <= d.Magnitude() &&
                0 <= p1.Dot(d))
                return true;

            collisionInfo = null;
            return false;
        }

        public static bool CollidesWith(CircleCollider c, LineCollider l, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            Vector2f lineBaseToCenter = c.Center - l.Base;
            Vector2f projectedOnDirection = lineBaseToCenter.ProjectOnto(l.Direction);
            Vector2f nearestPointToCenter = l.Base + projectedOnDirection;
            return (CollidesWith(c, nearestPointToCenter, out collisionInfo));
        }

        public static bool CollidesWith(Vector2f p, SegmentCollider s, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;
            Vector2f d = s.Point2 - s.Point1;
            Vector2f lp = p - s.Point1;
            Vector2f pr = lp.ProjectOnto(d);

            if (lp.Equals(pr) && pr.Magnitude() <= d.Magnitude() && 0 <= pr.Dot(d))
            {
                Vector2f projectionOnSegment = p.ProjectOnto(s.Point2 - s.Point1);

                Vector2f psp1 = s.Point1 - p;
                Vector2f psp2 = s.Point2 - p;

                if (psp1.MagnitudeRaw() < psp2.MagnitudeRaw())
                    collisionInfo = new CollisionInfo(p, psp1.Normalize(), psp1.Magnitude());
                else
                    collisionInfo = new CollisionInfo(p, psp2.Normalize(), psp2.Magnitude());

                return true;
            }
            return false;
        }

        public static bool CollidesWith(Vector2f p, LineCollider l, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            Vector2f lp = p - l.Base;
            if (lp.IsParallelTo(l.Direction))
            {
                Vector2f projectionOnLine = p.ProjectOnto(l.Direction);
                collisionInfo = new CollisionInfo(p, l.Direction.Normalize(), _diagonalScreen);
                return true;
            }
            else
                return false;
        }

        public static bool CollidesWith(Vector2f p, RectangleCollider r, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;

            if (EarlyExitPointxAABB(ref p, ref r))
                return false;

            float bestDistance = float.MaxValue;
            int bestIndex = -1;

            for (int i = 0; i < r.FaceNormal.Length; i++)
            {
                //normal de A
                //Vector2f n = r.FaceNormal[i];

                //-n é a direção e o vertice I é o ponto na aresta
                //Vector2f dir = -n;
                //Vector2f ptOnEdge = p;

                //acha o suporte em B, o ponto com maior distancia na aresta I
                var tmpSupport = r.FindSupportPoint(-r.FaceNormal[i], p);

                if (tmpSupport.Distance == float.MinValue)
                    return false;

                //pega o ponto de suporte com menor profundidade
                if (tmpSupport.Distance < bestDistance)
                {
                    bestDistance = tmpSupport.Distance;
                    bestIndex = i;
                }
            }

            collisionInfo = new CollisionInfo(p, -r.FaceNormal[bestIndex], bestDistance);
            return true;
        }

        public static bool CollidesWith(Vector2f p, CircleCollider c, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;
            Vector2f distance = c.Center - p;
            bool d = distance.Magnitude() <= c.Radius;
            if (d)
            {
                collisionInfo = new CollisionInfo(p, -distance.Normalize(), c.Radius - distance.Magnitude());
                return true;
            }
            return false;
        }

        public static bool CollidesWith(Vector2f p1, Vector2f p2, out CollisionInfo collisionInfo)
        {
            collisionInfo = null;
            float tolerance = 0.1f;
            Vector2f d = p1 - p2;
            float mag = d.Magnitude();

            if (p1.Equals(p2) || mag <= tolerance)
            {
                if (mag == 0)
                {
                    d = new Vector2f(0, 1);
                    mag = tolerance;
                }

                collisionInfo = new CollisionInfo(p1, d.Normalize(), mag);
                return true;
            }

            return false;
        }

        #endregion =================================================================================
    }
}

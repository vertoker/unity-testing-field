namespace CollisionDetection2D
{
    using num = System.Single;
    //using num = System.Double;

    public static class CollisionDetectionStatic
    {
        #region Static
        private static num Sqrt(num x)
        {
            if (x <= 0)
                return 0;
            num root = x / 3;
            for (int i = 0; i < 32; i++)
                root = (root + x / root) / 2;
            return root;
        }
        private static num Abs(num x)
        {
            if (x < 0)
                return -x;
            return x;
        }
        private static num Distance(num x1, num y1, num x2, num y2)
        {
            num distX = x2 - x1, distY = y2 - y1;
            return Sqrt((distX * distX) + (distY * distY));
        }
        #endregion

        #region Point
        public static bool PointPoint(num x1, num y1, num x2, num y2)
        {
            return x1 == x2 && y1 == y2;
        }
        public static bool PointCircle(num px, num py, num cx, num cy, num cr)
        {
            return Distance(px, py, cx, cy) <= cr;
        }
        public static bool PointRectangle(num px, num py, num rx, num ry, num rw, num rh)
        {
            num w2 = rw / 2, h2 = rh / 2;
            return px >= rx - w2 && px <= rx + w2 && py >= ry - h2 && py <= ry + h2;
        }
        public static bool PointLine(num px, num py, num lx1, num ly1, num lx2, num ly2, num buf = 0.1f)
        {
            num d1 = Distance(px, py, lx1, ly1);
            num d2 = Distance(px, py, lx2, ly2);
            num lineLen = Distance(lx1, ly1, lx2, ly2);
            return d1 + d2 >= lineLen - buf && d1 + d2 <= lineLen + buf;
        }
        public static bool PointPolygon(num px, num py, Point[] vertices)
        {
            int next; bool collision = false;
            for (int current = 0; current < vertices.Length; current++)
            {
                next = current + 1;
                if (next == vertices.Length)
                    next = 0;

                Point vc = vertices[current];
                Point vn = vertices[next];

                if (((vc.y >= py && vn.y < py) || (vc.y < py && vn.y >= py)) &&
                     (px < (vn.x - vc.x) * (py - vc.y) / (vn.y - vc.y) + vc.x))
                    collision = !collision;
            }
            return collision;
        }
        public static bool PointTriangle(num px, num py, num x1, num y1, num x2, num y2, num x3, num y3)
        {
            num areaOrig = Abs((x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1));
            num area1 = Abs((x1 - px) * (y2 - py) - (x2 - px) * (y1 - py));
            num area2 = Abs((x2 - px) * (y3 - py) - (x3 - px) * (y2 - py));
            num area3 = Abs((x3 - px) * (y1 - py) - (x1 - px) * (y3 - py));
            return area1 + area2 + area3 == areaOrig;
        }
        #endregion

        #region Circle
        public static bool CircleCircle(num x1, num y1, num r1, num x2, num y2, num r2)
        {
            return Distance(x1, y1, x2, y2) <= r1 + r2;
        }
        public static bool CircleRectangle(num cx, num cy, num cr, num rx, num ry, num rw, num rh)
        {
            num xmin = rx - rw / 2;
            num xmax = rx + rw / 2;
            num ymin = ry - rh / 2;
            num ymax = ry + rh / 2;

            num testX = cx;
            num testY = cy;
            if (cx < xmin)
                testX = xmin;
            else if (cx > xmax)
                testX = xmax;
            if (cy < ymin)
                testY = ymin;
            else if (cy > ymax)
                testY = ymax;

            return Distance(cx, cy, testX, testY) <= cr;
        }
        public static bool CircleLine(num cx, num cy, num cr, num lx1, num ly1, num lx2, num ly2, num buf = 0.1f)
        {
            bool inside1 = PointCircle(lx1, ly1, cx, cy, cr);
            bool inside2 = PointCircle(lx2, ly2, cx, cy, cr);
            if (inside1 || inside2)
                return true;

            num len = Distance(lx1, ly1, lx2, ly2);
            num dot = (((cx - lx1) * (lx2 - lx1)) + ((cy - ly1) * (ly2 - ly1))) / (len * len);

            num closestX = lx1 + (dot * (lx2 - lx1));
            num closestY = ly1 + (dot * (ly2 - ly1));

            if (!PointLine(closestX, closestY, lx1, ly1, lx2, ly2))
                return false;

            return Distance(cx, cy, closestX, closestY) <= cr;
        }
        public static bool CirclePolygon(num cx, num cy, num cr, Point[] vertices)
        {
            bool inPolygon = PointPolygon(cx, cy, vertices);
            if (inPolygon)
                return true;

            int next;
            for (int current = 0; current < vertices.Length; current++)
            {
                next = current + 1;
                if (next == vertices.Length)
                    next = 0;

                Point vc = vertices[current];
                Point vn = vertices[next];

                if (CircleLine(cx, cy, cr, vc.x, vc.y, vn.x, vn.y))
                    return true;
            }
            return inPolygon;
        }
        public static bool CircleTriangle(num cx, num cy, num cr, num x1, num y1, num x2, num y2, num x3, num y3)
        {
            if (PointTriangle(cx, cy, x1, y1, x2, y2, x3, y3))
                return true;

            num p1 = Distance(cx, cy, x1, y1);
            if (p1 <= cr)
                return true;

            num p2 = Distance(cx, cy, x2, y2);
            if (p2 <= cr)
                return true;

            num p3 = Distance(cx, cy, x3, y3);
            if (p3 <= cr)
                return true;

            num dist = Distance(x1, y1, x2, y2);
            num s = (dist + p1 + p2) / 2;
            if (2 * Sqrt(s * (s - dist) * (s - p1) * (s - p2)) / dist <= cr)
                return true;

            dist = Distance(x2, y2, x3, y3);
            s = (dist + p2 + p3) / 2;
            if (2 * Sqrt(s * (s - dist) * (s - p2) * (s - p3)) / dist <= cr)
                return true;

            dist = Distance(x1, y1, x3, y3);
            s = (dist + p1 + p3) / 2;
            return 2 * Sqrt(s * (s - dist) * (s - p1) * (s - p3)) / dist <= cr;
        }
        #endregion

        #region Rectangle
        public static bool RectangleRectangle(num x1, num y1, num w1, num h1, num x2, num y2, num w2, num h2)
        {
            num w1h = w1 / 2, h1h = h1 / 2, w2h = w2 / 2, h2h = h2 / 2;
            return x1 + w1h >= x2 - w2h && x1 - w1h <= x2 + w2h && y1 + h1h >= y2 - h2h && y1 - h1h <= y2 + h2h;
        }
        public static bool RectangleLine(num rx, num ry, num rw, num rh, num x1, num y1, num x2, num y2)
        {
            num xmin = rx - rw / 2, xmax = rx + rw / 2;
            num ymin = ry - rh / 2, ymax = ry + rh / 2;
            bool left = LineLine(x1, y1, x2, y2, xmin, ymin, xmin, ymax);
            bool right = LineLine(x1, y1, x2, y2, xmax, ymin, xmax, ymax);
            bool top = LineLine(x1, y1, x2, y2, xmin, ymin, xmax, ymin);
            bool bottom = LineLine(x1, y1, x2, y2, xmin, ymax, xmax, ymax);
            return left || right || top || bottom;
        }
        public static bool RectanglePolygon(num rx, num ry, num rw, num rh, Point[] vertices)
        {
            bool inPolygon = PointPolygon(rx, ry, vertices);
            if (inPolygon)
                return true;

            int next;
            for (int current = 0; current < vertices.Length; current++)
            {
                next = current + 1;
                if (next == vertices.Length)
                    next = 0;

                Point vc = vertices[current];
                Point vn = vertices[next];

                if (RectangleLine(rx, ry, rw, rh, vc.x, vc.y, vn.x, vn.y))
                    return true;
            }
            return inPolygon;
        }
        public static bool RectangleTriangle(num rx, num ry, num rw, num rh, num x1, num y1, num x2, num y2, num x3, num y3)
        {
            if (PointRectangle(x1, y1, rx, ry, rw, rh) || PointRectangle(x2, y2, rx, ry, rw, rh) || PointRectangle(x3, y3, rx, ry, rw, rh))
                return true;

            num xmin = rx - rw / 2;
            num ymin = ry - rh / 2;
            if (PointTriangle(xmin, ymin, x1, y1, x2, y2, x3, y3))
                return true;

            num xmax = rx + rw / 2;
            if (PointTriangle(xmax, ymin, x1, y1, x2, y2, x3, y3))
                return true;

            num ymax = ry + rh / 2;
            return PointTriangle(xmin, ymax, x1, y1, x2, y2, x3, y3) || PointTriangle(xmax, ymax, x1, y1, x2, y2, x3, y3);
        }
        #endregion

        #region Line
        public static bool LineLine(num x1, num y1, num x2, num y2, num x3, num y3, num x4, num y4)
        {
            num uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            num uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
        }
        public static bool LinePolygon(num x1, num y1, num x2, num y2, Point[] vertices)
        {
            bool inPolygon = PointPolygon(x1, y1, vertices) || PointPolygon(x2, y2, vertices);
            if (inPolygon)
                return true;

            int next;
            for (int current = 0; current < vertices.Length; current++)
            {
                next = current + 1;
                if (next == vertices.Length)
                    next = 0;

                if (LineLine(x1, y1, x2, y2, vertices[current].x, vertices[current].y, vertices[next].x, vertices[next].y))
                    return true;
            }
            return inPolygon;
        }
        public static bool LineTriangle(num x1, num y1, num x2, num y2, num x3, num y3, num x4, num y4, num x5, num y5)
        {
            return PointTriangle(x1, y1, x3, y3, x4, y4, x5, y5) ||
                PointTriangle(x2, y2, x3, y3, x4, y4, x5, y5) ||
                LineLine(x1, y1, x2, y2, x3, y3, x4, y4) ||
                LineLine(x1, y1, x2, y2, x3, y3, x5, y5) ||
                LineLine(x1, y1, x2, y2, x4, y4, x5, y5);
        }
        #endregion

        #region Polygon
        public static bool PolygonPolygon(Point[] vertices1, Point[] vertices2)
        {
            bool inPolygon = PointPolygon(vertices2[0].x, vertices2[0].y, vertices1);
            if (inPolygon)
                return true;

            int next;
            for (int current = 0; current < vertices1.Length; current++)
            {
                next = current + 1;
                if (next == vertices1.Length)
                    next = 0;

                Point vc = vertices1[current];
                Point vn = vertices1[next];

                if (LinePolygon(vc.x, vc.y, vn.x, vn.y, vertices2))
                    return true;
            }
            return inPolygon;
        }
        public static bool PolygonTriangle(Point[] vertices, num x1, num y1, num x2, num y2, num x3, num y3)
        {
            bool inPolygon = PointTriangle(x1, y1, vertices) || PointTriangle(x2, y2, vertices) || PointTriangle(x3, y3, vertices);
            if (inPolygon)
                return true;

            int next;
            for (int current = 0; current < vertices.Length; current++)
            {
                next = current + 1;
                if (next == vertices.Length)
                    next = 0;

                Point vc = vertices[current];
                Point vn = vertices[next];

                if (LineTriangle(vc.x, vc.y, vn.x, vn.y, x1, y1, x2, y2, x3, y3))
                    return true;
            }
            return false;
        }
        #endregion

        #region Triangle
        public static bool TriangleTriangle(num x1, num y1, num x2, num y2, num x3, num y3, num x4, num y4, num x5, num y5, num x6, num y6)
        {
            static bool PointTriangle(num px, num py, num x1, num y1, num x2, num y2, num x3, num y3, num areaOrig)
            {
                num area1 = Abs((x1 - px) * (y2 - py) - (x2 - px) * (y1 - py));
                num area2 = Abs((x2 - px) * (y3 - py) - (x3 - px) * (y2 - py));
                num area3 = Abs((x3 - px) * (y1 - py) - (x1 - px) * (y3 - py));
                return area1 + area2 + area3 == areaOrig;
            }

            num areaOrig1 = Abs((x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1));
            if (PointTriangle(x4, y4, x1, y1, x2, y2, x3, y3, areaOrig1) ||
                PointTriangle(x5, y5, x1, y1, x2, y2, x3, y3, areaOrig1) ||
                PointTriangle(x6, y6, x1, y1, x2, y2, x3, y3, areaOrig1))
                return true;

            num areaOrig2 = Abs((x5 - x4) * (y6 - y4) - (x6 - x4) * (y5 - y4));
            return PointTriangle(x1, y1, x4, y4, x5, y5, x6, y6, areaOrig2) ||
                PointTriangle(x2, y2, x4, y4, x5, y5, x6, y6, areaOrig2) ||
                PointTriangle(x3, y3, x4, y4, x5, y5, x6, y6, areaOrig2) ||
                LineLine(x1, y1, x2, y2, x4, y4, x5, y5) ||
                LineLine(x1, y1, x2, y2, x4, y4, x6, y6) ||
                LineLine(x1, y1, x2, y2, x5, y5, x6, y6) ||
                LineLine(x1, y1, x3, y3, x4, y4, x5, y5) ||
                LineLine(x1, y1, x3, y3, x4, y4, x6, y6) ||
                LineLine(x1, y1, x3, y3, x5, y5, x6, y6) ||
                LineLine(x2, y2, x3, y3, x4, y4, x5, y5) ||
                LineLine(x2, y2, x3, y3, x4, y4, x6, y6) ||
                LineLine(x2, y2, x3, y3, x5, y5, x6, y6);
        }
        #endregion
    }
}

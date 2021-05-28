namespace CollisionDetection2D
{
    public struct Triangle : ICollision
    {
        public float x1, y1, x2, y2, x3, y3;

        public bool CollisionDetection(ICollision collision)
        {
            return collision.CollisionDetectionTriangle(this);
        }
        public bool CollisionDetectionPoint(Point point)
        {
            return CollisionDetectionStatic.PointTriangle(point.x, point.y, x1, y1, x2, y2, x3, y3);
        }
        public bool CollisionDetectionCircle(Circle circle)
        {
            return CollisionDetectionStatic.CircleTriangle(circle.x, circle.y, circle.r, x1, y1, x2, y2, x3, y3);
        }
        public bool CollisionDetectionRectangle(Rectangle rect)
        {
            return CollisionDetectionStatic.RectangleTriangle(rect.x, rect.y, rect.w, rect.h, x1, y1, x2, y2, x3, y3);
        }
        public bool CollisionDetectionLine(Line line)
        {
            return CollisionDetectionStatic.LineTriangle(line.x1, line.y1, line.x2, line.y2, x1, y1, x2, y2, x3, y3);
        }
        public bool CollisionDetectionPolygon(Polygon polygon)
        {
            return CollisionDetectionStatic.PolygonTriangle(polygon.vertices, x1, y1, x2, y2, x3, y3);
        }
        public bool CollisionDetectionTriangle(Triangle trian)
        {
            return CollisionDetectionStatic.TriangleTriangle(trian.x1, trian.y1, trian.x2, trian.y2, trian.x3, trian.y3, x1, y1, x2, y2, x3, y3);
        }
    }
}
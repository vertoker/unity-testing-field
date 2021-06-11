namespace CollisionDetection2D
{
    public struct Point : ICollision
    {
        public float x, y;
        public bool CollisionDetection(ICollision collision)
        {
            return collision.CollisionDetectionPoint(this);
        }
        public bool CollisionDetectionPoint(Point point)
        {
            return CollisionDetectionStatic.PointPoint(x, y, point.x, point.y);
        }
        public bool CollisionDetectionCircle(Circle circle)
        {
            return CollisionDetectionStatic.PointCircle(x, y, circle.x, circle.y, circle.r);
        }
        public bool CollisionDetectionRectangle(Rectangle rect)
        {
            return CollisionDetectionStatic.PointRectangle(x, y, rect.x, rect.y, rect.w, rect.h);
        }
        public bool CollisionDetectionLine(Line line)
        {
            return CollisionDetectionStatic.PointLine(x, y, line.x1, line.y1, line.x2, line.y2, line.buf);
        }
        public bool CollisionDetectionPolygon(Polygon polygon)
        {
            return CollisionDetectionStatic.PointPolygon(x, y, polygon.vertices);
        }
        public bool CollisionDetectionTriangle(Triangle triangle)
        {
            return CollisionDetectionStatic.PointTriangle(x, y, triangle.x1, triangle.y1, triangle.x2, triangle.y2, triangle.x3, triangle.y3);
        }
    }
}

namespace CollisionDetection2D
{
    public struct Rectangle : ICollision
    {
        public float x, y, w, h;
        public bool CollisionDetection(ICollision collision)
        {
            return CollisionDetectionRectangle(this);
        }
        public bool CollisionDetectionPoint(Point point)
        {
            return CollisionDetectionStatic.PointRectangle(point.x, point.y, x, y, w, h);
        }
        public bool CollisionDetectionCircle(Circle circle)
        {
            return CollisionDetectionStatic.CircleRectangle(circle.x, circle.y, circle.r, x, y, w, h);
        }
        public bool CollisionDetectionRectangle(Rectangle rect)
        {
            return CollisionDetectionStatic.RectangleRectangle(x, y, w, h, rect.x, rect.y, rect.w, rect.h);
        }
        public bool CollisionDetectionLine(Line line)
        {
            return CollisionDetectionStatic.RectangleLine(x, y, w, h, line.x1, line.y1, line.x2, line.y2);
        }
        public bool CollisionDetectionPolygon(Polygon polygon)
        {
            return CollisionDetectionStatic.RectanglePolygon(x, y, w, h, polygon.vertices);
        }
        public bool CollisionDetectionTriangle(Triangle triangle)
        {
            return CollisionDetectionStatic.RectangleTriangle(x, y, w, h, triangle.x1, triangle.y1, triangle.x2, triangle.y2, triangle.x3, triangle.y3);
        }
    }
}
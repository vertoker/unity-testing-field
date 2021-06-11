namespace CollisionDetection2D
{
    public struct Polygon : ICollision
    {
        public Point[] vertices;
        public bool CollisionDetection(ICollision collision)
        {
            return collision.CollisionDetectionPolygon(this);
        }
        public bool CollisionDetectionPoint(Point point)
        {
            return CollisionDetectionStatic.PointPolygon(point.x, point.y, vertices);
        }
        public bool CollisionDetectionCircle(Circle circle)
        {
            return CollisionDetectionStatic.CirclePolygon(circle.x, circle.y, circle.r, vertices);
        }
        public bool CollisionDetectionRectangle(Rectangle rect)
        {
            return CollisionDetectionStatic.RectanglePolygon(rect.x, rect.y, rect.w, rect.h, vertices);
        }
        public bool CollisionDetectionLine(Line line)
        {
            return CollisionDetectionStatic.LinePolygon(line.x1, line.y1, line.x2, line.y2, vertices);
        }
        public bool CollisionDetectionPolygon(Polygon polygon)
        {
            return CollisionDetectionStatic.PolygonPolygon(vertices, polygon.vertices);
        }
        public bool CollisionDetectionTriangle(Triangle triangle)
        {
            return CollisionDetectionStatic.PolygonTriangle(vertices, triangle.x1, triangle.y1, triangle.x2, triangle.y2, triangle.x3, triangle.y3);
        }
    }
}

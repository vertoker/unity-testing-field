namespace CollisionDetection2D
{
    public struct Line : ICollision
    {
        public float x1, y1, x2, y2, buf;
        public bool CollisionDetection(ICollision collision)
        {
            return CollisionDetectionLine(this);
        }
        public bool CollisionDetectionPoint(Point point)
        {
            return CollisionDetectionStatic.PointLine(point.x, point.y, x1, y1, x2, y2, buf);
        }
        public bool CollisionDetectionCircle(Circle circle)
        {
            return CollisionDetectionStatic.CircleLine(circle.x, circle.y, circle.r, x1, y1, x2, y2, buf);
        }
        public bool CollisionDetectionRectangle(Rectangle rect)
        {
            return CollisionDetectionStatic.RectangleLine(rect.x, rect.y, rect.w, rect.h, x1, y1, x2, y2);
        }
        public bool CollisionDetectionLine(Line line)
        {
            return CollisionDetectionStatic.LineLine(line.x1, line.y1, line.x2, line.y2, x1, y1, x2, y2);
        }
        public bool CollisionDetectionPolygon(Polygon polygon)
        {
            return CollisionDetectionStatic.LinePolygon(x1, y1, x2, y2, polygon.vertices);
        }
        public bool CollisionDetectionTriangle(Triangle triangle)
        {
            return CollisionDetectionStatic.LineTriangle(x1, y1, x2, y2, triangle.x1, triangle.y1, triangle.x2, triangle.y2, triangle.x3, triangle.y3);
        }
    }
}
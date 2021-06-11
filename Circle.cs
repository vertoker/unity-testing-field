namespace CollisionDetection2D
{
    public struct Circle : ICollision
    {
        public float x, y, r;
        public bool CollisionDetection(ICollision collision)
        {
            return collision.CollisionDetectionCircle(this);
        }
        public bool CollisionDetectionPoint(Point point)
        {
            return CollisionDetectionStatic.PointCircle(point.x, point.y, x, y, r);
        }
        public bool CollisionDetectionCircle(Circle circle)
        {
            return CollisionDetectionStatic.CircleCircle(x, y, r, circle.x, circle.y, circle.r);
        }
        public bool CollisionDetectionRectangle(Rectangle rect)
        {
            return CollisionDetectionStatic.CircleRectangle(x, y, r, rect.x, rect.y, rect.w, rect.h);
        }
        public bool CollisionDetectionLine(Line line)
        {
            return CollisionDetectionStatic.CircleLine(x, y, r, line.x1, line.y1, line.x2, line.y2, line.buf);
        }
        public bool CollisionDetectionPolygon(Polygon polygon)
        {
            return CollisionDetectionStatic.CirclePolygon(x, y, r, polygon.vertices);
        }
        public bool CollisionDetectionTriangle(Triangle triangle)
        {
            return CollisionDetectionStatic.CircleTriangle(x, y, r, triangle.x1, triangle.y1, triangle.x2, triangle.y2, triangle.x3, triangle.y3);
        }
    }
}

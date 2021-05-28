namespace CollisionDetection2D
{
    public interface ICollision
    {
        public bool CollisionDetection(ICollision collision);
        public bool CollisionDetectionPoint(Point point);
        public bool CollisionDetectionCircle(Circle circle);
        public bool CollisionDetectionRectangle(Rectangle rect);
        public bool CollisionDetectionLine(Line line);
        public bool CollisionDetectionPolygon(Polygon polygon);
        public bool CollisionDetectionTriangle(Triangle triangle);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nAlpha
{
    public class AlphaShapeCalculator
    {
        public double Alpha { get; set; }
        public bool CloseShape { get; set; }
        public double Radius => 1/Alpha;

        private List<Tuple<int, int>> resultingEdges = new List<Tuple<int, int>>();
        private List<Point> resultingVertices = new List<Point>();
        private Point[] points;

        public Shape CalculateShape(Point[] points)
        {
            SetData(points);
            CalculateShape();
            CloseShapeInternal();
            return GetShape();
        }

        private void SetData(Point[] points)
        {
            resultingEdges.Clear();
            resultingVertices.Clear();
            this.points = points;
        }

        private void CalculateShape()
        {
            foreach (var point in points)
            {
                ProcessPoint(point);
            }
        }

        private void CloseShapeInternal()
        {
            if (CloseShape)
            {
                Dictionary<int, int> vertices = new Dictionary<int, int>();

                foreach (var edge in resultingEdges)
                {
                    CountVertexIndex(vertices, edge.Item1);
                    CountVertexIndex(vertices, edge.Item2);
                }

                var vertexIndices = vertices.Where(kvp => kvp.Value < 2).Select(kvp => kvp.Key).ToArray();
                foreach (var vertexIndex in vertexIndices)
                {
                    var nearestPendingVertex = GetNearestPendingVertex(vertexIndices, vertexIndex);
                    AddEdge(resultingVertices[vertexIndex], resultingVertices[nearestPendingVertex]);
                }
            }
        }

        private static void CountVertexIndex(Dictionary<int, int> vertices, int i)
        {
            if (!vertices.ContainsKey(i))
            {
                vertices.Add(i, 0);
            }
            vertices[i]++;
        }

        private int GetNearestPendingVertex(int[] vertices, int vertexIndex)
        {
            var vertexPoint = GetVertex(vertexIndex);
            var vertexIndicesWithDistance =
                vertices.Where(v => v != vertexIndex).Select(v => new {Index = v, Distance = resultingVertices[v].DistanceTo(vertexPoint)});
            return vertexIndicesWithDistance.Aggregate((a, b) => a.Distance < b.Distance ? a : b).Index;
        }

        private Point GetVertex(int vertexIndex)
        {
            return resultingVertices[vertexIndex];
        }

        private void ProcessPoint(Point point)
        {
            var nearbyPoints = NearbyPoints(point);
            foreach (var otherPoint in nearbyPoints.Where(p => p!=point))
            {
                Tuple<Point, Point> alphaDiskCenters = CalculateAlphaDiskCenters(point, otherPoint);
                if (NearbyPoints(alphaDiskCenters.Item1).Count(p => p != point && p != otherPoint) == 0
                    || NearbyPoints(alphaDiskCenters.Item2).Count(p => p != point && p != otherPoint) == 0)
                {
                    AddEdge(point, otherPoint);
                }
            }
        }

        private void AddEdge(Point p1, Point p2)
        {
            int indexP1;
            int indexP2;

            indexP1 = AddVertex(p1);
            indexP2 = AddVertex(p2);

            AddEdge(indexP1, indexP2);
        }

        private void AddEdge(int indexP1, int indexP2)
        {
            if (!resultingEdges.Contains(new Tuple<int, int>(indexP1, indexP2))
                && !resultingEdges.Contains(new Tuple<int, int>(indexP2, indexP1)))
                resultingEdges.Add(new Tuple<int, int>(indexP1, indexP2));
        }

        private int AddVertex(Point p)
        {
            int index;
            if (!resultingVertices.Contains(p))
            {
                resultingVertices.Add(p);
            }
            index = resultingVertices.IndexOf(p);
            return index;
        }

        private Point[] NearbyPoints(Point point)
        {
            var nearbyPoints = points.Where(p => p.DistanceTo(point) <= Radius).ToArray();
            return nearbyPoints;
        }

        private Tuple<Point, Point> CalculateAlphaDiskCenters(Point p1, Point p2)
        {
            double d = p1.DistanceTo(p2);
            double s = d;
            double r = Radius;
            double h = r - Math.Sqrt(r*r - s*s/4);

            Point center = new Point((p1.X+p2.X)/2, (p1.Y + p2.Y)/2);
            Point vector = new Point((p2.X-p1.X)/d, (p2.Y-p1.Y)/d);
            Point normalVector = new Point(vector.Y, -vector.X);

            return
                new Tuple<Point, Point>(
                    new Point(center.X + normalVector.X*(r - h), center.Y + normalVector.Y*(r - h)),
                    new Point(center.X - normalVector.X*(r - h), center.Y - normalVector.Y*(r - h)));
        }

        private Shape GetShape()
        {
            return new Shape(resultingVertices.ToArray(), resultingEdges.ToArray());
        }
    }
}

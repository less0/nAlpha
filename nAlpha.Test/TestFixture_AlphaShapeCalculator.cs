using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace nAlpha.Test
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    [TestFixture]
    public class TestFixture_AlphaShapeCalculator
    {
        [Test]
        public void CalculateAlphaShape()
        {
            AlphaShapeCalculator shapeCalculator = new AlphaShapeCalculator();
            shapeCalculator.Alpha = .1;
            var shape = shapeCalculator.CalculateShape(new Point[]
            {
                new Point(0, 0),
                new Point(.5, 0),
                new Point(1, 0),
                new Point(1, .5),
                new Point(1, 1),
                new Point(.5, 1),
                new Point(0, 1),
                new Point(0, .5),
                new Point(.5, .5),
            });
            Assert.That(shape.Vertices, Has.Length.EqualTo(8));
            Assert.That(shape.Edges, Has.No.Contains(new Point(.5, .5)));
        }

        [TestCase("0|0,1|1,0|1", 1, "0|0,1|1,0|1")]
        [TestCase("0.1|0.1,0.9|0.1,0.9|0.9,0.8|0.9,0.8|0.2,0.1|0.2", 1.25, "0.1|0.1,0.9|0.1,0.9|0.9,0.8|0.9,0.8|0.2,0.1|0.2")]
        public void CalculateAlphaShape_ContainsAllPoints(string inputPointsString, double alpha, string expectedResultingPointsString)
        {
            var inputPoints = ParsePoints(inputPointsString);
            var expectedResultingPoints = ParsePoints(expectedResultingPointsString);

            var shapeCalculator = new AlphaShapeCalculator();
            shapeCalculator.Alpha = alpha;

            var resultingShape = shapeCalculator.CalculateShape(inputPoints);
            
            Assert.That(resultingShape.Vertices, Is.EquivalentTo(expectedResultingPoints));
        }

        private Point[] ParsePoints(string stringRepresentation)
        {
            var singlePointStringRepresentations = stringRepresentation.Split(',');
            return singlePointStringRepresentations.Select(this.ParsePoint).ToArray();
        }

        private Point ParsePoint(string stringRepresentation)
        {
            if (Regex.IsMatch(stringRepresentation, "^[0-9]+(\\.[0-9]+)?|[0-9]+(\\.[0-9]+)?"))
            {
                var indexOfSeparator = stringRepresentation.IndexOf('|');
                var xCoordinate = double.Parse(stringRepresentation.Substring(0, indexOfSeparator), CultureInfo.InvariantCulture);
                var yCoordinate = double.Parse(stringRepresentation.Substring(indexOfSeparator + 1), CultureInfo.InvariantCulture);
                return new Point(xCoordinate, yCoordinate);
            }

            throw new Exception("Failed parsing the points");
        }
    }
}

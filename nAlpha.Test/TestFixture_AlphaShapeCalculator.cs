using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace nAlpha.Test
{
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
            Assert.That(shape.Edges, Has.No.Contains(new Point(.5,.5)));
        }
    }
}

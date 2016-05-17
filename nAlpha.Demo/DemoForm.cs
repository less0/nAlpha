using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nAlpha.Demo
{
    public partial class DemoForm : Form
    {
        private Point[] points;
        private Point[] vertices
            ;

        private Tuple<int, int>[] edges;

        public DemoForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            AlphaShapeCalculator shapeCalculator = new AlphaShapeCalculator();
            shapeCalculator.Alpha = (double) numericUpDownAlpha.Value / Width;
            shapeCalculator.CloseShape = checkBoxCloseShape.Checked;

            List<Point> points = new List<Point>();

            for (int i = 0; i < numericUpDown2.Value; i++)
            {
                points.Add(new Point(random.NextDouble()*(Width-100)+50, random.NextDouble()*(Height-100)+50));
            }

            var shape = shapeCalculator.CalculateShape(points.ToArray());

            this.points = points.ToArray();
            this.vertices = shape.Vertices;
            this.edges = shape.Edges;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var points = this.points;
            DrawPoints(graphics, Brushes.Black, points);
            DrawPoints(graphics, Brushes.CadetBlue, vertices);
            DrawEdges(graphics, Pens.DarkBlue, vertices, edges);
        }

        private void DrawEdges(Graphics graphics, Pen pen, Point[] vertices1, Tuple<int, int>[] edges)
        {
            if (edges != null)
            {
                foreach (var edge in edges)
                {
                    graphics.DrawLine(pen, (float) vertices1[edge.Item1].X, (float) vertices1[edge.Item1].Y,
                        (float) vertices1[edge.Item2].X, (float) vertices1[edge.Item2].Y);
                }
            }
        }

        private static void DrawPoints(Graphics graphics, Brush brush, Point[] points)
        {
            if (points != null)
            {
                foreach (var point in points)
                {
                    graphics.FillEllipse(brush, (float) (point.X - 5), (float) (point.Y - 5), 10, 10);
                }
            }
        }
    }
}

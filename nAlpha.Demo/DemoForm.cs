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

    private void Form1_Load( object sender, EventArgs e )
    {

    }

    private void buttonStart_Click( object sender, EventArgs e )
    {
      var shapeCalculator = GetAlphaShapeCalculator();
      var points = GetRandomPoints();
      var shape = shapeCalculator.CalculateShape( points.ToArray() );
      SaveResults( points, shape );

      Invalidate();
    }

    private void SaveResults( List<Point> points, Shape shape )
    {
      this.points = points.ToArray();
      this.vertices = shape.Vertices;
      this.edges = shape.Edges;
    }

    private AlphaShapeCalculator GetAlphaShapeCalculator()
    {
      AlphaShapeCalculator shapeCalculator = new AlphaShapeCalculator();
      shapeCalculator.Alpha = (double) numericUpDownAlpha.Value / Width;
      //shapeCalculator.CloseShape = checkBoxCloseShape.Checked;
      return shapeCalculator;
    }

    private List<Point> GetRandomPoints()
    {
      List<Point> points = new List<Point>();

#if RANDOM_POINTS
      Random random = new Random();

      for( int i = 0; i < numericUpDown2.Value; i++ )
      {
        points.Add( new Point( Math.Round( random.NextDouble(), 1 ) * ( Width - 100 ) + 50, Math.Round( random.NextDouble(), 1 ) * ( Height - 100 ) + 50 ) );
      }
#else
      int x = ( Width - 100 ) + 50;
      int y = ( Height - 100 ) + 50;


      points.Add( new Point( 0.1 * x, 0.1 * y ) );
      points.Add( new Point( 0.9 * x, 0.1 * y ) );
      points.Add( new Point( 0.9 * x, 0.9 * y ) );
      points.Add( new Point( 0.8 * x, 0.9 * y ) );
      points.Add( new Point( 0.8 * x, 0.2 * y ) );
      points.Add( new Point( 0.1 * x, 0.2 * y ) );
#endif // RANDOM_POINTS

      return points;
    }

    protected override void OnPaint( PaintEventArgs e )
    {
      base.OnPaint( e );
      var graphics = InitGraphics( e );
      DrawPoints( graphics, Brushes.Black, points );
      DrawPoints( graphics, Brushes.CadetBlue, vertices );
      DrawEdges( graphics, Pens.DarkBlue, vertices, edges );
    }

    private static Graphics InitGraphics( PaintEventArgs e )
    {
      var graphics = e.Graphics;
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      return graphics;
    }

    private void DrawEdges( Graphics graphics, Pen pen, Point[] vertices1, Tuple<int, int>[] edges )
    {
      if( edges != null )
      {
        foreach( var edge in edges )
        {
          graphics.DrawLine( pen, (float) vertices1[edge.Item1].X, (float) vertices1[edge.Item1].Y,
              (float) vertices1[edge.Item2].X, (float) vertices1[edge.Item2].Y );
        }
      }
    }

    private static void DrawPoints( Graphics graphics, Brush brush, Point[] points )
    {
      if( points != null )
      {
        foreach( var point in points )
        {
          graphics.FillEllipse( brush, (float) ( point.X - 5 ), (float) ( point.Y - 5 ), 10, 10 );
        }
      }
    }
  }
}

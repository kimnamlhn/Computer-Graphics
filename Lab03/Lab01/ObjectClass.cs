using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;


namespace Lab01
{
    //các đối tượng hình vẽ
    class Object
    {
        //màu đường thẳng tạo thành hình
        public Color LineColor { get; set; } = new Color();

        //Điểm chặn trên trái, dưới phải
        protected Point p1 = new Point();
        protected Point p2 = new Point();

        public virtual void set(Point A, Point B)
        {
            p1.setPoint(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y));
            p2.setPoint(Math.Max(A.X, B.X), Math.Max(A.X, B.X));
        }
        public virtual void Draw(OpenGL gl) { }
    }

    //điểm
    class Point
    {
        public int X { get; set; }  //Tọa độ x
        public int Y { get; set; }  //Tọa độ y
        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
        public void setPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void setPoint(Point c)
        {
            X = c.X;
            Y = c.Y;
        }
        public int getX() { return X; }
        public int getY() { return Y; }
    }

    //hình tròn
    class Circle : Ellipse
    {
        private void DrawPoint(OpenGL gl, int u, int v)
        {

            gl.Vertex2sv(new short[] { (short)(u), (short)(v) });

        }


        // vẽ hình tròn
        public override void Draw(OpenGL gl)
        {
            gl.Color(LineColor.R, LineColor.G, LineColor.B);
            gl.Begin(OpenGL.GL_POINTS);
            int u, v;

            // vẽ 4 điểm tạo thành 4 cái một phần tư của hình tròn
            int[][] quarter = {
                                new int[] { 1, 1 },
                                new int[] { -1, 1 },
                                new int[] { 1, -1 },
                                new int[] { -1, -1 }
                               };

            // Tính các thông số 
            int cx = (p1.X + p2.X) / 2,
                cy = (p1.Y + p2.Y) / 2,
                rx = p2.X - cx,
                ry = rx,
                x = 0, y = ry,
                rx2 = rx * rx,
                ry2 = ry * ry,
                x2 = 2 * ry2 * x,
                y2 = 2 * rx2 * y;
            double p = (ry2 - rx2 * ry) + rx2 / 4.0;

            //Vẽ 
            while (x2 < y2)
            {
                foreach (int[] i in quarter)
                {
                    u = x * i[0] + cx;
                    v = y * i[1] + cy;
                    DrawPoint(gl, u, v);
                }

                x++;
                x2 += (ry2 * 2);
                if (p < 0)
                {
                    p += (x2 + ry2);
                }
                else
                {
                    y--;
                    y2 -= (rx2 * 2);
                    p += (x2 - y2 + ry2);
                }
            }

            p = (double)ry2 * (x + 0.5) * (x + 0.5) + (double)rx2 * (y - 1) * (y - 1) - (double)rx2 * ry2;
            while (y > 0)
            {
                foreach (int[] i in quarter)
                {
                    u = x * i[0] + cx;
                    v = y * i[1] + cy;
                    DrawPoint(gl, u, v);
                }

                y--;
                y2 -= (rx2 * 2);
                if (p > 0)
                {
                    p += (rx2 - y2);
                }
                else
                {
                    x++;
                    x2 += (ry2 * 2);
                    p += (x2 - y2 + rx2);
                }
            }
            foreach (int[] i in quarter)
            {
                u = x * i[0] + cx;
                v = y * i[1] + cy;
                DrawPoint(gl, u, v);
            }
            gl.End();
        }

    }

    //màu sắc
    class Color
    {
        public float R { get; set; }    //Kênh màu red
        public float G { get; set; }    //Kênh màu green
        public float B { get; set; }    //Kênh màu blue

        public Color(float r = 0, float g = 0, float b = 0)
        {
            R = r;
            G = g;
            B = b;
        }
        public Color(Color c)
        {
            R = c.R;
            G = c.G;
            B = c.B;
        }

        public float getR() { return R; }
        public float getG() { return G; }
        public float getB() { return B; }
    }

    //elipse
    class Ellipse : Shape
    {
        public override void set(Point A, Point B)
        {
            p1.setPoint(A.X < B.X ? A.X : B.X, A.Y < B.Y ? A.Y : B.Y);
            p2.setPoint(A.X > B.X ? A.X : B.X, A.Y > B.Y ? A.Y : B.Y);
        }


        private void DrawPoint(OpenGL gl, int u, int v)
        {

            gl.Vertex2sv(new short[] { (short)(u), (short)(v) });

        }

        public override void Draw(OpenGL gl)
        {
            int cx = (p1.X + p2.X) / 2,
                 cy = (p1.Y + p2.Y) / 2;
            gl.Color(LineColor.R, LineColor.G, LineColor.B);
            gl.PushMatrix();
            gl.Translate(cx, cy, 0.0);
            gl.Begin(OpenGL.GL_POINTS);
            int u, v;

            // vẽ 4 điểm tạo thành 4 cái một phần tư
            int[][] quarter = {
                                new int[] { 1, 1 },
                                new int[] { -1, 1 },
                                new int[] { 1, -1 },
                                new int[] { -1, -1 }
                               };

            // Tính các thông số
            int rx = p2.X - cx,
            ry = p2.Y - cy,
            x = 0, y = ry,
            rx2 = rx * rx,
            ry2 = ry * ry,
            x2 = 2 * ry2 * x,
            y2 = 2 * rx2 * y;
            double p = (ry2 - rx2 * ry) + rx2 / 4.0;
            cx = cy = 0;

            //Vẽ
            while (x2 < y2)
            {
                foreach (int[] i in quarter)
                {
                    u = x * i[0] + cx;
                    v = y * i[1] + cy;
                    DrawPoint(gl, u, v);
                }

                x++;
                x2 += (ry2 * 2);
                if (p < 0)
                {
                    p += (x2 + ry2);
                }
                else
                {
                    y--;
                    y2 -= (rx2 * 2);
                    p += (x2 - y2 + ry2);
                }
            }

            p = (double)ry2 * (x + 0.5) * (x + 0.5) + (double)rx2 * (y - 1) * (y - 1) - (double)rx2 * ry2;
            while (y > 0)
            {
                foreach (int[] i in quarter)
                {
                    u = x * i[0] + cx;
                    v = y * i[1] + cy;
                    DrawPoint(gl, u, v);
                }

                y--;
                y2 -= (rx2 * 2);
                if (p > 0)
                {
                    p += (rx2 - y2);
                }
                else
                {
                    x++;
                    x2 += (ry2 * 2);
                    p += (x2 - y2 + rx2);
                }
            }
            foreach (int[] i in quarter)
            {
                u = x * i[0] + cx;
                v = y * i[1] + cy;
                DrawPoint(gl, u, v);
            }
            gl.End();
            gl.PopMatrix();
        }

    }

    //lục giác
    class Hexagon : Polygon
    {
        public Hexagon()
        {
            nPoly = 6;
            init();
        }
        public override void set(Point pi, Point pj)
        {
            base.set(pi, pj);
            p2r.Y = p2.Y = p1.Y + (int)Math.Round((p2.X - p1.X) * Math.Sqrt(3) / 2.0);
            int rx = Math.Abs(p1.X - p2.X) / 2, ry = Math.Abs(p1.Y - p2.Y) / 2;
            int dx = rx / 2;

            nPoints[0].setPoint(-dx, ry);
            nPoints[1].setPoint(dx, ry);
            nPoints[2].setPoint(rx, 0);
            nPoints[3].setPoint(dx, -ry);
            nPoints[4].setPoint(-dx, -ry);
            nPoints[5].setPoint(-rx, 0);

        }
    }

    //đường thẳng
    class Line : Object
    {
        public Line() { }
        public Line(Point s, Point e)
        {
            p1.setPoint(s);
            p2.setPoint(e);
        }
        public override void set(Point A, Point B)
        {
            p1.setPoint(A.X, A.Y);
            p2.setPoint(B.X, B.Y);
        }
        public Point getP1()
        {
            return p1;
        }
        public Point getP2()
        {
            return p2;
        }


        public override void Draw(OpenGL gl)
        {
            gl.Color(LineColor.R, LineColor.G, LineColor.B);

            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex2sv(new short[] { (short)p1.X, (short)p1.Y });
            gl.Vertex2sv(new short[] { (short)p2.X, (short)p2.Y });
            gl.End();
        }
    }

    //hình n đỉnh 
    class MultiP_Poly : Polygon
    {
        List<Point> vertexes = new List<Point>();

        public void addVertex(Point p)
        {
            vertexes.Add(p);
            nPoly++;
        }

        public void moveVertex(Point p)
        {
            if (nPoly > 0)
                vertexes[nPoly - 1].setPoint(p);
        }

        public override void Draw(OpenGL gl)
        {
            if (nPoly <= 1) return;
            Line li = new Line();
            li.LineColor = LineColor;

            Point start = new Point(), end = new Point();
            start.setPoint(vertexes[nPoly - 1]);
            for (int i = 0; i < nPoly; i++)
            {
                end.setPoint(vertexes[i]);
                li.set(start, end);
                li.Draw(gl);
                start.setPoint(end);
            }
        }

        public Polygon getPolygon()
        {
            nPoly--;
            if (nPoly < 2) return null;

            Polygon p = new Polygon();
            p.nPoly = nPoly;
            p.nPoints = new Point[nPoly];
            p.LineColor = LineColor;


            int x1 = vertexes[0].X, x2 = vertexes[0].X, y1 = vertexes[0].Y, y2 = vertexes[0].Y;
            for (int i = 1; i < nPoly; i++)
            {
                if (vertexes[i].X < x1) x1 = vertexes[i].X;
                if (vertexes[i].X > x2) x2 = vertexes[i].X;

                if (vertexes[i].Y < y1) y1 = vertexes[i].Y;
                if (vertexes[i].Y > y2) y2 = vertexes[i].Y;
            }
            p.set(new Point(x1, y1), new Point(x2, y2));
            int xc = (x1 + x2) / 2,
                yc = (y1 + y2) / 2;
            for (int i = 0; i < nPoly; i++)
            {
                p.nPoints[i] = new Point(vertexes[i].X - xc, vertexes[i].Y - yc);
            }

            return p;
        }
    }

    //ngũ giác
    class Pentagon : Polygon
    {
        public Pentagon()
        {
            nPoly = 5;
            init();
        }

        public override void set(Point A, Point B)
        {
            base.set(A, B);
            p2r.Y = p2.Y = p1.Y + (p2.X - p1.X);
            int rx = Math.Abs(p1.X - p2.X) / 2, ry = Math.Abs(p1.Y - p2.Y) / 2;

            int x18 = (int)Math.Round(rx * Math.Cos(18 * Math.PI / 180)),
                x54 = (int)Math.Round(rx * Math.Cos(54 * Math.PI / 180)),
                y18 = (int)Math.Round(ry * Math.Sin(18 * Math.PI / 180)),
                y54 = (int)Math.Round(ry * Math.Sin(54 * Math.PI / 180));
            nPoints[0].setPoint(0, ry);
            nPoints[1].setPoint(x18, y18);
            nPoints[2].setPoint(x54, -y54);
            nPoints[3].setPoint(-x54, -y54);
            nPoints[4].setPoint(-x18, y18);
        }
    }

    //đa giác
    class Polygon : Shape
    {
        public int nPoly = 0;
        public Point[] nPoints;
        protected Point p1r = new Point(), p2r = new Point();
        protected void init()
        {
            nPoints = new Point[nPoly];
            for (int i = 0; i < nPoly; i++)
                nPoints[i] = new Point();
        }
        public override void set(Point A, Point B)
        {
            base.set(A, B);
            p1r.setPoint(p1);
            p2r.setPoint(p2);
        }


        public override void Draw(OpenGL gl)
        {
            Line li = new Line();
            li.LineColor = LineColor;
            int cx = (p1.X + p2.X) / 2, cy = (p1.Y + p2.Y) / 2;
            gl.PushMatrix();
            gl.Translate(cx, cy, 0.0);
            gl.Scale((double)(p2.X - p1.X) / (p2r.X - p1r.X), (double)(p2.Y - p1.Y) / (p2r.Y - p1r.Y), 0.0);

            Point start = new Point(), end = new Point();

            start.setPoint(nPoints[nPoly - 1]);
            for (int i = 0; i < nPoly; i++)
            {
                end.setPoint(nPoints[i]);
                li.set(start, end);
                li.Draw(gl);
                start.setPoint(end);
            }
            gl.PopMatrix();
        }


    }

    //hình chữ nhật
    class Rectangle : Polygon
    {
        public Rectangle()
        {
            nPoly = 4;
            init();
        }

        public override void set(Point A, Point B)
        {
            base.set(A, B);
            int rx = Math.Abs(p1.X - p2.X) / 2, ry = Math.Abs(p1.Y - p2.Y) / 2;

            //Tọa độ 4 đỉnh
            nPoints[0].setPoint(-rx, -ry);
            nPoints[1].setPoint(rx, -ry);
            nPoints[2].setPoint(rx, ry);
            nPoints[3].setPoint(-rx, ry);
        }
    }

    // tam giác
    class Triangle : Polygon
    {
        public Triangle()
        {
            nPoly = 3;
            init();
        }

        public override void set(Point A, Point B)
        {
            base.set(A, B);


            p2r.Y = p2.Y = p1.Y + (int)Math.Round((p2.X - p1.X) * Math.Sqrt(3) / 2.0);

            int rx = Math.Abs(p1.X - p2.X) / 2, ry = Math.Abs(p1.Y - p2.Y) / 2;

            //Tọa độ 3 đỉnh            
            nPoints[0].setPoint(0, ry);
            nPoints[1].setPoint(-rx, -ry);
            nPoints[2].setPoint(rx, -ry);
        }
    }

    //dạng
    class Shape : Object
    {

        public override void set(Point pi, Point pj)
        {
            p1.setPoint(pi.X < pj.X ? pi.X : pj.X, pi.Y < pj.Y ? pi.Y : pj.Y);
            p2.setPoint(pi.X > pj.X ? pi.X : pj.X, pi.Y > pj.Y ? pi.Y : pj.Y);
        }

    }



}





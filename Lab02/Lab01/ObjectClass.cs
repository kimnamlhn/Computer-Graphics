using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;


namespace Lab01
{

    //các đối tượng
    class Object
    {
        public Color LineColor { get; set; } = new Color();
        public Color FillColor { get; set; } = new Color();
        public double Angle { get; set; } = 0.0;
        //Độ dày viền
        public int LineWidth { get; set; } = 1;
        //Điểm chặn trên trái
        protected Point p1 = new Point();
        //Điểm chặn dưới phải
        protected Point p2 = new Point();
        /*p1-------
         * 
         * 
         * -------p2
         */
        public virtual void set(Point A, Point B)
        {
            p1.setPoint(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y));
            p2.setPoint(Math.Max(A.X, B.X), Math.Max(A.X, B.X));
        }
        public virtual void Draw(OpenGL gl) { }
        //Xác định hình có được chọn không
        public virtual bool isInsideBox(int x, int y) { return false; }
        //Xác định điểm điều khiển được chọn
        public virtual int getControlPointId(int x, int y) { return -1; }
        //Tịnh tiến hình
        public virtual void translate(Point s, Point e) { }
        //Co giản hình qua điểm điều khiển
        public virtual void dragControlPoint(int CPid, Point s, Point e) { }
        //Vẽ các điểm điều khiển
        public virtual void drawControlBox(OpenGL gl) { }
        //Tô màu, nếu mode là true thì flood fill ngược lại là scanline fill
        public virtual void Fill(OpenGL gl, bool mode) { }
    }

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

    class Circle : Ellipse
    {


        //Vẽ điểm ảnh với nét vẽ dày tùy chọn
        private void DrawPoint(OpenGL gl, int u, int v)
        {
            for (int i = 0; i < LineWidth; i++)
                for (int j = 0; j < LineWidth; j++)
                    gl.Vertex2sv(new short[] { (short)(u + i), (short)(v + j) });

        }

        public override void Draw(OpenGL gl)
        {
            gl.Color(LineColor.R, LineColor.G, LineColor.B);
            gl.Begin(OpenGL.GL_POINTS);

            int u, v;

            int[][] quarter = {     // vẽ 4 điểm ứng với 4 phần tư của hình
                                new int[] { 1, 1 },
                                new int[] { -1, 1 },
                                new int[] { 1, -1 },
                                new int[] { -1, -1 }
                               };

            // Tính các thông số cơ bản
            int cx = (p1.X + p2.X) / 2,
                cy = (p1.Y + p2.Y) / 2,     // Tọa độ tâm
                rx = p2.X - cx,
                ry = rx,             // 2 bán kính
                x = 0, y = ry,              // tọa độ điểm bắt đầu vẽ
                rx2 = rx * rx,
                ry2 = ry * ry,              // rx^2, ry^2 
                x2 = 2 * ry2 * x,           // 2ry^2 * x(k+1)
                y2 = 2 * rx2 * y;           // 2rx^2 * y(k+1)
            double p = (ry2 - rx2 * ry) + rx2 / 4.0;// p1o

            //vùng 1
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

            // vùng 2
            // tính lại p từ xlast, ylast
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
        //public override void Fill(OpenGL gl, bool mode)
        //{

        //}
    }

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
        public void setColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static bool operator ==(Color color1, Color color2)
        {
            float e = 1.0f / 10000000.0f;
            return (Math.Abs(color1.R - color2.R) < e && Math.Abs(color1.G - color2.G) < e && Math.Abs(color1.B - color2.B) < e);
        }

        public static bool operator !=(Color color1, Color color2)
        {
            return !(color1 == color2);
        }
        public float getR() { return R; }
        public float getG() { return G; }
        public float getB() { return B; }
    }

    class Ellipse : Shape
    {
        public override void set(Point A, Point B)
        {
            p1.setPoint(A.X < B.X ? A.X : B.X, A.Y < B.Y ? A.Y : B.Y);
            p2.setPoint(A.X > B.X ? A.X : B.X, A.Y > B.Y ? A.Y : B.Y);
        }


        //Vẽ điểm ảnh với nét vẽ dày tùy chọn
        private void DrawPoint(OpenGL gl, int u, int v)
        {
            for (int i = 0; i < LineWidth; i++)
                for (int j = 0; j < LineWidth; j++)
                    gl.Vertex2sv(new short[] { (short)(u + i), (short)(v + j) });

        }

        public override void Draw(OpenGL gl)
        {
            int cx = (p1.X + p2.X) / 2,
                 cy = (p1.Y + p2.Y) / 2;
            gl.Color(LineColor.R, LineColor.G, LineColor.B);
            gl.PushMatrix();
            gl.Translate(cx, cy, 0.0);
            gl.Rotate(Angle, 0.0, 0.0, 1.0);
            gl.Begin(OpenGL.GL_POINTS);
            int u, v;

            int[][] quarter = {     // vẽ 4 điểm ứng với 4 phần tư của hình
                                new int[] { 1, 1 },
                                new int[] { -1, 1 },
                                new int[] { 1, -1 },
                                new int[] { -1, -1 }
                               };

            // Tính các thông số cơ bản
            int rx = p2.X - cx,
            ry = p2.Y - cy,             // 2 bán kính
            x = 0, y = ry,              // tọa độ điểm bắt đầu vẽ
            rx2 = rx * rx,
            ry2 = ry * ry,              // rx^2, ry^2 
            x2 = 2 * ry2 * x,           // 2ry^2 * x(k+1)
            y2 = 2 * rx2 * y;           // 2rx^2 * y(k+1)
            double p = (ry2 - rx2 * ry) + rx2 / 4.0;// p1o
            cx = cy = 0;
            //vùng 1
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

            // vùng 2
            // tính lại p từ xlast, ylast
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
        //public override void Fill(OpenGL gl, bool mode)
        //{
        //    int cx = (p1.X + p2.X) / 2,
        //       cy = (p1.Y + p2.Y) / 2,     // Tọa độ tâm
        //       rx = p2.X - cx,
        //       ry = p2.Y - cy,             // 2 bán kính
        //       x = 0, y = ry,              // tọa độ điểm bắt đầu vẽ
        //       rx2 = rx * rx,
        //       ry2 = ry * ry,              // rx^2, ry^2 
        //       x2 = 2 * ry2 * x,           // 2ry^2 * x(k+1)
        //       y2 = 2 * rx2 * y;           // 2rx^2 * y(k+1)
        //    double p = (ry2 - rx2 * ry) + rx2 / 4.0;// p1o
        //    gl.Color(LineColor.R, LineColor.G, LineColor.B);
        //    gl.LineWidth(LineWidth);
        //    gl.Begin(OpenGL.GL_LINES);

        //    int u, v;

        //    int[][] quarter = {     // vẽ 4 điểm ứng với 4 phần tư của hình
        //                        new int[] { 1, 1 },
        //                        //new int[] { -1, 1 },
        //                        new int[] { 1, -1 },
        //                        //new int[] { -1, -1 }
        //                       };

        //    // Tính các thông số cơ bản


        //    //vùng 1
        //    while (x2 < y2)
        //    {
        //        foreach (int[] i in quarter)
        //        {
        //            u = x * i[0] + cx;
        //            v = y * i[1] + cy;
        //            gl.Vertex2sv(new short[] { (short)u, (short)v });
        //            gl.Vertex2sv(new short[] { (short)(u-2*Math.Abs(cx-u)), (short)(v) });
        //        }

        //        x++;
        //        x2 += (ry2 * 2);
        //        if (p < 0)
        //        {
        //            p += (x2 + ry2);
        //        }
        //        else
        //        {
        //            y--;
        //            y2 -= (rx2 * 2);
        //            p += (x2 - y2 + ry2);
        //        }
        //    }

        //    // vùng 2
        //    // tính lại p từ xlast, ylast
        //    p = (double)ry2 * (x + 0.5) * (x + 0.5) + (double)rx2 * (y - 1) * (y - 1) - (double)rx2 * ry2;
        //    while (y > 0)
        //    {
        //        foreach (int[] i in quarter)
        //        {
        //            u = x * i[0] + cx;
        //            v = y * i[1] + cy;
        //            gl.Vertex2sv(new short[] { (short)u, (short)v });
        //            gl.Vertex2sv(new short[] { (short)(u - 2*Math.Abs(cx - u)), (short)(v) });
        //        }

        //        y--;
        //        y2 -= (rx2 * 2);
        //        if (p > 0)
        //        {
        //            p += (rx2 - y2);
        //        }
        //        else
        //        {
        //            x++;
        //            x2 += (ry2 * 2);
        //            p += (x2 - y2 + rx2);
        //        }
        //    }
        //    gl.Vertex2sv(new short[] { (short)p1.X, (short)cy });
        //    gl.Vertex2sv(new short[] { (short)(p2.X), (short)cy });

        //    gl.End();
        //}
    }

    class Hexagon : Polygon
    {
        public Hexagon()
        {
            nPoly = 6;
            init();
            //equilateral = true;
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
        public override bool isInsideBox(int x, int y)
        {
            if (Math.Abs(y - p1.Y) <= 2.0 && Math.Abs(p2.Y - p1.Y) <= 2.0)
                return (x >= p1.X && x <= p2.X) || (x < p1.X && x > p2.X);
            if (Math.Abs(x - p1.X) <= 2.0 && Math.Abs(p2.X - p1.X) <= 2.0)
                return (y >= p1.Y && y <= p2.Y) || (y < p1.Y && y > p2.Y);

            double a = (double)(x - p1.X) / (y - p1.Y), b = (double)(p2.X - p1.X) / (p2.Y - p1.Y);
            return (Math.Abs(a - b) < 0.5);
        }

        public override void translate(Point s, Point e)
        {
            //Vector tịnh tiến
            int vx = e.X - s.X,
                vy = e.Y - s.Y;

            //Tịnh tiến các điểm điều khiển
            p1.X += vx;
            p2.X += vx;
            p1.Y += vy;
            p2.Y += vy;
        }

        public override int getControlPointId(int x, int y)
        {
            //Tọa độ các điểm điều khiển
            int[][] cp = new int[][] {
                new int[]{p1.X, p1.Y},
                 new int[]{ p2.X, p2.Y }
            };

            //Xác định điểm được chọn
            for (int i = 0; i < 2; i++)
            {
                if (x >= cp[i][0] - 5 && x <= cp[i][0] + 5 && y >= cp[i][1] - 5 && y <= cp[i][1] + 5)
                    return i;
            }
            return -1;
        }

        public override void dragControlPoint(int CPid, Point s, Point e)
        {
            //Vector tịnh tiến
            int vx = e.X - s.X,
                vy = e.Y - s.Y;

            int x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
            Point pu = new Point(), pv = new Point();
            pu.setPoint(p1); pv.setPoint(p2);

            //Co giãn hình
            if (CPid == 0)
                pu.setPoint(x1 + vx, y1 + vy);
            else
                pv.setPoint(x2 + vx, y2 + vy);

            p1.setPoint(pu);
            p2.setPoint(pv);
        }

        public override void drawControlBox(OpenGL gl)
        {
            Rectangle box = new Rectangle();
            Color c = new Color(0.5f, 0.5f, 0.5f);
            box.LineColor = c;

            //Tọa độ các điểm điều khiển
            int[][] cp = new int[][] {
                new int[]{p1.X, p1.Y},
                 new int[]{ p2.X, p2.Y }
            };

            //Vẽ các điểm điều khiển
            foreach (int[] p in cp)
            {
                box.set(new Point(p[0] - 3, p[1] - 3), new Point(p[0] + 3, p[1] + 3));
                box.Draw(gl);
                gl.Flush();
            }
        }

        public override void Draw(OpenGL gl)
        {
            gl.Color(LineColor.R, LineColor.G, LineColor.B);
            gl.LineWidth(LineWidth);

            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex2sv(new short[] { (short)p1.X, (short)p1.Y });
            gl.Vertex2sv(new short[] { (short)p2.X, (short)p2.Y });
            gl.End();
        }
    }

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
            li.LineWidth = LineWidth;

            //Vẽ từng cạnh bằng các nối lần lược các đỉnh
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
            p.LineWidth = LineWidth;
            p.FillColor = FillColor;

            // tìm xmin, ymin, xmax, ymax để vẽ khung
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

    class Pentagon : Polygon
    {
        public Pentagon()
        {
            nPoly = 5;
            init();
        }

        public override void set(Point A, Point B)
        {

            //Point Min = new Point(A.X < B.X ? A.X : B.X, A.Y < B.Y ? A.Y : B.Y),
            //      Max1 = new Point(A.X > B.X ? A.X : B.X, A.Y > B.Y ? A.Y : B.Y);
            //int d = Max1.X - Min.X;
            //Point Max = new Point(Max1.X, Min.Y + d);


            ////Min chính là A bên dưới, Max chính là B bên dưới
            ///* 
            // *   C - 0 - B  
            // *   |/     \|
            // *   1       4
            // *   |\     /|
            // *   A-2 - 3-D
            // * 
            // */

            //nPoints[0].setPoint((Max.X + Min.X) / 2, Max.Y);
            ////Khoảng cách từ 0 -> C
            //int d0C = nPoints[0].X - Min.X;
            ////Khoảng cách từ C -> 1
            //int dC1 = (int)Math.Round(d0C / Math.Tan(54 * Math.PI / 180));
            //nPoints[1].setPoint(Min.X, Max.Y - dC1);
            //nPoints[4].setPoint(Max.X, Max.Y - dC1);
            ////Khoảng cách từ 1 -> A
            //int d1A = nPoints[1].Y - Min.Y;
            ////Khoảng cách từ A -> 2 = khoảng cách từ 3 -> D
            //int dA2 = (int)Math.Round(d1A / Math.Tan(72 * Math.PI / 180));
            //nPoints[2].setPoint(Min.X + dA2, Min.Y);
            //nPoints[3].setPoint(Max.X - dA2, Min.Y);
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
            li.LineWidth = LineWidth;
            li.LineColor = LineColor;
            int cx = (p1.X + p2.X) / 2, cy = (p1.Y + p2.Y) / 2;
            gl.PushMatrix();
            gl.Translate(cx, cy, 0.0);
            gl.Rotate(Angle, 0.0, 0.0, 1.0);
            gl.Scale((double)(p2.X - p1.X) / (p2r.X - p1r.X), (double)(p2.Y - p1.Y) / (p2r.Y - p1r.Y), 0.0);

            //Vẽ từng cạnh bằng các nối lần lược các đỉnh
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
        public override void Fill(OpenGL gl, bool mode)
        {
            if (mode)
            {
                base.Fill(gl, mode);
            }
            else //scanline
            {
                if (nPoly < 3) return;
                ScanFill fillPolygon = new ScanFill();
                List<Point> p = new List<Point>();
                for (int i = 0; i < nPoly; i++)
                {
                    p.Add(nPoints[i]);
                }
                int cx = (p1.X + p2.X) / 2, cy = (p1.Y + p2.Y) / 2;
                gl.PushMatrix();
                gl.Translate(cx, cy, 0.0);
                gl.Rotate(Angle, 0.0, 0.0, 1.0);
                gl.Scale((double)(p2.X - p1.X) / (p2r.X - p1r.X), (double)(p2.Y - p1.Y) / (p2r.Y - p1r.Y), 0.0);
                gl.Color(FillColor.getR(), FillColor.getG(), FillColor.getB());
                fillPolygon.setFill(p);
                fillPolygon.initEdges();
                fillPolygon.scanlineFill(gl);
                gl.PopMatrix();
            }
        }

    }

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

            //Tọa độ các đỉnh
            nPoints[0].setPoint(-rx, -ry);
            nPoints[1].setPoint(rx, -ry);
            nPoints[2].setPoint(rx, ry);
            nPoints[3].setPoint(-rx, ry);
        }
    }

    class ScanFill
    {
        //private Polygon polygon;
        private List<Point> ListOfIntersectPoints; //List giao điểm giữa đường quét và các cạnh
        private List<Line> ListOfEdges; //List các cạnh của đa giác
        private List<Point> polygon; //List các điểm của đa giác
        private int point_ymin; //startPoint Scanline
        private int point_ymax; //end point scaline
        public void setFill(List<Point> po)
        {
            this.polygon = po;
            ListOfIntersectPoints = new List<Point>();
            ListOfEdges = new List<Line>();
            point_ymin = 2000;
            point_ymax = 0;
        }

        public bool findIntersectGLPoint(int x1, int y1, int x2, int y2, int y, ref int x) //Tìm giao điểm của dòng quét y và cạnh
        {
            if (y2 == y1)
                return false;
            x = (x2 - x1) * (y - y1) / (y2 - y1) + x1;
            bool isInsideEdgeX;
            bool isInsideEdgeY;
            if (x1 < x2)
                isInsideEdgeX = (x1 <= x) && (x <= x2);
            else
                isInsideEdgeX = (x2 <= x) && (x <= x1);

            if (y1 < y2)
                isInsideEdgeY = (y1 <= y) && (y <= y2);
            else
                isInsideEdgeY = (y2 <= y) && (y <= y1);
            return isInsideEdgeX && isInsideEdgeY;
        }

        public void initEdges()
        {
            if (polygon.Count() > 2)
            {
                //foreach(Point p in polygon)
                //{
                //    if (p)
                //}

                for (int a = 1; a < polygon.Count(); a++) //Tìm point_ymin và point_ymax; Xây dựng list cạnh
                {
                    if (polygon[a - 1].getY() < point_ymin)
                        point_ymin = polygon[a - 1].getY();
                    if (polygon[a - 1].getY() > point_ymax)
                        point_ymax = polygon[a - 1].getY();
                    Line current = new Line(polygon[a - 1], polygon[a]);
                    ListOfEdges.Add(current);
                }
                int i = polygon.Count() - 1;
                if (polygon[i].getY() > point_ymax)
                    point_ymax = polygon[i].getY();
                if (polygon[i].getY() < point_ymin)
                    point_ymin = polygon[i].getY();
                Line last = new Line(polygon[i], polygon[0]);
                ListOfEdges.Add(last);
            }
        }

        public void scanlineFill(OpenGL gl)
        {
            int edgesSize = ListOfEdges.Count();
            for (int i = point_ymin; i <= point_ymax; i++) //Duyệt từng dòng quét
            {
                int intersectX = 0;
                for (int j = 0; j < edgesSize; j++) //Xây dựng danh sách các giao điểm
                {
                    if (findIntersectGLPoint(ListOfEdges[j].getP1().getX(), ListOfEdges[j].getP1().getY(), ListOfEdges[j].getP2().getX(), ListOfEdges[j].getP2().getY(), i, ref intersectX))
                    {
                        Point p = new Point(intersectX, i);
                        if (ListOfEdges[j].getP1().getY() > ListOfEdges[j].getP2().getY())
                        {
                            if (p.getY() == ListOfEdges[j].getP1().getY())
                                continue;
                        }
                        else
                            if (ListOfEdges[j].getP1().getY() < ListOfEdges[j].getP2().getY())
                        {
                            if (p.getY() == ListOfEdges[j].getP2().getY())
                                continue;
                        }
                        ListOfIntersectPoints.Add(p);
                    }
                }
                int intersectSize = ListOfIntersectPoints.Count();
                Point swap = new Point(0, 0);
                for (int a = 0; a < intersectSize - 1; a++)//Sắp xếp các giao điểm lại theo tọa độ x
                    for (int j = i + 1; j < intersectSize; j++)
                    {
                        if (ListOfIntersectPoints[a].getX() > ListOfIntersectPoints[a].getX())
                        {
                            swap = ListOfIntersectPoints[a];
                            ListOfIntersectPoints[a] = ListOfIntersectPoints[j];
                            ListOfIntersectPoints[j] = swap;
                        }
                    }

                int intersectPointsSize = ListOfIntersectPoints.Count();
                for (int j = 1; j < intersectPointsSize; j += 2) //Tô màu
                {

                    gl.Begin(OpenGL.GL_LINES);
                    gl.LineWidth(1);
                    gl.Vertex2sv(new short[] { (short)(ListOfIntersectPoints[j - 1].getX()), (short)ListOfIntersectPoints[j - 1].getY() });
                    gl.Vertex2sv(new short[] { (short)(ListOfIntersectPoints[j].getX()), (short)ListOfIntersectPoints[j].getY() });
                    gl.End();

                }
                ListOfIntersectPoints.Clear();
            }
        }
    }

    class Shape : Object
    {
        //Góc xoay hình
        public double Angle { get; set; } = 0.0;

        public override void set(Point pi, Point pj)
        {
            p1.setPoint(pi.X < pj.X ? pi.X : pj.X, pi.Y < pj.Y ? pi.Y : pj.Y);
            p2.setPoint(pi.X > pj.X ? pi.X : pj.X, pi.Y > pj.Y ? pi.Y : pj.Y);
        }

        public override bool isInsideBox(int x, int y)
        {
            //center
            int cx = (p1.X + p2.X) / 2, cy = (p1.Y + p2.Y) / 2;
            // vì khi vẽ mình tịnh tiến gốc tọa độ từ (0, 0) đến (cx, cy)
            x -= cx;
            y -= cy;
            // Áp dụng công thức trên lớp
            int u = (int)Math.Round(x * Math.Cos(-Angle * Math.PI / 180) - y * Math.Sin(-Angle * Math.PI / 180)),
                v = (int)Math.Round(y * Math.Cos(-Angle * Math.PI / 180) + x * Math.Sin(-Angle * Math.PI / 180));
            x = u; y = v;
            return !(x < p1.X - cx || x > p2.X - cx || y < p1.Y - cy || y > p2.Y - cy);
        }

        public override int getControlPointId(int x, int y)
        {
            int cx = 0, cy = 0;
            if (Angle != 0)
            {
                cx = (p1.X + p2.X) / 2; cy = (p1.Y + p2.Y) / 2;
                x -= cx;
                y -= cy;
                // áp dụng công thức
                int u = (int)Math.Round(x * Math.Cos(-Angle * Math.PI / 180) - y * Math.Sin(-Angle * Math.PI / 180)),
                    v = (int)Math.Round(y * Math.Cos(-Angle * Math.PI / 180) + x * Math.Sin(-Angle * Math.PI / 180));
                x = u; y = v;
            }

            int x1 = p1.X - cx - 5,
                y1 = p1.Y - cy - 5,
                x2 = p2.X - cx + 5,
                y2 = p2.Y - cy + 5;
            int xm = cx == 0 ? (p1.X + p2.X) / 2 : 0,
                ym = cy == 0 ? (p1.Y + p2.Y) / 2 : 0;
            /*Thứ tự Control Points
            *   8 
            2   7   1
            4       5
            0   6   3
            */

            //Tọa độ các điểm điều khiển
            int[][] cp = new int[][] {
                new int[]{x1, y1},
                new int[]{x2, y2},
                new int[]{x1, y2},
                new int[]{x2, y1},
                new int[]{x1, ym},
                new int[]{x2, ym},
                new int[]{xm, y1},
                new int[]{xm, y2},
                new int[]{xm, y2 + 20}
            };

            for (int i = 0; i < 9; i++)
            {
                if (x >= cp[i][0] - 5 && x <= cp[i][0] + 5 && y >= cp[i][1] - 5 && y <= cp[i][1] + 5)
                    return i;    //Điểm điều khiển đang được chọn           
            }
            return -1;
        }

        public override void dragControlPoint(int CPid, Point s, Point e)
        {
            //Vector tịnh tiến điểm điều khiển
            int vx = e.X - s.X,
                vy = e.Y - s.Y;

            if (CPid == 8)
            {
                int cx = (p1.X + p2.X) / 2, cy = (p1.Y + p2.Y) / 2;//;lấy tâm
                Point v1 = new Point(s.X - cx, s.Y - cy), v2 = new Point(e.X - cx, e.Y - cy);
                // góc xoay
                double alpha = Math.Acos((v1.X * v2.X + v1.Y * v2.Y) / (Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y) * Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y))) / Math.PI * 180.0;
                if (v1.X * v2.Y - v2.X * v1.Y < 0) alpha *= -1;
                Angle = alpha;
                return;
            }

            //Chiều cao và rộng của hình
            int h = p2.Y - p1.Y,
                w = p2.X - p1.X;
            //Tọa độ 2 điểm chặn
            int x1 = p1.X, y1 = p1.Y, x2 = p2.X, y2 = p2.Y;
            Point pu = new Point(), pv = new Point();
            pu.setPoint(p1); pv.setPoint(p2);

            /*Thứ tự Control Points
            *   8 
            2   7   1
            4       5
            0   6   3
            */

            if (CPid == 1 || CPid == 5 || CPid == 3)
            {
                if (-vx >= w)
                    vx = 1 - w;
            }

            else if (CPid == 2 || CPid == 4 || CPid == 0)
            {
                if (vx >= w)
                    vx = w - 1;
            }

            if (CPid == 2 || CPid == 7 || CPid == 1)
            {
                if (-vy >= h)
                    vy = 1 - h;
            }
            else if (CPid == 0 || CPid == 6 || CPid == 3)
            {
                if (vy >= h)
                    vy = h - 1;
            }

            //Co giãn các điểm điều khiển
            switch (CPid)
            {
                case 0:
                    pu.setPoint(x1 + vx, y1 + vy);
                    break;
                case 1:
                    pv.setPoint(x2 + vx, y2 + vy);
                    break;
                case 2:
                    pu.setPoint(x1 + vx, y1);
                    pv.setPoint(x2, y2 + vy);
                    break;
                case 3:
                    pu.setPoint(x1, y1 + vy);
                    pv.setPoint(x2 + vx, y2);
                    break;
                case 4:
                    pu.setPoint(x1 + vx, y1);
                    break;
                case 5:
                    pv.setPoint(x2 + vx, y2);
                    break;
                case 6:
                    pu.setPoint(x1, y1 + vy);
                    break;
                case 7:
                    pv.setPoint(x2, y2 + vy);
                    break;
            }

            p1.setPoint(pu);
            p2.setPoint(pv);
        }

        public override void drawControlBox(OpenGL gl)
        {
            int cx = (p1.X + p2.X) / 2,
                cy = (p1.Y + p2.Y) / 2;
            gl.Color(LineColor.R, LineColor.G, LineColor.B);
            gl.PushMatrix();
            gl.Translate(cx, cy, 0.0);
            gl.Rotate(Angle, 0.0, 0.0, 1.0);

            Rectangle box = new Rectangle();
            int x1 = p1.X - cx - 5, y1 = p1.Y - cy - 10, x2 = p2.X - cx + 5, y2 = p2.Y - cy + 10;
            Point pi = new Point(x1, y1),
                pj = new Point(x2, y2);
            box.LineColor = new Color(0.5f, 0.5f, 0.5f);

            //Vẽ khung bao quanh hình
            box.set(pi, pj);
            box.Draw(gl);

            //Vẽ 8 điểm điều khiển
            int xm = 0, //p1.X + p2.X) / 2,
               ym = 0;// (p1.Y + p2.Y) / 2;
            int[][] cp = new int[][] {
                new int[]{x1, y1},
                new int[]{x2, y2},
                new int[]{x1, y2},
                new int[]{x2, y1},
                new int[]{x1, ym},
                new int[]{x2, ym},
                new int[]{xm, y1},
                new int[]{xm, y2},
            };
            foreach (int[] p in cp)
            {
                box.set(new Point(p[0] - 3, p[1] - 3), new Point(p[0] + 3, p[1] + 3));
                box.Draw(gl);
            }

            //Vẽ điểm điều khiển xoay
            Line li = new Line();
            li.set(new Point(xm, y2), new Point(xm, y2 + 20 - 4));
            li.LineColor = new Color(0.5f, 0.5f, 0.5f);
            li.Draw(gl);

            Circle ci = new Circle();
            ci.set(new Point(xm - 3, y2 + 20 - 3), new Point(xm + 3, y2 + 20 + 3));
            ci.LineColor = new Color(0.5f, 0.5f, 0.5f);
            ci.Draw(gl);
            gl.PopMatrix();
        }

        public override void translate(Point s, Point e)
        {
            //Vector tịnh tiến
            int vx = e.X - s.X,
                vy = e.Y - s.Y;

            p1.X += vx;
            p1.Y += vy;
            p2.X += vx;
            p2.Y += vy;
        }

        private Color getPixelColor(OpenGL gl, int x, int y) //Lấy màu của pixel (x,y)
        {
            Color color = new Color();
            byte[] pixels = new byte[4];
            gl.ReadPixels(x, y, 2, 2, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, pixels);
            color.setColor(pixels[0] / 255.0f, pixels[1] / 255.0f, pixels[2] / 255.0f);
            return color;
        }

        private void setPixelColor(OpenGL gl, int x, int y, Color color) //Set màu cho pixel (x,y)
        {
            gl.Color(color.R, color.G, color.B);
            gl.Begin(OpenGL.GL_POINTS);
            gl.Vertex(x, y);
            gl.End();
            gl.Flush();
        }

        private void floodFill(OpenGL gl, int x, int y, Color oldColor)
        {
            Color color = new Color(getPixelColor(gl, x, y));
            if (color == oldColor)
            {
                setPixelColor(gl, x, y, FillColor);
                floodFill(gl, x + 1, y, FillColor);
                floodFill(gl, x - 1, y, FillColor);
                floodFill(gl, x, y + 1, FillColor);
                floodFill(gl, x, y - 1, FillColor);
            }
        }

        public override void Fill(OpenGL gl, bool mode)
        {
            if (mode == true)
            {
                int cx = (p1.X + p2.X) / 2,
                    cy = (p1.Y + p2.Y) / 2;
                Color oldColor = getPixelColor(gl, cx, cy);
                floodFill(gl, cx, cy, oldColor);
            }
        }
    }

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

            //Tọa độ các đỉnh            
            nPoints[0].setPoint(0, ry);
            nPoints[1].setPoint(-rx, -ry);
            nPoints[2].setPoint(rx, -ry);
        }
    }


}





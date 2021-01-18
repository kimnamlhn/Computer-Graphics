using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using SharpGL.WinForms;


//vertex - đỉnh
class Vertex
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

// danh sách các đỉnh
class VertexList
{
    public List<Vertex> Vertices { get; set; }
}

// đối tượng 3d
class Object3D
{
    public int Index { get; set; }
    public bool IsSelected { get; set; }
    public List<VertexList> Quads { get; set; }
    public Color Color { get; set; }
    public Vertex Center { get; set; }

    public Object3D()
    {
        IsSelected = false;
        Quads = new List<VertexList>();
    }

    // vẽ đối tượng
    public void Draw(OpenGL gl)
    {

        gl.Color((float)Color.R / 255,
            (float)Color.G / 255,
            (float)Color.B / 255);

        gl.Begin(OpenGL.GL_QUADS);
        for (int i = 0; i < Quads.Count; i++)
        {
            for (int j = 0; j < Quads[i].Vertices.Count; j++)
            {
                float X = Quads[i].Vertices[j].X;
                float Y = Quads[i].Vertices[j].Y;
                float Z = Quads[i].Vertices[j].Z;
                gl.Vertex(X, Y, Z);
            }
        }
        gl.End();
        gl.Flush();

        if (this.IsSelected == true)
        {
            gl.LineWidth(2);
            gl.Color(1.0f, 0.5f, 0);
        }
        else
        {
            gl.LineWidth(1);
            gl.Color(0.0f, 0.0f, 0.0f, 0.5f);
        }
        for (int i = 0; i < Quads.Count; i++)
        {
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int j = 0; j < Quads[i].Vertices.Count; j++)
            {
                float X = Quads[i].Vertices[j].X;
                float Y = Quads[i].Vertices[j].Y;
                float Z = Quads[i].Vertices[j].Z;
                gl.Vertex(X, Y, Z);
            }
            gl.End();
            gl.Flush();
        }
    }

    // di chuyển 
    public void Translate(OpenGL gl,
        float newX, float newY, float newZ)
    {
        Vertex moveDirection = new Vertex
        {
            X = newX - Center.X,
            Y = newY - Center.Y,
            Z = newZ - Center.Z
        };

        Center.X = newX;
        Center.Y = newY;
        Center.Z = newZ;

        for (int i = 0; i < Quads.Count; i++)
        {
            for (int j = 0; j < Quads[i].Vertices.Count; j++)
            {
                Quads[i].Vertices[j].X += moveDirection.X;
                Quads[i].Vertices[j].Y += moveDirection.Y;
                Quads[i].Vertices[j].Z += moveDirection.Z;
            }
        }
    }

}

// hình lập phương
class LapPhuong : Object3D
{

    public LapPhuong(float a)
    {
        VertexList[] quads = new VertexList[6];
        for (int i = 0; i < 6; i++)
            quads[i] = new VertexList();
        float half_a = a / 2;

        // dưới
        Vertex vertex1 = new Vertex { X = -half_a, Y = -half_a, Z = -half_a };
        Vertex vertex2 = new Vertex { X = half_a, Y = -half_a, Z = -half_a };
        Vertex vertex3 = new Vertex { X = half_a, Y = -half_a, Z = half_a };
        Vertex vertex4 = new Vertex { X = -half_a, Y = -half_a, Z = half_a };
        List<Vertex> vertices = new List<Vertex> {
                    vertex1, vertex2, vertex3, vertex4 };

        quads[0].Vertices = vertices;
        Quads.Add(quads[0]);

        // trái
        vertex1 = new Vertex { X = -half_a, Y = -half_a, Z = -half_a };
        vertex2 = new Vertex { X = -half_a, Y = half_a, Z = -half_a };
        vertex3 = new Vertex { X = half_a, Y = half_a, Z = -half_a };
        vertex4 = new Vertex { X = half_a, Y = -half_a, Z = -half_a };
        vertices = new List<Vertex> {
                    vertex1, vertex2, vertex3, vertex4 };

        quads[1].Vertices = vertices;
        Quads.Add(quads[1]);

        // phía sau
        vertex1 = new Vertex { X = -half_a, Y = -half_a, Z = -half_a };
        vertex2 = new Vertex { X = -half_a, Y = half_a, Z = -half_a };
        vertex3 = new Vertex { X = -half_a, Y = half_a, Z = half_a };
        vertex4 = new Vertex { X = -half_a, Y = -half_a, Z = half_a };
        vertices = new List<Vertex> {
                    vertex1, vertex2, vertex3, vertex4 };

        quads[2].Vertices = vertices;
        Quads.Add(quads[2]);

        // phải
        vertex1 = new Vertex { X = -half_a, Y = half_a, Z = half_a };
        vertex2 = new Vertex { X = half_a, Y = half_a, Z = half_a };
        vertex3 = new Vertex { X = half_a, Y = -half_a, Z = half_a };
        vertex4 = new Vertex { X = -half_a, Y = -half_a, Z = half_a };
        vertices = new List<Vertex> {
                    vertex1, vertex2, vertex3, vertex4 };

        quads[3].Vertices = vertices;
        Quads.Add(quads[3]);

        // phía trước
        vertex1 = new Vertex { X = half_a, Y = half_a, Z = half_a };
        vertex2 = new Vertex { X = half_a, Y = half_a, Z = -half_a };
        vertex3 = new Vertex { X = half_a, Y = -half_a, Z = -half_a };
        vertex4 = new Vertex { X = half_a, Y = -half_a, Z = half_a };
        vertices = new List<Vertex> {
                    vertex1, vertex2, vertex3, vertex4 };

        quads[4].Vertices = vertices;
        Quads.Add(quads[4]);

        // đằng trên
        vertex1 = new Vertex { X = -half_a, Y = half_a, Z = -half_a };
        vertex2 = new Vertex { X = half_a, Y = half_a, Z = -half_a };
        vertex3 = new Vertex { X = half_a, Y = half_a, Z = half_a };
        vertex4 = new Vertex { X = -half_a, Y = half_a, Z = half_a };
        vertices = new List<Vertex> {
                    vertex1, vertex2, vertex3, vertex4 };

        quads[5].Vertices = vertices;
        Quads.Add(quads[5]);

    }

    public override string ToString()
    {
        return "Lap phuong " + Index;
    }
}

// hình chóp
class HinhChop : Object3D
{
    public HinhChop(float a, float height)
    {
        VertexList[] quads = new VertexList[5];
        for (int i = 0; i < 5; i++)
            quads[i] = new VertexList();
        float half_a = a / 2;
        float half_height = height / 2;

        Vertex vertex1, vertex2, vertex3, vertex4;
        List<Vertex> vertices;

        // trái
        vertex1 = new Vertex { X = 0.0f, Y = half_height, Z = 0.0f };
        vertex2 = new Vertex { X = -half_a, Y = -half_height, Z = -half_a };
        vertex3 = new Vertex { X = half_a, Y = -half_height, Z = -half_a };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3 };

        quads[0].Vertices = vertices;
        Quads.Add(quads[0]);

        // sau
        vertex1 = new Vertex { X = 0.0f, Y = half_height, Z = 0.0f };
        vertex2 = new Vertex { X = -half_a, Y = -half_height, Z = half_a };
        vertex3 = new Vertex { X = -half_a, Y = -half_height, Z = -half_a };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3 };

        quads[1].Vertices = vertices;
        Quads.Add(quads[1]);

        // phải
        vertex1 = new Vertex { X = 0.0f, Y = half_height, Z = 0.0f };
        vertex2 = new Vertex { X = half_a, Y = -half_height, Z = half_a };
        vertex3 = new Vertex { X = -half_a, Y = -half_height, Z = half_a };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3 };

        quads[2].Vertices = vertices;
        Quads.Add(quads[2]);

        // trước
        vertex1 = new Vertex { X = 0.0f, Y = half_height, Z = 0.0f };
        vertex2 = new Vertex { X = half_a, Y = -half_height, Z = -half_a };
        vertex3 = new Vertex { X = half_a, Y = -half_height, Z = half_a };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3 };

        quads[3].Vertices = vertices;
        Quads.Add(quads[3]);

        // dưới
        vertex1 = new Vertex { X = -half_a, Y = -half_height, Z = -half_a };
        vertex2 = new Vertex { X = half_a, Y = -half_height, Z = -half_a };
        vertex3 = new Vertex { X = half_a, Y = -half_height, Z = half_a };
        vertex4 = new Vertex { X = -half_a, Y = -half_height, Z = half_a };
        vertices = new List<Vertex> {
                    vertex1, vertex2, vertex3, vertex4 };

        quads[4].Vertices = vertices;
        Quads.Add(quads[4]);

    }

    public override string ToString()
    {
        return "Hinh chop " + Index;
    }
}

// hình lăng trụ
class LangTru : Object3D
{
    public LangTru(float size, float height)
    {
        VertexList[] quads = new VertexList[5];
        for (int i = 0; i < 5; i++)
            quads[i] = new VertexList();

        float a, b, c;
        a = size * (float)Math.Sqrt(6) / 6;
        b = size * (float)(-Math.Sqrt(6) + 3 * Math.Sqrt(2)) / 12;
        c = size * (float)(-Math.Sqrt(6) - 3 * Math.Sqrt(2)) / 12;
        float half_height = height / 2;


        Vertex vertex1, vertex2, vertex3, vertex4;
        List<Vertex> vertices;



        // vẽ các mặt của hình lăng trụ


        vertex1 = new Vertex { X = b, Y = -half_height, Z = c };
        vertex2 = new Vertex { X = a, Y = -half_height, Z = a };
        vertex3 = new Vertex { X = a, Y = half_height, Z = a };
        vertex4 = new Vertex { X = b, Y = half_height, Z = c };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3, vertex4 };

        quads[0].Vertices = vertices;
        Quads.Add(quads[0]);

        vertex1 = new Vertex { X = a, Y = -half_height, Z = a };
        vertex2 = new Vertex { X = c, Y = -half_height, Z = b };
        vertex3 = new Vertex { X = c, Y = half_height, Z = b };
        vertex4 = new Vertex { X = a, Y = half_height, Z = a };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3, vertex4 };

        quads[1].Vertices = vertices;
        Quads.Add(quads[1]);


        vertex1 = new Vertex { X = b, Y = -half_height, Z = c };
        vertex2 = new Vertex { X = c, Y = -half_height, Z = b };
        vertex3 = new Vertex { X = c, Y = half_height, Z = b };
        vertex4 = new Vertex { X = b, Y = half_height, Z = c };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3, vertex4 };

        quads[2].Vertices = vertices;
        Quads.Add(quads[2]);


        vertex1 = new Vertex { X = b, Y = -half_height, Z = c };
        vertex2 = new Vertex { X = a, Y = -half_height, Z = a };
        vertex3 = new Vertex { X = c, Y = -half_height, Z = b };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3 };

        quads[3].Vertices = vertices;
        Quads.Add(quads[3]);


        vertex1 = new Vertex { X = b, Y = half_height, Z = c };
        vertex2 = new Vertex { X = a, Y = half_height, Z = a };
        vertex3 = new Vertex { X = c, Y = half_height, Z = b };

        vertices = new List<Vertex> { vertex1, vertex2, vertex3 };

        quads[4].Vertices = vertices;
        Quads.Add(quads[4]);
    }

    public override string ToString()
    {
        return "Lang tru " + Index;

    }

}


using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
    public partial class Lab01 : Form
    {
        OpenGL gl;
        List<Object> shapes = new List<Object>(); //Danh sách cách hình đã được vẽ
        Object currentShape; //Hình đang được vẽ
        enum Mode { Line, Circle, Rectangle, Ellipse, Triangle, Pentagon, Hexagon, Polygon, Select }; //Các mode vẽ hình
        Mode currentMode = Mode.Line;   //mode hiện tại
        bool isDrawing = false; //biến xác định có phải đang vẽ hay không
        Point pStart = new Point(), //Điểm khởi đầu bấm chuột
            pEnd = new Point(); //Điểm cuối cùng khi thả chuột      
        int timeDrawing = 0;    //Thời gian vẽ hình


        int chooseObject = -1;  //Hình đang được chọn
        int chooseControlPoint = -1;  // điểm điều khiển được chọn
        int numOfObject = 0; //Số lượng hình    

        OpenGLControl openGLControl;

        private void renderShapes()
        {
            //Reset 
            // vẽ lại các hình đã lưu ở object
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            foreach (Object s in shapes)
            {
                s.Draw(gl);
                s.Draw(gl);
            }
                
            gl.Flush();

        }

//***************************Khởi tạo các thành phần trong chương trình******************************************
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.btnEllipse = new System.Windows.Forms.Button();
            this.btnCircle = new System.Windows.Forms.Button();
            this.BtnHexagon = new System.Windows.Forms.Button();
            this.btnPentagon = new System.Windows.Forms.Button();
            this.btnRectangle = new System.Windows.Forms.Button();
            this.btnLine = new System.Windows.Forms.Button();
            this.btnTriangle = new System.Windows.Forms.Button();
            this.btnPolygon = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer_Drawing = new System.Windows.Forms.Timer(this.components);
            this.lb_Time = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.DrawFPS = false;
            this.openGLControl.Location = new System.Drawing.Point(3, 69);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(774, 443);
            this.openGLControl.TabIndex = 0;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.Load += new System.EventHandler(this.openGLControl_Load);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // btnEllipse
            // 
            this.btnEllipse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEllipse.Location = new System.Drawing.Point(295, 12);
            this.btnEllipse.Name = "btnEllipse";
            this.btnEllipse.Size = new System.Drawing.Size(70, 25);
            this.btnEllipse.TabIndex = 14;
            this.btnEllipse.Text = "Ellipse";
            this.btnEllipse.UseVisualStyleBackColor = true;
            this.btnEllipse.Click += new System.EventHandler(this.btnEllipse_Click);
            // 
            // btnCircle
            // 
            this.btnCircle.Location = new System.Drawing.Point(106, 12);
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(70, 25);
            this.btnCircle.TabIndex = 13;
            this.btnCircle.Text = "Circle";
            this.btnCircle.UseVisualStyleBackColor = true;
            this.btnCircle.Click += new System.EventHandler(this.btnCircle_Click);
            // 
            // BtnHexagon
            // 
            this.BtnHexagon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnHexagon.Location = new System.Drawing.Point(576, 12);
            this.BtnHexagon.Name = "BtnHexagon";
            this.BtnHexagon.Size = new System.Drawing.Size(70, 25);
            this.BtnHexagon.TabIndex = 12;
            this.BtnHexagon.Text = "Hexagon";
            this.BtnHexagon.UseVisualStyleBackColor = true;
            this.BtnHexagon.Click += new System.EventHandler(this.BtnHexagon_Click);
            // 
            // btnPentagon
            // 
            this.btnPentagon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPentagon.Location = new System.Drawing.Point(482, 12);
            this.btnPentagon.Name = "btnPentagon";
            this.btnPentagon.Size = new System.Drawing.Size(70, 25);
            this.btnPentagon.TabIndex = 11;
            this.btnPentagon.Text = "Pentagon";
            this.btnPentagon.UseVisualStyleBackColor = true;
            this.btnPentagon.Click += new System.EventHandler(this.btnPentagon_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRectangle.Location = new System.Drawing.Point(205, 12);
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(70, 25);
            this.btnRectangle.TabIndex = 10;
            this.btnRectangle.Text = "Rectangle";
            this.btnRectangle.UseVisualStyleBackColor = true;
            this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
            // 
            // btnLine
            // 
            this.btnLine.Enabled = false;
            this.btnLine.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLine.Location = new System.Drawing.Point(12, 12);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(70, 25);
            this.btnLine.TabIndex = 9;
            this.btnLine.Text = "Line";
            this.btnLine.UseVisualStyleBackColor = true;
            this.btnLine.Click += new System.EventHandler(this.btnLine_Click);
            // 
            // btnTriangle
            // 
            this.btnTriangle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTriangle.Location = new System.Drawing.Point(385, 12);
            this.btnTriangle.Name = "btnTriangle";
            this.btnTriangle.Size = new System.Drawing.Size(70, 25);
            this.btnTriangle.TabIndex = 8;
            this.btnTriangle.Text = "Triangle";
            this.btnTriangle.UseVisualStyleBackColor = true;
            this.btnTriangle.Click += new System.EventHandler(this.btnTriangle_Click);
            // 
            // btnPolygon
            // 
            this.btnPolygon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPolygon.Location = new System.Drawing.Point(675, 12);
            this.btnPolygon.Name = "btnPolygon";
            this.btnPolygon.Size = new System.Drawing.Size(70, 25);
            this.btnPolygon.TabIndex = 23;
            this.btnPolygon.Text = "Polygon";
            this.btnPolygon.UseVisualStyleBackColor = true;
            this.btnPolygon.Click += new System.EventHandler(this.btnPolygon_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(664, 43);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(70, 20);
            this.textBox1.TabIndex = 22;
            this.textBox1.Text = "Drawing time:";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // timer_Drawing
            // 
            this.timer_Drawing.Tick += new System.EventHandler(this.timerDrawingTick);
            // 
            // lb_Time
            // 
            this.lb_Time.AutoSize = true;
            this.lb_Time.Location = new System.Drawing.Point(740, 46);
            this.lb_Time.Name = "lb_Time";
            this.lb_Time.Size = new System.Drawing.Size(37, 13);
            this.lb_Time.TabIndex = 21;
            this.lb_Time.Text = "0:00.0";
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelect.Location = new System.Drawing.Point(12, 40);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(164, 25);
            this.btnSelect.TabIndex = 24;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // Lab01
            // 
            this.ClientSize = new System.Drawing.Size(780, 516);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnPolygon);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lb_Time);
            this.Controls.Add(this.btnEllipse);
            this.Controls.Add(this.btnCircle);
            this.Controls.Add(this.BtnHexagon);
            this.Controls.Add(this.btnPentagon);
            this.Controls.Add(this.btnRectangle);
            this.Controls.Add(this.btnLine);
            this.Controls.Add(this.btnTriangle);
            this.Controls.Add(this.openGLControl);
            this.Name = "Lab01";
            this.Text = "Lab02";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //constructor
        public Lab01()
        {
            InitializeComponent();

        }

//***************************Khi click vào các button******************************************

        private void btnLine_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = false;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = true;
            btnSelect.Enabled = true;
            currentMode = Mode.Line;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = false;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = true;
            btnSelect.Enabled = true;
            currentMode = Mode.Circle;
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = false;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = true;
            btnSelect.Enabled = true;
            currentMode = Mode.Rectangle;
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = false;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = true;
            btnSelect.Enabled = true;
            currentMode = Mode.Ellipse;
        }

        private void btnTriangle_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = false;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = true;
            btnSelect.Enabled = true;
            currentMode = Mode.Triangle;
        }

        private void btnPentagon_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = false;
            BtnHexagon.Enabled = true;
            btnSelect.Enabled = true;
            currentMode = Mode.Pentagon;
        }

        private void BtnHexagon_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = false;
            btnSelect.Enabled = true;
            currentMode = Mode.Hexagon;
        }



        private void btnSelect_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = true;
            btnSelect.Enabled = false;
            currentMode = Mode.Select;
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            btnLine.Enabled = true;
            btnCircle.Enabled = true;
            btnRectangle.Enabled = true;
            btnEllipse.Enabled = true;
            btnTriangle.Enabled = true;
            btnPentagon.Enabled = true;
            BtnHexagon.Enabled = true;
            btnPolygon.Enabled = false;
            btnSelect.Enabled = true;
            currentMode = Mode.Polygon;
        }

        //***************************Các action của chuột******************************************



        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {

            pStart.setPoint(e.Location.X, openGLControl.Height - e.Location.Y);
            // vẽ đa giác
            if (currentMode == Mode.Polygon)
            {
                if (!isDrawing)
                {
                    renderShapes();
                    currentShape = new MultiP_Poly();
                    isDrawing = true;
                    timer_Drawing.Start();
                    timeDrawing = 0;

                }
                return;
            }

            isDrawing = true;

            //chế độ chọn
            if (currentMode == Mode.Select)
            {
                int x = pStart.X, y = pStart.Y;

                if (chooseObject >= 0) //Kiểm tra xem có đang muốn kéo điểm diều khiển
                {
                    chooseControlPoint = shapes[chooseObject].isControlPointChoose(x, y);
                    if (chooseControlPoint >= 0)
                    {
                        return;
                    }
                }

                //đang chọn hình hình set giá trị
                chooseObject = -1;
                for (int i = numOfObject - 1; i >= 0; i--)
                    if (shapes[i].isChooseObject(x, y))
                    {
                        chooseObject = i;
                        break;
                    }

                renderShapes();

                if (chooseObject >= 0) //Check hình được chọn
                {
                    shapes[chooseObject].drawControlBox(gl); // vẽ khung điều khiển
                }

                return;
            }
            timer_Drawing.Start();
            timeDrawing = 0;
            switch (currentMode)
            {
                case Mode.Line:
                    currentShape = new Line();
                    break;
                case Mode.Triangle:
                    currentShape = new Triangle();
                    break;
                case Mode.Rectangle:
                    currentShape = new Rectangle();
                    break;
                case Mode.Circle:
                    currentShape = new Circle();
                    break;
                case Mode.Ellipse:
                    currentShape = new Ellipse();
                    break;
                case Mode.Pentagon:
                    currentShape = new Pentagon();
                    break;
                case Mode.Hexagon:
                    currentShape = new Hexagon();
                    break;
            }
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            //cập nhật điểm cuối
            pEnd.setPoint(e.Location.X, openGLControl.Height - e.Location.Y); 
            if (currentMode == Mode.Polygon)
            {
                ((MultiP_Poly)currentShape).moveVertex(pEnd);
                renderShapes();
                currentShape.Draw(gl);
            }

            else if (currentMode == Mode.Select)
            {
                if (chooseObject >= 0)
                {   //Co giãn hình 
                    if (chooseControlPoint >= 0)
                        shapes[chooseObject].dragObject(chooseControlPoint, pStart, pEnd);
                    //Di chuyển hình
                    else shapes[chooseObject].translate(pStart, pEnd); 
                    

                    if (chooseControlPoint != 8)
                        pStart.setPoint(pEnd);
                    renderShapes();
                    shapes[chooseObject].drawControlBox(gl);
                }
            }
            else
            {
                //Tiếp tục vẽ
                currentShape.set(pStart, pEnd);    
                renderShapes();
                currentShape.Draw(gl);
                gl.Flush();
            }
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentMode == Mode.Polygon)
            {
                if (isDrawing)
                {
                    //kết thúc
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)    
                    {
                        shapes.Add(((MultiP_Poly)currentShape).getPolygon());
                        numOfObject++;
                        isDrawing = false;
                        timer_Drawing.Stop();
                        renderShapes();
                    }
                    else
                    {
                        if (((MultiP_Poly)currentShape).nPoly == 0)
                        {
                            ((MultiP_Poly)currentShape).addVertex(new Point(e.Location.X, openGLControl.Height - e.Location.Y));
                        }
                        ((MultiP_Poly)currentShape).addVertex(new Point(e.Location.X, openGLControl.Height - e.Location.Y));
                        currentShape.Draw(gl);
                    }
                }
                return;
            }

            isDrawing = false;

            if (currentMode == Mode.Select)
            {
                if (chooseControlPoint >= 0)  
                {
                    renderShapes();
                    shapes[chooseObject].drawControlBox(gl);
                    chooseControlPoint = -1;
                }
            }
            else
            {
                //Hoàn thành
                timer_Drawing.Stop();
                //thêm hình
                shapes.Add(currentShape);
                numOfObject++;
            }
        }

        //****************************************************************************************************

        // đếm thời gian
        private void timerDrawingTick(object sender, EventArgs e)
        {
            timeDrawing++;
            int min, sec, mil;
            mil = timeDrawing % 10;
            sec = (timeDrawing / 10) % 60;
            min = timeDrawing / 600;
            lb_Time.Text = min.ToString() + ":" + (sec < 10 ? "0" : "") + sec.ToString() + "." + mil.ToString();
        }



        // các hàm onpenGL control
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = openGLControl.OpenGL;
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            //Load the identity.
            gl.LoadIdentity();
            //Create a perspective transformation.
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);
            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);
        }

        private void openGLControl_Load(object sender, EventArgs e)
        {
            gl.ClearColor(1f, 1f, 1f, 1f);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        }






        // để 1 dòng chữ thời gian đang vẽ
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }


    }
}

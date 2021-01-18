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


namespace Lab04
{
    public partial class Form1 : Form
    {
        //khởi tạo ban đầu - constructor
        public Form1()
        {
            // tạo các đối thành phần trong màn hình
            InitializeComponent();
        }

        bool isDraw = false; // biến đang vẽ
        float sizeEdge = 0.0f; // biến độ dày đường viền
        float height = 0.0f; // biến chiều cao đối với hình chóp và lăng trụ
        Color currentColor; //màu hiên tại
        int numLapPhuong = 0, numHinhChop = 0, numLangTru = 0; // số lượng mỗi hình đã vẽ 
        List<Object3D> objects = new List<Object3D>();  // danh sách hình đã vẽ
        int selectedIndex = -1; // có phải là đang chọn


        // khởi tạo danh sách các thành phần 
        private void InitializeComponent()
        {
            this.boxObject = new System.Windows.Forms.ComboBox();
            this.firstObject = new System.Windows.Forms.ListBox();
            this.btnDraw = new System.Windows.Forms.Button();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.txtSizeEdge = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtScaleZ = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtScaleY = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtRotationZ = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRotationY = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPositionZ = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPositionY = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtScaleX = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRotationX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPositionX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // boxObject
            // box chứa các object
            this.boxObject.FormattingEnabled = true;
            this.boxObject.Location = new System.Drawing.Point(196, 2);
            this.boxObject.Name = "boxObject";
            this.boxObject.Size = new System.Drawing.Size(136, 21);
            this.boxObject.TabIndex = 0;
            this.boxObject.SelectedIndexChanged += new System.EventHandler(this.ObjectSelectChanged);
            // 
            // firstObject
            // hinh dau tien
            this.firstObject.FormattingEnabled = true;
            this.firstObject.Location = new System.Drawing.Point(12, 2);
            this.firstObject.Name = "firstObject";
            this.firstObject.Size = new System.Drawing.Size(149, 121);
            this.firstObject.TabIndex = 1;
            this.firstObject.SelectedIndexChanged += new System.EventHandler(this.ObjectSelected);
            // 
            // btnDraw
            // button vẽ
            this.btnDraw.Location = new System.Drawing.Point(6, 70);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(62, 24);
            this.btnDraw.TabIndex = 2;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // openGLControl
            // 
            this.openGLControl.BackColor = System.Drawing.Color.Black;
            this.openGLControl.DrawFPS = false;
            this.openGLControl.Location = new System.Drawing.Point(3, 141);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(803, 453);
            this.openGLControl.TabIndex = 3;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.Load += new System.EventHandler(this.openGLControl_Load);
            // 
            // txtSizeEdge
            // độ dày viền 
            this.txtSizeEdge.Location = new System.Drawing.Point(44, 13);
            this.txtSizeEdge.Name = "txtSizeEdge";
            this.txtSizeEdge.Size = new System.Drawing.Size(77, 20);
            this.txtSizeEdge.TabIndex = 4;
            this.txtSizeEdge.TextChanged += new System.EventHandler(this.txtSizeEdge_TextChanged);
            // 
            // label1
            // hiển thị dòng chữ độ dày
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Edge";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.txtHeight);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSizeEdge);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnDraw);
            this.groupBox1.Location = new System.Drawing.Point(190, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size";
            // 
            // button xóa 
            // 
            this.btnClear.Location = new System.Drawing.Point(74, 70);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(62, 24);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chiều cao
            // 
            this.txtHeight.Location = new System.Drawing.Point(44, 40);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(77, 20);
            this.txtHeight.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Height";
            // 
            // group các box 
            // 
            this.groupBox2.Controls.Add(this.txtScaleZ);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtScaleY);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.txtRotationZ);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtRotationY);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtPositionZ);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtPositionY);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtScaleX);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtRotationX);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtPositionX);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(356, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 96);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transform";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // txtScaleZ
            // 
            this.txtScaleZ.Location = new System.Drawing.Point(340, 68);
            this.txtScaleZ.Name = "txtScaleZ";
            this.txtScaleZ.Size = new System.Drawing.Size(100, 20);
            this.txtScaleZ.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(320, 71);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Z";
            // 
            // txtScaleY
            // 
            this.txtScaleY.Location = new System.Drawing.Point(214, 68);
            this.txtScaleY.Name = "txtScaleY";
            this.txtScaleY.Size = new System.Drawing.Size(100, 20);
            this.txtScaleY.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(194, 71);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Y";
            // 
            // txtRotationZ
            // 
            this.txtRotationZ.Location = new System.Drawing.Point(340, 42);
            this.txtRotationZ.Name = "txtRotationZ";
            this.txtRotationZ.Size = new System.Drawing.Size(100, 20);
            this.txtRotationZ.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(320, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Z";
            // 
            // txtRotationY
            // 
            this.txtRotationY.Location = new System.Drawing.Point(214, 42);
            this.txtRotationY.Name = "txtRotationY";
            this.txtRotationY.Size = new System.Drawing.Size(100, 20);
            this.txtRotationY.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(194, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Y";
            // 
            // txtPositionZ
            // 
            this.txtPositionZ.Location = new System.Drawing.Point(340, 17);
            this.txtPositionZ.Name = "txtPositionZ";
            this.txtPositionZ.Size = new System.Drawing.Size(100, 20);
            this.txtPositionZ.TabIndex = 12;
            this.txtPositionZ.TextChanged += new System.EventHandler(this.positionZChange);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(320, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Z";
            // 
            // txtPositionY
            // 
            this.txtPositionY.Location = new System.Drawing.Point(214, 17);
            this.txtPositionY.Name = "txtPositionY";
            this.txtPositionY.Size = new System.Drawing.Size(100, 20);
            this.txtPositionY.TabIndex = 10;
            this.txtPositionY.TextChanged += new System.EventHandler(this.positionYChange);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(194, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Y";
            // 
            // txtScaleX
            // 
            this.txtScaleX.Location = new System.Drawing.Point(88, 68);
            this.txtScaleX.Name = "txtScaleX";
            this.txtScaleX.Size = new System.Drawing.Size(100, 20);
            this.txtScaleX.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(68, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Scale";
            // 
            // txtRotationX
            // 
            this.txtRotationX.Location = new System.Drawing.Point(88, 42);
            this.txtRotationX.Name = "txtRotationX";
            this.txtRotationX.Size = new System.Drawing.Size(100, 20);
            this.txtRotationX.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Rotation";
            // 
            // txtPositionX
            // 
            this.txtPositionX.Location = new System.Drawing.Point(88, 17);
            this.txtPositionX.Name = "txtPositionX";
            this.txtPositionX.Size = new System.Drawing.Size(100, 20);
            this.txtPositionX.TabIndex = 2;
            this.txtPositionX.TextChanged += new System.EventHandler(this.positionXChange);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Position";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 600);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.openGLControl);
            this.Controls.Add(this.firstObject);
            this.Controls.Add(this.boxObject);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lab04";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        // ********************* OPENGL CONTROL *********************
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA,
                OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;

            //set ma tran viewport
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);

            //set ma trận phép chiếu
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            gl.Perspective(60,
           openGLControl.Width / openGLControl.Height,
               1.0, 20.0);

            //set ma tran model view
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LookAt(
                9, 8, 12,
                0, 0, 0,
                0, 1, 0);
        }



        //vẽ các hình
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            RedrawScreen(gl);

            if (isDraw == true)
            {
                // lập phương
                if (boxObject.SelectedIndex == 0)
                {
                    LapPhuong lp = new LapPhuong(sizeEdge)
                    {
                        Index = numLapPhuong,
                        Color = currentColor,
                        Center = new Vertex { X = 0.0f, Y = 0.0f, Z = 0.0f }
                    };

                    firstObject.Items.Add(lp);
                    objects.Add(lp);
                    numLapPhuong++;
                }
                // hinh chop
                else if (boxObject.SelectedIndex == 1)
                {
                    HinhChop hc = new HinhChop(sizeEdge, height)
                    {
                        Index = numHinhChop,
                        Color = currentColor,
                        Center = new Vertex { X = 0.0f, Y = 0.0f, Z = 0.0f }
                    };

                    firstObject.Items.Add(hc);
                    objects.Add(hc);
                    numHinhChop++;
                }
                // lang tru
                else if (boxObject.SelectedIndex == 2)
                {
                    LangTru ltr = new LangTru(sizeEdge, height)
                    {
                        Index = numLangTru,
                        Color = currentColor,
                        Center = new Vertex { X = 0.0f, Y = 0.0f, Z = 0.0f }
                    };
                    firstObject.Items.Add(ltr);
                    objects.Add(ltr);
                    numLangTru++;
                }

                isDraw = false;
            }
        }

        //********************** CÁC HÀM ****************
        //form hiện danh sách các hình
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] objectNames = { "Lap phuong", "Hinh chop", "Lang tru tam giac" };
            boxObject.Items.AddRange(objectNames);
            boxObject.SelectedIndex = 0;

            currentColor = Color.White;
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            isDraw = true;
            bool success;
            success = float.TryParse(txtSizeEdge.Text, out sizeEdge);



            txtSizeEdge.Text = txtHeight.Text = "";
        }

        private void ObjectSelectChanged(object sender, EventArgs e)
        {


        }

        // xác định của hình được chọn
        private void ObjectSelected(object sender, EventArgs e)
        {
            if (selectedIndex != -1)
                objects[selectedIndex].IsSelected = false; 

            int idx = firstObject.SelectedIndex;
            objects[idx].IsSelected = true;
            selectedIndex = idx;
            txtPositionX.Text = objects[idx].Center.X.ToString();
            txtPositionY.Text = objects[idx].Center.Y.ToString();
            txtPositionZ.Text = objects[idx].Center.Z.ToString();
        }

        //thay đổi vị trí X
        private void positionXChange(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            if (selectedIndex != -1)
            {
                bool success = float.TryParse(txtPositionX.Text, out float newX);
                if (success == true)
                {
                    int idx = firstObject.SelectedIndex;
                    float newY = objects[idx].Center.Y;
                    float newZ = objects[idx].Center.Z;
                    objects[idx].Translate(gl, newX, newY, newZ);
                }
            }
        }

        //thay đổi vị trí y
        private void positionYChange(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            if (selectedIndex != -1)
            {
                bool success = float.TryParse(txtPositionY.Text, out float newY);
                if (success == true)
                {
                    int idx = firstObject.SelectedIndex;
                    float newX = objects[idx].Center.X;
                    float newZ = objects[idx].Center.Z;
                    objects[idx].Translate(gl, newX, newY, newZ);
                }
            }
        }
        // thay đổi vị trí z
        private void positionZChange(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            if (selectedIndex != -1)
            {
                bool success = float.TryParse(txtPositionZ.Text, out float newZ);
                if (success == true)
                {
                    int idx = firstObject.SelectedIndex;
                    float newX = objects[idx].Center.X;
                    float newY = objects[idx].Center.Y;
                    objects[idx].Translate(gl, newX, newY, newZ);
                }
            }
        }

        private void openGLControl_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void txtSizeEdge_TextChanged(object sender, EventArgs e)
        {

        }

        void RedrawScreen(OpenGL gl)
        {
            foreach (Object3D object3D in firstObject.Items)
                object3D.Draw(gl);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            numLapPhuong = numHinhChop = numLangTru = 0;
            firstObject.Items.Clear();
            objects.Clear();

            txtSizeEdge.Text = txtHeight.Text = "";
        }


    }
}

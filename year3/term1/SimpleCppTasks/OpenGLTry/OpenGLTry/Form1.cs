using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform;
using Tao.Platform.Windows;

namespace OpenGLTry
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //code for initialization of openGl
            simpleOpenGlControl1.InitializeContexts();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            Gl.glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            Gl.glBegin(Gl.GL_TRIANGLES);
            
            //red apex
            Gl.glColor3f(1, 0, 0);
            Gl.glVertex2d(-1.0, -1.0);

            //green apex
            Gl.glColor3f(0, 1, 0);
            Gl.glVertex2d(1.0, -1.0);

            //blue apex
            Gl.glColor3f(0, 0, 1);
            Gl.glVertex2d(0, 1.0);

            Gl.glEnd();
        }
    }
}

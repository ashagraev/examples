using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace repetitor_on_event
{
    public partial class Form1 : Form
    {
        int step = 0;
        int steps = 3;

        State TheState;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TheState = new State();
            TheState.Load("C:/Users/alex-sh/Desktop/repetitor_event_images", steps, Width, Height);
            TheState.Reset();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (TheState.Transition == null)
            {
                return;
            }

            e.Graphics.DrawImage(TheState.Transition[step], new Point(0, 0));
            e.Graphics.Flush();
        }

        private void StartTransition()
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();

            if (step == steps)
            {
                step = 0;
                timer1.Enabled = false;
            }
            else
            {
                ++step;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.A)
            {
                TheState.SetupTransition(1);
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.B)
            {
                TheState.SetupTransition(2);
            }
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.C)
            {
                TheState.SetupTransition(3);
            }
            else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.D)
            {
                TheState.SetupTransition(4);
            } else
            {
                TheState.Reset();
            }
            
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            StartTransition();
        }
    }
}

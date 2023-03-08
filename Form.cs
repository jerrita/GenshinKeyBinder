using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinMidiBinder
{
    public partial class Form : System.Windows.Forms.Form
    {
        MidiIn? midiIn;
        bool running = false;

        public Form()
        {
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Enum.GetValues(typeof(Logic.Mode));
            button1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mode = (Logic.Mode)comboBox1.SelectedItem;
            var deviceNo = (int)numericUpDown1.Value;

            button1.Enabled = false;

            try
            {
                if (running)
                {
                    Logic.Stop(midiIn!);
                    button1.Text = "Start";
                    running = false;
                    textBox1.Text = "";
                }
                else
                {
                    midiIn = new MidiIn(deviceNo);
                    Logic.Run(textBox1, mode, midiIn);
                    running = true;
                    button1.Text = "Stop";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            button1.Focus();
        }
    }
}

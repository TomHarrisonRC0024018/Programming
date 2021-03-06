﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Unit_35__Computer_Programming
{
    public partial class Form1 : Form
    {

        class row
        {
            public double time;
            public double altitude;
            public double velocity;
            public double acceleration;


        }

        List<row> table = new List<row>();
        //here i have defined the key variables for my program
        public Form1()
        {
            InitializeComponent();
        }

        void calculateVelocity()
        {
            for (int i = 1; i < table.Count; i++)
            {
                double dt = table[i].time - table[i - 1].time;
                double dalt = table[i].altitude - table[i - 1].altitude;
                table[i].velocity = dalt / dt;
            }
        }
        //here is where velocity is calculated from the data, using the derivative of altitude and time.
        void calculatedAcceleration()
        {
            for (int i = 2; i < table.Count; i++)
            {
                double dt = table[i].time - table[i - 1].time;
                double dv = table[i].velocity - table[i - 1].velocity;
                table[i].acceleration = dv / dt;
            }
        }
        //here is where acceleration is calculated from the data, using the derivative of velocity and time.


        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "csv files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            table.Add(new row());
                            string[] r = sr.ReadLine().Split(',');
                            table.Last().time = double.Parse(r[0]);
                            table.Last().altitude = double.Parse(r[1]);
                        }
                    }
                    //this is where my program opens a csv file to retreive the data
                    calculateVelocity();
                    calculatedAcceleration();
                }
                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " failed to open.");
                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format");
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format");
                }
                catch (DivideByZeroException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " has rows that has the same time");
                }
            }

        }
        //this is where my program will display an error if something unexpected is entered
        private void velocityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Velocity",
                Color = Color.Coral,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.acceleration);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "acceleration /ms^-2";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }
        //this is the graph for velocity
        private void altitudeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Altitude",
                Color = Color.PaleGreen,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.altitude);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "altitude /m";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }
        //this is the graph for altitude
        private void accelerationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Acceleration",
                Color = Color.Purple,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.velocity);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "velocity /ms^-1";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }
        //the graph for acceleration
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "csv Files|*.csv";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.WriteLine("Time /s, altitude /m, velocity /ms^-1, acceleration /ms^-2");
                        foreach (row r in table)
                        {
                            sw.WriteLine(r.time + "," + r.altitude + "," + r.velocity + "," + r.acceleration);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + "Failed to save");

                }
            }
        }
        //here is where the program can save a CSV file
        private void savePNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "png Files|*.png";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);

                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + "Failed to save");

                }
            }
        }
        //this is where a PNG file can be saved
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}




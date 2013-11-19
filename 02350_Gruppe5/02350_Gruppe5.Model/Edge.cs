using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace _02350_Gruppe5.Model
{
    public class Edge : NotifyBase
    {
        // Constructor der bruges når en kant tilføjes, points sættes til den rigtige kant
        public Edge(ClassBox a, ClassBox b)
        {
            endA = a;
            endB = b;

            points = setPoints(endA, endB);
        }

        // Get og set for endepunkterne
        private ClassBox endA;
        public ClassBox EndA
        {
            get { return endA; }
            set { if (endA == value) return; endA = value; NotifyPropertyChanged("EndA"); }
        }
        private ClassBox endB;
        public ClassBox EndB
        {
            get { return endB; }
            set { if (endB == value) return; endB = value; NotifyPropertyChanged("EndB"); }
        }



        // Bruges af EdgeUserControl til at læse punkterne i kanten
        private PointCollection points = new PointCollection();
        public PointCollection Points
        {
            get
            {
                return points;
            }
            set
            {
                //System.Windows.Forms.MessageBox.Show("hej");
                points = setPoints(endA, endB);
                NotifyPropertyChanged("Points");
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
                NotifyPropertyChanged("SelectedColor");
            }

        }
        public Brush SelectedColor { get { return IsSelected ? Brushes.Blue : Brushes.Black; } }
        // Metoden der udregner hvordan en kant skal se ud afh. af forholdet mellem de to endepunkter
        private PointCollection setPoints(ClassBox endA, ClassBox endB)
        {
            int x1 = endA.CenterX, y1 = endA.CenterY, width1 = endA.Width, height1 = endA.Height,
                x2 = endB.CenterX, y2 = endB.CenterY, width2 = endB.Width, height2 = endB.Height;
            int STEP = 25, stepX, stepY, xWidth = Math.Abs(x1 - x2), yWidth = Math.Abs(y1 - y2);

            Point start, second, third, fourth, fifth, end;
            PointCollection points = new PointCollection();

            if (xWidth <= yWidth)
            {
                if (y1 <= y2)
                {
                    start = new Point(x1, y1 + height1 / 2);
                    end = new Point(x2, y2 - height2 / 2);
                    stepY = Math.Min(STEP, (yWidth - (height1 + height2) / 2) / 10);
                    second = new Point(x1, y1 + height1 / 2 + stepY);
                    fifth = new Point(x2, y2 - height2 / 2 - stepY);
                    stepX = xWidth / 2;
                    if (x1 < x2)
                    {
                        stepX = -1 * stepX;
                    }
                    third = new Point(x1 - stepX, y1 + height1 / 2 + stepY);
                    fourth = new Point(x2 + stepX, y2 - height2 / 2 - stepY);
                }
                else
                {
                    start = new Point(x1, y1 - height1 / 2);
                    end = new Point(x2, y2 + height2 / 2);
                    stepY = Math.Min(STEP, (yWidth - (height1 + height2) / 2) / 10);
                    second = new Point(x1, y1 - height1 / 2 - stepY);
                    fifth = new Point(x2, y2 + height2 / 2 + stepY);
                    stepX = xWidth / 2;
                    if (x1 < x2)
                    {
                        stepX = -1 * stepX;
                    }
                    third = new Point(x1 - stepX, y1 - height1 / 2 - stepY);
                    fourth = new Point(x2 + stepX, y2 + height2 / 2 + stepY);
                }
            }
            else
            {
                if (x1 <= x2)
                {
                    start = new Point(x1 + width1 / 2, y1);
                    end = new Point(x2 - width2 / 2, y2);
                    stepX = Math.Min(STEP, (xWidth - (width1 + width2) / 2) / 10);
                    second = new Point(x1 + width1 / 2 + stepX, y1);
                    fifth = new Point(x2 - width2 / 2 - stepX, y2);
                    stepY = yWidth / 2;
                    if (y1 < y2)
                    {
                        stepY = -1 * stepY;
                    }
                    third = new Point(x1 + width1 / 2 + stepX, y1 - stepY);
                    fourth = new Point(x2 - width2 / 2 - stepX, y2 + stepY);
                }
                else
                {
                    start = new Point(x1 - width1 / 2, y1);
                    end = new Point(x2 + width2 / 2, y2);
                    stepX = Math.Min(STEP, (xWidth - (width1 + width2) / 2) / 10);
                    second = new Point(x1 - width1 / 2 - stepX, y1);
                    fifth = new Point(x2 + width2 / 2 + stepX, y2);
                    stepY = yWidth / 2;
                    if (y1 < y2)
                    {
                        stepY = -1 * stepY;
                    }
                    third = new Point(x1 - width1 / 2 - stepX, y1 - stepY);
                    fourth = new Point(x2 + width2 / 2 + stepX, y2 + stepY);
                }
            }

            points.Add(start);
            points.Add(second);
            points.Add(third);
            points.Add(fourth);
            points.Add(fifth);
            points.Add(end);

            return points;
        }
    }
}

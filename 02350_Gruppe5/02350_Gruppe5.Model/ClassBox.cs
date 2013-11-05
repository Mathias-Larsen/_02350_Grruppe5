using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _02350_Gruppe5.Model
{
    public class ClassBox : NotifyBase
    {
        private int x, y, number;
        public int Number { get { return number; } private set { number = value; } } // skal bruges til at finde nummeret på en enkelt klasse
        
        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); } } // der skal også være noget notify property changed
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y");} }
        private int width, height;
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width");} }
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height"); } }
        public int CenterX { get { return X + Width / 2; } set { X = value - Width / 2; NotifyPropertyChanged("X"); } }
        public int CenterY { get { return Y + Height / 2; } set { Y = value - Height / 2; NotifyPropertyChanged("Y"); } }
        
        private string className;
        public string ClassName { get { return className; } set { className = value; NotifyPropertyChanged("ClassName"); } }

        private List<string> attNames, methodNames;
        public List<string> AttNames { get { return attNames; } set { attNames = value; NotifyPropertyChanged("AttNames"); } }
        public List<string> MethodNames { get { return methodNames; } set { methodNames = value; NotifyPropertyChanged("MethodNames"); } }

        public ClassBox(int num)
        {
            Number = num;
            X = Y = 100; //hvor skal de dukke op?
            Width = Height = 100; //og hvor store skal de være?
            className = "Class name";
            attNames = new List<string>();
            methodNames = new List<string>();
            attNames.Add("test");
            attNames.Add("Test");
        }
        // ViewModel properties. Den burde være i sin egen ViewModel klasse som Node klasserne wrappes af, men i dette tilfælde er det her meget lettere.
        private bool isSelected;
        public bool IsSelected { 
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


    }
}

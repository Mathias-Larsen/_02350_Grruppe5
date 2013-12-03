using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace _02350_Gruppe5.Model
{
    //
    // Class defining a ClassBox and its methods
    //

    public class ClassBox : NotifyBase
    {
        private int x, y, number; // position and id
        public int Number { get { return number; } private set { number = value; } } // skal bruges til at finde nummeret på en enkelt klasse

        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); } } 
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); } }
        private int width, height;
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width"); } }
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height"); } }
        public int CenterX { get { return X + Width / 2; } set { X = value - Width / 2; NotifyPropertyChanged("X"); } }
        public int CenterY { get { return Y + Height / 2; } set { Y = value - Height / 2; NotifyPropertyChanged("Y"); } }

        private string className;
        public string ClassName { get { return className; } set { className = value; NotifyPropertyChanged("ClassName"); } }

        private List<attOrMethodName> attNamesClass, methodNamesClass;
        public List<attOrMethodName> AttNamesClass { get { return attNamesClass; } set { attNamesClass = value; NotifyPropertyChanged("Attribut names"); } }
        public List<attOrMethodName> MethodNamesClass { get { return methodNamesClass; } set { methodNamesClass = value; NotifyPropertyChanged("MethodNames"); } }

        // Constructor, setting new ClassBoxes to default values
        public ClassBox(int num)
        {
            Number = num;
            X = Y = 100; //hvor skal de dukke op?
            Width = Height = 200; //og hvor store skal de være?
            className = "Class name";
            attNamesClass = new List<attOrMethodName>();
            methodNamesClass = new List<attOrMethodName>();
            attNamesClass.Add(new attOrMethodName("+ Attribute : Type"));
            attNamesClass.Add(new attOrMethodName("- Attribute : Type"));
            methodNamesClass.Add(new attOrMethodName("+ Method( ) : ReturnType"));
            methodNamesClass.Add(new attOrMethodName("- Method( ) : ReturnType"));
        }
        
        private bool isSelected;
        public bool IsSelected
        {
            get{ return isSelected;}
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
                NotifyPropertyChanged("SelectedColor");
            }

        }
        public Brush SelectedColor { get { return IsSelected ? Brushes.Blue : Brushes.Black; } }

        public class attOrMethodName
        {
            public attOrMethodName(string _name)
            {
                name = _name;
                //FontStyle fStyle = FontStyles.Normal;
                //FontWeight fWeight = FontWeights.Normal;
            }
            private string name;
            public string Name { get { return name; } set { name = value; } }
            //private FontStyle fStyle;
            //private FontWeight fWeight;
            //public FontStyle FStyle { get { return fStyle; } set { fStyle = value; } }
            //public FontWeight FWeight { get { return fWeight; } set { fWeight = value; } }
        }
    }
}

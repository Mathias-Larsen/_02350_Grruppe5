using _02350_Gruppe5.Command;
using _02350_Gruppe5.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _02350_Gruppe5.ViewModel
{
    /// <summary>
    /// Denne ViewModel er bundet til MainWindow.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Holder styr på undo/redo.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        // Bruges til at ændre tilstand når en kant er ved at blive tilføjet.
        private bool isAddingEdge;
        // Bruges til at gemme det første punkt når en kant tilføjes. 
        private ClassBox addingEdgeEndA;
        // Bruges til at gemme den valgte klasse. 
       // private ClassBox selectedClassBox;
        // Gemmer det første punkt som punktet har under en flytning.
        private Point moveClassBoxPoint;


        // Formålet med at benytte en ObservableCollection er at den implementere INotifyCollectionChanged, der er forskellige fra INotifyPropertyChanged.
        // INotifyCollectionChanged smider en event når mængden af elementer i en kollektion ændres (altså når et element fjernes eller tilføjes).
        // Denne event giver GUI'en besked om ændringen.
        // Dette er en generisk kollektion. Det betyder at den kan defineres til at indeholde alle slags klasser, 
        // men den holder kun klasser af en type når den benyttes.
        public ObservableCollection<ClassBox> ClassBoxs { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }
        public ObservableCollection<ClassBox> SelectedClassBox { get; set; }
        

        // Kommandoer som UI bindes til.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand AddClassCommand { get; private set; }
        public ICommand RemoveClassCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }
        public ICommand RemoveEdgesCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand MouseDownClassBoxCommand { get; private set; }
        public ICommand MouseMoveClassBoxCommand { get; private set; }
        public ICommand MouseUpClassBoxCommand { get; private set; }

        public MainViewModel()
        {
            SelectedClassBox = new ObservableCollection<ClassBox>();
            // Her fyldes listen af klasser med to klasser. Her benyttes et alternativ til konstruktorer med syntaksen 'new Type(){ Attribut = Værdi }'
            // Det der sker er at der først laves et nyt object og så sættes objektets attributer til de givne værdier.
            ClassBoxs = new ObservableCollection<ClassBox>() { 
                new ClassBox() { X = 30, Y = 40, Width = 80, Height = 80 }, 
                new ClassBox() { X = 140, Y = 230, Width = 100, Height = 100 } };
            // ElementAt() er en LINQ udvidelses metode som ligesom mange andre kan benyttes på stort set alle slags kollektioner i .NET.
            Edges = new ObservableCollection<Edge>() { 
                new Edge() { EndA = ClassBoxs.ElementAt(0), EndB = ClassBoxs.ElementAt(1) } };

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes. Her vidersendes metode kaldne til UndoRedoControlleren.
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            AddClassCommand = new RelayCommand(AddClassBox);
            RemoveClassCommand = new RelayCommand<IList>(RemoveClassBox, CanRemoveClassBox);
            AddEdgeCommand = new RelayCommand(AddEdge);
            RemoveEdgesCommand = new RelayCommand<IList>(RemoveEdges, CanRemoveEdges);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            MouseDownClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            MouseMoveClassBoxCommand = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            MouseUpClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);
           
        }

        // Tilføjer punkt med kommando.
        public void AddClassBox()
        {
            undoRedoController.AddAndExecute(new AddClassCommand(ClassBoxs));
        }

        // Tjekker om valgte punkt/er kan fjernes. Det kan de hvis der er nogle der er valgt.
        public bool CanRemoveClassBox(IList _classBoxs)
        {
            return _classBoxs.Count == 1;
        }

        // Fjerner valgte punkter med kommando.
        public void RemoveClassBox(IList _classBoxs)
        {
            undoRedoController.AddAndExecute(new RemoveClassCommand(ClassBoxs, Edges, _classBoxs.Cast<ClassBox>().First()));
        }

        // Starter proceduren der tilføjer en kant.
        public void AddEdge()
        {
            isAddingEdge = true;
            RaisePropertyChanged("ModeOpacity");
        }

        // Tjekker om valgte kant/er kan fjernes. Det kan de hvis der er nogle der er valgt.
        public bool CanRemoveEdges(IList _edges)
        {
            return _edges.Count > 0;
        }

        // Fjerner valgte kanter med kommando.
        public void RemoveEdges(IList _edges)
        {
            undoRedoController.AddAndExecute(new RemoveEdgesCommand(Edges, _edges.Cast<Edge>().ToList()));
        }

        // Hvis der ikke er ved at blive tilføjet en kant så fanges musen når en musetast trykkes ned. Dette bruges til at flytte punkter.
        public void MouseDownClassBox(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                e.MouseDevice.Target.CaptureMouse();
                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
                ClassBox movingClassBox = (ClassBox)movingClass.DataContext;
                movingClassBox.IsSelected = true;

                if (SelectedClassBox.Count == 0)
                {
                    SelectedClassBox.Add(movingClassBox);
                }
                else if(movingClassBox != SelectedClassBox.ElementAt(0))
                {
                    SelectedClassBox.ElementAt(0).IsSelected = false;
                    SelectedClassBox.Clear();
                    SelectedClassBox.Add(movingClassBox);
                }
                
            }
        }

        // Bruges til at flytter punkter.
        public void MouseMoveClassBox(MouseEventArgs e)
        {
            // Tjek at musen er fanget og at der ikke er ved at blive tilføjet en kant.
            if (Mouse.Captured != null && !isAddingEdge)
            {
                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
                ClassBox movingClassBox = (ClassBox)movingClass.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingClass);
                // Musens position i forhold til canvas skaffes her.
                Point mousePosition = Mouse.GetPosition(canvas);

                if (moveClassBoxPoint == default(Point))
                    moveClassBoxPoint = mousePosition;

                movingClassBox.X = (int)mousePosition.X;
                movingClassBox.Y = (int)mousePosition.Y;
            }
        }

        // Benyttes til at flytte punkter og tilføje kanter.
        public void MouseUpClassBox(MouseButtonEventArgs e)
        {
            FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
            ClassBox movingClassBox = (ClassBox)movingClass.DataContext;

            if (isAddingEdge)
            {
                // Hvis det er den første klasse der er blevet trykket på under tilføjelsen af kanten, så gemmes punktet bare og punktet bliver markeret som valgt.
                if (addingEdgeEndA == null)
                {
                    addingEdgeEndA = movingClassBox;
                    addingEdgeEndA.IsSelected = true;
                }
                // Ellers hvis det ikke er den første og de to noder der hører til klasserne er forskellige, så oprettes kanten med kommando.
                else if (addingEdgeEndA != movingClassBox)
                {
                    undoRedoController.AddAndExecute(new AddEdgeCommand(Edges, addingEdgeEndA, (ClassBox)movingClass.DataContext));
                    // De tilhørende værdier nulstilles.
                    isAddingEdge = false;
                    RaisePropertyChanged("ModeOpacity");
                    addingEdgeEndA.IsSelected = false;
                    addingEdgeEndA = null;
                }
            }
            else
            {
                    Canvas canvas = FindParentOfType<Canvas>(movingClass);
                    Point mousePosition = Mouse.GetPosition(canvas);
                    undoRedoController.AddAndExecute(new MoveClassBoxCommand(movingClassBox, (int)mousePosition.X, (int)mousePosition.Y, (int)moveClassBoxPoint.X, (int)moveClassBoxPoint.Y));
                    // Nulstil værdier.
                    moveClassBoxPoint = new Point();
                    // Musen frigøres.
                    e.MouseDevice.Target.ReleaseMouseCapture();
                }
            
        }

        // Rekursiv metode der benyttes til at finde et af et grafisk elements forfædre ved hjælp af typen, der ledes højere og højere op indtil en af typen findes.
        // Syntaksen "() ? () : ()" betyder hvis den første del bliver sand så skal værdien være den anden del, ellers skal den være den tredje del.
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
        // Bruges til at gøre punkterne gennemsigtige når en ny kant tilføjes.
        public double ModeOpacity
        {
            get
            {
                return isAddingEdge ? 0.4 : 1.0;
            }
        }
    }
}
using _02350_Gruppe5.Command;
using _02350_Gruppe5.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged  
    {
        // Holder styr på undo/redo.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        // Holder om en kant er ved at blive tilføjet
        private bool isAddingEdge;
        // Første endepunkt når en kant er ved at blive tilføjet
        private ClassBox addingEdgeEndA;

        // Gemmer det første punkt som punktet har under en flytning.
        private Point moveClassBoxPoint;
        private Point offsetPosition; //Bruges så klassen bliver flyttet flot rundt
        private int oldPosX;
        private int oldPosY;

        public ObservableCollection<ClassBox> ClassBoxs { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }
        public ObservableCollection<ClassBox> SelectedClassBox { get; set; }
        public Edge selectedEdge; // Holds an Edge if one is selected
        public ClassBox toPaste; // Holds a ClassBox if one is copied

        // Kommandoer som UI bindes til.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand AddClassCommand { get; private set; }
        public ICommand RemoveClassCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }

        public ICommand MouseDownClassBoxCommand { get; private set; }
        public ICommand MouseMoveClassBoxCommand { get; private set; }
        public ICommand MouseUpClassBoxCommand { get; private set; }

        public ICommand PasteClassCommand { get; private set; }
        public ICommand CopyClassCommand { get; private set; }
        public ICommand SaveProgram { get; private set; }
        public ICommand OpenProgram { get; private set; }
        public ICommand SaveToPictureCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }
        public ICommand ShutdownProgram { get; private set; }

        public ICommand AddMethodComm { get; private set; }
        public ICommand AddAttComm { get; private set; }

        public ICommand MouseDownEdgeCommand { get; private set; }
        public ICommand MouseUpEdgeCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }
        public ICommand Deselect { get; private set; }
        
        public MainViewModel()
        {
            SelectedClassBox = new ObservableCollection<ClassBox>();
            ClassBoxs = new ObservableCollection<ClassBox>(); // List of shown ClassBoxes
            Edges = new ObservableCollection<Edge>(); // List of shown Edges

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes. Her vidersendes metode kaldne til UndoRedoControlleren.
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            AddClassCommand = new RelayCommand(AddClassBox);
            RemoveClassCommand = new RelayCommand(RemoveClassBox, SelectedClass);
            AddEdgeCommand = new RelayCommand(AddEdge, canAddEdge);

            MouseDownEdgeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownEdge);
            MouseUpEdgeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpEdge);

            MouseDownClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            MouseMoveClassBoxCommand = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            MouseUpClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);

            CopyClassCommand = new RelayCommand(CopyClass, SelectedClass);
            PasteClassCommand = new RelayCommand(PasteClass, CanPaste);
            SaveProgram = new RelayCommand(saveProgram);
            OpenProgram = new RelayCommand(openProgram);
            SaveToPictureCommand = new RelayCommand<StackPanel>(saveScreen);
            PrintCommand = new RelayCommand<StackPanel>(printScreen);
            ShutdownProgram = new RelayCommand(CloseProgram);

            AddMethodComm = new RelayCommand(addMethod, SelectedClass);
            AddAttComm = new RelayCommand(addAtt, SelectedClass);

            DeleteCommand = new RelayCommand(DeleteEdgeAndClass, SelectedClassOrEdge);
        }
        // Add attribute
        public void addAtt()
        {
            undoRedoController.AddAndExecute(new AddAttCommand(ClassBoxs, SelectedClassBox));
        }
        // Add method
        public void addMethod()
        {
            undoRedoController.AddAndExecute(new AddMethodCommand(ClassBoxs, SelectedClassBox));
        }
        // Load saved program
        public void openProgram()
        {
            new OpenCommand(ClassBoxs, Edges);
        }
        // Add ClassBox to Grid
        public void AddClassBox()
        {
            undoRedoController.AddAndExecute(new AddClassCommand(ClassBoxs));
        }
        // Paste copied class
        public void PasteClass()
        {
            undoRedoController.AddAndExecute(new PasteClassCommand(ClassBoxs, toPaste));
            toPaste = null;
        }
        // Hold copy of selected ClassBox
        public void CopyClass()
        {
            toPaste = SelectedClassBox.ElementAt(0);
        }
        // is a ClassBox selected?
        public bool SelectedClass()
        {
            return SelectedClassBox.Count == 1;
        }
        // is anything selected?
        public bool SelectedClassOrEdge()
        {
            if (SelectedClassBox.Count == 1){return true;}
            else if (selectedEdge != null) { return true; }
            else{return false;}
        }
        // Holding something to paste?
        public bool CanPaste()
        {
            return toPaste != null;
        }
        // Can add Edge?
        public bool canAddEdge()
        {
            return ClassBoxs.Count >= 2;
        }
        // Save image of Grid
        public void saveScreen(StackPanel input)
        {
            new SavePictureCommand(input);
        }
        // Print Grid
        public void printScreen(StackPanel input)
        {
            new PrinterCommand(input);
        }
        // Remove ClassBox
        public void RemoveClassBox()
        {
            SelectedClassBox.ElementAt(0).IsSelected = false;
            undoRedoController.AddAndExecute(new RemoveClassCommand(ClassBoxs, Edges, SelectedClassBox.ElementAt(0)));
            SelectedClassBox.Clear();
        }
        // Add Edge
        public void AddEdge()
        {
            if (SelectedClassBox.Count >= 1)
            {
                SelectedClassBox.ElementAt(0).IsSelected = false;
            }
            SelectedClassBox.Clear();
            isAddingEdge = true;
            RaisePropertyChanged("ModeOpacity");
        }
        // Remove ClassBox and connected Edges
        public void DeleteEdgeAndClass()
        {
            if (selectedEdge != null)
            {
                selectedEdge.IsSelected = false;
                undoRedoController.AddAndExecute(new RemoveEdgesCommand(Edges, selectedEdge));
                selectedEdge = null;
            }
            else if (SelectedClassBox.Count == 1)
            {
                SelectedClassBox.ElementAt(0).IsSelected = false;
                undoRedoController.AddAndExecute(new RemoveClassCommand(ClassBoxs, Edges, SelectedClassBox.ElementAt(0)));
                SelectedClassBox.Clear();
            }
        }
        // Close program
        public void CloseProgram()
        {
            Application.Current.Shutdown();
        }
        // Action for Mouse down trigger on Edge
        public void MouseDownEdge(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                if (SelectedClassBox.Count == 1)
                {
                    SelectedClassBox.ElementAt(0).IsSelected = false;
                    SelectedClassBox.Clear();
                }
                e.MouseDevice.Target.CaptureMouse();
                FrameworkElement edgeElement = (FrameworkElement)e.MouseDevice.Target;
                Edge edge = (Edge)edgeElement.DataContext;
                edge.IsSelected = true;
                if (selectedEdge != null)
                {
                    selectedEdge.IsSelected = false;
                }
                selectedEdge = edge;
            }
        }
        // Action for Mouse up trigger on Edge
        public void MouseUpEdge(MouseButtonEventArgs e)
        {
            e.MouseDevice.Target.ReleaseMouseCapture();
        }
        // Action for Mouse down trigger on ClassBox
        // Hvis der ikke er ved at blive tilføjet en kant så fanges musen når en musetast trykkes ned. Dette bruges til at flytte punkter.
        public void MouseDownClassBox(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                if (selectedEdge != null)
                {
                    selectedEdge.IsSelected = false;
                    selectedEdge = null;
                }
                e.MouseDevice.Target.CaptureMouse();
                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
                ClassBox movingClassBox = (ClassBox)movingClass.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingClass);
                offsetPosition = Mouse.GetPosition(canvas);
                oldPosX = movingClassBox.X;
                oldPosY = movingClassBox.Y;
                
                if (SelectedClassBox.Count == 0)
                {
                    SelectedClassBox.Add(movingClassBox);
                }
                else
                {
                    SelectedClassBox.ElementAt(0).IsSelected = false;
                    SelectedClassBox.Clear();
                    SelectedClassBox.Add(movingClassBox);
                }
                movingClassBox.IsSelected = true;
            }
        }
        // Action for Mouse move trigger
        public void MouseMoveClassBox(MouseEventArgs e)
        {
            // Tjek at musen er fanget og at der ikke er ved at blive tilføjet en kant.
            if (Mouse.Captured != null && !isAddingEdge)
            {
                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
                ClassBox movingClassBox = (ClassBox)movingClass.DataContext;

                Canvas canvas = FindParentOfType<Canvas>(movingClass);
                Point mousePosition = Mouse.GetPosition(canvas);

                mousePosition.X -= offsetPosition.X;
                mousePosition.Y -= offsetPosition.Y;

                if (oldPosX + mousePosition.X >= 0){moveClassBoxPoint.X = movingClassBox.X = oldPosX + (int)mousePosition.X;}
                else{ moveClassBoxPoint.X = movingClassBox.X = 0;}

                if (oldPosY + mousePosition.Y >= 0){moveClassBoxPoint.Y = movingClassBox.Y = oldPosY + (int)mousePosition.Y;}
                else{moveClassBoxPoint.Y = movingClassBox.Y = 0;}

                // Updating the edges associated with the classbox being moved
                foreach (Edge edge in Edges)
                {
                    if (movingClassBox.Equals(edge.EndA))
                    {
                        edge.Points = new Edge(movingClassBox, edge.EndB).Points;
                    }
                    if (movingClassBox.Equals(edge.EndB))
                    {
                        edge.Points = new Edge(edge.EndA, movingClassBox).Points;
                    }
                }
            }
        }
        // Action for Mouse up trigger on ClassBox
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
            else if (moveClassBoxPoint != default(Point))
            {
                Canvas canvas = FindParentOfType<Canvas>(movingClass);
                Point mousePosition = Mouse.GetPosition(canvas);
                undoRedoController.AddAndExecute(new MoveClassBoxCommand(movingClassBox, Edges, movingClassBox.X, movingClassBox.Y, (int)oldPosX, (int)oldPosY));
                // Nulstil værdier.
                moveClassBoxPoint = new Point();
                // Musen frigøres.
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
            else
                e.MouseDevice.Target.ReleaseMouseCapture();
        }

        // Rekursiv metode der benyttes til at finde et af et grafisk elements forfædre ved hjælp af typen, der ledes højere og højere op indtil en af typen findes.
        // Syntaksen "() ? () : ()" betyder hvis den første del bliver sand så skal værdien være den anden del, ellers skal den være den tredje del.
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
        // Bruges til at gøre punkterne gennemsigtige når en ny kant tilføjes.
        public double ModeOpacity{get{return isAddingEdge ? 0.4 : 1.0;}}

        /////////////////////////////////Save /////////////////////////////////////////
        BackgroundWorker bw = new BackgroundWorker();

        private int progress = 0;
        private String status = "Test";
        public String Status { get { return status; } set { status = value; OnPropertyChanged("Status"); } }
        public int Progress { get { return progress; } set { progress = value; OnPropertyChanged("Progress"); } }

        public void saveProgram()
        {
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
        }
        public void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            new SaveCommand(ClassBoxs, Edges, sender, e);
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true)) { Status = "Canceled!"; }
            else if (!(e.Error == null)) { Status = ("Error: " + e.Error.Message); }
            else { Status = "Done!"; Progress = 100; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
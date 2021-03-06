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
        // Dette er vores undo/redo controller der holder styr paa alle de commands der kan undo/redods. 
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        // Er der ved at blive tilfojet en kant?
        private bool isAddingEdge;
        
        // F�rste endepunkt n�r en kant er ved at blive tilf�jet
        private ClassBox addingEdgeEndA;

        //Punkter n�r der flyttes rundt. 
        private Point moveClassBoxPoint;// Gemmer det f�rste punkt som punktet har under en flytning.
        private Point offsetPosition; //Bruges s� klassen bliver flyttet flot rundt
        private int oldPosX; // bruges naar moveClassCommand kaldes
        private int oldPosY;// bruges naar moveClassCommand kaldes
        
        
        //Boolean til at tjekke om der skal gemmes foer der laves et nyt diagram.
        private Boolean saveBeforeNew;

        //Dette er alle classes og edges i vores program:
        public ObservableCollection<ClassBox> ClassBoxs { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }
        
        //Foelgende observable collection bruges til at holde styr paa den valgte klasse. 
        //Kunne implementeres som kun en ClassBox, men vi har valgt at lave det som en liste,
        //hvis der evt. skal fjernes flere paa et tidspunkt. 
        public ObservableCollection<ClassBox> SelectedClassBox { get; set; }
        public Edge selectedEdge; // Holds an Edge if one is selected
        public ClassBox toPaste; // Holds a ClassBox if one is copied

        // F�lgende commands er bundet til vores GUI. 
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand AddClassCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }
        public ICommand ReverseEdgeCommand { get; private set; }
        
        public ICommand RemoveClassCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand MouseDownClassBoxCommand { get; private set; }
        public ICommand MouseMoveClassBoxCommand { get; private set; }
        public ICommand MouseUpClassBoxCommand { get; private set; }

        public ICommand MouseDownEdgeCommand { get; private set; }
        public ICommand MouseUpEdgeCommand { get; private set; }

        public ICommand PasteClassCommand { get; private set; }
        public ICommand CopyClassCommand { get; private set; }

        public ICommand SaveProgram { get; private set; }
        public ICommand OpenProgram { get; private set; }
        public ICommand ShutdownProgram { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand SaveToPictureCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }
        
        public ICommand AddMethodComm { get; private set; }
        public ICommand AddAttComm { get; private set; }

        //public ICommand Deselect { get; private set; }
        
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
            AddEdgeCommand = new RelayCommand(AddEdge, canAddEdge);
            ReverseEdgeCommand = new RelayCommand(ChangeArrow,EdgeSelected);

            RemoveClassCommand = new RelayCommand(DeleteEdgeAndClass, SelectedClass);
            DeleteCommand = new RelayCommand(DeleteEdgeAndClass, SelectedClassOrEdge);

            MouseDownEdgeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownEdge);
            MouseUpEdgeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpEdge);

            MouseDownClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownClassBox);
            MouseMoveClassBoxCommand = new RelayCommand<MouseEventArgs>(MouseMoveClassBox);
            MouseUpClassBoxCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpClassBox);

            CopyClassCommand = new RelayCommand(CopyClass, SelectedClass);
            PasteClassCommand = new RelayCommand(PasteClass, CanPaste);
            
            SaveProgram = new RelayCommand(saveProgram);
            OpenProgram = new RelayCommand(openProgram);
            NewCommand = new RelayCommand(newDiagram);
            ShutdownProgram = new RelayCommand(CloseProgram);
            SaveToPictureCommand = new RelayCommand<StackPanel>(saveScreen);
            PrintCommand = new RelayCommand<StackPanel>(printScreen);
            

            AddMethodComm = new RelayCommand(addMethod, SelectedClass);
            AddAttComm = new RelayCommand(addAtt, SelectedClass);
        }

        // Change arrow direction on Edge
        public void ChangeArrow()
        {
            if (selectedEdge != null)
            {
                undoRedoController.AddAndExecute(new ChangeArrowCommand(Edges, selectedEdge));
            }
            selectedEdge = null;
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

        //////////////////////////////////////Is selected/////////////////////////////////////
        // is a edge selected?
        private Boolean EdgeSelected()
        {
            return selectedEdge != null;
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


        /////////////////////////Add class methods///////////////////////////////
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
        // Add ClassBox to Grid
        public void AddClassBox()
        {
            undoRedoController.AddAndExecute(new AddClassCommand(ClassBoxs));
        }

        //////////////////////Add edge/////////////////////////////////
        // Can add Edge?
        public bool canAddEdge()
        {
            return ClassBoxs.Count >= 2;
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


        //////////////////Mouse actions//////////////////////////////////

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
        // Hvis der ikke er ved at blive tilf�jet en kant s� fanges musen n�r en musetast trykkes ned. Dette bruges til at flytte punkter.
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
            // Tjek at musen er fanget og at der ikke er ved at blive tilf�jet en kant.
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
        // Benyttes til at flytte punkter og tilf�je kanter.
        public void MouseUpClassBox(MouseButtonEventArgs e)
        {
            FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
            ClassBox movingClassBox = (ClassBox)movingClass.DataContext;

            if (isAddingEdge)
            {
                // Hvis det er den f�rste klasse der er blevet trykket p� under tilf�jelsen af kanten, s� gemmes punktet bare og punktet bliver markeret som valgt.
                if (addingEdgeEndA == null)
                {
                    addingEdgeEndA = movingClassBox;
                    addingEdgeEndA.IsSelected = true;
                }
                // Ellers hvis det ikke er den f�rste og de to noder der h�rer til klasserne er forskellige, s� oprettes kanten med kommando.
                else if (addingEdgeEndA != movingClassBox)
                {
                    undoRedoController.AddAndExecute(new AddEdgeCommand(Edges, addingEdgeEndA, (ClassBox)movingClass.DataContext));
                    // De tilh�rende v�rdier nulstilles.
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
                // Nulstil v�rdier.
                moveClassBoxPoint = new Point();
                // Musen frig�res.
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
            else
                e.MouseDevice.Target.ReleaseMouseCapture();
        }

        ////////////////////////////// Copy/paste //////////////////////////////////
        // Hold copy of selected ClassBox
        public void CopyClass()
        {
            toPaste = SelectedClassBox.ElementAt(0);
        }
        // Holding something to paste?
        public bool CanPaste()
        {
            return toPaste != null;
        }
        // Paste copied class
        public void PasteClass()
        {
            undoRedoController.AddAndExecute(new PasteClassCommand(ClassBoxs, toPaste));
            toPaste = null;
        }

        //////////////////////////////////////////////Print picture and to print///////////////////////////////////
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



        /////////////////////////////////Save, load, close and new /////////////////////////////////////////
        BackgroundWorker bw = new BackgroundWorker();

        private int progress = 0;
        private String status = "Ready";
        private String filename = null;
        public String Status { get { return status; } set { status = value; OnPropertyChanged("Status"); } }
        public int Progress { get { return progress; } set { progress = value; OnPropertyChanged("Progress"); } }

        public void saveProgram()
        {
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            
            // Process save file dialog box results 
            if (result == true)
            {
                if (bw.IsBusy != true)
                {
                    filename = dlg.FileName;
                    bw.RunWorkerAsync();
                }
            }
        }
        public void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            new SaveCommand(ClassBoxs, Edges, sender, e,filename);
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true)) { Status = "Canceled!"; }
            else if (!(e.Error == null)) { Status = ("Error: " + e.Error.Message); }
            else 
            { 
                Status = "Done!"; Progress = 100;
                if (saveBeforeNew)
                {
                    clearDiagram();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        // Load saved program
        public void openProgram()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                clearDiagram();
                string name = dlg.FileName;
                new OpenCommand(ClassBoxs, Edges, name);
            }
        }
        private void newDiagram()
        {
            string message = "Vil du gemme �ndringer?";
            MessageBoxButton b = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(message, "Vil du gemme", b, icon);

            if (result == MessageBoxResult.Yes)
            {
                saveBeforeNew = true;
                saveProgram();
            }
            else if (result == MessageBoxResult.No)
            {
                clearDiagram();
            }
        }
        // Close program
        public void CloseProgram()
        {
            Application.Current.Shutdown();
        }
        //Clear diagram
        private void clearDiagram()
        {
            SelectedClassBox.Clear();
            ClassBoxs.Clear();
            Edges.Clear();
            selectedEdge = null;
            Status = "Ready";
            Progress = 0;
            saveBeforeNew = false;
            toPaste = null;
            filename = null;
        }


        // Rekursiv metode der benyttes til at finde et af et grafisk elements forf�dre ved hj�lp af typen, der ledes h�jere og h�jere op indtil en af typen findes.
        // Syntaksen "() ? () : ()" betyder hvis den f�rste del bliver sand s� skal v�rdien v�re den anden del, ellers skal den v�re den tredje del.
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
        // Bruges til at g�re punkterne gennemsigtige n�r en ny kant tilf�jes.
        public double ModeOpacity { get { return isAddingEdge ? 0.4 : 1.0; } }
    }

}
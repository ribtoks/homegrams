using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HierarchyClasses;
using ExtraClasses = HierarchyClasses.MatrixClasses;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Markup;
using ExternalClasses;
using System.Threading;
using AHP;
using System.Runtime.Remoting.Messaging;
using System.Windows.Threading;

namespace HierarchyInterface
{
    public delegate SolverResults SolveDelegate(OrientatedGraph<int, ExtraClasses.Matrix> graph);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Class Properties (that in normal project must be organized in dictionary and serialized to registry or file)

        string nodeButtonName = "nodeButton";
        string nodeLabelName = "nodeLabel";
        string hierarchyRowName = "hierarchyRow";
        string lineName = "line";

        string matrixNodeButtonName = "showMatrixButton";
        
        double minDegree = 0.1;
        double maxDegree = 10.0;
        double smallChange = 0.1;
        double largeChange = 0.5;

        double maxLabelWidth = 100.0;

        #endregion

        #region Class Data

        int addedNodesCount = 0;
        bool drawingArrowNow = false;
        Line lastLine = null;
        int rowsCount = 0;
        Grid lastElementsGrid = null;
        // dictionary of connectors - [rowindex -> list of lines, which start on it]
        Dictionary<int, List<Line>> connectors = new Dictionary<int, List<Line>>();
        // dictionary of connectors - [line -> rowindex, on which it starts]
        Dictionary<Line, int> connectorsIndices = new Dictionary<Line, int>();
        int lastStartRow = -1;
        int lastEndRow = -1;
        int lastNodeId = -1;
        int lastClickedId = -1;

        string lastExpert = string.Empty;

        // graph of all nodes with matrices
        OrientatedGraph<int, ExpertsMatrix> graph = new OrientatedGraph<int, ExpertsMatrix>();

        SolveDelegate sd;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            sd = SolveProblems;

            myClickWaitTimer =
                new DispatcherTimer(
                new TimeSpan(0, 0, 0, 0, 500),
                DispatcherPriority.Background,
                mouseWaitTimer_Tick,
                Dispatcher.CurrentDispatcher);
        }

        void removeButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var grid = btn.Parent as Grid;

            // last row of buttons in hierarchy stack
            var lastStackElement = hierarchyStack.Children[hierarchyStack.Children.Count - 1] as Grid;

            // deprecated. problem solved using bindings
            // (code is here for backward compatability)
            if ((lastStackElement == null) || lastStackElement != grid)
            {
                MessageBox.Show("You can remove only last hierarchy row", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            if (MessageBox.Show("You're going to remove last row. Proceed?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // first remove all buttons
                int childrenCount = hierarchyStack.Children.Count - 1;
                hierarchyStack.UnregisterName((hierarchyStack.Children[childrenCount] as Grid).Name);

                // remove entries from graph
                foreach (var button in ((hierarchyStack.Children[childrenCount] as Grid).Children[0] as Grid).Children.OfType<Button>())
                {
                    string sid = button.Name.Substring(nodeButtonName.Length);
                    int id = Convert.ToInt32(sid);

                    // delete apex with all connections to it from other cells
                    // O(n + m) complexity
                    graph.DeleteApex(id);
                }

                // remove last row
                hierarchyStack.Children.RemoveAt(hierarchyStack.Children.Count - 1);
                --rowsCount;

                if (hierarchyStack.Children.Count == 0)
                    return;

                // and now remove all lines
                var lines = connectors[hierarchyStack.Children.Count - 1];
                foreach (Line line in lines)
                {
                    int[] ids = getIdsFromName(line.Name);
                    graph.RemoveEdge(ids[0], ids[1]);
                    connectorsIndices.Remove(line);
                    
                    wrapperGrid.Children.Remove(line);
                }
                connectors[hierarchyStack.Children.Count - 1].Clear();
            }
        }

        void wrapperGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawingArrowNow)
            {
                var point = e.GetPosition(wrapperGrid);

                lastLine.X2 = (int)point.X;
                lastLine.Y2 = (int)point.Y;

                // some hacks're here - for mouse 
                // pointer was beyond line

                if (lastLine.X2 > lastLine.X1)
                    lastLine.X2 -= 2;
                else
                    lastLine.X2 += 2;

                if (lastLine.Y2 > lastLine.Y1)
                    lastLine.Y2 -= 2;
                else
                    lastLine.Y2 += 4;
            }
        }

        private void innerEllipseButton_Click(object sender, RoutedEventArgs e)
        {
            var innerButton = sender as Button;
            var ellipse = (Ellipse)innerButton.Template.FindName("ellipse", innerButton);
            var outerButton = innerButton.TemplatedParent as Button;
            // parent of parent of button
            var elementsGrid = outerButton.Parent as Grid;
            var clickedGrid = (outerButton.Parent as Grid).Parent as Grid;
            string buttonNumber = outerButton.Name.Substring(nodeButtonName.Length);

            #region A lot of stuff
            if (!drawingArrowNow)
            {
                Line line = new Line();
                line.Stroke = Brushes.DimGray;
                line.StrokeThickness = 4;
                line.StrokeEndLineCap = PenLineCap.Round;
                line.StrokeStartLineCap = PenLineCap.Triangle;

                var point = ellipse.TransformToAncestor(wrapperGrid).Transform(new Point(0, 0));
                point.X += ellipse.ActualWidth / 2;
                point.Y += ellipse.ActualHeight / 2;

                line.X1 = point.X;
                line.Y1 = point.Y;

                line.X2 = line.X1;
                line.Y2 = line.Y1;

                drawingArrowNow = true;
                lastLine = line;

                wrapperGrid.Children.Add(line);
                Grid.SetRowSpan(line, wrapperGrid.RowDefinitions.Count);

                // add line binding

                MultiBinding lineX1Binding = new MultiBinding();
                
                lineX1Binding.Bindings.Add(new Binding("ActualWidth") 
                    {ElementName=string.Format("{0}{1}", nodeLabelName, buttonNumber), Mode=BindingMode.OneWay});

                lineX1Binding.Bindings.Add(new Binding("ActualWidth") 
                    {Source=wrapperGrid, Mode=BindingMode.OneWay});
                lineX1Binding.Converter = new EllipseCoordsXConverter();
                lineX1Binding.ConverterParameter = new object[] { ellipse, wrapperGrid };
                line.SetBinding(Line.X1Property, lineX1Binding);

                Binding lineY1Binding = new Binding("Margin") { Source = clickedGrid, Mode = BindingMode.OneWay };
                lineY1Binding.Mode = BindingMode.OneWay;
                lineY1Binding.Converter = new EllipseCoordsYConverter();
                lineY1Binding.ConverterParameter = new object[] { ellipse, wrapperGrid };
                line.SetBinding(Line.Y1Property, lineY1Binding);

                lastNodeId = Convert.ToInt32(buttonNumber);
                lastStartRow = hierarchyStack.Children.IndexOf(clickedGrid);
            }
            else
            {
                lastEndRow = hierarchyStack.Children.IndexOf(clickedGrid);

                if (Math.Abs(lastStartRow - lastEndRow) != 1)
                {
                    MessageBox.Show("You can only connect items on two consecutive rows", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                    // remove this line
                    wrapperGrid.Children.RemoveAt(wrapperGrid.Children.Count - 1);

                    // do not pass click on inner button
                    // ahead for generating click on outer button
                    e.Handled = true;

                    lastLine = null;
                    drawingArrowNow = false;
                    lastEndRow = -1;
                    lastStartRow = -1;

                    return;
                }

                // fix the line there

                var point = ellipse.TransformToAncestor(wrapperGrid).Transform(new Point(0, 0));
                point.X += ellipse.ActualWidth / 2;
                point.Y += ellipse.ActualHeight / 2;

                lastLine.X2 = point.X;
                lastLine.Y2 = point.Y;

                lastLine.StrokeStartLineCap = PenLineCap.Round;

                // add line binding
                MultiBinding lineX2Binding = new MultiBinding();

                lineX2Binding.Bindings.Add(new Binding("ActualWidth") { ElementName = string.Format("{0}{1}", nodeLabelName, buttonNumber), Mode = BindingMode.OneWay });

                lineX2Binding.Bindings.Add(new Binding("ActualWidth") { Source = wrapperGrid, Mode = BindingMode.OneWay });
                lineX2Binding.Converter = new EllipseCoordsXConverter();
                lineX2Binding.ConverterParameter = new object[] { ellipse, wrapperGrid };
                lastLine.SetBinding(Line.X2Property, lineX2Binding);

                Binding lineY2Binding = new Binding("Margin") { Source = clickedGrid, Mode = BindingMode.OneWay };
                lineY2Binding.Mode = BindingMode.OneWay;
                lineY2Binding.Converter = new EllipseCoordsYConverter();
                lineY2Binding.ConverterParameter = new object[] { ellipse, wrapperGrid };
                lastLine.SetBinding(Line.Y2Property, lineY2Binding);

                // save different system information

                int row = Math.Min(lastStartRow, lastEndRow);
                connectors[row].Add(lastLine);
                connectorsIndices[lastLine] = row;

                int currNodeId = Convert.ToInt32(buttonNumber);
                int minId = lastNodeId, maxId = currNodeId;
                
                if (lastStartRow > lastEndRow)
                {
                    minId = currNodeId;
                    maxId = lastNodeId;
                }

                lastLine.Name = string.Format("{0}{1}_{2}", lineName, minId, maxId);

                if (wrapperGrid.FindName(lastLine.Name) != null)
                    wrapperGrid.UnregisterName(lastLine.Name);
                wrapperGrid.RegisterName(lastLine.Name, lastLine);
                // add edge to graph
                graph.AddEdge(minId, maxId);

                // deletion of connector is implemented by right mouse click
                lastLine.MouseRightButtonUp += delegate(object s, MouseButtonEventArgs me)
                {
                    var line = s as Line;
                    int[] ids = getIdsFromName(line.Name);
                    int id1 = ids[0];
                    int id2 = ids[1];

                    string s1 = (hierarchyStack.FindName(string.Format("{0}{1}", nodeButtonName, id1)) as Button).Content as string;
                    string s2 = (hierarchyStack.FindName(string.Format("{0}{1}", nodeButtonName, id2)) as Button).Content as string;

                    if (MessageBox.Show(string.Format("Do you really want to remove line between {0} and {1}?", s1, s2), "Question",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        graph.RemoveEdge(id1, id2);
                        
                        connectors[ connectorsIndices[line] ].Remove(line);
                        connectorsIndices.Remove(line);
                        
                        wrapperGrid.Children.Remove(line);
                    }
                };

                lastNodeId = -1;
                drawingArrowNow = false;
                lastLine = null;
                lastEndRow = -1;
                lastStartRow = -1;                
            }
            e.Handled = true;

            #endregion
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addNewRow(rowsCount);
            rowsCount++;
        }

        /// <summary>
        /// Adds one button to current row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var addButton = sender as Button;
            var grid = addButton.Parent as Grid;
            var elementsGrid = grid.Children[0] as Grid;

            lastElementsGrid = elementsGrid;

            valueTextBox.Text = "";
            Storyboard sb = (Storyboard)this.Resources["showOverlay"];
            sb.Begin();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            processNewNode();
        }

        private void valueTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                processNewNode();
            }
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            lastElementsGrid = null;
            Storyboard sb = (Storyboard)this.Resources["hideOverlay"];
            sb.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            valueTextBox.Focus();
        }
        
        // click for showing or generating matrix with grid of sliders
        void btn_Click(object sender)//, RoutedEventArgs e)
        {
            if (drawingArrowNow)
                return;

            ((Storyboard)this.Resources["showMatricesOverlay"]).Begin();
            lastClickedId = Convert.ToInt32((sender as Button).Name.Substring(nodeButtonName.Length));
            
            foreach (var keyvalue in graph.Data[lastClickedId].Matrices)
            {
                addMatrixEditButton(keyvalue.Key, lastClickedId);
            }
        }

        private void cancelMatrix_Click(object sender, RoutedEventArgs e)
        {
            //lastClickedId = -1;
            (this.Resources["hideMatrixOverlay"] as Storyboard).Begin();
        }

        private void submitMatrix_Click(object sender, RoutedEventArgs e)
        {
            if (matrixGrid.Children.Count == 0)
            {
                (this.Resources["hideMatrixOverlay"] as Storyboard).Begin();
                //lastClickedId = -1;
                return;
            }

            // submit a matrid to my graph class instance
            int count = matrixGrid.RowDefinitions.Count - 1;

            graph.Data[lastClickedId].Matrices[lastExpert] = ExtraClasses.Matrix.Get_E(count);
            foreach (var uielement in matrixGrid.Children.OfType<Slider>())
            {
                int i = Grid.GetRow(uielement);
                int j = Grid.GetColumn(uielement);

                // if upper right triangle of square matrix
                if ((i >= 1) && (j >= i + 1))
                {
                    double value = uielement.Value;

                    graph.Data[lastClickedId].Matrices[lastExpert][i - 1, j - 1] = value;
                    graph.Data[lastClickedId].Matrices[lastExpert][j - 1, i - 1] = 1.0 / value;
                }
            }
            (this.Resources["hideMatrixOverlay"] as Storyboard).Begin();
            //lastClickedId = -1;
        }

        // save hierarchy to file
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Hierarchy Files (.hrr)|*.hrr|All Files|*.*";
            dlg.DefaultExt = ".hrr";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    // save hierarchy there
                    var formatter = new BinaryFormatter();
                    var stream = new FileStream(System.IO.Path.GetFileNameWithoutExtension(dlg.FileName) + ".hrr",
                        FileMode.Create, FileAccess.Write);

                    formatter.Serialize(stream, graph);
                    formatter.Serialize(stream, serializeHierarchyStack());
                    formatter.Serialize(stream, serializeLines());
                    formatter.Serialize(stream, serializeConnectors());
                    formatter.Serialize(stream, serializeConnectorsIndices());
                    stream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while saving. Connect to developer." + Environment.NewLine + 
                        Environment.NewLine + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // deserialize hierarchy from file
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Hierarchy Files (.hrr)|*.hrr";
            if (dlg.ShowDialog() == true)
            {
                var str = string.Empty;
                
                try
                {
                    var formatter = new BinaryFormatter();
                    var stream = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);

                    graph = (OrientatedGraph<int, ExpertsMatrix>)formatter.Deserialize(stream);
                    deserializeHierarchyStack((List<List<string>>)formatter.Deserialize(stream));
                    deserializeLines((List<string>)formatter.Deserialize(stream));
                    // do some hacks for WPF build their VisualTree
                    hierarchyStack.UpdateLayout();
                    wrapperGrid.UpdateLayout();
                    // ... and continue deserializing
                    deserializeConnectors((Dictionary<int, List<string>>)formatter.Deserialize(stream));
                    deserializeConnectorsIndices((Dictionary<string, int>)formatter.Deserialize(stream));
                    stream.Close();
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw;
#else
                    MessageBox.Show("Error while restoring. Connect to developer." + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
                }
            }
        }

        private void addNewMatrix_Click(object sender, RoutedEventArgs e)
        {
            (this.Resources["showExpertNameOverlay"] as Storyboard).Begin();
            expertNameTextBox.Text = string.Empty;
        }

        private void submitNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(expertNameTextBox.Text.Trim()))
            {
                MessageBox.Show("Cannot create an expert with empty name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            submitName();
            expertNameTextBox.Text = string.Empty;
        }

        private void cancelNameButton_Click(object sender, RoutedEventArgs e)
        {
            lastExpert = string.Empty;
            expertNameTextBox.Text = string.Empty;
            (this.Resources["hideExpertNameOverlay"] as Storyboard).Begin();
        }

        private void submitMatrices_Click(object sender, RoutedEventArgs e)
        {
            lastClickedId = -1;
            lastExpert = string.Empty;
            (this.Resources["hideMatricesOverlay"] as Storyboard).Begin();
            matricesStack.Children.Clear();
        }

        private void deleteCurrMatrix_Click(object sender, RoutedEventArgs e)
        {
            var outerButton = (sender as Button).TemplatedParent as Button;
            string id = outerButton.Name.Substring(matrixNodeButtonName.Length);
            
            string expertName = outerButton.Content as string;

            if (MessageBox.Show("Delete matrix from expert " + expertName + "?", "Question", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                matricesStack.Children.Remove(outerButton);

                graph.Data[lastClickedId].Matrices.Remove(expertName);
                graph.Data[lastClickedId].ExpertIds.Remove(expertName);
            }

            e.Handled = true;
        }

        private void Storyboard_Completed_1(object sender, EventArgs e)
        {
            expertNameTextBox.Focus();
        }

        private void expertNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submitName();
            }
        }



        private void submitHierarchy_Click(object sender, RoutedEventArgs e)
        {
            ((Storyboard)this.Resources["showWatingResults"]).Begin();
            ((Storyboard)this.Resources["lineAnimation"]).Begin();

            sd.BeginInvoke(GetUserGraph(), new AsyncCallback(delegate(IAsyncResult iar)
            {
                AsyncResult ar = (AsyncResult)iar;

                var sr = ((SolveDelegate)ar.AsyncDelegate).EndInvoke(iar);

                resultsStack.Dispatcher.Invoke(new ThreadStart(delegate()
                    {
                        ((Storyboard)this.Resources["lineAnimation"]).Stop();
                        ShowResults(sr);
                    }));

            }), null);
            /*
            DoSomeTask db = brr;
            db.BeginInvoke(delegate(IAsyncResult ar)
            {
            }, null);
            */
        }

        protected void ShowResults(SolverResults sr)
        {
            ((Storyboard)this.Resources["showResults"]).Begin();
            ((Storyboard)this.Resources["hideWatingResults"]).Begin();
            resultsStack.Children.Clear();

            Label indexLabel = new Label();
            indexLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            indexLabel.FontSize = 15;
            indexLabel.Foreground = Brushes.White;
            indexLabel.FontWeight = FontWeights.Bold;
            indexLabel.Content = string.Format("Consistency Index is {0}", sr.ConsistencyIndex);
            indexLabel.Margin = new Thickness(0, 0, 0, 10);
            resultsStack.Children.Add(indexLabel);

            SortedDictionary<double, int> sortedValues = new SortedDictionary<double, int>();
            foreach (var keyvalue in sr.PriorityDictionary)
            {
                sortedValues[keyvalue.Value] = keyvalue.Key;
            }

            foreach (var keyvalue in sortedValues.Reverse())
            {
                Label lbl = new Label();
                lbl.Foreground = Brushes.White;
                lbl.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                lbl.FontSize = 14;

                string value = keyvalue.Value.ToString();
                try
                {
                    value = (hierarchyStack.FindName(nodeButtonName + value) as Button).Content as string;
                }
                catch { }

                lbl.Content = string.Format("{0}  -  {1}", value, keyvalue.Key);
                resultsStack.Children.Add(lbl);
            }
        }

        private void stopProcessButton_Click(object sender, RoutedEventArgs e)
        {
            ((Storyboard)this.Resources["lineAnimation"]).Stop();
            ((Storyboard)this.Resources["hideWatingResults"]).Begin();
            //sd.EndInvoke(null);
        }

        protected SolverResults SolveProblems(OrientatedGraph<int, ExtraClasses.Matrix> myGraph)
        {
            SolverResults sr = new SolverResults(new Dictionary<int, double>(), 0.0);
            Thread.Sleep(2000);
            try
            {
                sr = AHPSolver.Solve(myGraph);
            }
            catch
            {
#if DEBUG
                throw;
#else
                ;
#endif
            }

            return sr;
        }

        private void closeResultsButton_Click(object sender, RoutedEventArgs e)
        {
            ((Storyboard)this.Resources["hideResults"]).Begin();
        }

        private void wrapperGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (drawingArrowNow)
            {
                wrapperGrid.Children.RemoveAt(wrapperGrid.Children.Count - 1);

                e.Handled = true;

                lastLine = null;
                drawingArrowNow = false;
                lastEndRow = -1;
                lastStartRow = -1;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Some day help will be here");
        }

        private void lineIntervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (hierarchyStack == null)
                return;

            e.Handled = false;

            foreach (var grid in hierarchyStack.Children.OfType<Grid>())
            {
                grid.GetBindingExpression(Grid.MarginProperty).UpdateTarget();
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (hierarchyStack == null)
                return;

            if (wrapperGrid == null)
                return;

            foreach (var line in wrapperGrid.Children.OfType<Line>())
            {
                line.GetBindingExpression(Line.Y1Property).UpdateTarget();
                line.GetBindingExpression(Line.Y2Property).UpdateTarget();
            }

            try
            {
                wrapperGrid.GetBindingExpression(Grid.ActualWidthProperty).UpdateSource();
                lineIntervalSlider.GetBindingExpression(Slider.ValueProperty).UpdateSource();
            }
            catch { }
        }
    }
}

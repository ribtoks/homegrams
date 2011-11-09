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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Markup;
using System;
using ExternalClasses;
using ExtraClasses = HierarchyClasses.MatrixClasses;
using System.Windows.Threading;

namespace HierarchyInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer myClickWaitTimer;
        private Button lastButton;
        private bool firstTick = true;

        private void mouseWaitTimer_Tick(object sender, EventArgs e)
        {
            myClickWaitTimer.Stop();

            if (firstTick)
            {
                firstTick = false;
                return;
            }

            btn_Click(lastButton);
            lastButton = null;
        }

        private Grid addNewRow(int rowIndex)
        {
            Grid grid = new Grid();
            grid.Name = string.Format("{0}{1}", hierarchyRowName, rowIndex);

            Binding marginBinding = new Binding("Value");
            marginBinding.Converter = new DoubleToThicknessConverter();
            marginBinding.Source = lineIntervalSlider;
            marginBinding.Mode = BindingMode.TwoWay;
            grid.SetBinding(Grid.MarginProperty, marginBinding);

            // left margin
            ColumnDefinition cl1 = new ColumnDefinition();
            cl1.Width = new GridLength(10.0);
            // content
            ColumnDefinition cl2 = new ColumnDefinition();
            cl2.Width = new GridLength(1.0, GridUnitType.Star);
            // button "add new button"
            ColumnDefinition cl3 = new ColumnDefinition();
            cl3.Width = new GridLength(20.0);
            // spacer
            ColumnDefinition cl4 = new ColumnDefinition();
            cl4.Width = new GridLength(5.0);
            // button "delete this row"
            ColumnDefinition cl5 = new ColumnDefinition();
            cl5.Width = new GridLength(20.0);

            grid.ColumnDefinitions.Add(cl1);
            grid.ColumnDefinitions.Add(cl2);
            grid.ColumnDefinitions.Add(cl3);
            grid.ColumnDefinitions.Add(cl4);
            grid.ColumnDefinitions.Add(cl5);

            // ----- add childs ------
            Grid elementsGrid = new Grid();
            elementsGrid.Name = string.Format("nodesGrid{0}", rowIndex);
            elementsGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            grid.Children.Add(elementsGrid);
            Grid.SetColumn(elementsGrid, 1);
            //grid.RegisterName(elementsGrid.Name, elementsGrid);

            // ----- add add-remove buttons ------
            Button addButton = new Button();
            addButton.Style = (Style)this.Resources["AddButton"];
            addButton.Click += Button_Click_1;
            grid.Children.Add(addButton);
            Grid.SetColumn(addButton, 2);

            Button removeButton = new Button();
            removeButton.Name = string.Format("removeButton{0}", rowIndex);
            removeButton.Style = (Style)this.Resources["CloseButton"];
            removeButton.Click += new RoutedEventHandler(removeButton_Click);
            grid.Children.Add(removeButton);
            Grid.SetColumn(removeButton, 4);

            // ---- now add new row to visual elements -----
            connectors[hierarchyStack.Children.Count] = new List<Line>();
            hierarchyStack.Children.Add(grid);
                
            if (hierarchyStack.FindName(grid.Name) != null)
                hierarchyStack.UnregisterName(grid.Name);
            hierarchyStack.RegisterName(grid.Name, grid);

            // button "remove this row" is enabled only on last row
            Binding enabledBinding = new Binding("Children.Count");
            enabledBinding.Source = hierarchyStack;
            enabledBinding.Converter = new CountToBoolConverter();
            enabledBinding.ConverterParameter = Convert.ToInt32(grid.Name.Substring(hierarchyRowName.Length));
            enabledBinding.Mode = BindingMode.OneWay;
            removeButton.SetBinding(Button.IsEnabledProperty, enabledBinding);

            return grid;
        }

        protected void processNewNode()
        {
            var str = valueTextBox.Text.Trim();
            if (str.Length == 0)
            {
                MessageBox.Show("Please, enter some name");
                return;
            }

            Storyboard sb = (Storyboard)this.Resources["hideOverlay"];
            sb.Begin();

            addNewButton(lastElementsGrid, str, addedNodesCount);

            // add new apex to graph
            graph.AddApex(addedNodesCount, new ExpertsMatrix());

            ++addedNodesCount;

            lastElementsGrid = null;
        }

        protected Button addNewButton(Grid grid, string buttonContent, int buttonId)
        {
            ColumnDefinition eg_cl = new ColumnDefinition();
            eg_cl.Width = new GridLength(1.0, GridUnitType.Star);
            eg_cl.MinWidth = 60.0;

            grid.ColumnDefinitions.Add(eg_cl);

            // add button and label

            Button btn = new Button();
            btn.Name = string.Format("{0}{1}", nodeButtonName, buttonId);
            btn.Style = (Style)this.Resources["NodeButton"];
            //btn.MouseDown += new MouseButtonEventHandler(btn_MouseDown);
            btn.Click += new RoutedEventHandler(button_click);
            btn.MouseDoubleClick += new MouseButtonEventHandler(btn_MouseDoubleClick);
            btn.Content = buttonContent;
            // do some hacks for WPF build VisualTree of this component
            btn.ApplyTemplate();
            var innerButton = btn.Template.FindName("innerEllipseButton", btn) as Button;
            innerButton.ApplyTemplate();

            Label lbl = new Label();
            lbl.Name = string.Format("{0}{1}", nodeLabelName, buttonId);
            grid.Children.Add(lbl);
            if (hierarchyStack.FindName(lbl.Name) != null)
                hierarchyStack.UnregisterName(lbl.Name);
            hierarchyStack.RegisterName(lbl.Name, lbl);
            //lbl.Background = (buttonId % 2 == 0) ? Brushes.DimGray : Brushes.DeepSkyBlue;
            Grid.SetColumn(lbl, grid.ColumnDefinitions.Count - 1);

            grid.Children.Add(btn);
            if (hierarchyStack.FindName(btn.Name) != null)
                hierarchyStack.UnregisterName(btn.Name);
            hierarchyStack.RegisterName(btn.Name, btn);
            Grid.SetColumn(btn, grid.ColumnDefinitions.Count - 1);

            return btn;
        }

        void button_click(object sender, RoutedEventArgs e)
        {
            myClickWaitTimer.Start();
            lastButton = sender as Button;
        }

        void btn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            myClickWaitTimer.Stop();

            var button = sender as Button;
            var grid = (button.Parent as Grid).Parent as Grid;

            int index = hierarchyStack.Children.IndexOf(grid);
            int id = Convert.ToInt32(button.Name.Substring(nodeButtonName.Length));

            if ((index + 1) == hierarchyStack.Children.Count)
                return;

            Grid lowerGrid = (hierarchyStack.Children[index + 1] as Grid).Children[0] as Grid;

            foreach (var btn in lowerGrid.Children.OfType<Button>())
            {
                int lowerId = Convert.ToInt32(btn.Name.Substring(nodeButtonName.Length));
                if (!graph.EdgeExists(id, lowerId))
                {
                    Line line = addNewConnector(id, lowerId);
                    connectors[index].Add(line);
                    connectorsIndices[line] = index;

                    graph.AddEdge(id, lowerId);
                }
            }

            e.Handled = true;
        }

        protected Line addNewConnector(int buttonId1, int buttonId2)
        {
            var btn1 = hierarchyStack.FindName(nodeButtonName + buttonId1.ToString()) as Button;
            var innerButton1 = btn1.Template.FindName("innerEllipseButton", btn1) as Button;
            var ellipse1 = (Ellipse)innerButton1.Template.FindName("ellipse", innerButton1);

            var btn2 = hierarchyStack.FindName(nodeButtonName + buttonId2.ToString()) as Button;
            var innerButton2 = btn2.Template.FindName("innerEllipseButton", btn2) as Button;
            var ellipse2 = (Ellipse)innerButton2.Template.FindName("ellipse", innerButton2);

            var grid1 = (btn1.Parent as Grid).Parent as Grid;
            var grid2 = (btn2.Parent as Grid).Parent as Grid;
                        
            Line line = new Line();
            line.StrokeThickness = 4.0;
            line.Stroke = Brushes.DimGray;

            line.Name = string.Format("{0}{1}_{2}", lineName, buttonId1, buttonId2);
            wrapperGrid.Children.Add(line);
            if (wrapperGrid.FindName(line.Name) == null)
                wrapperGrid.RegisterName(line.Name, line);
            Grid.SetRowSpan(line, 1000);            

            // set bindings
            MultiBinding lineX1Binding = new MultiBinding();

            lineX1Binding.Bindings.Add(new Binding("ActualWidth") { ElementName = string.Format("{0}{1}", nodeLabelName, buttonId1), Mode = BindingMode.OneWay });
            lineX1Binding.Bindings.Add(new Binding("ActualWidth") { Source = wrapperGrid, Mode = BindingMode.OneWay });
            lineX1Binding.Converter = new EllipseCoordsXConverter();
            lineX1Binding.ConverterParameter = new object[] { ellipse1, wrapperGrid };
            line.SetBinding(Line.X1Property, lineX1Binding);

            Binding lineY1Binding = new Binding("Margin") { Source = grid1, Mode = BindingMode.OneWay };
            lineY1Binding.Converter = new EllipseCoordsYConverter();
            lineY1Binding.ConverterParameter = new object[] { ellipse1, wrapperGrid };
            line.SetBinding(Line.Y1Property, lineY1Binding);

            // ----------------------------------------------------

            MultiBinding lineX2Binding = new MultiBinding();

            lineX2Binding.Bindings.Add(new Binding("ActualWidth") { ElementName = string.Format("{0}{1}", nodeLabelName, buttonId2), Mode = BindingMode.OneWay });
            lineX2Binding.Bindings.Add(new Binding("ActualWidth") { Source = wrapperGrid, Mode = BindingMode.OneWay });
            lineX2Binding.Converter = new EllipseCoordsXConverter();
            lineX2Binding.ConverterParameter = new object[] { ellipse2, wrapperGrid };
            line.SetBinding(Line.X2Property, lineX2Binding);

            Binding lineY2Binding = new Binding("Margin") { Source = grid2, Mode = BindingMode.OneWay };
            lineY2Binding.Converter = new EllipseCoordsYConverter();
            lineY2Binding.ConverterParameter = new object[] { ellipse2, wrapperGrid };
            line.SetBinding(Line.Y2Property, lineY2Binding);
            
            line.StrokeStartLineCap = PenLineCap.Round;
            line.StrokeEndLineCap = PenLineCap.Round;

            // --------------------------------------------------

            line.MouseRightButtonUp += delegate(object s, MouseButtonEventArgs me)
            {
                var innerLine = s as Line;
                string[] arr = innerLine.Name.Substring(lineName.Length).Split('_');
                int id1 = Convert.ToInt32(arr[0]);
                int id2 = Convert.ToInt32(arr[1]);

                string s1 = (hierarchyStack.FindName(string.Format("{0}{1}", nodeButtonName, id1)) as Button).Content as string;
                string s2 = (hierarchyStack.FindName(string.Format("{0}{1}", nodeButtonName, id2)) as Button).Content as string;

                if (MessageBox.Show(string.Format("Do you really want to remove line between {0} and {1}?", s1, s2), "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    graph.RemoveEdge(id1, id2);

                    connectors[connectorsIndices[innerLine]].Remove(innerLine);
                    connectorsIndices.Remove(innerLine);

                    wrapperGrid.Children.Remove(innerLine);
                }
            };

            return line;
        }

        protected int[] getIdsFromName(string lineElementName)
        {
            string[] arr = lineElementName.Substring(lineName.Length).Split('_');
            int id1 = Convert.ToInt32(arr[0]);
            int id2 = Convert.ToInt32(arr[1]);
            return new int[] { id1, id2 };
        }

        protected List<List<string>> serializeHierarchyStack()
        {
            List<List<string>> values = new List<List<string>>();
            foreach (var grid in hierarchyStack.Children.OfType<Grid>())
            {
                values.Add(new List<string>());
                int index = values.Count - 1;

                foreach (var button in (grid.Children[0] as Grid).Children.OfType<Button>())
                {
                    values[index].Add(button.Name.Substring(nodeButtonName.Length) + "|" + (button.Content as string));
                }
            }

            return values;
        }

        protected void deserializeHierarchyStack(List<List<string>> values)
        {
            hierarchyStack.Children.Clear();
            addedNodesCount = 0;
            rowsCount = 0;
            
            foreach (var list in values)
            {
                var grid = addNewRow(rowsCount);
                rowsCount++;

                foreach (var str in list)
                {
                    string[] arr = str.Split('|');
                    int buttonIndex = Convert.ToInt32(arr[0]);
                    if (buttonIndex > addedNodesCount)
                        addedNodesCount = buttonIndex;
                    string buttonContent = arr[1];

                    addNewButton(grid.Children[0] as Grid, buttonContent, Convert.ToInt32(buttonIndex));
                }
                grid.UpdateLayout();
            }

            addedNodesCount++;
        }

        protected Dictionary<int, List<string>> serializeConnectors()
        {
            Dictionary<int, List<string>> dict = new Dictionary<int, List<string>>();

            foreach (var keyvalue in connectors)
            {
                dict.Add(keyvalue.Key, new List<string>());
                int index = dict.Count - 1;

                foreach (var line in keyvalue.Value)
                {
                    dict[index].Add(line.Name);
                }
            }

            return dict;
        }

        protected void deserializeConnectors(Dictionary<int, List<string>> dict)
        {
            connectors.Clear();

            foreach (var keyvalue in dict)
            {
                connectors.Add(keyvalue.Key, new List<Line>());
                int index = connectors.Count - 1;

                foreach (var str in keyvalue.Value)
                {
                    connectors[index].Add(hierarchyStack.FindName(str) as Line);
                }
            }
        }

        protected Dictionary<string, int> serializeConnectorsIndices()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (var keyvalue in connectorsIndices)
            {
                dict.Add(keyvalue.Key.Name, keyvalue.Value);
            }

            return dict;
        }

        protected void deserializeConnectorsIndices(Dictionary<string, int> dict)
        {
            connectorsIndices.Clear();
            foreach (var keyvalue in dict)
            {
                connectorsIndices.Add(hierarchyStack.FindName(keyvalue.Key) as Line, keyvalue.Value);
            }
        }

        protected List<string> serializeLines()
        {
            List<string> lines = new List<string>();
            foreach (var line in wrapperGrid.Children.OfType<Line>())
            {
                lines.Add(line.Name);
            }
            return lines;
        }

        protected void deserializeLines(List<string> lines)
        {
            var existingLines = wrapperGrid.Children.OfType<Line>();
            foreach (var line in existingLines)
                wrapperGrid.Children.Remove(line);

            foreach (var line_name in lines)
            {
                int[] ids = getIdsFromName(line_name);

                addNewConnector(ids[0], ids[1]);
            }
        }

        protected void showMatrixToUser(int nodeId, ExtraClasses.Matrix matr)
        {
            (this.Resources["showMatrixOverlay"] as Storyboard).Begin();

            // reset grid
            matrixGrid.RowDefinitions.Clear();
            matrixGrid.ColumnDefinitions.Clear();
            matrixGrid.Children.Clear();

            int id = nodeId;

            int count = graph.Apices[id].Count;

            matrixGrid.ShowGridLines = true;

            // add one more columnt and row for signings
            for (int i = 0; i < count + 1; ++i)
            {
                matrixGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });
                matrixGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            }

            var cell = graph.Apices[id].Next;
            int index = 1;
            while (cell != null)
            {
                string labelName = (hierarchyStack.FindName(string.Format("{0}{1}",
                    nodeButtonName, cell.Apex)) as Button).Content as string;

                // ----------------------------------------

                Label horisontalLbl = new Label();
                horisontalLbl.Content = labelName;
                horisontalLbl.Foreground = Brushes.White;
                horisontalLbl.MaxWidth = maxLabelWidth;
                horisontalLbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                horisontalLbl.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                matrixGrid.Children.Add(horisontalLbl);
                Grid.SetColumn(horisontalLbl, 0);
                Grid.SetRow(horisontalLbl, index);

                // ----------------------------------------

                Label verticalLbl = new Label();
                verticalLbl.Content = labelName;
                verticalLbl.Foreground = Brushes.White;
                verticalLbl.MaxWidth = maxLabelWidth;
                verticalLbl.RenderTransformOrigin = new Point(0.5, 0.5);
                verticalLbl.RenderTransform = new RotateTransform(-90.0, 0.5, 0.5);
                verticalLbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                verticalLbl.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                matrixGrid.Children.Add(verticalLbl);
                Grid.SetRow(verticalLbl, 0);
                Grid.SetColumn(verticalLbl, index);

                // ----------------------------------------

                ++index;
                cell = cell.Next;
            }

            for (int i = 1; i < count; ++i)
            {
                for (int j = i; j < count; ++j)
                {
                    Slider slHorizontal = new Slider();
                    Slider slVertical = new Slider();

                    slHorizontal.Minimum = slVertical.Minimum = minDegree;
                    slHorizontal.Maximum = slVertical.Maximum = maxDegree;
                    slHorizontal.SmallChange = slVertical.SmallChange = smallChange;
                    slHorizontal.LargeChange = slVertical.LargeChange = largeChange;
                    slHorizontal.SetBinding(Slider.ToolTipProperty, new Binding("Value") { Mode = BindingMode.OneWay, Source = slHorizontal });
                    slHorizontal.Value = slVertical.Value = 1.0;

                    if (matr.Height != 0)
                    {
                        double val = matr[i - 1, j];
                        if (!double.IsNaN(val))
                        {
                            slHorizontal.Value = val;
                        }
                    }

                    slVertical.Height = slVertical.Width;
                    slHorizontal.Height = slHorizontal.Width;

                    slVertical.Orientation = Orientation.Vertical;
                    slHorizontal.Orientation = Orientation.Horizontal;

                    slVertical.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    slHorizontal.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                    // set bindings for sliders
                    Binding slidersBinding = new Binding("Value");
                    slidersBinding.Source = slHorizontal;
                    slidersBinding.Mode = BindingMode.TwoWay;
                    slidersBinding.Converter = new SliderToSliderConverter();
                    slidersBinding.ConverterParameter = maxDegree;
                    slVertical.SetBinding(Slider.ValueProperty, slidersBinding);

                    matrixGrid.Children.Add(slHorizontal);
                    matrixGrid.Children.Add(slVertical);

                    Grid.SetColumn(slHorizontal, j + 1);
                    Grid.SetRow(slHorizontal, i);

                    Grid.SetColumn(slVertical, i);
                    Grid.SetRow(slVertical, j + 1);
                }
            }

            int copy_count = count;
            while (0 != copy_count--)
            {
                Slider sl = new Slider();

                sl.Minimum = (maxDegree + minDegree) / 2.0;
                sl.Maximum = (maxDegree + minDegree) / 2.0;
                sl.Value = (maxDegree + minDegree) / 2.0;
                sl.IsEnabled = false;
                sl.Orientation = Orientation.Horizontal;
                sl.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                sl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                //sl.Height = sl.Width;

                sl.RenderTransformOrigin = new Point(0.5, 0.5);

                TransformGroup tg = new TransformGroup();
                tg.Children.Add(new RotateTransform(-45.0, 0.5, 0.5));
                tg.Children.Add(new TranslateTransform(30, 30));

                sl.RenderTransform = tg;

                matrixGrid.Children.Add(sl);

                Grid.SetRow(sl, copy_count + 1);
                Grid.SetColumn(sl, copy_count + 1);
            }
        }

        protected void addMatrixEditButton(string expertName, int buttonId)
        {
            int id = 0;
            if (graph.Data[buttonId].ExpertIds.ContainsKey(expertName))
                id = graph.Data[buttonId].ExpertIds[expertName];
            else
                id = graph.Data[buttonId].LastId++;

            if (!graph.Data[buttonId].Matrices.ContainsKey(expertName))
            {
                int count = graph.Apices[buttonId].Count;
                graph.Data[buttonId].Matrices[expertName] = new ExtraClasses.Matrix(graph.Apices[buttonId].Count);
                ExtraClasses.Matrix matr = graph.Data[buttonId].Matrices[expertName];

                for (int i = 0; i < count; ++i)
                {
                    for (int j = i; j < count; ++j)
                    {
                        if (i != j)
                        {
                            matr[i, j] = minDegree;
                            matr[j, i] = maxDegree;
                        }
                        else
                            matr[i, j] = 1;
                    }
                }

                graph.Data[buttonId].ExpertIds[expertName] = id;
            }

            Button mBtn = new Button();
            mBtn.Content = expertName;
            mBtn.Name = string.Format("{0}{1}", matrixNodeButtonName, id);
            mBtn.Style = (Style)this.Resources["MatrixNodeButton"];
            mBtn.Click += delegate(object s, RoutedEventArgs rea)
            {
                lastExpert = expertName;
                showMatrixToUser(buttonId, graph.Data[buttonId].Matrices[expertName]);
            };

            matricesStack.Children.Add(mBtn);
        }

        protected void submitName()
        {
            lastExpert = expertNameTextBox.Text.Trim();

            if (graph.Data[lastClickedId].Matrices.ContainsKey(lastExpert))
            {
                MessageBox.Show("Such expert has already added a matrix. Try other name.");
                lastExpert = string.Empty;
                return;
            }

            (this.Resources["hideExpertNameOverlay"] as Storyboard).Begin();
            addMatrixEditButton(lastExpert, lastClickedId);
        }

        protected OrientatedGraph<int, ExtraClasses.Matrix> GetUserGraph()
        {
            OrientatedGraph<int, ExtraClasses.Matrix> userGraph = new OrientatedGraph<int, ExtraClasses.Matrix>();
            foreach (var g in graph.Apices)
            {
                userGraph.AddApex(g.Key, graph.Data[g.Key].GenerateMatrix());

                var cell = graph.Apices[g.Key].Next;

                while (cell != null)
                {
                    userGraph.AddEdge(g.Key, cell.Apex);
                    cell = cell.Next;
                }
            }

            return userGraph;
        }
    }
}

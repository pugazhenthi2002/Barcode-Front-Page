using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ReportFrontPage
{
    /// <summary>
    /// Interaction logic for BarcodeWindow.xaml
    /// </summary>
    /// 

    public enum FilterModes
    {
        FilterOFF,
        FilterON
    }


    public partial class BarcodeWindow : Window
    {
        private FilterModes FilterMode { get; set; }

        private enum ChilderAddMode
        {
            Insert, Add
        }

        private bool isDragging;
        private int autoAdjustPanelRows, underReviewCurrentViewableRow = 1, reviewedCurrentViewableRow = 1;
        private int underReviewStartRow, underReviewEndRow, reviewedStartRow, reviewedEndRow;
        private double individualRowHeight;
        private ReportStatuses currentReportStatus;
        private Point dragDropStartPoint;
        private CheckBox recentAddedBoard;  //For Opacity Animation
        private List<CheckBox> SelectedCheckBox;
        private List<ReportProfile> reportCollection;    //Backup Collection of Reports
        private List<ReportProfile> filteredCollection;    //Backup Collection of Reports
        private List<List<ReportProfile>> underReviewReportCollection, reviewedReportCollection;
        private List<List<CheckBox>> underReviewBoardCollection, reviewedBoardCollection;
        private ControlTemplate checkBoxTemplate;
        private Style checkBoxStyle;

        public BarcodeWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            IsEnabled = true;
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri("BoardViewStyle.xaml", UriKind.Relative);
            checkBoxTemplate = (ControlTemplate)resourceDictionary["ReportBoardControlTemplate"];
            checkBoxStyle = (Style)resourceDictionary["ReportBoardStyle"];

            reportCollection = new List<ReportProfile>();
            SelectedCheckBox = new List<CheckBox>();

            reviewedBoardCollection = new List<List<CheckBox>>();
            reviewedReportCollection = new List<List<ReportProfile>>();
            reviewedCurrentViewableRow = 1;

            underReviewBoardCollection = new List<List<CheckBox>>();
            underReviewReportCollection = new List<List<ReportProfile>>();
            underReviewCurrentViewableRow = 1;

            autoAdjustPanelRows = 4;

            for (int i = 0; i < autoAdjustPanelRows + 2; i++)
            {
                underReviewBoardCollection.Add(new List<CheckBox>());
                reviewedBoardCollection.Add(new List<CheckBox>());
            }
            SetIndividualHeight(autoAdjustPanelRows);
        }

        #region Auto Adjust Boards
        public void AddReport(ReportProfile report)
        {
            SetIndividualHeight(autoAdjustPanelRows);
            reportCollection.Add(report);
            if (FilterMode == FilterModes.FilterON)
                TryAddBoardOnFilterOn(report);
            else
                GenerateBoard(report);
        }

        private void TryAddBoardOnFilterOn(ReportProfile report)
        {
            if (report.ReportName.Contains(reportNameTextBox.AnimatedText))
            {
                GenerateBoard(report);
            }

        }

        private void GenerateBoard(ReportProfile report)
        {
            double entireRowActualWidth = 0;

            CheckBox board = new CheckBox()
            {
                Template = checkBoxTemplate,
                Style = checkBoxStyle,
                DataContext = report,
                Opacity = 0,
                MinWidth = 200,
                Margin = new Thickness(5)
            };


            board.Width = BoardActualWidth((board.DataContext as ReportProfile).ReportName);
            board.Height = individualRowHeight - board.Margin.Top - board.Margin.Bottom;

            board.PreviewMouseDown += ReportBoardMouseDown;
            board.Checked += OnReportChecked;
            board.Unchecked += OnReportUnChecked;
            board.PreviewMouseMove += ReportBoardMouseMove;


            currentReportStatus = report.ReportStatus;

            int currentViewableRow, startRow, endRow;
            Canvas canvas;
            List<List<ReportProfile>> viewableReportCollection;
            List<List<CheckBox>> viewableBoardCollection;

            if (currentReportStatus == ReportStatuses.UnderReview)
            {
                currentViewableRow = underReviewReportCollection.Count < 4 ? underReviewReportCollection.Count : autoAdjustPanelRows;
                startRow = underReviewStartRow;
                endRow = underReviewEndRow;
                canvas = underReviewCanvas;
                viewableReportCollection = underReviewReportCollection;
                viewableBoardCollection = underReviewBoardCollection;
            }
            else
            {
                currentViewableRow = reviewedReportCollection.Count < 4 ? reviewedReportCollection.Count : autoAdjustPanelRows;
                startRow = reviewedStartRow;
                endRow = reviewedEndRow;
                canvas = reviewedCanvas;
                viewableReportCollection = reviewedReportCollection;
                viewableBoardCollection = reviewedBoardCollection;
            }

            entireRowActualWidth = GetEntireRowBoardWidth(currentViewableRow, currentReportStatus);
            double firstRowBoardCanvasTop = Canvas.GetTop(viewableBoardCollection[1][0]);
            //If Current row is at Last Viewable Board Row
            if (endRow == viewableReportCollection.Count - 1 && (entireRowActualWidth + board.Width + board.Margin.Left + board.Margin.Right) > canvas.ActualWidth && currentViewableRow == autoAdjustPanelRows && firstRowBoardCanvasTop == 0)
            {
                if (report.ReportStatus == ReportStatuses.UnderReview)
                {
                    underReviewCurrentViewableRow--; underReviewStartRow++;
                }
                else
                {
                    reviewedCurrentViewableRow--; reviewedStartRow++;
                }
                PaginateDown();
                AdjustRowEqually(autoAdjustPanelRows, entireRowActualWidth, viewableBoardCollection[autoAdjustPanelRows].Count, currentReportStatus);
                AddControl(board, currentReportStatus);
                ReorderControlOnPaginateDown();
            }
            else if (endRow != viewableReportCollection.Count - 1 || (endRow == viewableReportCollection.Count - 1 && firstRowBoardCanvasTop > 0)) // If Current Row is at middle of the row
            {
                LoadReportCollection(reportCollection);
                LoadBoardOnCanvas(ReportStatuses.UnderReview);
                LoadBoardOnCanvas(ReportStatuses.Reviewed);
            }
            else
            {
                AddControl(board, currentReportStatus);
            }

        }

        private void PaginateDown()
        {
            double prevTop = individualRowHeight;
            for (int row = 0; row < underReviewBoardCollection.Count; row++)
            {
                for (int col = 0; col < underReviewBoardCollection[row].Count; col++)
                {
                    DoubleAnimation animation = new DoubleAnimation()
                    {
                        From = Canvas.GetTop(underReviewBoardCollection[row][col]),
                        To = Canvas.GetTop(underReviewBoardCollection[row][col]) - prevTop,
                        Duration = TimeSpan.FromMilliseconds(100)
                    };
                    Canvas.SetTop(underReviewBoardCollection[row][col], Canvas.GetTop(underReviewBoardCollection[row][col]) - prevTop);
                    //underReviewBoardCollection[row][col].BeginAnimation(Canvas.TopProperty, animation);
                }
            }
        }

        private void AddControl(CheckBox board, ReportStatuses status)
        {
            Canvas canvas;
            List<List<CheckBox>> boards;
            List<List<ReportProfile>> reports;
            int startRow, endRow, currentRow;

            if (status == ReportStatuses.UnderReview)
            {
                canvas = underReviewCanvas;
                startRow = underReviewStartRow;
                endRow = underReviewEndRow;
                currentRow = underReviewReportCollection.Count < 4 ? underReviewReportCollection.Count : autoAdjustPanelRows;
                boards = underReviewBoardCollection;
                reports = underReviewReportCollection;
            }
            else
            {
                canvas = reviewedCanvas;
                startRow = reviewedStartRow;
                endRow = reviewedEndRow;
                currentRow = reviewedReportCollection.Count < 4 ? reviewedReportCollection.Count : autoAdjustPanelRows;
                boards = reviewedBoardCollection;
                reports = underReviewReportCollection;
            }

            double canvasTop = 0, canvasLeft = 0;
            double entireRowActualWidth = 0;
            int ctr = boards[0].Count;
            bool flag = false;
            for (int row = 1, idx = startRow; row < boards.Count - 1; row++, idx++)
            {
                entireRowActualWidth = GetEntireRowBoardWidth(row, status);

                if (entireRowActualWidth + board.Width + board.Margin.Left + board.Margin.Right <= canvas.ActualWidth)
                {
                    //Replace in Prev Rows
                    if (row < boards.Count - 2 && row < reports.Count)
                    {
                        double remainingWidth = entireRowActualWidth + board.Width + board.Margin.Left + board.Margin.Right;
                        Canvas.SetLeft(board, 1000);
                        Canvas.SetTop(board, canvasTop);

                        recentAddedBoard = board;

                        if (board.DataContext is ReportProfile)
                        {
                            if (reports.Count <= idx)
                            {
                                flag = true;
                                Canvas.SetLeft(board, 0);
                                reports.Add(new List<ReportProfile>());
                            }

                            reports[idx].Add((board.DataContext as ReportProfile));
                            canvas.Children.Insert(ctr + boards[row].Count, board);
                            boards[row].Add(board);

                            if (!flag)
                                AdjustRowEqually(row, remainingWidth, boards[row].Count, ReportStatuses.UnderReview);
                            else
                                LoadBoard(board);
                        }
                    }
                    else
                    {
                        //Add in Current Row
                        canvasLeft = entireRowActualWidth;
                        Canvas.SetLeft(board, canvasLeft);
                        Canvas.SetTop(board, canvasTop);

                        canvas.Children.Add(board);

                        if (reports.Count == 0)
                        {
                            startRow = 0; endRow = 0;
                            if (board.DataContext is ReportProfile)
                            {
                                reports.Add(new List<ReportProfile>()
                                {
                                    (board.DataContext as ReportProfile)
                                });
                            }
                        }
                        else
                        {
                            if (board.DataContext is ReportProfile)
                            {
                                reports[idx].Add((board.DataContext as ReportProfile));
                            }

                        }
                        LoadBoard(board);
                        boards[row].Add(board);
                    }
                    return;
                }
                else
                {
                    //Add Control to New Row
                    if (row == currentRow)
                    {
                        recentAddedBoard = board;
                        AdjustRowEqually(row, entireRowActualWidth, underReviewBoardCollection[row].Count, ReportStatuses.UnderReview);

                        if (row != autoAdjustPanelRows)
                            canvasTop = canvasTop + individualRowHeight;

                        Canvas.SetLeft(board, canvasLeft);
                        Canvas.SetTop(board, canvasTop);
                        currentRow++; endRow++;

                        if (status == ReportStatuses.UnderReview)
                            underReviewEndRow = endRow;
                        else
                            reviewedEndRow = endRow;

                        if (board.DataContext is ReportProfile)
                        {
                            reports.Add(new List<ReportProfile>()
                            {
                                board.DataContext as ReportProfile,
                            });
                        }
                        boards[row + 1].Add(board);
                        canvas.Children.Add(board);
                        return;
                    }
                }
                canvasTop += individualRowHeight;
                ctr += boards[row].Count;
            }
        }

        private void AdjustRowEqually(int row, double entireRowWidth, int rowCnt, ReportStatuses status)
        {
            Canvas canvas;
            List<List<CheckBox>> boards;
            if (status == ReportStatuses.UnderReview)
            {
                canvas = underReviewCanvas;
                boards = underReviewBoardCollection;
            }
            else
            {
                canvas = reviewedCanvas;
                boards = reviewedBoardCollection;
            }

            double prevLeft = 0;
            double rem = (canvas.ActualWidth - entireRowWidth) / rowCnt;

            for (int col = 0; col < boards[row].Count; col++)
            {
                CheckBox board = boards[row][col];
                double controlWidth = BoardActualWidth((board.DataContext as ReportProfile).ReportName) + board.Margin.Left + board.Margin.Right;

                DoubleAnimation canvasLeftAnimation = new DoubleAnimation()
                {
                    From = Canvas.GetLeft(boards[row][col]),
                    To = prevLeft,
                    Duration = TimeSpan.FromMilliseconds(200)
                };

                DoubleAnimation widthAnimation = new DoubleAnimation()
                {
                    From = boards[row][col].ActualWidth,
                    To = controlWidth + rem - boards[row][col].Margin.Left - boards[row][col].Margin.Right,
                    Duration = TimeSpan.FromMilliseconds(200)
                };

                if (boards[row].Count == col + 1)
                {
                    widthAnimation.Completed += OnRowsAdjustAnimationCompletion;
                }

                prevLeft += controlWidth + rem;

                boards[row][col].BeginAnimation(Canvas.LeftProperty, canvasLeftAnimation);
                boards[row][col].BeginAnimation(WidthProperty, widthAnimation);
            }
        }

        private void LoadBoard(CheckBox board)
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(500)
            };

            board.BeginAnimation(OpacityProperty, animation);
        }

        private void OnRowsAdjustAnimationCompletion(object sender, EventArgs e)
        {
            if (recentAddedBoard != null)
            {
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(500)
                };

                recentAddedBoard.BeginAnimation(OpacityProperty, animation);
            }
        }

        private void ReorderControlOnPaginateDown()
        {

            for (int col = 0; col < underReviewBoardCollection[0].Count; col++)
            {
                underReviewCanvas.Children.Remove(underReviewBoardCollection[0][col]);
            }
            underReviewBoardCollection[0].Clear();

            for (int ctr = 0; ctr < underReviewBoardCollection.Count - 1; ctr++)
            {
                underReviewBoardCollection[ctr] = underReviewBoardCollection[ctr + 1];
            }

            underReviewBoardCollection[underReviewBoardCollection.Count - 1] = new List<CheckBox>();
        }

        private double GetEntireRowBoardWidth(int row, ReportStatuses status)
        {
            List<List<CheckBox>> boards = status == ReportStatuses.UnderReview ? boards = underReviewBoardCollection : boards = reviewedBoardCollection;


            double entireRowActualWidth = 0;
            for (int col = 0; col < boards[row].Count; col++)
            {
                CheckBox board = boards[row][col];
                entireRowActualWidth += BoardActualWidth((board.DataContext as ReportProfile).ReportName) + board.Margin.Left + board.Margin.Right;
            }

            return entireRowActualWidth;
        }

        private Double BoardActualWidth(string text)
        {
            var typeface = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
            var formattedText = new FormattedText(text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 18, Brushes.Black);

            return formattedText.Width + 30 <= 200 ? 200 : formattedText.Width + 30;
        }

        private void SetIndividualHeight(int value) => individualRowHeight = underReviewCanvas.ActualHeight / value;

        private void CanvasMouseWheelChanged(object sender, MouseWheelEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            int endRow, startRow;
            List<List<ReportProfile>> reports;

            if (canvas == underReviewCanvas)
            {
                reports = underReviewReportCollection;
                endRow = underReviewEndRow;
                startRow = underReviewStartRow;
            }
            else
            {
                reports = reviewedReportCollection;
                endRow = reviewedEndRow;
                startRow = reviewedStartRow;
            }

            if (e.Delta < 0 && endRow <= reports.Count - 1)
            {
                ScrollDown(canvas);
            }
            else if (e.Delta > 0 && startRow >= 0)
            {
                ScrollUp(canvas);
            }

        }

        private void ScrollDown(Canvas canvas)
        {
            int endRow, startRow;
            List<List<ReportProfile>> reports;
            List<List<CheckBox>> boards;
            ReportStatuses status;

            if (canvas == underReviewCanvas)
            {
                canvas = underReviewCanvas;
                boards = underReviewBoardCollection;
                reports = underReviewReportCollection;
                endRow = underReviewEndRow;
                startRow = underReviewStartRow;
                status = ReportStatuses.UnderReview;
            }
            else
            {
                canvas = reviewedCanvas;
                boards = reviewedBoardCollection;
                reports = reviewedReportCollection;
                endRow = reviewedEndRow;
                startRow = reviewedStartRow;
                status = ReportStatuses.Reviewed;
            }

            if (reports.Count < autoAdjustPanelRows + 1 || (endRow == reports.Count - 1 && reports.Count > autoAdjustPanelRows + 1 && reports[autoAdjustPanelRows].Count > 0 && Canvas.GetTop(boards[autoAdjustPanelRows][0]) + boards[autoAdjustPanelRows][0].ActualHeight < canvas.ActualHeight))
            {
                return;
            }


            for (int row = 0; row < boards.Count; row++)
            {
                for (int col = 0; col < boards[row].Count; col++)
                {
                    Canvas.SetTop(boards[row][col], Canvas.GetTop(boards[row][col]) - 30);
                }
            }

            if (boards.Count > 0 && boards[1].Count > 0 && Canvas.GetTop(boards[1][0]) + individualRowHeight <= 0)
            {
                endRow++; startRow++;
                foreach (var Iter in boards[0])
                {
                    canvas.Children.Remove(Iter);
                }
                boards[0].Clear();

                for (int row = 0; row < boards.Count - 1; row++)
                {
                    boards[row] = boards[row + 1];
                }

                boards[autoAdjustPanelRows + 1] = new List<CheckBox>();

                if (canvas == underReviewCanvas)
                {
                    underReviewEndRow = endRow;
                    underReviewStartRow = startRow;
                }
                else
                {
                    reviewedEndRow = endRow;
                    reviewedStartRow = startRow;
                }

                if (endRow + 1 != reports.Count)
                {
                    var canvasTop = Canvas.GetTop(boards[autoAdjustPanelRows][0]) + boards[autoAdjustPanelRows][0].ActualHeight + boards[autoAdjustPanelRows][0].Margin.Top + boards[autoAdjustPanelRows][0].Margin.Bottom;
                    LoadEntireRowBoardByReportRowAndAdjustEqually(autoAdjustPanelRows + 1, endRow + 1, canvasTop, ChilderAddMode.Add, status);
                }

                GC.Collect();
            }

            if (endRow == reports.Count - 1 && boards.Count > autoAdjustPanelRows && boards[autoAdjustPanelRows].Count > 0 && Canvas.GetTop(boards[autoAdjustPanelRows][0]) + boards[autoAdjustPanelRows][0].ActualHeight < canvas.ActualHeight)
            {
                SetIndividualHeight(autoAdjustPanelRows);
                double canvasTop = -individualRowHeight;
                for (int row = 0; row < boards.Count; row++)
                {
                    for (int col = 0; col < boards[row].Count; col++)
                    {
                        Canvas.SetTop(boards[row][col], canvasTop);
                    }
                    canvasTop += individualRowHeight;
                }
            }
        }

        private void ScrollUp(Canvas canvas)
        {
            int endRow, startRow;
            List<List<ReportProfile>> reports;
            List<List<CheckBox>> boards;
            ReportStatuses status;

            if (canvas == underReviewCanvas)
            {
                canvas = underReviewCanvas;
                boards = underReviewBoardCollection;
                reports = underReviewReportCollection;
                endRow = underReviewEndRow;
                startRow = underReviewStartRow;
                status = ReportStatuses.UnderReview;
            }
            else
            {
                canvas = reviewedCanvas;
                boards = reviewedBoardCollection;
                reports = reviewedReportCollection;
                endRow = reviewedEndRow;
                startRow = reviewedStartRow;
                status = ReportStatuses.Reviewed;
            }


            if (reports.Count < autoAdjustPanelRows + 1 || (startRow == 0 && reports[1].Count > 0 && Canvas.GetTop(boards[1][0]) >= 0))
            {
                return;
            }

            for (int row = 0; row < boards.Count; row++)
            {
                for (int col = 0; col < boards[row].Count; col++)
                {
                    var x = Canvas.GetTop(boards[row][col]);
                    Canvas.SetTop(boards[row][col], Canvas.GetTop(boards[row][col]) + 30);
                    var y = Canvas.GetTop(boards[row][col]);
                }
            }

            if (boards.Count > 0 && boards[0].Count > 0 && Canvas.GetTop(boards[0][0]) >= 0)
            {
                endRow--; startRow--;
                foreach (var Iter in boards[autoAdjustPanelRows + 1])
                {
                    canvas.Children.Remove(Iter);
                }
                boards[autoAdjustPanelRows + 1].Clear();

                for (int row = boards.Count - 1; row > 0; row--)
                {
                    boards[row] = boards[row - 1];
                }
                boards[0] = new List<CheckBox>();


                if (canvas == underReviewCanvas)
                {
                    underReviewEndRow = endRow;
                    underReviewStartRow = startRow;
                }
                else
                {
                    reviewedEndRow = endRow;
                    reviewedStartRow = startRow;
                }

                if (startRow - 1 >= 0)
                    LoadEntireRowBoardByReportRowAndAdjustEqually(0, startRow - 1, -individualRowHeight + Canvas.GetTop(boards[1][0]), ChilderAddMode.Insert, status);
            }

            if (startRow == 0 && boards.Count > 1 && boards[1].Count > 0 && Canvas.GetTop(boards[1][0]) > 0)
            {
                double canvasTop = 0;
                SetIndividualHeight(autoAdjustPanelRows);
                for (int row = 1; row < boards.Count; row++)
                {
                    for (int col = 0; col < boards[row].Count; col++)
                    {
                        Canvas.SetTop(boards[row][col], canvasTop);
                    }
                    canvasTop += individualRowHeight;
                }
            }

            GC.Collect();
        }

        public void LoadReportCollection(List<ReportProfile> reports)
        {
            bool flag;
            List<List<ReportProfile>> viewableReports;
            underReviewReportCollection.Clear();
            reviewedReportCollection.Clear();
            double entireRowWidth = 0, marginLeft = 5, marginRight = 5;

            foreach (var report in reports)
            {
                viewableReports = report.ReportStatus == ReportStatuses.UnderReview ? underReviewReportCollection : reviewedReportCollection;
                flag = true;
                for (int row = 0; row < viewableReports.Count; row++)
                {
                    flag = false;
                    entireRowWidth = 0;
                    for (int col = 0; col < viewableReports[row].Count; col++)
                    {
                        entireRowWidth += BoardActualWidth(viewableReports[row][col].ReportName) + marginLeft + marginRight;
                    }

                    if (entireRowWidth + BoardActualWidth(report.ReportName) + 10 <= underReviewCanvas.ActualWidth)
                    {
                        flag = false;
                        viewableReports[row].Add(report);
                        break;
                    }
                    else if (row == viewableReports.Count - 1)
                    {
                        flag = true;
                    }
                }

                if (flag)
                    viewableReports.Add(new List<ReportProfile>() { report });

            }

        }

        private void LoadBoardOnCanvas(ReportStatuses status)
        {
            SetIndividualHeight(autoAdjustPanelRows);
            double canvasTop = 0;
            int endRow, startRow;
            List<List<ReportProfile>> reports;
            List<List<CheckBox>> boards;
            Canvas canvas;

            if (status == ReportStatuses.UnderReview)
            {
                reports = underReviewReportCollection;
                boards = underReviewBoardCollection;
                canvas = underReviewCanvas;
            }
            else
            {
                reports = reviewedReportCollection;
                boards = reviewedBoardCollection;
                canvas = reviewedCanvas;
            }

            endRow = reports.Count - 1;
            startRow = (endRow - autoAdjustPanelRows + 1) < 0 ? 0 : (endRow - autoAdjustPanelRows + 1);
            canvas.Children.Clear();
            boards.Clear();

            for (int ctr = 0; ctr < autoAdjustPanelRows + 2; ctr++)
            {
                boards.Add(new List<CheckBox>());
            }

            if (status == ReportStatuses.UnderReview)
            {
                underReviewEndRow = endRow;
                underReviewStartRow = startRow;
            }
            else
            {
                reviewedEndRow = endRow;
                reviewedStartRow = startRow;
            }

            for (int row = startRow, idx = 1; row <= endRow; row++, idx++)
            {
                LoadEntireRowBoardByReportRowAndAdjustEqually(idx, row, canvasTop, ChilderAddMode.Add, status);
                canvasTop += individualRowHeight;
            }

            if (startRow - 1 >= 0)
            {
                boards[0].Clear();
                canvasTop = -individualRowHeight;
                LoadEntireRowBoardByReportRowAndAdjustEqually(0, startRow - 1, canvasTop, ChilderAddMode.Insert, status);
            }

        }

        private double LoadEntireRowBoardByReportRowAndAdjustEqually(int boardRow, int reportRow, double canvasTop, ChilderAddMode mode, ReportStatuses status)
        {
            double canvasLeft = 0, entireRowWidth = 0;
            List<List<CheckBox>> boards;
            List<List<ReportProfile>> reports;
            Canvas canvas;
            int endRow;

            if (status == ReportStatuses.UnderReview)
            {
                boards = underReviewBoardCollection;
                reports = underReviewReportCollection;
                canvas = underReviewCanvas;
                endRow = underReviewEndRow;
            }
            else
            {
                boards = reviewedBoardCollection;
                reports = reviewedReportCollection;
                canvas = reviewedCanvas;
                endRow = reviewedEndRow;
            }

            for (int col = 0; col < reports[reportRow].Count; col++)
            {
                boards[boardRow].Add(new CheckBox()
                {
                    Template = checkBoxTemplate,
                    Style = checkBoxStyle,
                    DataContext = reports[reportRow][col],
                    Opacity = 1,
                    MinWidth = 200,
                    Margin = new Thickness(5)
                });

                CheckBox board = boards[boardRow][col];
                board.Width = BoardActualWidth((board.DataContext as ReportProfile).ReportName);
                board.Height = individualRowHeight - board.Margin.Top - board.Margin.Bottom;

                board.PreviewMouseDown += ReportBoardMouseDown;
                board.PreviewMouseMove += ReportBoardMouseMove;
                //board.MouseEnter += ReportBoardMouseEnter;
                //board.MouseLeave += ReportBoardMouseLeave;
                board.Checked += OnReportChecked;
                board.Unchecked += OnReportUnChecked;

                Canvas.SetLeft(board, canvasLeft);
                Canvas.SetTop(board, canvasTop);

                canvasLeft = entireRowWidth += board.Width + board.Margin.Left + board.Margin.Right;

                if (mode == ChilderAddMode.Add)
                    canvas.Children.Add(board);
                else
                    canvas.Children.Insert(col, board);
            }

            if (reportRow != reports.Count - 1)
                AdjustRowEqually(boardRow, entireRowWidth, boards[boardRow].Count, status);

            return entireRowWidth;
        }

        #endregion

        #region Drag And Drop

        private void ReportBoardMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                dragDropStartPoint = e.GetPosition(null);
                isDragging = true;
            }
        }

        private void ReportBoardMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.GetPosition(null);
                Vector diff = dragDropStartPoint - currentPosition;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                     Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    CheckBox checkBox = sender as CheckBox;
                    if (checkBox != null)
                    {
                        DataObject dragData = new DataObject("SelectedReports", FetchSelectedReports());

                        DragDrop.DoDragDrop(checkBox, dragData, DragDropEffects.Move);
                    }
                }
            }

        }

        private void CanvasDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("SelectedReports") && isDragging)
            {
                List<ReportProfile> reports = e.Data.GetData("SelectedReports") as List<ReportProfile>;
                if (reports != null)
                {
                    Canvas newParentCanvas;
                    foreach (ReportProfile report in reports)
                    {
                        newParentCanvas = sender as Canvas;
                        report.ReportStatus = newParentCanvas == reviewedCanvas ? ReportStatuses.Reviewed : ReportStatuses.UnderReview;
                        report.IsSelected = false;
                    }

                    if (reports.Count > 0)
                    {
                        if (FilterMode == FilterModes.FilterON)
                            LoadReportCollection(filteredCollection);
                        else
                            LoadReportCollection(reportCollection);

                        LoadBoardOnCanvas(ReportStatuses.UnderReview);
                        LoadBoardOnCanvas(ReportStatuses.Reviewed);
                    }
                }
            }

            isDragging = false;
        }

        private void OnReportChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Canvas canvas = VisualTreeHelper.GetParent(checkBox) as Canvas;
            List<List<ReportProfile>> deselectReports = canvas == underReviewCanvas ? reviewedReportCollection : underReviewReportCollection;

            (checkBox.DataContext as ReportProfile).IsSelected = true;

            for (int row = 0; row < deselectReports.Count; row++)
            {
                for (int col = 0; col < deselectReports[row].Count; col++)
                {
                    deselectReports[row][col].IsSelected = false;
                }
            }
        }

        private void OnReportUnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            (checkBox.DataContext as ReportProfile).IsSelected = false;
        }

        private List<ReportProfile> FetchSelectedReports()
        {
            List<ReportProfile> selectedReports = new List<ReportProfile>();


            for (int row = 0; row < underReviewReportCollection.Count; row++)
            {
                for (int col = 0; col < underReviewReportCollection[row].Count; col++)
                {
                    if (underReviewReportCollection[row][col].IsSelected)
                    {
                        selectedReports.Add(underReviewReportCollection[row][col]);
                    }
                }
            }


            for (int row = 0; row < reviewedReportCollection.Count; row++)
            {
                for (int col = 0; col < reviewedReportCollection[row].Count; col++)
                {
                    if (reviewedReportCollection[row][col].IsSelected)
                    {
                        selectedReports.Add(reviewedReportCollection[row][col]);
                    }
                }
            }

            return selectedReports;
        }

        #endregion

        #region Filter

        private void OnFilterClicked(object sender, RoutedEventArgs e)
        {
            string filterText = reportNameTextBox.AnimatedText;

            filteredCollection = new List<ReportProfile>();
            foreach (var Iter in reportCollection)
            {
                if (Iter.ReportName.Contains(filterText))
                {
                    filteredCollection.Add(Iter);
                }
            }
            FilterMode = FilterModes.FilterON;
            LoadReportCollection(filteredCollection);
            LoadBoardOnCanvas(ReportStatuses.UnderReview);
            LoadBoardOnCanvas(ReportStatuses.Reviewed);
        }

        #endregion

        private void CreateReportClick(object sender, RoutedEventArgs e)
        {
            CreateReport window = new CreateReport();
            window.ReportCreate += WindowReportCreate;
            window.Show();

            reportCollection = new List<ReportProfile>();
            for (int ctr = 1; ctr <= 100; ctr++)
            {
                reportCollection.Add(new ReportProfile()
                {
                    ReportName = "RWS" + ctr,
                    DefectCount = 123,
                    Prefix = "AA",
                    OperatorName = "Mr. XYZ"
                });
            }

            LoadReportCollection(reportCollection);
            LoadBoardOnCanvas(ReportStatuses.UnderReview);
            LoadBoardOnCanvas(ReportStatuses.Reviewed);
        }

        private void WindowReportCreate(object sender, ReportProfile e)
        {
            AddReport(e);
        }

    }
}

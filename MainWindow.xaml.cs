using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using System.Diagnostics;
using System.IO;


namespace MapaMysli
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();
            this.Top = 0;
            this.Left = 0;
            Loaded += MyWindow_Loaded;
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InfIndexOtw = -10;
            OtwarcieMenu_Click(null, null);

            GlowneOkno.Height = 500;
            GlowneOkno.Width = 800;
            grid.Height = 400000;
            grid.Width = 400000;
            var x = (grid.Width - GlowneOkno.Width) / 2;
            var y = (grid.Height - GlowneOkno.Height) / 2;
            grid.Margin = new Thickness(-x, -y, -x, -y);
            grid.Background = Brushes.Blue;

            GridScaleTransform.ScaleX = 1;
            GridScaleTransform.ScaleY = 1;

            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
            canvas.Margin = new Thickness(1138, 786, 712, 1064);
            indexx = canvas;

            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
            OdswiezanieKatalogowSave();
        }

        int InfIndexOtw;
        int InfIndexZam;
        private void OtwarcieMenu_Click(object sender, RoutedEventArgs e)
        {
            var ob = (Menu)Application.LoadComponent(new Uri("xml2.xaml", System.UriKind.RelativeOrAbsolute));
            foreach (var c in ob.Items.OfType<MenuItem>())
            {
                if (c.Header.ToString() == "ADD")
                {
                    c.Click += Button_Click;
                }
                if (c.Header.ToString() == "ADD TO")
                {
                    c.Click += ButtonToAdd_Click;
                }
                if (c.Header.ToString() == "DELETE")
                {
                    c.Click += Delete_Click;
                }
                if (c.Header.ToString() == "NSAVE")
                {
                    c.Click += NadpiszSave_Click;
                }
                if (c.Header.ToString() == "SAVE")
                {
                    c.Click += Save_Click;
                }
                if (c.Header.ToString() == "LOAD")
                {
                    c.Click += Load_Click;
                }
                if (c.Header.ToString() == "DSAVE")
                {
                    c.Click += DeleteSave_Click;
                }
                if (c.Header.ToString() == "DRAW")
                {
                    c.Click += DrawLine_Click;
                }
                if (c.Header.ToString() == "DROP")
                {
                    c.Click += DropLine_Click;
                }
                if (c.Header.ToString() == "X ")
                {
                    c.Click += ZamkniecieMenu_Click;
                }
            }
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo DirectoryPathh = new DirectoryInfo(path);
            DirectoryInfo[] dirDirection = DirectoryPathh.GetDirectories("Save" + "*.*");
            bool okotwarcie = false;
            foreach (DirectoryInfo foundDirection in dirDirection)
            {
                okotwarcie = true;
            }
            if (okotwarcie) { OdswiezenieComboxItemkow(ob); }

            //IndexZ Dokpanel
            if (InfIndexOtw > -5)
            {
                Dokpanel.Children.RemoveAt(InfIndexZam);
            }
            Dokpanel.Children.Insert(0, ob);
            InfIndexOtw = Dokpanel.Children.IndexOf(ob);
        }
        bool it = true;
        Key zapis = Key.A;
        Stopwatch stopWatch = new Stopwatch();

        public void HandleKeyPress(object sender, KeyEventArgs e)
        {

            if (stopWatch.Elapsed.TotalSeconds > 1)
            {
                it = true;
                zapis = Key.A;
                stopWatch.Reset();
            }
            if (it)
            {
                zapis = e.Key;
                it = false;
                stopWatch.Start();
                return;
            }

            stopWatch.Stop();

            it = true;
            if ((e.Key == Key.D && zapis == Key.LeftCtrl) || (e.Key == Key.LeftCtrl && zapis == Key.D))
            {
                //Rysowanie lini Ctrl + D
                DrawLine_Click(null, null);
                zapis = Key.A;
            }
            if ((e.Key == Key.M && zapis == Key.LeftCtrl) || (e.Key == Key.LeftCtrl && zapis == Key.M))
            {
                //WywolanieFunkcjiSlowMontion
                //Zmiana dzialania funkcji ResizeCanvas()
                if (slowmontionresizecanvas)
                {
                    slowmontionresizecanvas = false;
                }
                else
                {
                    slowmontionresizecanvas = true;
                }
                zapis = Key.A;
            }
            stopWatch.Reset();
        }

        private void ZamkniecieMenu_Click(object sender, RoutedEventArgs e)
        {
            Dokpanel.Children.RemoveAt(InfIndexOtw);
            var Mob = (Menu)Application.LoadComponent(new Uri("xml3.xaml", System.UriKind.RelativeOrAbsolute));
            foreach (var c in Mob.Items.OfType<MenuItem>())
            {
                if (c.Header.ToString() == "<--")
                {
                    c.Click += OtwarcieMenu_Click;
                }
            }
            Dokpanel.Children.Insert(0, Mob);
            InfIndexZam = Dokpanel.Children.IndexOf(Mob);
        }

        private void OdswiezenieComboxItemkow(Menu ObjektMenu)
        {
            int indexloadsave = 1;
            int indexxwhile = indexloadsave;
            bool IndexZero = true;
            string pathfind = System.AppDomain.CurrentDomain.BaseDirectory;
            string savepathfind = pathfind + @"Save" + indexloadsave;
            foreach (var itemdel in ObjektMenu.Items.OfType<ComboBox>())
            {
                if (itemdel.Name.ToString() == "ComboxxItems")
                {
                    itemdel.Items.Clear();
                }
            }
            while (Directory.Exists(savepathfind))
            {
                IndexZero = false;
                savepathfind = pathfind + @"Save" + indexloadsave;
                ComboBoxItem newItembox = new ComboBoxItem();
                newItembox.Tag = indexloadsave;
                newItembox.Content = indexloadsave;
                foreach (var item in ObjektMenu.Items.OfType<ComboBox>())
                {
                    if (item.Name.ToString() == "ComboxxItems")
                    {
                        item.Items.Add(newItembox);
                    }
                }
                indexxwhile = indexloadsave + 1;
                if (Directory.Exists(pathfind + @"Save" + indexxwhile)) { indexloadsave++; } else { break; }
            }
            if (IndexZero)
            {
                foreach (var item in ObjektMenu.Items.OfType<ComboBox>())
                {
                    ComboBoxItem newItemboxZero = new ComboBoxItem();
                    newItemboxZero.Tag = 0;
                    newItemboxZero.Content = 0;
                    newItemboxZero.IsSelected = true;
                    if (item.Name.ToString() == "ComboxxItems")
                    {
                        item.Items.Add(newItemboxZero);
                    }
                }
            }
            indexSave = indexloadsave;
            foreach (var item in ObjektMenu.Items.OfType<ComboBox>())
            {
                foreach (var item2 in item.Items.OfType<ComboBoxItem>())
                {
                    if (Convert.ToInt32(item2.Tag) == indexSave)
                    {
                        item2.IsSelected = true;
                    }
                }
            }
        }

        private void DodawanieBlokowMouseFind(Point pozycjamyszki)
        {
            var blok = (Canvas)Application.LoadComponent(new Uri("xml1.xaml", System.UriKind.RelativeOrAbsolute));
            DodawanieEventowDoBlokow(blok);
            var x = pozycjamyszki.X;
            var y = pozycjamyszki.Y;
            if ((x < grid.ActualWidth && x > 0) && (y < grid.ActualHeight && y > 0))
            {
                var h = grid.ActualHeight;
                var w = grid.ActualWidth;
                var ch = blok.Height / 2;
                var cw = blok.Width / 2;

                blok.Margin = new Thickness(x - cw, y - ch, w - x - cw, h - y - ch);
            }
            grid.Children.Add(blok);
        }

        private void WychodzenieMouseZTextBox()
        {
            foreach (var Canvas in grid.Children.OfType<Canvas>())
            {
                foreach (var Bordertxt in Canvas.Children.OfType<Border>())
                {
                    if (Bordertxt.Name == "BorderTytul")
                    {
                        TextBox textBoxtytul = (TextBox)Bordertxt.Child;
                        if (textBoxtytul.IsFocused)
                        {
                            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                            ((TextBox)textBoxtytul).MoveFocus(request);
                        }
                    }
                    if (Bordertxt.Name == "BorderZawartosc")
                    {
                        TextBox textBoxzawartosc = (TextBox)Bordertxt.Child;
                        if (textBoxzawartosc.IsFocused)
                        {
                            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                            ((TextBox)textBoxzawartosc).MoveFocus(request);
                        }
                    }
                    //Dziala
                }
            }
            foreach (var textbox in grid.Children.OfType<TextBox>())
            {
                if (textbox.IsFocused)
                {
                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                    ((TextBox)textbox).MoveFocus(request);
                }
            }
            return;
        }
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //skala
            if (e.Delta > 0)
            {
                GridScaleTransform.ScaleX *= 1.1;
                GridScaleTransform.ScaleY *= 1.1;
            }
            else
            {
                GridScaleTransform.ScaleX *= 0.9;
                GridScaleTransform.ScaleY *= 0.9;
            }
        }


        Point m_start;
        Vector m_startOffset;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as Grid;
            var positionmouse = Mouse.GetPosition(grid);

            if (AddtoBlok)
            {
                DodawanieBlokowMouseFind(positionmouse);
                AddtoBlok = false;
                ActiveAddtoBlok = true;
                System.Windows.Input.Mouse.OverrideCursor = null;
                return;
            }

            if (!indexx.IsMouseOver)
            {
                WychodzenieMouseZTextBox();
                m_start = e.GetPosition(GlowneOkno);
                m_startOffset = new Vector(tt.X, tt.Y);
                element.CaptureMouse();
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as Grid;


            if (!lockresizeCanvas)
            {
                if (Canvaselements != null)
                {
                    if (!Canvaselements.IsMouseOver)
                    {
                        if (resizeCanvas)
                        {
                            System.Windows.Input.Mouse.OverrideCursor = null;
                            resizeCanvas = false;
                        }
                    }
                }
            }

            if (element.IsMouseCaptured)
            {
                Vector offset = Point.Subtract(e.GetPosition(GlowneOkno), m_start);

                tt.X = m_startOffset.X + offset.X;
                tt.Y = m_startOffset.Y + offset.Y;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as Grid;
            element.ReleaseMouseCapture();

        }

        bool DeleteLine = false;
        bool ActiveDeleteLine = true;
        private void LineDelete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Line element = sender as Line;
            if (DeleteLine)
            {
                grid.Children.Remove(element);
                DeleteLine = false;
                System.Windows.Input.Mouse.OverrideCursor = null;
                ActiveDeleteLine = true;
                return;
            }
        }

        private void DropLine_Click(object sender, RoutedEventArgs e)
        {
            ActiveDelete = true;
            ActiveDrawLine = true;
            ActiveAddtoBlok = true;
            if (ActiveDeleteLine)
            {
                DeleteLine = true;
                Delete = false;
                DrawLine = false;
                AddtoBlok = false;
                ActiveDeleteLine = false;
                System.Windows.Input.Mouse.OverrideCursor = Cursors.UpArrow;
                return;
            }
            DeleteLine = false;
            ActiveDeleteLine = true;
            System.Windows.Input.Mouse.OverrideCursor = null;
        }

        private void DeleteLineCanvas(Canvas element)
        {
            var HashCodeLineCanvas = element.GetHashCode().ToString();
            string HashCodeLineCanvasConvert = KodowanieNaZankiName2(HashCodeLineCanvas, null);
            for (int i = grid.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = grid.Children[i];
                if (Child is Line)
                {
                    foreach (var liny in grid.Children.OfType<Line>())
                    {
                        var nazwaliny = liny.Name;
                        var indexzzline = nazwaliny.IndexOf("zz", 0);
                        if (indexzzline >= 0)
                        {
                            string LineElementCanvas1 = nazwaliny.Substring(0, indexzzline);
                            string LineElementCanvas2 = nazwaliny.Substring(indexzzline + 2);
                            if (HashCodeLineCanvasConvert == LineElementCanvas1)
                            {
                                grid.Children.Remove(liny);
                                break;
                            }
                            if (HashCodeLineCanvasConvert == LineElementCanvas2)
                            {
                                grid.Children.Remove(liny);
                                break;
                            }
                        }
                    }
                }
            }

        }


        Canvas dragObject = null;
        Canvas indexx;
        Canvas element1;
        Canvas element2;
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas element = sender as Canvas;

            if (Delete)
            {
                grid.Children.Remove(element);
                DeleteLineCanvas(element);
                Delete = false;
                System.Windows.Input.Mouse.OverrideCursor = null;
                return;
            }
            if (DrawLine && !AddtoBlok)
            {
                if (DrawLine1)
                {
                    System.Windows.Input.Mouse.OverrideCursor = Cursors.Cross;
                    element1 = element;
                    DrawLine1 = false;
                    DrawLine2 = true;
                    return;
                }
                if (DrawLine2)
                {
                    element2 = element;
                    bool czyistniejelina = SprawdzenieCzyIsteniejeLinia(element1, element2);
                    if (!czyistniejelina)
                    {
                        if (element1.GetHashCode() != element2.GetHashCode())
                        {
                            RysowanieLine(element1, element2);
                        }
                    }
                    DrawLine2 = false;
                    DrawLine = false;
                    ActiveDrawLine = true;
                    ActiveDelete = true;
                    ActiveDeleteLine = true;
                    ActiveAddtoBlok = true;
                    System.Windows.Input.Mouse.OverrideCursor = null;
                    return;
                }
                return;
            }

            dragObject = sender as Canvas;
            element.CaptureMouse();
            indexx = element;
            ActiveDrawLine = true;
            ActiveDelete = true;
            ActiveDeleteLine = true;
            ActiveAddtoBlok = true;
            System.Windows.Input.Mouse.OverrideCursor = Cursors.SizeAll;
            grid.Children.RemoveAt(grid.Children.IndexOf(indexx));
            grid.Children.Insert(grid.Children.Count, indexx);
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!DrawLine)
            {
                //Mouse release
                lockresizeCanvas = false;
                dragObject = null;
                indexx.ReleaseMouseCapture();
                System.Windows.Input.Mouse.OverrideCursor = null;
            }

        }

        Canvas Canvaselements = null;
        bool resizeCanvas = false;
        bool lockresizeCanvas = false;
        bool slowmontionresizecanvas = false;

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (!DrawLine)
            {
                Canvaselements = sender as Canvas;
                //Pozycja myszki
                var positioncanvas = Mouse.GetPosition(Canvaselements);
                if (!lockresizeCanvas)
                {
                    if (positioncanvas.X < 20 && positioncanvas.Y < 20)
                    {
                        //Aktywacja skalowania bloku
                        System.Windows.Input.Mouse.OverrideCursor = Cursors.SizeNWSE;
                        resizeCanvas = true;
                    }
                    else if (resizeCanvas)
                    {
                        System.Windows.Input.Mouse.OverrideCursor = null;
                        resizeCanvas = false;
                    }
                }
                if (indexx.IsMouseCaptured)
                {
                    if (dragObject != null)
                    {
                        var position = Mouse.GetPosition(grid);

                        var x = position.X;
                        var y = position.Y;

                        var h = grid.ActualHeight;
                        var w = grid.ActualWidth;
                        var ch = indexx.ActualHeight / 2;
                        var cw = indexx.ActualWidth / 2;

                        if (resizeCanvas)
                        {
                            //Resize
                            if ((x < grid.ActualWidth && x > 0) && (y < grid.ActualHeight && y > 0))
                            {
                                lockresizeCanvas = true;
                                var positionCanvas = Mouse.GetPosition(indexx);

                                ResizeCanvas(positionCanvas);
                                OdswiezaniePozycjiLin(x + cw, y + ch);

                                if (!slowmontionresizecanvas)
                                {
                                    dragObject.Margin = new Thickness(x, y, w - x, h - y);
                                }
                            }
                        }
                        else
                        {
                            //Move
                            if ((x < grid.ActualWidth && x > 0) && (y < grid.ActualHeight && y > 0))
                            {
                                OdswiezaniePozycjiLin(x, y);
                                dragObject.Margin = new Thickness(x - cw, y - ch, w - x - cw, h - y - ch);
                            }
                        }
                    }
                }
            }

        }

        private void ResizeCanvas(Point postionselectCanvas)
        {
            //Resize
            var postionriszeX = Convert.ToInt32(postionselectCanvas.X);
            var postionriszeY = Convert.ToInt32(postionselectCanvas.Y);

            if (slowmontionresizecanvas)
            {
                if (postionriszeX < -1)
                {
                    postionriszeX = -1;
                }
                if (postionriszeX > 1)
                {
                    postionriszeX = 1;
                }
                if (postionriszeY < -1)
                {
                    postionriszeY = -1;
                }
                if (postionriszeY > 1)
                {
                    postionriszeY = 1;
                }
            }

            if (indexx.Width - postionriszeX >= 105)
            {
                indexx.Width -= postionriszeX;
            }
            if (indexx.Height - postionriszeY >= 130)
            {
                indexx.Height -= postionriszeY;
            }


            foreach (var Bordertextresize in indexx.Children.OfType<Border>())
            {
                if (Bordertextresize.Name == "BorderTytul")
                {
                    if (Bordertextresize.Width - postionriszeX > 10)
                    {
                        Bordertextresize.Width -= postionriszeX;
                    }
                }
                if (Bordertextresize.Name == "BorderZawartosc")
                {
                    if (Bordertextresize.Width - postionriszeX > 10)
                    {
                        Bordertextresize.Width -= postionriszeX;
                    }
                    if (Bordertextresize.Height - postionriszeY > 10)
                    {
                        Bordertextresize.Height -= postionriszeY;
                    }
                }
            }

            foreach (var Przycisk in indexx.Children.OfType<Button>())
            {
                if (Przycisk.Name == "DodawaniePodstawa")
                {
                    Canvas.SetLeft(Przycisk, indexx.Width - 60);
                    Canvas.SetTop(Przycisk, indexx.Height - 30);
                }
                if (Przycisk.Name == "Dodawanie")
                {
                    Canvas.SetLeft(Przycisk, indexx.Width - 105);
                    Canvas.SetTop(Przycisk, indexx.Height - 30);
                }
                if (Przycisk.Name == "Usuwanie")
                {
                    Canvas.SetLeft(Przycisk, indexx.Width - 60);
                    Canvas.SetTop(Przycisk, indexx.Height - 30);
                }
            }

            return;
        }

        private void OdswiezaniePozycjiLin(double pozycjaX, double pozycjaY)
        {
            var HashCodeLine = indexx.GetHashCode().ToString();
            string HashCodeLineConvert = KodowanieNaZankiName2(HashCodeLine, null);
            foreach (var liny in grid.Children.OfType<Line>())
            {
                var nazwaliny = liny.Name;
                var czyjestindex = nazwaliny.IndexOf("zz", 0);
                if (czyjestindex >= 0)
                {
                    //Linia
                    string lineElement1 = nazwaliny.Substring(0, czyjestindex);
                    string lineElement2 = nazwaliny.Substring(czyjestindex + 2);

                    if (HashCodeLineConvert == lineElement1)
                    {
                        //Linia1
                        liny.X1 = pozycjaX;
                        liny.Y1 = pozycjaY;
                    }
                    if (HashCodeLineConvert == lineElement2)
                    {
                        //Linia2
                        liny.X2 = pozycjaX;
                        liny.Y2 = pozycjaY;
                    }

                }
            }
            return;
        }

        private bool SprawdzenieCzyIsteniejeLinia(Canvas objekkt1, Canvas objekkt2)
        {
            string HashCodeOb1 = objekkt1.GetHashCode().ToString();
            string HashCodeOb2 = objekkt2.GetHashCode().ToString();
            string HashCodeAllObConvert = KodowanieNaZankiName2(HashCodeOb1, HashCodeOb2);
            bool linaistnieje = false;
            foreach (var linyspr in grid.Children.OfType<Line>())
            {
                if (linyspr.Name == HashCodeAllObConvert)
                {
                    linaistnieje = true;
                }
            }
            return linaistnieje;
        }

        bool Save = false;
        int indexSave = 1;

        private void WczytanieWartoscPublicIndexSave()
        {
            foreach (var menuelement in Dokpanel.Children.OfType<Menu>())
            {
                if (menuelement.Name == "Mennu")
                {
                    foreach (var item in menuelement.Items.OfType<ComboBox>())
                    {
                        if (item.Name.ToString() == "ComboxxItems")
                        {
                            indexSave = Convert.ToInt32(item.SelectedValue.ToString().Substring(38));
                            if (indexSave == 0) { indexSave = 1; }
                        }
                    }
                }
            }
        }

        private void NadpiszSave_Click(object sender, RoutedEventArgs e)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo DirectoryPathh = new DirectoryInfo(path);
            DirectoryInfo[] dirDirection = DirectoryPathh.GetDirectories("Save" + "*.*");
            bool oknadpis = false;
            foreach (DirectoryInfo foundDirection in dirDirection)
            {
                oknadpis = true;
            }
            if (oknadpis)
            {
                WczytanieWartoscPublicIndexSave();
            }
            Save = false;
            SaveZapis();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save = true;
            SaveZapis();
        }

        private void SaveZapis()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string savepath = path + @"Save" + indexSave;
            if (!Save)
            {
                if (Directory.Exists(savepath))
                {
                    System.IO.DirectoryInfo Directorytytul = new DirectoryInfo(savepath + @"\Tytul");

                    if (!Directory.Exists(savepath + @"\Tytul"))
                    {
                        string pathpolaczenie = System.IO.Path.Combine(savepath, "Tytul");
                        Directory.CreateDirectory(pathpolaczenie);
                    }

                    foreach (FileInfo file in Directorytytul.GetFiles())
                    {
                        file.Delete();
                    }

                    System.IO.DirectoryInfo di = new DirectoryInfo(savepath + @"\Zawartosc");

                    if (!Directory.Exists(savepath + @"\Zawartosc"))
                    {
                        string pathpolaczenie = System.IO.Path.Combine(savepath, "Zawartosc");
                        Directory.CreateDirectory(pathpolaczenie);
                    }

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    Zapis(savepath);
                }
                else
                {
                    Save = true;
                }
            }
            while (Save)
            {
                if (Directory.Exists(savepath))
                {
                    indexSave++;
                    savepath = path + @"Save" + indexSave;
                    Save = true;
                }
                else
                {
                    Directory.CreateDirectory(savepath);
                    Zapis(savepath);
                    foreach (var menuelement in Dokpanel.Children.OfType<Menu>())
                    {
                        if (menuelement.Name == "Mennu")
                        {
                            OdswiezenieComboxItemkow(menuelement);
                        }
                    }
                    Save = false;
                }
            }
        }
        public bool IsDirectoryEmpty(string path)
        {
            string[] dirs = System.IO.Directory.GetDirectories(path); string[] files = System.IO.Directory.GetFiles(path);
            return dirs.Length == 0 && files.Length == 0;
        }
        private void Zapis(string savepath)
        {
            string y = "";
            int intindyfikator = 0;
            foreach (var tb in FindVisualChildren<Canvas>(GlowneOkno))
            {
                ++intindyfikator;
                y += "<blok id=" + intindyfikator + ">" + "\n";
                y += "<pozycja>" + tb.Margin.ToString() + "</pozycja>" + "\n" + "<index>" + grid.Children.IndexOf(tb) + "</index>" + "\n" + "<hashcode>" + tb.GetHashCode() + "</hashcode>" + "\n";
                foreach (var tc in FindVisualChildren<TextBox>(tb))
                {
                    if (tc.Name == "Zawartosctxt")
                    {
                        string pathsprawdzanie = savepath + @"\Zawartosc";
                        string pathpolaczenie = System.IO.Path.Combine(savepath, "Zawartosc"); //SubDirectory
                        if (!Directory.Exists(pathsprawdzanie))
                        {
                            Directory.CreateDirectory(pathpolaczenie);
                        }
                        File.WriteAllText(savepath + @"\Zawartosc\" + tb.GetHashCode() + grid.Children.IndexOf(tb) + ".txt", tc.Text);
                    }
                    if (tc.Name == "Tytultxt")
                    {
                        string pathsprawdzanie = savepath + @"\Tytul";
                        string pathpolaczenie = System.IO.Path.Combine(savepath, "Tytul"); //SubDirectory
                        if (!Directory.Exists(pathsprawdzanie))
                        {
                            Directory.CreateDirectory(pathpolaczenie);
                        }
                        File.WriteAllText(savepath + @"\Tytul\" + tb.GetHashCode() + grid.Children.IndexOf(tb) + ".txt", tc.Text);
                    }
                }
                y += "</blok>" + "\n";
            }
            y += "<iloscblokow>" + intindyfikator + "</iloscblokow>" + "\n";
            int iddlin = 0;
            foreach (var lina in grid.Children.OfType<Line>())
            {
                iddlin++;
                y += "<linia id=" + iddlin + ">" + "\n";
                y += "<nazwa>" + lina.Name + "</nazwa>" + "\n";
                y += "<pozycja>" + "\n";
                y += "<X1>" + lina.X1 + "</X1>" + "<Y1>" + lina.Y1 + "</Y1>" + "\n";
                y += "<X2>" + lina.X2 + "</X2>" + "<Y2>" + lina.Y2 + "</Y2>" + "\n";
                y += "</pozycja>" + "\n";
                y += "</linia>" + "\n";
            }
            y += "<ilosclini>" + iddlin + "</ilosclini>";
            File.WriteAllText(savepath + @"\text.txt", y);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string partialName = "Save";

            DirectoryInfo DirectoryPathh = new DirectoryInfo(path);
            DirectoryInfo[] dirsInDir = DirectoryPathh.GetDirectories(partialName + "*.*");
            bool ok = false;
            foreach (DirectoryInfo foundDir in dirsInDir)
            {
                ok = true;
                string fullName = foundDir.FullName;

            }
            if (ok)
            {
                WczytanieWartoscPublicIndexSave();
                if (File.Exists(path + @"Save" + indexSave + @"\text.txt"))
                {
                    string txt = File.ReadAllText(path + @"Save" + indexSave + @"\text.txt");

                    LadowanieBlokow(txt);


                    Thickness pozycja = LadowaniePozycjiMargin(true, txt, null, null);
                    canvas.Margin = pozycja;
                }
            }
        }

        private void LadowanieBlokow(string textzawartosc)
        {
            for (int i = grid.Children.Count - 1; i >= 0; i += -1)
            {
                UIElement Child = grid.Children[i];
                if (Child is Canvas)
                {
                    grid.Children.Remove(Child);
                }
                if (Child is Line)
                {
                    grid.Children.Remove(Child);
                }
            }
            //Ilosc blokow
            string startiloscblokow = "<iloscblokow>";
            string konieciloscblokow = "</iloscblokow>";
            int iloscblokowstring = Convert.ToInt32(SzukanieTekstu(startiloscblokow, konieciloscblokow, textzawartosc));


            string[] Hashe = new string[iloscblokowstring];
            for (int i = 1; i <= iloscblokowstring; i++)
            {
                //Zawartosc bloku
                string startbloku = "<blok id=" + i + ">";
                string koniecbloku = "</blok>";
                string zawartoscbloku = SzukanieTekstu(startbloku, koniecbloku, textzawartosc);
                LadowaniePojedynczegoBloku(zawartoscbloku, i, Hashe);
            }

            //Ilosc lini
            string startilosclini = "<ilosclini>";
            string koniecilosclini = "</ilosclini>";
            int ilosclinistring = Convert.ToInt32(SzukanieTekstu(startilosclini, koniecilosclini, textzawartosc));

            for (int i = 1; i <= ilosclinistring; i++)
            {
                //Zawartosc lini
                string startlinia = "<linia id=" + i + ">";
                string konieclinia = "</linia>";
                string zawartosclini = SzukanieTekstu(startlinia, konieclinia, textzawartosc);
                LadowaniePojedynczejLini(zawartosclini, Hashe);
            }
        }

        private void LadowaniePojedynczejLini(string zawartoscdanejlini, string[] Hashee)
        {
            string Nazwalini = ZmianaHashCodowNazwa(zawartoscdanejlini, Hashee);
            var pozycja = LadowaniePozycji(zawartoscdanejlini);
            double X1 = pozycja.Item1;
            double Y1 = pozycja.Item2;
            double X2 = pozycja.Item3;
            double Y2 = pozycja.Item4;
            Line LiniaDrawLoad = new Line();
            LiniaDrawLoad.Name = Nazwalini;
            LiniaDrawLoad.X1 = X1;
            LiniaDrawLoad.Y1 = Y1;
            LiniaDrawLoad.X2 = X2;
            LiniaDrawLoad.Y2 = Y2;
            LiniaDrawLoad.Stroke = Brushes.Black;
            LiniaDrawLoad.StrokeThickness = 4;
            LiniaDrawLoad.MouseLeftButtonDown += LineDelete_MouseLeftButtonDown;
            grid.Children.Insert(0, LiniaDrawLoad);
            return;
        }

        private Tuple<double, double, double, double> LadowaniePozycji(string zawartosclini)
        {
            string startpozycjilini = "<pozycja>";
            string koniecpozycjilini = "</pozycja>";
            string zawartospozycjilini = SzukanieTekstu(startpozycjilini, koniecpozycjilini, zawartosclini);
            string startx1 = "<X1>";
            string koniecx1 = "</X1>";
            double zawartoscx1 = Convert.ToDouble(SzukanieTekstu(startx1, koniecx1, zawartospozycjilini));
            string starty1 = "<Y1>";
            string koniecy1 = "</Y1>";
            double zawartoscy1 = Convert.ToDouble(SzukanieTekstu(starty1, koniecy1, zawartospozycjilini));
            string startx2 = "<X2>";
            string koniecx2 = "</X2>";
            double zawartoscx2 = Convert.ToDouble(SzukanieTekstu(startx2, koniecx2, zawartospozycjilini));
            string starty2 = "<Y2>";
            string koniecy2 = "</Y2>";
            double zawartoscy2 = Convert.ToDouble(SzukanieTekstu(starty2, koniecy2, zawartospozycjilini));
            return Tuple.Create(zawartoscx1, zawartoscy1, zawartoscx2, zawartoscy2);
        }

        private string ZmianaHashCodowNazwa(string zawartosclini, string[] Hashy)
        {
            string Nazwalini = LadowanieNazwyLini(zawartosclini);
            var OdkodowanaNazwa = OdkodowanieZeZnakiName2(Nazwalini);
            string NazwaHashCodeOdkodowany1 = OdkodowanaNazwa.Item1;
            string NazwaHashCodeOdkodowany2 = OdkodowanaNazwa.Item2;
            string OstatecznaNazwa = "Lina";
            if (NazwaHashCodeOdkodowany1 != "brak" && NazwaHashCodeOdkodowany2 != "brak")
            {
                foreach (var ix in Hashy)
                {
                    //Porownywanie starego HashCode bloka z nowym i ustawianie nowego HashCode dla lini
                    int IndexWycieciaHasha = ix.IndexOf(",");
                    string Hashcodestary = ix.Substring(0, IndexWycieciaHasha);
                    string Hashcodenowy = ix.Substring(IndexWycieciaHasha + 1);

                    if (NazwaHashCodeOdkodowany1 == Hashcodestary)
                    {
                        NazwaHashCodeOdkodowany1 = Hashcodenowy;
                    }
                    if (NazwaHashCodeOdkodowany2 == Hashcodestary)
                    {
                        NazwaHashCodeOdkodowany2 = Hashcodenowy;
                    }
                }
                OstatecznaNazwa = KodowanieNaZankiName2(NazwaHashCodeOdkodowany1, NazwaHashCodeOdkodowany2);
            }
            //Ostateczna nazwa: zakodowane dwa nowe HashCode-y
            return OstatecznaNazwa;
        }


        private static string LadowanieNazwyLini(string zawartosclini)
        {
            string startnazwa = "<nazwa>";
            string koniecnazwa = "</nazwa>";
            string zawartoscnazwa = SzukanieTekstu(startnazwa, koniecnazwa, zawartosclini);
            return zawartoscnazwa;
        }


        public void LadowaniePojedynczegoBloku(string zawartoscdanegobloku, int indexi, string[] Hashy)
        {

            Thickness Pozycjabloku = LadowaniePozycjiMargin(true, zawartoscdanegobloku, null, null);
            int Indexoff = LadowanieIndexuChild(zawartoscdanegobloku);
            int Hashcode = LadowanieHashCode(zawartoscdanegobloku);
            var Textzawartosctextblock = LadowanieZawartoscText(zawartoscdanegobloku, Indexoff);
            string Textzawartosctytul = Textzawartosctextblock.Item1;
            string Textzawartoscbloku = Textzawartosctextblock.Item2;
            var blok = (Canvas)Application.LoadComponent(new Uri("xml1.xaml", System.UriKind.RelativeOrAbsolute));
            DodawanieEventowDoBlokow(blok);
            blok.Margin = Pozycjabloku;
            DodawanieZawarotscDoBlkow(blok, Textzawartosctytul, Textzawartoscbloku);
            Hashy[indexi - 1] = Hashcode.ToString() + "," + blok.GetHashCode().ToString();
            int Indexindexof = grid.Children.Count;
            if (Indexindexof < Indexoff)
            {
                grid.Children.Insert(0, blok);
            }
            else
            {
                grid.Children.Insert(Indexoff, blok);
            }
        }

        private void DodawanieZawarotscDoBlkow(Canvas blok, string tytultextbox, string zawartosctextbox)
        {
            foreach (var Bordertxt in blok.Children.OfType<Border>())
            {
                if (Bordertxt.Name == "BorderTytul")
                {
                    TextBox textBoxtytul = (TextBox)Bordertxt.Child;
                    textBoxtytul.Text = tytultextbox;
                }
                if (Bordertxt.Name == "BorderZawartosc")
                {
                    TextBox textBoxtxt = (TextBox)Bordertxt.Child;
                    textBoxtxt.Text = zawartosctextbox;
                }
            }
        }

        private void DodawanieEventowDoBlokow(Canvas blok)
        {
            foreach (var c in blok.Children.OfType<Button>())
            {
                if (c.Name == "Dodawanie")
                {
                    c.Click += Button_Click;
                }
            }
            blok.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            blok.MouseMove += Canvas_MouseMove;
            blok.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
        }

        private int LadowanieHashCode(string zawartoscbloku)
        {
            string starthashcode = "<hashcode>";
            string koniechashcode = "</hashcode>";
            int hashcode = Convert.ToInt32(SzukanieTekstu(starthashcode, koniechashcode, zawartoscbloku));
            return hashcode;
        }

        public Tuple<string, string> LadowanieZawartoscText(string zawartoscbloku, int indexoffcode)
        {
            string starthashcode = "<hashcode>";
            string koniechashcode = "</hashcode>";
            int hashcode = Convert.ToInt32(SzukanieTekstu(starthashcode, koniechashcode, zawartoscbloku));
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string pahttext = path + @"Save" + indexSave + @"\Zawartosc\" + hashcode + indexoffcode + ".txt";
            string pahttexttytul = path + @"Save" + indexSave + @"\Tytul\" + hashcode + indexoffcode + ".txt";
            string Zawartosctextbloktytul = "Brak tytulu";
            string Zawartosctextblok = "Nic tu nie ma";
            if (File.Exists(pahttexttytul))
            {
                Zawartosctextbloktytul = File.ReadAllText(pahttexttytul);
            }
            if (File.Exists(pahttext))
            {
                Zawartosctextblok = File.ReadAllText(pahttext);
            }
            return Tuple.Create(Zawartosctextbloktytul, Zawartosctextblok);
        }


        public static int LadowanieIndexuChild(string zawartoscbloku)
        {
            string startindex = "<index>";
            string koniecindex = "</index>";
            int Wyjscieint = Convert.ToInt32(SzukanieTekstu(startindex, koniecindex, zawartoscbloku));
            return Wyjscieint;
        }

        public static Thickness LadowaniePozycjiMargin(bool domyslnie, string zawartoscmargin, string confstartinfmargin, string confkoniecinfmargin)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            string startinfmargin, koniecinfmargin;
            if (domyslnie)
            {
                startinfmargin = "<pozycja>";
                koniecinfmargin = "</pozycja>";
            }
            else
            {
                startinfmargin = confstartinfmargin;
                koniecinfmargin = confkoniecinfmargin;
            }
            string margin = SzukanieTekstu(startinfmargin, koniecinfmargin, zawartoscmargin);

            int indexPozycji = 0;
            double x = SzukaniePozycjiMargin(margin, indexPozycji);
            indexPozycji += x.ToString().Length + 1;
            double y = SzukaniePozycjiMargin(margin, indexPozycji);
            indexPozycji += y.ToString().Length + 1;
            double z = SzukaniePozycjiMargin(margin, indexPozycji);
            indexPozycji += z.ToString().Length + 1;
            string Ostatniev = margin.Substring(indexPozycji, margin.Length - indexPozycji);
            double v = Convert.ToDouble(Ostatniev, provider);

            Thickness Wyjscie = new Thickness(x, y, z, v);
            return Wyjscie;
        }

        public static double SzukaniePozycjiMargin(string tekst, int pozycjastart)
        {
            int koniecSaba;
            string Wyjscie;
            koniecSaba = tekst.IndexOf(",", pozycjastart);
            Wyjscie = tekst.Substring(pozycjastart, koniecSaba - pozycjastart);
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            double WyjscieDouble = Convert.ToDouble(Wyjscie, provider);
            return WyjscieDouble;
        }

        public static string SzukanieTekstu(string starttxt, string koniectxt, string tekst)
        {
            if (tekst.Contains(starttxt) && tekst.Contains(koniectxt))
            {
                int startSaba, koniecSaba;
                string Wyjscie;
                startSaba = tekst.IndexOf(starttxt, 0) + starttxt.Length;
                koniecSaba = tekst.IndexOf(koniectxt, startSaba);
                Wyjscie = tekst.Substring(startSaba, koniecSaba - startSaba);
                return Wyjscie;
            }
            return null;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
                yield return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child is T)
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
        private void DeleteSave_Click(object sender, RoutedEventArgs e)
        {
            WczytanieWartoscPublicIndexSave();
            string sciezka = System.AppDomain.CurrentDomain.BaseDirectory;
            string sciezkazapisu = sciezka + @"Save" + indexSave;
            MessageBoxResult WynikDzialania = MessageBox.Show("Chcesz usunac swoj ceny zapis?", "Czy aby napewno? Czy wiesz co robisz?!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (WynikDzialania == MessageBoxResult.Yes)
            {
                if (Directory.Exists(sciezkazapisu))
                {
                    Directory.Delete(sciezkazapisu, true);
                    foreach (var menuItem in Dokpanel.Children.OfType<Menu>())
                    {
                        if (menuItem.Name == "Mennu")
                        {
                            OdswiezenieComboxItemkow(menuItem);
                        }
                    }
                    if (!Directory.Exists(sciezkazapisu))
                    {
                        MessageBox.Show(
                            "Twoj zapis zostal usniety",
                            "Tak szybko umieraja",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.OK,
                            MessageBoxOptions.ServiceNotification
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            "Niestety twoj zapis ma potezna zbroje",
                            "Nie mozna usunac zapisu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information,
                            MessageBoxResult.OK,
                            MessageBoxOptions.ServiceNotification
                        );
                    }
                }
                else
                {
                    MessageBox.Show(
                            "Brak zapisu\n" +
                            "dysk zjadl\n" +
                            "); Smuteczek\n" +
                            "",
                            "No czasem tak bywa",
                            MessageBoxButton.OK,
                            MessageBoxImage.Stop,
                            MessageBoxResult.OK,
                            MessageBoxOptions.RtlReading
                    );
                }
            }
            OdswiezanieKatalogowSave();
        }

        private void OdswiezanieKatalogowSave()
        {
            string sciezka = System.AppDomain.CurrentDomain.BaseDirectory;
            int katalogi = 0;
            DirectoryInfo[] LokalizcjaKatalogow = new DirectoryInfo(sciezka).GetDirectories(@"Save" + "*");
            int[] zawartosckatalogow = new int[LokalizcjaKatalogow.Length];
            int minusoweLiczby = 0;
            for (int i = 0; LokalizcjaKatalogow.Length > i; i++)
            {
                string indexdlugosctext = LokalizcjaKatalogow[i].Name.Substring("Save".Length);
                int indexdlugosc = 0;
                if (int.TryParse(indexdlugosctext, out int n))
                {
                    indexdlugosc = Convert.ToInt32(indexdlugosctext);
                }
                if (indexdlugosctext == "0")
                {
                    minusoweLiczby++;
                    indexdlugosc = 0;
                }
                if (indexdlugosctext == "")
                {
                    minusoweLiczby++;
                    indexdlugosc = 0;
                }
                else
                {
                    for (int znakchar = 32; znakchar < 48; znakchar++)
                    {
                        if (indexdlugosctext.Substring(0, 1) == ((char)znakchar).ToString())
                        {
                            //Minusowa liczba: indexdlugosctext
                            minusoweLiczby++;
                            indexdlugosc = 0;
                        }
                    }
                }
                zawartosckatalogow[i] = indexdlugosc;
            }


            void ZawartoscKatalogo0i1()
            {
                var skladowa2 = zawartosckatalogow[0];
                for (int oko = 0; zawartosckatalogow.Length > oko + 1; oko++)
                {
                    zawartosckatalogow[oko] = zawartosckatalogow[oko + 1];
                }
                zawartosckatalogow[zawartosckatalogow.Length - 1] = skladowa2;

                var skladowa = LokalizcjaKatalogow[0];
                for (int oko = 0; LokalizcjaKatalogow.Length > oko + 1; oko++)
                {
                    LokalizcjaKatalogow[oko] = LokalizcjaKatalogow[oko + 1];
                }
                LokalizcjaKatalogow[LokalizcjaKatalogow.Length - 1] = skladowa;
                return;
            }

            while (minusoweLiczby > 0)
            {
                ZawartoscKatalogo0i1();
                minusoweLiczby--;
            }


            for (int i = 0; LokalizcjaKatalogow.Length > i; i++)
            {

                if (i + 1 != zawartosckatalogow[i] && zawartosckatalogow[i] != 0)
                {
                    int co = 0;
                    int g = 0;
                    int gdzie = 0;
                    bool iplus = false;
                    bool iduzo = false;
                    for (int z = i; zawartosckatalogow.Length > z; z++)
                    {
                        if (i + 1 == zawartosckatalogow[z] && zawartosckatalogow[z] != 0 && iduzo == false)
                        {
                            gdzie = z;
                            iplus = true;
                            break;
                        }
                        if ((i + 1 < zawartosckatalogow[z] && zawartosckatalogow[z] != 0) || (iduzo == true && zawartosckatalogow[z] != 0))
                        {
                            if (iduzo)
                            {
                                //TRUE
                                g = zawartosckatalogow[z];
                                if (g < co)
                                {
                                    co = zawartosckatalogow[z];
                                    gdzie = z;
                                }
                            }
                            else
                            {
                                //FALSE
                                co = zawartosckatalogow[z];
                                gdzie = z;
                            }
                            iplus = true;
                            iduzo = true;
                        }
                    }
                    if (iplus)
                    {
                        var schowek2 = zawartosckatalogow[i];
                        zawartosckatalogow[i] = zawartosckatalogow[gdzie];
                        zawartosckatalogow[gdzie] = schowek2;


                        var schowek = LokalizcjaKatalogow[i];
                        LokalizcjaKatalogow[i] = LokalizcjaKatalogow[gdzie];
                        LokalizcjaKatalogow[gdzie] = schowek;

                    }

                }
            }

            foreach (DirectoryInfo SzukaneKatalogi2 in LokalizcjaKatalogow)
            {
                //Katalog
                string indexkatalogutext = SzukaneKatalogi2.Name.Substring("Save".Length);
                if (indexkatalogutext != null)
                {
                    katalogi++;
                    int indexkatalogu;
                    if (int.TryParse(indexkatalogutext, out int n))
                    {
                        indexkatalogu = Convert.ToInt32(indexkatalogutext);
                    }
                    else
                    {
                        indexkatalogu = 0;
                    }

                    if (indexkatalogu != katalogi || indexkatalogutext.Substring(0, 1) == "+")
                    {
                        SzukaneKatalogi2.MoveTo(sciezka + @"Save" + katalogi);
                    }
                }
            }
            foreach (var menuitemek in Dokpanel.Children.OfType<Menu>())
            {
                if (menuitemek.Name == "Mennu")
                {
                    OdswiezenieComboxItemkow(menuitemek);
                }
            }
            return;
        }


        bool DrawLine = false;
        bool DrawLine1 = false;
        bool DrawLine2 = false;
        bool ActiveDrawLine = true;
        private void DrawLine_Click(object sender, RoutedEventArgs e)
        {
            ActiveDeleteLine = true;
            ActiveAddtoBlok = true;
            ActiveDelete = true;
            if (ActiveDrawLine)
            {
                DrawLine = true;
                DrawLine1 = true;
                DeleteLine = false;
                Delete = false;
                AddtoBlok = false;
                ActiveDrawLine = false;
                System.Windows.Input.Mouse.OverrideCursor = Cursors.Help;
                return;
            }
            DrawLine = false;
            DrawLine1 = false;
            ActiveDrawLine = true;
            System.Windows.Input.Mouse.OverrideCursor = null;
        }


        private void RysowanieLine(Canvas element1, Canvas element2)
        {
            var position = Mouse.GetPosition(grid);

            Line LiniaDraw = new Line();

            var ActualWidthelement1 = element1.ActualWidth / 2;
            var ActualHeightelement1 = element1.ActualHeight / 2;

            var ActualWidthelement2 = element2.ActualWidth / 2;
            var ActualHeightelement2 = element2.ActualHeight / 2;

            var x1 = element1.Margin.Left + ActualWidthelement1;
            var y1 = element1.Margin.Top + ActualHeightelement1;
            LiniaDraw.X1 = x1;
            LiniaDraw.Y1 = y1;

            var x2 = element2.Margin.Left + ActualWidthelement2;
            var y2 = element2.Margin.Top + ActualHeightelement2;
            LiniaDraw.X2 = x2;
            LiniaDraw.Y2 = y2;
            LiniaDraw.Stroke = Brushes.Black;
            LiniaDraw.StrokeThickness = 4;
            LiniaDraw.MouseLeftButtonDown += LineDelete_MouseLeftButtonDown;
            string hashcodel1 = element1.GetHashCode().ToString();
            string hashcodel2 = element2.GetHashCode().ToString();
            string NameLina = KodowanieNaZankiName2(hashcodel1, hashcodel2);
            LiniaDraw.Name = NameLina;

            grid.Children.Insert(0, LiniaDraw);
        }

        private string KodowanieNaZankiName2(string element1, string element2)
        {
            //ASCII
            string wynikconvert = "";
            foreach (char znaki in element1)
            {
                int intcode = znaki + 17;
                wynikconvert += (char)intcode;
            }
            if (element2 != null)
            {
                wynikconvert += "zz";
                foreach (char znaki in element2)
                {
                    int intcode = znaki + 17;
                    wynikconvert += (char)intcode;
                }
            }

            return wynikconvert;
        }

        private Tuple<string, string> OdkodowanieZeZnakiName2(string textdoodkodowania)
        {
            var indexodkodownania = textdoodkodowania.IndexOf("zz", 0);
            if (indexodkodownania >= 0)
            {
                string hashcodedoodkodowania1 = textdoodkodowania.Substring(0, indexodkodownania);
                string hashcodedoodkodowania2 = textdoodkodowania.Substring(indexodkodownania + 2);
                string wynikodkodowanie1 = "";
                string wynikodkodowanie2 = "";
                foreach (char znak in hashcodedoodkodowania1)
                {
                    int zmianacodu = znak - 17;
                    wynikodkodowanie1 += (char)zmianacodu;
                }
                foreach (char znak in hashcodedoodkodowania2)
                {
                    int zmianacodu = znak - 17;
                    wynikodkodowanie2 += (char)zmianacodu;
                }

                return Tuple.Create(wynikodkodowanie1, wynikodkodowanie2);
            }
            return Tuple.Create("brak", "brak");
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ob = (Canvas)Application.LoadComponent(new Uri("xml1.xaml", System.UriKind.RelativeOrAbsolute));
            foreach (var c in ob.Children.OfType<Button>())
            {
                if (c.Name == "Dodawanie")
                {
                    c.Click += Button_Click;
                }
            }
            ob.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            ob.MouseMove += Canvas_MouseMove;
            ob.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            grid.Children.Add(ob);
        }

        bool AddtoBlok = false;
        bool ActiveAddtoBlok = true;

        private void ButtonToAdd_Click(object sender, RoutedEventArgs e)
        {
            ActiveDeleteLine = true;
            ActiveDelete = true;
            ActiveDrawLine = true;
            if (ActiveAddtoBlok)
            {
                DeleteLine = false;
                Delete = false;
                DrawLine = false;
                AddtoBlok = true;
                ActiveAddtoBlok = false;
                System.Windows.Input.Mouse.OverrideCursor = Cursors.UpArrow;
                return;
            }
            AddtoBlok = false;
            ActiveAddtoBlok = true;
            System.Windows.Input.Mouse.OverrideCursor = null;
        }

        bool Delete = false;
        bool ActiveDelete = true;
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ActiveDeleteLine = true;
            ActiveAddtoBlok = true;
            ActiveDrawLine = true;
            if (ActiveDelete)
            {
                DeleteLine = false;
                Delete = true;
                DrawLine = false;
                AddtoBlok = false;
                ActiveDelete = false;
                System.Windows.Input.Mouse.OverrideCursor = Cursors.No;
                return;
            }
            Delete = false;
            ActiveDelete = true;
            System.Windows.Input.Mouse.OverrideCursor = null;
        }

    }

}
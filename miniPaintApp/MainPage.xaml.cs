using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace miniPaintApp
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Point punktPoczatkowy;
        SolidColorBrush pedzel = new SolidColorBrush(Windows.UI.Colors.Black);
        Boolean czyRysuje;
        UIElement usunPoprzednie = null;
        int rysowanie;
        Stack<UIElement> listaUndo = new Stack<UIElement>();
        int j = 0;
        Stack<int> listaUndo2 = new Stack<int>();



        public MainPage()
        {
            this.InitializeComponent();
            
            
        }
        private void rysujLinie(Line linia, Point po)
        {
            if (czyRysuje == true)
            {
                if (rysowanie == 2)
                {

                    linia.X1 = punktPoczatkowy.X;
                    linia.Y1 = punktPoczatkowy.Y;
                    linia.X2 = po.X;
                    linia.Y2 = po.Y;
                    linia.Stroke = pedzel;
                    linia.StrokeThickness = SliderThickness.Value;
                    linia.StrokeEndLineCap = PenLineCap.Round;
                    linia.StrokeStartLineCap = PenLineCap.Round;
                    if (usunPoprzednie != null)
                    {
                        poleRysowania.Children.Remove(usunPoprzednie);
                    }
                    usunPoprzednie = linia;
                    poleRysowania.Children.Add(linia);
                    usunPoprzednie = linia;
                    listaUndo.Push(linia);
                    j++;



                }
                else if (rysowanie == 1)
                {
                    Point pktAkt = po;
                    
                    Windows.UI.Xaml.Shapes.Line kreska = new Windows.UI.Xaml.Shapes.Line {
                        Stroke = pedzel,
                        X2 = pktAkt.X,
                        Y2 = pktAkt.Y,
                        X1 = punktPoczatkowy.X,
                        Y1 = punktPoczatkowy.Y,
                        StrokeThickness = SliderThickness.Value,
                        StrokeEndLineCap = PenLineCap.Round,
                        StrokeStartLineCap = PenLineCap.Round
                        
                        
                    };
                    
                    punktPoczatkowy = pktAkt;
                    poleRysowania.Children.Add(kreska);
                    listaUndo.Push(kreska);
                    j++;

                }
            }
        }

        private void rdbDowolny_Checked(object sender, RoutedEventArgs e)
        {
            rysowanie = 1;
            
        }

        private void rdbProsty_Checked(object sender, RoutedEventArgs e)
        {
            rysowanie = 2;
        }

        private void poleRysowania_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            punktPoczatkowy = e.GetCurrentPoint(poleRysowania).Position;
            
                czyRysuje = true;
            
            
        }

        private void poleRysowania_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Point punktObecny = e.GetCurrentPoint(poleRysowania).Position;
            Line linia = new Line();
            rysujLinie(linia, punktObecny);
            

        }

        private void poleRysowania_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            czyRysuje = false;
        }

       

      

        private void spKolory_PointerPressed(object sender, PointerRoutedEventArgs e) {
           
        }

        private void StackPanelRectangleBlack_PointerPressed(object sender, PointerRoutedEventArgs e) {
            pedzel = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void StackPanelRectangleRed_PointerPressed(object sender, PointerRoutedEventArgs e) {
            pedzel = new SolidColorBrush(Windows.UI.Colors.Red);
        }

        private void StackPanelRectangleGreen_PointerPressed(object sender, PointerRoutedEventArgs e) {
            pedzel = new SolidColorBrush(Windows.UI.Colors.Green);
            
            
        }

        private void StackPanelRectangleBlue_PointerPressed(object sender, PointerRoutedEventArgs e) {
            pedzel = new SolidColorBrush(Windows.UI.Colors.Blue);
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) {

        }

        private void ButtonCofnij_Click(object sender, RoutedEventArgs e) {

            if (listaUndo.Count > 0) {

                for (int i = listaUndo2.Pop(); i != 0; i--) {
                    Shape undo = (Shape)listaUndo.Pop();
                    poleRysowania.Children.Remove(undo);
                }

            }

        }

        private void poleRysowania_PointerReleased(object sender, PointerRoutedEventArgs e) {
            czyRysuje = false;
            usunPoprzednie = null;
            listaUndo2.Push(j);
            j = 0;
        }

        private async void ButtonClose_Click(object sender, RoutedEventArgs e) {
            string tekst = "Czy chcesz opuścić program?";
            //Syntezator 
            var mowa = new SpeechSynthesizer();
            mowa.Options.AudioVolume = 1;
            //Odtwarzacz
            MediaElement odtwarzacz = new MediaElement();
            SpeechSynthesisStream stream = await mowa.SynthesizeTextToStreamAsync(tekst);
            odtwarzacz.SetSource(stream, stream.ContentType);
            odtwarzacz.Play();
            var messageDialog = new MessageDialog("Czy chcesz wyjść z programu?", "Wyjście");
            messageDialog.Commands.Add(new UICommand("Tak", new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand("Nie"));

            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command) {
            System.Environment.Exit(1);
        }
    }
        
    }


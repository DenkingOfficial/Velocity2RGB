using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Velocity2RGB
{
    public partial class Converter : Window
    {
        Colors colors = new Colors();
        Rectangle[] padsPT1 = new Rectangle[64];
        Rectangle[] padsPT2 = new Rectangle[64];

        public Converter()
        {
            InitializeComponent();
            Velocity.SelectedIndex = 0;
            colors.R = StringToArray(Velocity2RGB.Properties.Resources.defaultRed);
            colors.G = StringToArray(Velocity2RGB.Properties.Resources.defaultGreen);
            colors.B = StringToArray(Velocity2RGB.Properties.Resources.defaultBlue);
            for (int i = 0; i < 128; i++)
                Velocity.Items.Add(i);
            BuildPreview();
        }

        public void BuildPreview()
        {
            int colornum = 0;
            int pt2assist = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    padsPT1[colornum] = new Rectangle();
                    padsPT1[colornum].Name = $"Pad_{colornum}";
                    padsPT1[colornum].Width = 14;
                    padsPT1[colornum].Height = 14;
                    padsPT1[colornum].Fill = new SolidColorBrush(Color.FromRgb((byte)((colors.R[colornum] + 1) * 4 - 1), (byte)((colors.G[colornum] + 1) * 4 - 1), (byte)((colors.B[colornum] + 1) * 4 - 1)));
                    padsPT1[colornum].Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    padsPT1[colornum].Visibility = Visibility.Visible;
                    padsPT1[colornum].HorizontalAlignment = HorizontalAlignment.Left;
                    padsPT1[colornum].VerticalAlignment = VerticalAlignment.Top;
                    padsPT1[colornum].Margin = new Thickness(0 + 16 * j, 0 + 16 * i, 0, 0);
                    padLayout.Children.Add(padsPT1[colornum]);
                    ++colornum;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    padsPT2[pt2assist] = new Rectangle();
                    padsPT2[pt2assist].Name = $"Pad_{colornum}";
                    padsPT2[pt2assist].Width = 14;
                    padsPT2[pt2assist].Height = 14;
                    padsPT2[pt2assist].Fill = new SolidColorBrush(Color.FromRgb((byte)((colors.R[colornum] + 1) * 4 - 1), (byte)((colors.G[colornum] + 1) * 4 - 1), (byte)((colors.B[colornum] + 1) * 4 - 1)));
                    padsPT2[pt2assist].Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    padsPT2[pt2assist].Visibility = Visibility.Visible;
                    padsPT2[pt2assist].HorizontalAlignment = HorizontalAlignment.Left;
                    padsPT2[pt2assist].VerticalAlignment = VerticalAlignment.Top;
                    padsPT2[pt2assist].Margin = new Thickness(0 + 16 * j, 0 + 16 * i, 0, 0);
                    padLayout2.Children.Add(padsPT2[pt2assist]);
                    ++colornum;
                    ++pt2assist;
                }
            }
        }
        public void UpdatePreview()
        {
            for (int i = 0; i < 64; i++)
            {
                padsPT1[i].Fill = new SolidColorBrush(Color.FromRgb((byte)((colors.R[i] + 1) * 4 - 1), (byte)((colors.G[i] + 1) * 4 - 1), (byte)((colors.B[i] + 1) * 4 - 1)));
                padsPT2[i].Fill = new SolidColorBrush(Color.FromRgb((byte)((colors.R[i+64] + 1) * 4 - 1), (byte)((colors.G[i+64] + 1) * 4 - 1), (byte)((colors.B[i+64] + 1) * 4 - 1)));
            }
        }
        static public int[] StringToArray(string str)
        {
            int[] colorValues = new int[128];
            string[] charsToRemove = new string[] { "{", ",", "}" };
            foreach (string chars in charsToRemove)
                str = str.Replace(chars, string.Empty);
            string[] strValues = str.Split(' ');
            for (int i = 0; i < 128; i++)
                colorValues[i] = int.Parse(strValues[i]);
            return colorValues;
        }

        private void Velocity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = Velocity.SelectedIndex;
            ColorPreview.Fill = new SolidColorBrush(Color.FromRgb((byte)((colors.R[index]+1)*4-1), (byte)((colors.G[index]+1)*4-1), (byte)((colors.B[index]+1)*4-1)));
            Rval.Text = colors.R[index].ToString();
            Gval.Text = colors.G[index].ToString();
            Bval.Text = colors.B[index].ToString();
            HexVal.Text = "#" + colors.R[index].ToString("X2") + colors.G[index].ToString("X2") + colors.B[index].ToString("X2");
        }

        private void OpenPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "TXT Retina Palette (*.txt)|*.txt";
            var result = dlg.ShowDialog();
            if (result.Value)
            {
                string[] lines = File.ReadAllLines(dlg.FileName);
                for (int i = 0; i < 128; i++)
                {
                    lines[i] = lines[i].Replace(",", string.Empty);
                    lines[i] = lines[i].Replace(";", string.Empty);
                    string[] tempValues = lines[i].Split(' ');
                    colors.R[i] = int.Parse(tempValues[1]);
                    colors.G[i] = int.Parse(tempValues[2]);
                    colors.B[i] = int.Parse(tempValues[3]);
                }
            }
            Velocity.SelectedIndex = 0;
            UpdatePreview();
        }

        private void CopyHex_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(HexVal.Text.Replace("#", string.Empty));
        }
    }
}

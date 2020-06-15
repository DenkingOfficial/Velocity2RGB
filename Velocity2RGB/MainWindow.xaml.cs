using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace Velocity2RGB
{
    public partial class Converter : Window
    {
        Colors colors = new Colors();
        public Converter()
        {
            InitializeComponent();
            Velocity.SelectedIndex = 0;
            colors.R = StringToArray(Velocity2RGB.Properties.Resources.defaultRed);
            colors.G = StringToArray(Velocity2RGB.Properties.Resources.defaultGreen);
            colors.B = StringToArray(Velocity2RGB.Properties.Resources.defaultBlue);
            for (int i = 0; i < 128; i++)
                Velocity.Items.Add(i);
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
                string[] charsToRemove = new string[] { ";", "," };
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
        }

        private void CopyHex_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(HexVal.Text);
        }
    }
}

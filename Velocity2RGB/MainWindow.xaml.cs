using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
    }
}

using System.Drawing.Imaging;
using System.Drawing.Text;

namespace runtime_images_list
{
    public partial class HostForm : Form
    {
        const string IMGBASE = @"D:\Github\xamarin-21\sasquatch-net-standard-21\host-form\ImageGen\";
        public HostForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); 
            BackColor = Color.Teal;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts", "database.ttf")!;
            privateFontCollection.AddFontFile(path);

            var fontFamily = privateFontCollection.Families[0];
            var font = new Font(fontFamily, 13.5F);
            var backColor = Color.FromArgb(128, Color.Teal);
            tableLayoutPanel.BackColor = backColor;

            // Stage the glyphs in the TableLayoutPanel.
            setLabelAttributes(label: label0, font: font, text: "\uE800", foreColor: Color.LightGray, backColor: backColor);
            setLabelAttributes(label: label1, font: font, text: "\uE800", foreColor: Color.LightSalmon, backColor: backColor);
            setLabelAttributes(label: label2, font: font, text: "\uE800", foreColor: Color.LightGreen, backColor: backColor);
            setLabelAttributes(label: label3, font: font, text: "\uE800", foreColor: Color.Blue, backColor: backColor);
            setLabelAttributes(label: label4, font: font, text: "\uE800", foreColor: Color.Gold, backColor: backColor);
            setLabelAttributes(label: label5, font: font, text: "\uE801", foreColor: Color.LightGray, backColor: backColor);
            setLabelAttributes(label: label6, font: font, text: "\uE801", foreColor: Color.LightSalmon, backColor: backColor); 
            setLabelAttributes(label: label7, font: font, text: "\uE801", foreColor: Color.LightGreen, backColor: backColor);
            setLabelAttributes(label: label8, font: font, text: "\uE801", foreColor: Color.Blue, backColor: backColor);
            setLabelAttributes(label: label9, font: font, text: "\uE801", foreColor: Color.Gold, backColor: backColor);
            setLabelAttributes(label: label10, font: font, text: "\uE803", foreColor: Color.LightGray, backColor: backColor);
            setLabelAttributes(label: label11, font: font, text: "\uE803", foreColor: Color.LightSalmon, backColor: backColor);
            setLabelAttributes(label: label12, font: font, text: "\uE803", foreColor: Color.LightGreen, backColor: backColor);
            setLabelAttributes(label: label13, font: font, text: "\uE803", foreColor: Color.Blue, backColor: backColor);
            setLabelAttributes(label: label14, font: font, text: "\uE802", foreColor: Color.LightGray, backColor: backColor);
            setLabelAttributes(label: label15, font: font, text: "\uE804", foreColor: Color.LightGreen, backColor: backColor);

            makeRuntimeImageList();
        }        private void setLabelAttributes(Label label, Font font, string text, Color foreColor, Color backColor)
        {
            label.UseCompatibleTextRendering = true;
            label.Font = font;
            label.Text = text;
            label.ForeColor = foreColor;
            label.BackColor = Color.FromArgb(200, backColor);
        }
        private void makeRuntimeImageList()
        {
            var imageList22 = new ImageList(this.components);
            imageList22.ImageSize = new Size(32, 32);
            imageList22.ColorDepth = ColorDepth.Depth8Bit;
            foreach (
                var label in 
                tableLayoutPanel.Controls
                .Cast<Control>()
                .Where(_=>_ is Label)
                .OrderBy(_=>int.Parse(_.Name.Replace("label", string.Empty))))
            {
                Bitmap bitmap = new Bitmap(label.Width, label.Height);
                label.DrawToBitmap(bitmap, label.ClientRectangle);
                imageList22.Images.Add(bitmap);
            }
            this.treeView.StateImageList = imageList22;
        }
        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
    }
}

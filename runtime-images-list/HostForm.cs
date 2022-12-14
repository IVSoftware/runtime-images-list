using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;

namespace runtime_images_list
{
    public partial class HostForm : Form
    {
        public HostForm()
        {
            InitializeComponent();
#if DEBUG
            _imgFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Paint")!;
            Directory.CreateDirectory(_imgFolder);
#endif
        }
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

            radioButtonUseBitmaps.CheckedChanged += (sender, e) =>
            {
                if (radioButtonUseBitmaps.Checked) BeginInvoke(() => makeRuntimeImageList());
            };
            radioButtonUseDrawstring.CheckedChanged += (sender, e) =>
            {
                if (radioButtonUseDrawstring.Checked) BeginInvoke(() => makeRuntimeImageListDirect());
            };
        }        
        private void setLabelAttributes(Label label, Font font, string text, Color foreColor, Color backColor)
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
                // Might not be a foregone conclusion. Screen scaling might affect.
                // Debug.Assert(label.Width.Equals(32), "Expecting 32 pixels");
                // Debug.Assert(label.Height.Equals(32), "Expecting 32 pixels");
                Bitmap bitmap = new Bitmap(label.Width, label.Height);
                label.DrawToBitmap(bitmap, label.ClientRectangle);
                imageList22.Images.Add(bitmap);
#if DEBUG
                bitmap.Save(Path.Combine(_imgFolder, $"{label.Name}.{ImageFormat.Bmp}"), ImageFormat.Bmp);
#endif
            }
            this.treeView.StateImageList = imageList22;
        }
        private void makeRuntimeImageListDirect()
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
                Bitmap drawDirect = new Bitmap(32, 32);
                using (Graphics graphics = Graphics.FromImage(drawDirect))
                {
                    using (var brush = new SolidBrush(treeView.BackColor))
                    {
                        graphics.FillRectangle(brush, new Rectangle(0, 0, drawDirect.Width, drawDirect.Height));
                    }
                    using (var brush = new SolidBrush(label.ForeColor))
                    {
                        graphics.DrawString(label.Text, label.Font, brush, new PointF());
                    }
                }
                imageList22.Images.Add(drawDirect);
            }
            this.treeView.StateImageList = imageList22;
        }

#if DEBUG
        readonly string _imgFolder;
#endif
        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
    }
}

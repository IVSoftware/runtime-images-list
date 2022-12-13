My objective is to use some glyphs from a TTF font for the state images in a `TreeView` and wanted to avoid `OwnerDraw` of the tree nodes if possible. As a proof-of-concept, I had an idea to stage the images in a `TableLayoutPanel` (where it would be easy to draw the glyphs) and then generate an `ImageList` at runtime that could be assigned to the `TreeView`.  

[![glyphs][1]][1]

**Fig. 1 - Looking good in the TableLayoutPanel**
***

In `#DEBUG` mode, the bitmaps are written to .bmp files for the sole purpose of inspecting using MS Paint. Here are a couple examples of the 32 x 32 bitmaps thus generated (greatly magnified of course).

[![generated bitmaps][2]][2]

**Fig. 2 - Looking good in the .bmp files.**.
***

Something bad (resizing, anti-aliasing, ?...) seems to be occurring in the final state images, however, even when I'm careful to use consistent 32 x 32 sizes for everything. I've messed with the `ColorDepth` ans `ImageSize` properties of the `ImageList`. I've wasted hours trying to understand and fix it.  It's happening in my production code. It's happening in the minimal reproducible sample I have detailed below.

[![artifacts][3]][3]

***
_Tried `TextRenderer` (using black-on-white) per dr.null's comment._

[![minimal][4]][4]

***
Here's my code or [browse]() full sample on GitHub.

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
                Bitmap bitmap = new Bitmap(label.Width, label.Height);
                label.DrawToBitmap(bitmap, label.ClientRectangle);
                imageList22.Images.Add(bitmap);
    #if DEBUG
                bitmap.Save(Path.Combine(_imgFolder, $"{label.Name}.{ImageFormat.Bmp}"), ImageFormat.Bmp);
    #endif
            }
            this.treeView.StateImageList = imageList22;
        }

    #if DEBUG
        readonly string _imgFolder;
    #endif
        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
    }


  [1]: https://i.stack.imgur.com/U46hn.png
  [2]: https://i.stack.imgur.com/R4p2B.png
  [3]: https://i.stack.imgur.com/CRMk4.png
  [4]: https://i.stack.imgur.com/TmLWr.png
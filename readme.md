On my brand new Windows 11 desktop, the bitmap I've been using to indicate a 'connected database' state in my stable production app is suddenly looking terrible on my 4K 150% scaled display (the monitor is still the same as before). The issue seems specific to `TreeView` because the same bitmap on the same `Form` looks OK when set as the image for a `Label` for example. It _also_ still looks OK on a Win 10 VM running on the new machine. And it mainly affects the green one. Weird.

[![comparison][1]][1]

***

Anyway, I can't just sit and cry about it - I really do need to come up with a new way of drawing this that looks right 100% of the time. So I'm trying a new approach using a glyph font and it looks nice and clear when I put it up on a set of labels. 

[![glyphs][2]][2]

**Looking good in the TableLayoutPanel.**
***
What I need to do now is generate an ImageList to use for the tree view, and as a proof of concept I tried using `Control.DrawToBitmap` to generate a runtime ImageList from the labels. I added a `#DEBUG` block that saves the bitmaps and I can open them up in MS Paint and they look fine (here greatly magnified of course).

[![generated bitmaps][3]][3]

**Looking good in the .bmp files.**
***
And for sure this improves things, but there are still some obvious pixel defects that look like noisy anti-aliasing or resizing artifacts, even though I'm taking care to use consistent 32 x 32 sizes for everything. I've messed with the `ColorDepth` ans `ImageSize` properties of the `ImageList`. I've wasted hours trying to understand and fix it.  It's happening in my production code. It's happening in the minimal reproducible sample I have detailed below. So, before I tear the rest of my hair out, maybe someone can spot what I'm doing wrong, or show me a better way. 

[![artifacts][4]][4]

***
Here's my code or [browse](https://github.com/IVSoftware/runtime-images-list.git) full sample on GitHub.

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


  [1]: https://i.stack.imgur.com/sqUlV.png
  [2]: https://i.stack.imgur.com/U46hn.png
  [3]: https://i.stack.imgur.com/R4p2B.png
  [4]: https://i.stack.imgur.com/tojHI.png
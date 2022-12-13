My objective is to use some glyphs from a TTF font for the state images in a `TreeView` and wanted to avoid `OwnerDraw` of the tree nodes if possible. As a proof-of-concept, I had an idea to stage the images in a `TableLayoutPanel` (where it would be easy to draw the glyphs) and then generate an `ImageList` at runtime that could be assigned to the `TreeView`. This all seems to work just fine, except that I'm getting pixel defects in the state images. SOmthing bad (resizing, anti-aliasing, ?...) seems to be occurring.

[![glyphs][1]][1]

My next step is to render these as `BitMap` using this method:

    private void drawToBitmap(Label label)
    {
        Bitmap bitmap = new Bitmap(label.Width, label.Height);
        label.DrawToBitmap(bitmap, label.ClientRectangle);
        var path = Path.Combine(IMGBASE, $"{label.Name}.png");
        bitmap.Save(path, ImageFormat.Png); // Or Bmp makes NO difference.
        // Add the bitmaps to an ImageList
        imageList22.Images.Add(new Bitmap(bitmap, imageList22.ImageSize));
    }

Here is a representative sampling of the 32 x 32 bitmaps thus generated (greatly magnified of course).

[![generated bitmaps][2]][2]

***

The problem is when I use the bitmaps in an `ImageList` for the `StateImages` for my `TreeView`. They show the kind of weird artifact you might see from compression, even when I'm careful to use consistent 32 x 32 sizes for everything. I've messed with the `ColorDepth` ans `ImageSize` properties of the `ImageList`. All in all, this is something I keep coming back to and have wasted hours trying to understand and fix.

This kind of thing happens with `TreeView` on any app I build, no matter how minimal the test. 

[![artifacts][3]][3]

Is there some kind of explanation for what I'm seeing here? It's puzzling that different controls on the same exact form seem to be rendering the images so differently. What am I missing? 

***
**EDIT**

Thanks for the suggestions to try drawing direct to the ImageList and in particular I hadn't thought about `TextRenderer` and really wanted to try dr.null's snippet. In doing so, I started out drawing black-on-white to eliminate variables and also tried various `SmoothingMode` settings, but you can still see below how ratty the image is. And why are there still pixels with color now...? And why is that noise identical from one image to the next...? I'm going to have to sleep on this the wall is flattening my forehead.

    private void drawToBitmapA(Font font, string text, Color foreColor, Color backColor)
    {
        Bitmap drawDirect = new Bitmap(32, 32);
        using (Graphics graphics = Graphics.FromImage(drawDirect))
        {
            // Tried all the settings no difference.
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            TextRenderer.DrawText(graphics, text, font, new Point(), Color.Black, Color.White);
        }
        imageList22.Images.Add(drawDirect);
    }

[![minimal][4]][4]

[![ratty B&W][5]][5]


  [1]: https://i.stack.imgur.com/U46hn.png
  [2]: https://i.stack.imgur.com/R4p2B.png
  [3]: https://i.stack.imgur.com/CRMk4.png
  [4]: https://i.stack.imgur.com/TmLWr.png
  [5]: https://i.stack.imgur.com/Hwd5o.png
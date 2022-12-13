My objective is to use some glyphs from a TTF font for the state images in a `TreeView` and wanted to avoid `OwnerDraw` of the tree nodes if possible. As a proof-of-concept, I had an idea to stage the images in a `TableLayoutPanel` (where it would be easy to draw the glyphs) and then generate an `ImageList` at runtime that could be assigned to the `TreeView`. This all seems to work just fine, except that I'm getting pixel defects in the state images. Something bad (resizing, anti-aliasing, ?...) seems to be occurring.

[![glyphs][1]][1]

Here is a representative sampling of the 32 x 32 bitmaps thus generated (greatly magnified of course).

[![generated bitmaps][2]][2]

***

The problem is when I use the bitmaps in an `ImageList` for the `StateImages` for my `TreeView`. They show the kind of weird artifact you might see from compression, even when I'm careful to use consistent 32 x 32 sizes for everything. I've messed with the `ColorDepth` ans `ImageSize` properties of the `ImageList`. All in all, this is something I keep coming back to and have wasted hours trying to understand and fix.

This kind of thing happens with `TreeView` on any app I build, no matter how minimal the test. 

[![artifacts][3]][3]

[![minimal][4]][4]


  [1]: https://i.stack.imgur.com/U46hn.png
  [2]: https://i.stack.imgur.com/R4p2B.png
  [3]: https://i.stack.imgur.com/CRMk4.png
  [4]: https://i.stack.imgur.com/TmLWr.png
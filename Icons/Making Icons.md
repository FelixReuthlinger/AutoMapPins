# Making Valheim-style icons

Currently all custom icons are taken and adapted from [The Noun Project](https://thenounproject.com/icons/). These are
generally 700x700 pixels, black on transparent.

Valheim icons seem to be 64x64 pixels, with a foreground of about #C9C9C9 and a drop shadow of about #363636.

## Paint dot net Workflow

This workflow assumes noun project PNGs where the extra text has been removed and the icon centered.

1. Duplicate layer
2. On top layer:
   * Adjustments -> Hue / Saturation -> Set Saturation to 80
3. On bottom layer:
   1. Adjustments -> Invert colors
   2. Effects -> Blurs -> Gaussian Blur -> Set Blur radius to 20
4. Save this
5. For full size icon:
   1. Resize to 64x64
   2. Save as PNG (flatten)
6. For reduced size icon:
   1. Resize to 48x48
   2. Extend canvas to 64x64
   3. Save as PNG (flatten)

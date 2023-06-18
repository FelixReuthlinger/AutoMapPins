# Making Valheim-style icons

Currently all custom icons are taken and adapted from [The Noun Project](https://thenounproject.com/icons/). These are
generally 700x700 pixels, black on transparent.

Valheim icons seem to be 64x64 pixels, with a foreground of about #C9C9C9 and a drop shadow of about #363636.

## Paint dot net Workflow

This workflow assumes noun project PNGs where the extra text has been removed and the icon centered.

1. Duplicate layer
2. On top layer:
    1. Set Saturation to 79
3. On bottom layer:
    1. Gaussian Blur 20
    2. Global select background with 0% tolerance
    3. Invert selection
    4. Fill selection with foreground color (backspace)
    5. Deselect
    6. Gaussian Blur 20
    7. Set Saturation to 21
4. Save this
5. For full size icon:
    1. Resize to 64x64
    2. Save as PNG (flatten)
6. For reduced size icon:
    7. Resize to 48x48
    8. Extend canvas to 64x64
    9. Save as PNG (flatten)
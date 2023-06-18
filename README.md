# Kempeth's Auto Map Pin Mod for Valheim
This mod serves as a kind of "radar" for your Valheim map, automatically creating temporary pins for various ingame objects.

As of Version 1.2.0.0 categories have mostly been replaced with individual toggles for all supported main world objects:

* Mineables like Copper, Tin, Iron, Silver, Obsidian, Meteorites, Leviathans and Ancient Remains
* Dungeons like Burial Chambers, Troll Caves, Sunken Crypts, Mountain Caves and Infested Mines
* Seeds like Carrot Seeds, Turnip Seeds, Barley and Flax
* Harvestables like Berries, Mushrooms, Remains and many more
* Flowers like Dandelions and Thistles

All type of markers except Dungeons will group multiple nearby objects of the same time into a single pin to reduce clutter on the map. The grouping distances are still subject to refinement.

Copper, Iron and Silver will show whether they are fully intact or partially mined.

All objects have custom icons and can be enabled and disabled via the **BepInEx ModConfiguration** plugin. In addition you can also toggle the display of all object types that I've currently identified in the game. But be warned that this will really clutter your map.

You can also now choose to have smaller pin marker and text to further reduce map clutter. Icons are 25% smaller and text about 50% smaller.

As of Version 1.3.0.0 dungeon pins are now persistent! This means their toggles in the settings work slightly different. Instead of hiding/showing the corresponding pins, the setting now determines whether or not newly discovered locations will be added to the map or not.

##  Planned Features
* Pins for Points of Interests like ruins, stone circles and camps

### Tentatively Planned Features
* Customizable limits below which pin is suppressed. Ie. only show Dandelions if there are at least 5.
* Customizable radar range. While I can't increase the mod's range I could reduce it to keep the need for exploration higher than it currently is.
* If possible some indicator for ore node mined percentage or dungeon cleared percentage

## Source Code
The source code of this mod can be found at: https://github.com/Kempeth/AutoMapPins

##  License and Credits
This mod is forked from bdew's AutoMapPins, who provided a great starting point.

This mod is free software: you can redistribute it and/or modify it under the terms of the [GNU Lesser General Public License](http://www.gnu.org/licenses/lgpl-3.0.en.html) as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Asset Licenses:
* Axe by Danil Polshin from [Noun Project](https://thenounproject.com/browse/icons/term/axe/) (CCBY) - modified
* cave by Kieu Thi Kim Cuong from [Noun Project](https://thenounproject.com/browse/icons/term/cave/) (CCBY) - modified
* Flower by Vectors Market from [Noun Project](https://thenounproject.com/browse/icons/term/flower/) (CCBY) - modified
* Mine by Edward Boatman from [Noun Project](https://thenounproject.com/browse/icons/term/mine/) (CCBY) - modified
* Mushroom by Anton Gajdosik from [Noun Project](https://thenounproject.com/browse/icons/term/mushroom/) (CCBY) - modified
* pick by Pham Duy Phuong Hung from [Noun Project](https://thenounproject.com/browse/icons/term/pick/) (CCBY) - modified
* raspberry by Laymik from [Noun Project](https://thenounproject.com/browse/icons/term/raspberry/) (CCBY) - modified
* seeds by Orin zuu from [Noun Project](https://thenounproject.com/browse/icons/term/seeds/) (CCBY) - modified
# AutoMapPin

This mod serves as a kind of "radar" for your Valheim map, automatically creating temporary and persistent pins for
various in-game objects.

## Features

* [Server Synced](https://github.com/blaxxun-boop/ServerSync)
* Will create pins on Map and Minimap
    * **WARNING**: do better not configure too many to be active, it will flood your map!
* Pins will only show in areas where the player did already discover the map
* Pins created are using custom icons, see [Available icons](#available-icons)
* Pins can be configured, see [Pin configuration](#pin-configuration)
* Pins can be temporary (just visible when in area) or permanent (once discovered added permanently to map)
  * Permanent pins needs to be removed manually, so think twice about using this too widely
* Pins can be configured with color as you like
* Pin are loaded/created for game objects of Valheim types:
    * Destructible
    * Leviathan
    * Location
    * MineRock (ore)
    * MineRock5 (ore)
    * Pickable
    * TeleportWorld (portals)
    * PickableItem
    * Container (chests)
* Unknown (missing config) pins will be added to a temporary list that can be printed to a file to simplify
  configuration.
* Config file watchers (updated config file will be releaded)
* Pins are part of an object in the game. If the object is destroyed / removed, the pin is also removed.

# Configuration

* The mod generates a short BepInEx config file `FixItFelix.AutoMapPins.cfg`
* Most configuration is provided via YAML files, see [data model](#config-data-model)

## Pin configuration

* A default configuration for vanilla objects is shipped with the mod. The setting of active categories and pins is from
  my own experience of how I prefer to play (not flood my map). But you can edit and change those as you prefer.
    * Remark: I actually do not play vanilla Valheim much, if there is something missing or someone wants to provide
      better configs, feel free to reach out. I am happy to also share more or different configs.
* If you play with objects beyond the vanilla ones, you might need to create your own configuration file, the following
  sections will explain how to.

### Debugging effective configuration

There is a console command 'print_effective_config' to print the effective config of (only active) configs. This can be
used to help debugging what actually was loaded.

### Config data model

* all category and pin configuration is provided using YAML files

#### YAML schema

Some statements to explain the intent of the schema:

1. The top level is the name of a category that you choose yourself to group configs and share certain settings in
   between the pins configured inside this category.
2. Each Category can (optional) have either or both of "individualConfiguredObjects" and "categoryConfiguredObjects",
   and each category does have the ability to also configure values for the whole category like you can configure for
   any individual config object entry.
3. individualConfiguredObjects: does contain a dictionary, the key is the game object "name" field content the value is
   the set of fields for configuration, see below.
4. categoryConfiguredObjects: does only contain a list of game object "name" field contents that are NOT configured
   individually, but all are only configured by any value set at category level.
5. Many configuration options are inherited from category down to categoryConfiguredObjects (they inherit all settings)
   and individualConfiguredObjects (they can inherit "name", "iconName" and "iconColorRGBA").
6. Any value that is not set will either be set with some default value or be inherited if available.

Config options per object:

1. "name" -> the name shown on map, can be omitted to not show a name
2. "iconName" -> the name of the icon to use, see available icons in the respective section of this readme
3. "iconColorRGBA" -> contains the fields "red", "green", "blue", and "alpha"; the values need to be set as int type (
   per channel, from 0 to 255, where 255 is full color); alpha sets the intensity of the color (also 0 to 255)
4. "isPermanent" -> pins with this flag set to true (default false) will not be removed when the player walks away from
   the area, and they will be persisted in the player save file
5. "isActive" -> activates the mod to use the config for this pin if set to true (default false)
6. "groupable" -> activated if set to true (default false) will enable pin grouping that will create one pin (counting
   the number of occurrences of the same object around it in the name)
7. "groupingDistance" -> the distance to be used to build groups of pins (default 30.0)

##### Yaml example

```yaml
seeds:
  individualConfiguredObjects: # using this option will let you define individual pins, like if you want to name each of them differently, or activate grouping just for some of them
    Pickable_SeedCarrot:
      name: Carrot
      isPermanent: false
      isActive: true
      groupable: true
  isActive: true
  iconName: seed
crypt:
  categoryConfiguredObjects: # using this option will apply same settings for all those pins
    - Crypt2
    - Crypt3
    - Crypt4
    - SunkenCrypt4
  name: Crypt
  isActive: true
  isPermanent: true
  iconName: dungeon
  iconColorRGBA:
    red: 120
    green: 255
    blue: 255
    alpha: 200
```

##### Yaml internal name parsing

Rules:

* removes `(Clone)`
* removes `(123)`

Example: in-game loaded `gameObject.name` entry like `Mistlands_DvergrTownEntrance1 (2)`
or `Mistlands_DvergrTownEntrance1 (Clone)`
will be parsed to: `Mistlands_DvergrTownEntrance1`

### Create config file

* You can create multiple additional config files, the mod will load every file matching the pattern inside any sub
  folder of the `.../BepInEx/config/` path.
* Pattern for new files: `FixItFelix.AutoMapPins.categories.*.yaml` (replace "*" with your choice of name)
* There is a mechanic provided by the mod that will record any kind of pin that has a missing config while running
  through the world.
    * You can output all found config files using the console command `amp write_missing_configs_file`
    * All configs created by that will have defaults set `n_a` and booleans set to `false`, change those according to
      the data model (see [data model](#config-data-model))

# Available Icons

These custom icons were made available with CCBY license, see attributions section.

| Icon name    | Description         | Used for                                    |
|--------------|---------------------|:--------------------------------------------|
| `axe`        | Axe 64 pixels       |                                             |
| `axe48`      | Axe 48 pixels       |                                             |
| `berry`      | Berry 64 pixels     | Pickable bushes like Raspberry              |
| `berry48`    | Berry 48 pixels     |                                             |
| `dungeon`    | Dungeon 64 pixels   | Dungeons, Caves, Crypts, ...                |
| `dungeon48`  | Dungeon 48 pixels   |                                             |
| `flower`     | Flower 64 pixels    |                                             |
| `flower48`   | Flower 48 pixels    |                                             |
| `hand`       | Hand 64 pixels      | Pickables like Flint, ...                   |
| `hand48`     | Hand 48 pixels      |                                             |
| `mine`       | Mine 64 pixels      | Minable ores, nodes, veins, ...             |
| `mine48`     | Mine 48 pixels      |                                             |
| `mushroom`   | Mushroom 64 pixels  | Mushroom, Yellow mushroom                   |
| `mushroom48` | Mushroom 48 pixels  |                                             |
| `seed`       | Seed 64 pixels      | Pickable seeds like carrot, turnip, ...     |
| `seed48`     | Seed 48 pixels      |                                             |
| `spawner`    | Pentagram 64 pixels | Spawners like Greydwarf nests or bone piles |
| `spawner48`  | Pentagram 48 pixels |                                             |
| `rune`       | Rune 64 pixels      | Runestones                                  |
| `rune48`     | Rune 48 pixels      |                                             |
| `dot`        | Circle 64 pixels    | Anything else                               |
| `dot48`      | Circle 48 pixels    |                                             |
| `herb`       | Herb 64 pixels      | Herbs like Thistle                          |
| `herb48`     | Herb 48 pixels      |                                             |
| `island`     | Island 64 pixels    | Leviathan                                   |
| `island48`   | Island 48 pixels    |                                             |
| `monument`   | Monument 64 pixels  |                                             |
| `monument48` | Monument 48 pixels  |                                             |
| `temple`     | Temple 64 pixels    |                                             |
| `temple48`   | Temple 48 pixels    |                                             |
| `treasure`   | Treasure 64 pixels  | Treasure chests                             |
| `treasure48` | Treasure 48 pixels  |                                             |
| `bones`      | Bones 64 pixels     |                                             |
| `bones48`    | Bones 48 pixels     |                                             |
| `portal`     | Portal 64 pixels    | Portal                                      |
| `portal48`   | Portal 48 pixels    |                                             |
| `hay`        | Hay 64 pixels       |                                             |
| `hay48`      | Hay 48 pixels       |                                             |
| `village`    | Village 64 pixels   |                                             |
| `village48`  | Village 48 pixels   |                                             |

# Miscellaneous

<details>
  <summary>License, credits, attributions</summary>

* [LGPLv3](https://www.gnu.org/licenses/lgpl-3.0.de.html) license based mod
* This mod is inspired from concepts of [Kempeth's AutoMapPins](https://github.com/Kempeth/AutoMapPins) provided via
  LGPLv3, but I did re-write most of the code from scratch to make the mod configurable, simplified a lot of the
  concepts.
* [Mod icon](https://www.flaticon.com/free-icons/radar)
* [Axe by Danil Polshin](https://thenounproject.com/browse/icons/term/axe/) (CCBY) - modified
* [cave dungeon by Amanda Hua](https://thenounproject.com/browse/icons/term/cave-dungeon/) (CCBY) - modified
* [Flower by Vectors Market](https://thenounproject.com/browse/icons/term/flower/) (CCBY) - modified
* [Mine by Edward Boatman](https://thenounproject.com/browse/icons/term/mine/) (CCBY) - modified
* [Mushroom by Anton Gajdosik](https://thenounproject.com/browse/icons/term/mushroom/) (CCBY) - modified
* [pick by Pham Duy Phuong Hung](https://thenounproject.com/browse/icons/term/pick/) (CCBY) - modified
* [raspberry by Laymik](https://thenounproject.com/browse/icons/term/raspberry/) (CCBY) - modified
* [seeds by Orin zuu](https://thenounproject.com/browse/icons/term/seeds/) (CCBY) - modified
* [rune by NoNsEnSe ThInGs](https://thenounproject.com/browse/icons/term/rune/) (CCBY) - modified
* [Monster by BomSymbols](https://thenounproject.com/browse/icons/term/monster/) (CCBY) - modified
* [dot by Saepul Nahwan](https://thenounproject.com/browse/icons/term/dot/) (CCBY) - modified
* [Tree by Icon Solid](https://thenounproject.com/browse/icons/term/tree/) (CCBY) - modified
* [herb by scarlett mckay](https://thenounproject.com/browse/icons/term/herb/) (CCBY) - modified
* [island by David Mühlenweg](https://thenounproject.com/browse/icons/term/island/) (CCBY) - modified
* [Fire by koto](https://thenounproject.com/browse/icons/term/fire/) (CCBY) - modified
* [Monument by Doodle Icons](https://thenounproject.com/browse/icons/term/monument/) (CCBY) - modified
* [temple by Deemak Daksina](https://thenounproject.com/browse/icons/term/temple/) (CCBY) - modified
* [treasure by Vectors Market](https://thenounproject.com/browse/icons/term/treasure/) (CCBY) - modified
* [Whale by Gregor Cresnar](https://thenounproject.com/browse/icons/term/whale/) (CCBY) - modified
* [bones by Sergey Demushkin](https://thenounproject.com/browse/icons/term/bones/) (CCBY) - modified
* [warp by Dank By Design](https://thenounproject.com/browse/icons/term/warp/) (CCBY) - modified
* [hay by Eucalyp](https://thenounproject.com/browse/icons/term/hay/) (CCBY) - modified
* [Village by Adrien Coquet](https://thenounproject.com/browse/icons/term/village/) (CCBY) - modified

</details>

<details>
  <summary>Contact</summary>

* https://github.com/FelixReuthlinger/AutoMapPins
* Discord: `fluuxxx` (you can find me around some of the Valheim modding discords, too)

</details>


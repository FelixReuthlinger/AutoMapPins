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
* If you play with objects beyond the vanilla ones, you might need to create your own configuration file, the following
  sections will explain how to.

### Config data model

* all category and pin configuration is provided using YAML files

#### YAML schema

| level | field name       | replace field name | type   | description                                                                                                                                                                       |
|-------|------------------|--------------------|--------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| root  | "category name"  | yes                | string | the top most element is intended for creating "categories" of pins                                                                                                                |
| 1     | categoryActive   | no                 | bool   | turns the category including all pins inside on (true) or off (false)                                                                                                             |
| 1     | pins             | no                 | list   | contains the list of pins of this category                                                                                                                                        |
| 2     | "pin name"       | yes                | string | internal name of the game object (`gameObject.name`), parsed, see [parsing](#yaml-internal-name-parsing)                                                                          |
| 3     | categoryName     | no                 | string | name of the category this pin belongs to (copy-paste from root level)                                                                                                             |
| 3     | internalName     | no                 | string | internal name of the game object (copy-paste from level 2)                                                                                                                        |
| 3     | name             | no                 | string | name to show with the pin on the map, choose something short                                                                                                                      |
| 3     | iconName         | no                 | string | name of the icon to use for displaying the pin on the map, see [icons](#available-icons)                                                                                          |
| 3     | isPermanent      | no                 | bool   | if permanent (true), the pin will be displayed even if the player leaves the area; if not permanent (false), pin is removed when leaving the area of the pin                      |
| 3     | isActive         | no                 | bool   | if active (true) this pin config is used, otherwise it will be ignored and pin is not displayed                                                                                   |
| 3     | groupable        | no                 | bool   | if the pin pins something that usually comes in groups, like raspberry bushes, you can set it to true, then all objects within the distance (next field) are grouped into one pin |
| 3     | groupingDistance | no                 | int    | distance in meter that other objects of the same config shall be taken into a group (default 15)                                                                                  |

##### Yaml example

```yaml
ores: # category name for all the ore nodes
  categoryActive: true # category is active, pins will be loaded and displayed
  pins:
    rockcopper: # internal parsed name 
      categoryName: ores # needs to be same as on top
      internalName: rockcopper # needs to be same as internal parsed name
      name: Copper # show this pin with text "Copper" on map
      iconName: mine # use "mine" icon
      isPermanent: true # this will be displayed even if player leaves the area
      isActive: true # this pin will be shown
    minerocktin:
      categoryName: ores
      internalName: minerocktin
      name: Tin
      iconName: mine
      isPermanent: false # if tin would be shown, only when player in area
      isActive: false # tin will not be shown
      groupable: true # there are many tin piles sometimes, grouping active
      groupingDistance: 15 # default to group for 15 meters
```

##### Yaml internal name parsing

Rules:

* removes `(Clone)`
* removes `_`, `(`, `)`, `-`
* removed any numbers
* turns everything to lower case characters

Example: in-game loaded `gameObject.name` entry like `Mistlands_DvergrTownEntrance1`
or `Mistlands_DvergrTownEntrance1(Clone)`
will be parsed to: `mistlandsdvergrtownentrance`

### Create config file

* You can create multiple additional config files, the mod will load every file matching the pattern inside any sub
  folder of the `.../BepInEx/config/` path.
* Pattern for new files: `FixItFelix.AutoMapPins.categories.*.yaml` (replace "*" with your choice of name)
* There is a mechanic provided by the mod that will record any kind of pin that has a missing config while running
  through the world.
    * You can output all found config files using the console command `amp print_pins_missing_configs`
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


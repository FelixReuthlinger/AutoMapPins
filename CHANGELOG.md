* 2.0.0 ->
    * Complete rework of the most logic and config parts
    * BEWARE: to use this new version, you will need to create new config files according to the changes, see mod
      description
    * Added maximum height to auto map objects to .cfg (but you do not want to change that, believe me)
    * Changed the default grouping distance to 30
    * **NEW** features:
        * **COLORFUL PINS**!!!1111... some people requested it, I don't know if those provide really much value, but now
          you can use colors in configs
        * **Update config -> updates pins in game** - requested many times via different reports
        * game object discovery mode improved (needs to be activated in .cfg), will print also player messages for new
          objects, print only once per new object
* 1.3.1 ->
    * Updated for Ashlands Valheim version 0.218.15
    * Added new pin configs for Ashlands objects
* 1.3.0 ->
    * compiled for latest Valheim version 0.217.38
    * fixed some server sync issue
    * often requested feature added : check if the map is already explored at the area of an object before adding
      the pin => only on explored map it will show pins
* 1.2.3 -> updated server sync and compiled against 0.217.30
* 1.2.2 -> updated server sync and compiled against 0.217.24
* 1.2.1 -> fixed double initiating commands
* 1.2.0 ->
    * added new config options to not spam your logs (check new default values and change if you need):
        * silent discovery mode -> not spams your logs if prefabs were found that are not configured, yet
        * enable discovery mode -> enables that not yet configured prefabs will be collected during game play
    * improved missing config check to only mention missing configs for not matching category name and internal name in
      combination, this will also drastically reduce log messages
    * will only print missing configs if there were configs recorded during game play, will show warning if none
      recorded
    * small change on config synced pin configs
* 1.1.8 ->
    * added new symbols:
        * Village
* 1.1.6 & 7 ->
    * compiled for Valheim version 0.217.14
    * fix console boot error
* 1.1.5 ->
    * fixed groups not being permanent
    * adjusted pickable pinning to only add pin if not "picked"
    * in case an object has a Destructible and Pickable components, we skip patching the Destructible one to only apply
      a patch on the pickable that will handle state properly
    * changed pickable update from patching "Drop" to "SetPicked"
* 1.1.4 ->
    * added missing oakstub
    * set portal as active and permanent per default
* 1.1.3 ->
    * added console command to remove all pins, use that if you created to many duplicate pins before, but it will
      really remove all pins, no matter what
    * added the birchstub too config I missed somehow before
* 1.1.2 -> fixed map pin duplication issue
* 1.1.1 -> added hay icon
* 1.1.0 ->
    * added patch for new types:
        * TeleportWorld (portal)
        * PickableItem
        * Container (chests)
    * added new icon "portal"
    * updated vanilla configs with new pins
* 1.0.4 -> replaced tree, dungeon and dot icon with less bold ones
* 1.0.3 -> fixed missing tree icons
* 1.0.1 & .2 ->
    * added icons:
        * monument
        * treasure
        * whale
        * temple
        * bones
    * added missing vanilla configs
* 1.0.0 -> first release on ThunderStore

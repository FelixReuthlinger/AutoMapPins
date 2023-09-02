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

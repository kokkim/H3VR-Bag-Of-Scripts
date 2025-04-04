1.10.1:
- Fixed potential edgecase bug with EjectHandgunMagOnEmpty
- Reversed readme entry order

1.10.0:
- Added EjectHandgunMagOnEmpty; works the same as a regular closed bolt automatic mag eject, but for handguns

1.9.1:
- Improved usage of TriggerActivatedLaser

1.9.0:
- Added TriggerActivatedLaser; Toggles on a laser when the trigger is pulled
- Added SecondaryAttachmentDetachPoint; Allows the user to detach an attachment from multiple different points, rather than just the interface trigger

1.8.0:
- Moved Record Player scripts into Bag of Scripts, this allows users to create their own records

1.7.0:
- Added RPMSecondarySwitch; switches the bolt speed of an open or closed bolt on a simple interaction

1.6.0:
- Added ToggleLerp; linearly translates/rotates an object between two set values on a simple interaction
- Fixed NambuSear not firing the gun more than once when not held

1.5.0:
- Skipped 1.4.0 because of an incorrectly set PluginInfo version
- Added CustomCenterOfMass; sets a custom center of mass for a weapon that optionally does not change when attachments are added

1.3.4:
- Fixed a bug with fade outs in ContinuousFiringSound

1.3.3:
- Added support for open bolt receivers in ContinuousFiringSound

1.3.2:
- Fixed a bug with underbarrel weapons in ContinuousFiringSound

1.3.1:
- Removed debug messages from ContinuousFiringSound

1.3.0:
- Added PermanentlyAttachedWeapon; prevents a specified attachable weapon from being detached
- Added ContinuousFiringSound; allows for a continuously firing sound when firing closed bolt weapons (support for open bolts and pistols coming later)

1.2.3:
- Included an editor version of the .dll for Meatkit

1.2.2:
- ACTUALLY removed debug messages from NambuSear

1.2.1:
- Removed debug messages from NambuSear
- Fixed DropSlideOnMagRelease applying to other guns

1.2.0:
- Added NambuSear; allows you to fire the gun by clicking on a simple interaction zone, or by hitting a physics collider
- Added DropSlideOnMagRelease; causes the slide (or bolt) to release from its locked state when releasing a magazine

1.1.0:
- Added PlaySoundOnLoadState; plays a sound either when the gun runs completely dry on ammo, or when the specified magazine is fully loaded
	* Only fill in the magazine or the firearm, not both!
1.0.0:
- Main release
- Added PistolBrace, PistolBraceToggle scripts; used to make toggleable pistol braces for one-handed shooting
- Added ToggleAnimation; simple interaction plays an object's animation back or forth, used as-is in other mods or with PistolBrace
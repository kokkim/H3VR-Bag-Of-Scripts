1.15:0:
- Added ImprovedAutoRackOnMagLoad; a reworked version of AutoRackOnMagload from H3VRUtilities that optionally doesn't eject the chambered round when a mag is inserted.
- Added CycleMuzzles; Cycles through an array of muzzles that it fires in sequence.
- Added BreakOpenTriggerGrabLatch; an addon to Cityrobo's Break Open Trigger, it adds a grabbable latch that opens the breech when pulled, like the USPSAECIAL.

1.14.0:
- Added RetractBipodOnCollapse; When a set bipod is collapsed, its legs will retract (or extend if desired).

1.13.1:
- Fixed editor .dll interfering with functionality

1.13.0:
- Added SimpleGrabSpawner; Lets the user grab a set FVRObject out of it a set number of times (or infinitely).

1.12.0:
- Added CopyLaserPropertiesToMaterial; this script copies the color and on/off state of a laser and applies it to a material.

1.11.1:
- Fixed typo in OpenBoltProgressiveTrigger

1.11.0:
- Added OpenBoltProgressiveTrigger; adds an AUG-style progressive full auto trigger to open bolt weapons.
- Added MagazineLoadedPhys; Allows GameObjects on a magazine to be enabled/disabled depending on if is loaded to a gun.

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
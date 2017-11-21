FREE TURRET SCRIPT
by parameciumkid

Use:
- Build a turret using game objects; examples can be found in the demo scenes. Make sure that discrete game objects exist for each of the moving parts in the turret, e.g. the base, swivel mount, and barrel.
- Assign the relevant game objects in the Inspector. A typical turret configuration uses the barrel as the pitch segment and the swivel mount as the yaw segment, but other configurations are possible.
- Set the pitch and yaw limits as desired. Values greater than 360, or less than or equal to zero, will result in free rotation.
- Set the rotation speed limits as desired.
- Add weapons as desired. The main script does not include any weapon firing functionality, but rather is designed to cooperate with other weapon scripts. In the example scenes, the weapons are empty game objects placed at the ends of the barrels.
- The turret can be aimed by setting the "target" Vector3 variable, either manually in the inspector or (more usefully) via the public "Target(Vector3)" function in the script.

Notes:
- Neither the pitch nor yaw segments need be assigned; for a turret that only swivels horizontally, for example, the pitch segment can be left out.
- The pitch and yaw segments must not be the same object, as this leads to errors and more importantly duplicates the functionality of the existing built-in "LookAt" Unity function.
- For multi-barrel turrets, use multiple turret scripts: one for the base, with only the yaw segment set; and one for each barrel, with only the pitch segment set.
- This package does not extrapolate target motion or "lead targets;" this functionality needs to be added by a custom turret AI script (not included) that assigns target positions based on the expected positions of target objects.

For further information and to submit bug reports, contact the developer via my publisher page on the Unity Asset Store.
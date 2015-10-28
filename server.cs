if(!isObject(PreferenceContainerGroup)) {
	new ScriptGroup(PreferenceContainerGroup);
}

$Pref::BLPrefs::ServerDebug = true;
$BLPrefs::Version = "0.0-dev";

exec("./compatibility.cs");
exec("./handshake.cs");
exec("./interaction.cs");

function registerBlocklandPref(%addon, %title, %type, %variable, %default, %params, %callback, %icon)
{
	// using famfamfam's silk icons. use an icon filename minus the extension for %icon
	// RTB prefs will use the old RTB icon by default

	// the server will not need them, only the soon-to-be client(s) will
	// if there's a way to send icons to clients without clients having it, by all means, please add that

	%valid = ":integer:string:slider:boolean:list";
	// possible future entries?: color (hex, rgb, set (via params)), time
	if(stripos(%valid, ":" @ %type) == -1)
	{
		warn("Invalid pref type:" SPC %type);
		return;
	}

	%pref = new scriptObject(BlocklandPrefSO)
	{
		class = "Preference";
		category = %addon;
		title = %title;
		defaultValue = %default;
		variable = %variable;
		type = %type;
		callback = %callback;
		params = %params;
		icon = %icon;
	};

	// use this for server-sided validation?
	switch$(%type)
	{
		case "integer":
			%pref.minValue = getField(%params, 0);
			%pref.maxValue = getField(%params, 1);

		case "string":
			%pref.maxLength = getField(%params, 0);
			%pref.stripML = getField(%params, 1);

		case "slider":
			%pref.minValue = getField(%params, 0);
			%pref.maxValue = getField(%params, 1);
			%pref.snapTo = getField(%params, 2);
			%pref.stepValue = getField(%params, 3);

		case "boolean":
			if(%pref.defaultValue > 1)
				%pref.defaultValue = 1;

			if(%pref.defaultValue < 0)
				%pref.defaultValue = 0;

		case "list":
			// "Host**4|Super Admin**3|Admin**2|All**1"

			// string nonsense to separate the list
			// "**" denotes the separation of the visible part and the value of the variable
			// "|" denotes a new choice
			echo("TODO: list prefs");
	}
}

function BlocklandPrefSO::onAdd(%obj)
{
	%obj.setName("");

	PreferenceContainerGroup.add(%obj);
}
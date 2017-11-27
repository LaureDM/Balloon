using System;

public class TreeTypeMapper
{
	public static string GetStringValue(TreeType type)
	{
		switch (type)
		{
			case TreeType.PINE_TREE:
				return "Pinetree";

			case TreeType.APPLE_TREE:
				return "Appletree";

			case TreeType.OAK:
				return "Oak";

			default:
				return null;
		}
	}

}


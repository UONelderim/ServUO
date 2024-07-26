#region References

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.RegexOptions;

#endregion

namespace Server
{
	public static partial class Utility
	{
		public static int RandomIndex(double[] chances)
		{
			double rand = RandomDouble();

			for (int i = 0; i < chances.Length; i++)
			{
				double chance = chances[i];

				if (rand < chance)
					return i;
				rand -= chance;
			}

			return chances.Length - 1;
		}

		public static T RandomWeigthed<T>(Dictionary<T, int> weightedItems)
		{
			if (weightedItems != null)
			{
				var sum = weightedItems.Values.Sum();
				var rnd = Random(sum);
				foreach (var keyValue in weightedItems)
				{
					if (rnd < keyValue.Value)
						return keyValue.Key;
					rnd -= keyValue.Value;
				}
			}
			return default;
		}
		
		public static T RandomWeigthed<T>(Dictionary<T, double> weightedItems)
		{
			const double resolution = 1000;
			if (weightedItems != null)
			{
				var sum = weightedItems.Values.Sum();
				var rnd = Random((int)(sum * resolution)) / resolution;
				foreach (var keyValue in weightedItems)
				{
					if (rnd < keyValue.Value)
						return keyValue.Key;
					rnd -= keyValue.Value;
				}
			}
			return default;
		}

		public static Direction GetDirection(int xSource, int ySource, int xDest, int yDest)
		{
			if (xSource < xDest)
			{
				if (ySource < yDest) return Direction.Down;
				if (ySource > yDest) return Direction.Right;
				return Direction.East;
			}

			if (xSource > xDest)
			{
				if (ySource < yDest) return Direction.Left;
				if (ySource > yDest) return Direction.Up;
				return Direction.West;
			}

			//xSource == xDest
			if (ySource < yDest) return Direction.South;
			if (ySource > yDest) return Direction.North;
			return Direction.North; //Source == Dest
		}

		public static string SplitCamelCase(string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", Compiled).Trim();
		}
	}
}

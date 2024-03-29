#region References

using System;

#endregion

namespace Arya.Jail
{
	/// <summary>
	///     Defines a filter used to purge jail history
	/// </summary>
	public class JailPurge
	{
		private readonly bool m_Banned;
		private readonly bool m_Deleted;
		private readonly bool m_Old;
		private readonly double m_Hours;

		public JailPurge(bool banned, bool deleted, bool old, int months)
		{
			m_Banned = banned;
			m_Deleted = deleted;
			m_Old = old;
			m_Hours = months * 720d;
		}

		/// <summary>
		///     Verifies if a JailEntry should be removed from history
		/// </summary>
		/// <param name="jail">The JailEntry object being checked</param>
		/// <returns>True if the JailEntry matches the purge conditions</returns>
		public bool PurgeCheck(JailEntry jail)
		{
			if (jail.Account == null && m_Deleted)
			{
				return true;
			}

			if (jail.Account != null && jail.Account.Banned && m_Banned)
			{
				return true;
			}

			if (m_Old)
			{
				TimeSpan age = DateTime.Now - jail.JailTime;

				if (age.TotalHours >= m_Hours)
				{
					return true;
				}
			}

			return false;
		}
	}
}

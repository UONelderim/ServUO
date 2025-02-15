#region References

using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

#endregion

namespace Arya.Jail
{
	/// <summary>
	///     Generic target for use in the jailing system
	/// </summary>
	public class JailTarget : Target
	{
		/// <summary>
		///     The function that should be called if the targeting action is succesful
		/// </summary>
		readonly JailTargetCallback m_Callback;

		/// <summary>
		///     Creates a new jail target
		/// </summary>
		/// <param name="callback">The function that will be called if the target action is succesful</param>
		public JailTarget(JailTargetCallback callback) : base(-1, false, TargetFlags.None)
		{
			m_Callback = callback;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			PlayerMobile pm = targeted as PlayerMobile;


			if (pm != null)
			{
				if (pm.AccessLevel == AccessLevel.Player)
				{
					try
					{
						m_Callback.Invoke(from, pm);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}
				else
				{
					from.SendMessage("The jail system isn't supposed to be used on staff");
				}
			}
			else
			{
				from.SendMessage("The jail system only works on players");
			}
		}
	}

	/// <summary>
	///     Target used for the quick jail system
	/// </summary>
	public class QuickJailTarget : Target
	{
		private readonly int m_Minutes;
		private readonly string m_Message;
		private readonly JailType m_Jail;

		public QuickJailTarget(int minutes, string msg, JailType jail) : base(-1, false, TargetFlags.None)
		{
			m_Minutes = minutes;
			m_Message = msg;
			m_Jail = jail;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			Mobile m = targeted as Mobile;

			if (m == null || !m.Player || m.Deleted)
			{
				from.SendMessage(0x40, "Invalid target");
				return;
			}

			if (!JailSystem.CanBeJailed(m))
			{
				from.SendMessage(0x40, "That player can't be jailed");
				return;
			}

			JailSystem.DoQuickJail(from, m, m_Minutes, m_Message, m_Jail);
		}
	}
}

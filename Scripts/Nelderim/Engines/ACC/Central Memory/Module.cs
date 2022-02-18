#region References

using System;

#endregion

namespace Server.ACC.CM
{
	public abstract class Module
	{
		/*
		 * Append( Module mod, bool negatively )
		 * This method MUST be inherited.
		 * This method is used to take what is already in CM
		 * and add/subtract from it what is on the Module mod.
		 * if( negatively ) means you want to remove stuff.
		 */
		public abstract void Append(Module mod, bool negatively);

		public abstract string Name();

		internal int m_TypeRef;
		public Serial Owner { get; }

		public Module(Serial ser)
		{
			Owner = ser;

			Type type = this.GetType();
			m_TypeRef = CentralMemory.m_Types.IndexOf(type);

			if (m_TypeRef == -1)
			{
				CentralMemory.m_Types.Add(type);
				m_TypeRef = CentralMemory.m_Types.Count - 1;
			}
		}

		public virtual void Serialize(GenericWriter writer)
		{
			writer.Write(0); //version
		}

		public virtual void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
		}
	}
}

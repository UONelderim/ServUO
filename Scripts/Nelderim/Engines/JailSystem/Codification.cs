// 05.09.28 :: troyan

#region References

using System.Collections;

#endregion

namespace Arya.Jail
{
	public class Codification
	{
		// Minimalny czas wiezienia za dane przewinienie
		private int m_MinJailTime;

		private string m_Description;

		// Wszystkie kary w regulaminie zaczynaja sie 4.#.#
		// gdzie # to kolejno major i minor numbers
		private int m_MajorNumber;
		private int m_MinorNumber;
		private bool m_Register;

		public Codification(int major, int minor, int mjt, string desc)
		{
			m_MinJailTime = mjt;
			m_Description = desc;
			m_MajorNumber = major;
			m_MinorNumber = minor;
			m_Register = true;
		}

		public Codification(int major, int minor, int mjt, string desc, bool reg)
		{
			m_MinJailTime = mjt;
			m_Description = desc;
			m_MajorNumber = major;
			m_MinorNumber = minor;
			m_Register = reg;
		}

		public static void Initialize()
		{
			PenalCode = new ArrayList();

			#region Inicjalizacja paragrafow

			#region 4.1 Wykroczenia z zakresu punktu 2.1.1 – Kultura

			PenalCode.Add(new Codification(1, 1, 2 * 24 * 60, "Obrazanie Mistrza Gry"));
			PenalCode.Add(new Codification(1, 2, 12 * 60, "Obrazanie innego Gracza/y zgloszone przez poszkodowanego"));
			PenalCode.Add(new Codification(1, 3, 6 * 60, "Obrazanie tekstem otwartym innego Gracza/y – nie zgloszone"));
			PenalCode.Add(new Codification(1, 4, 12 * 60, "Przeklenstwa tekstem otwartym"));
			PenalCode.Add(new Codification(1, 5, 30, "Przeklenstwa i obrazanie graczy na Party"));
			PenalCode.Add(new Codification(1, 6, 30, "Inne niekulturalne zachowania nieuwzglednione szczegolowo"));

			#endregion

			#region 4.2 Wykroczenia z zakresu punktu 2.1.2 – Zabawa

			PenalCode.Add(new Codification(2, 1, 2 * 60, "Utrudnianie prowadzenia questow i eventow"));
			PenalCode.Add(new Codification(2, 2, 15, "Zwierzeta i przywolance (blokowanie)"));
			PenalCode.Add(new Codification(2, 3, 15, "Custom Housing i budowanie"));
			PenalCode.Add(new Codification(2, 4, 30, "Inne wykroczenia przeciw dobrej zabawie"));

			#endregion

			#region 4.3 Wykroczenia z zakresu punktu 2.1.3 – RPG i fantasy

			PenalCode.Add(new Codification(3, 1, 2 * 60, "Akronimy, emotikony, homofony i rozmowy o swiecie realnym"));
			PenalCode.Add(new Codification(3, 2, 30, "Imiona i nazwy"));
			PenalCode.Add(new Codification(3, 3, 30,
				"Uzywanie jezyka razaco odbiegajacego od fabuly oraz konwencji fantasy"));
			PenalCode.Add(new Codification(3, 4, 0, "Nieuzasadnione odejscie od fabuly rasowej"));
			PenalCode.Add(new Codification(3, 5, 0, "Inne wykroczenia"));

			#endregion

			#region 4.4 Wykroczenia z zakresu punktu 2.1.4 – Rozsadek, a mechanika

			PenalCode.Add(new Codification(4, 1, 2 * 24 * 60, "Wykorzystywanie bledow serwera"));
			PenalCode.Add(new Codification(4, 2, 12 * 60, "Bierne makro"));
			PenalCode.Add(new Codification(4, 3, 2 * 60, "Atak przez przeszkode blokujaca linie widzenia"));
			PenalCode.Add(new Codification(4, 4, 30, "Atak przez przeszkode blokujaca linie dostepu"));
			PenalCode.Add(new Codification(4, 5, 30, "Przenoszenie ciezarow myszka"));
			PenalCode.Add(new Codification(4, 6, 0, "Zachowania irracjonalne"));
			PenalCode.Add(new Codification(4, 7, 0, "Sparing bronia nieproporcjonalna ..."));
			PenalCode.Add(new Codification(4, 8, 0, "Walka z mobkami nie majaca na celu ich zabicia"));
			PenalCode.Add(new Codification(4, 9, 30, "Inne wykroczenia przeciw mechanice"));

			#endregion

			#endregion
		}

		public static ArrayList PenalCode { get; private set; }
	}
}

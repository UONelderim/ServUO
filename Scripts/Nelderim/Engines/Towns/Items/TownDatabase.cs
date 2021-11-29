using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Commands;

namespace Nelderim.Towns
{
    class TownDatabase : Item
    {
        private static Hashtable m_CitizenList = new Hashtable();
        private static Hashtable m_TownList = new Hashtable();
        private static TownAnnouncer m_announcer; 

        [Constructable]
		public TownDatabase() : base(0x2815)
		{
			Movable = false;
            Hue = 38;
            Name = "Baza obywateli";
            m_announcer = new TownAnnouncer();
		}

        public TownDatabase(Serial serial)
            : base(serial)
		{
		}

        public override void OnDoubleClick(Mobile m)
        {
            m.SendGump(new TownDatabaseGump(m));
        }

		public override void Serialize(GenericWriter writer)
		{
            base.Serialize(writer);

            // Version
            writer.Write((int)8);

            // Serialize announcer
            writer.Write((DateTime)m_announcer.LastAlgorithmTime);

            // Serializacja miast
            writer.Write((int)m_TownList.Count);
            TownManager tEntry;
            if (m_TownList.Count > 0)
            {
                //TownManager entry;
                foreach (DictionaryEntry cit in m_TownList)
                {
                    tEntry = (TownManager)cit.Value;

                    // Parameters
                    writer.Write((int)tEntry.Town);
                    // Resources
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Zloto));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Deski));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Sztaby));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Skora));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Material));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Kosci));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Kamienie));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Piasek));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Klejnoty));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Ziola));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Zbroje));
                    writer.Write(tEntry.Resources.ResourceAmount(TownResourceType.Bronie));

                    writer.Write((int)tEntry.Buildings.Count);
                    // Buildings
                    foreach (TownBuilding building in tEntry.Buildings)
                    {
                        writer.Write((int)building.BuildingType);
                        writer.Write((int)building.Status);
                    }

                    // Relacje
                    writer.Write((int)tEntry.TownRelations.Count);
                    foreach (TownRelation tr in tEntry.TownRelations)
                    {
                        writer.Write((int)tr.TownOfRelation);
                        writer.Write((int)tr.AmountOfRelation);
                    }

                    // Serializacja posterunkow i zwiazanych z nimi parametrami
                    writer.Write((int)tEntry.RessurectFrequency);
                    writer.Write((DateTime)tEntry.LastRessurectTime);

                    writer.Write((int)tEntry.TownPosts.Count);
                    foreach (TownPost tp in tEntry.TownPosts)
                    {
                        writer.Write((int)tp.m_x);
                        writer.Write((int)tp.m_y);
                        writer.Write((int)tp.m_z);
                        writer.Write((int)tp.HomeTown);
                        if (tp.IsGuardAlive())
                        {
                            writer.Write(((Serial)tp.GuardSerial).Value);//Serial
                        }
                        else
                        {
                            writer.Write(0);//Serial
                        }
                        writer.Write((DateTime)tp.ActivatedDate);
                        writer.Write((String)tp.PostName);
                        writer.Write((int)tp.TownGuard);
                        writer.Write((int)tp.PostStatus);
                        writer.Write((int)tp.RessurectAmount);
                    }

                    // Podatki
                    writer.Write((int)tEntry.TaxesForThisTown);
                    writer.Write((int)tEntry.TaxesForOtherTowns);
                    writer.Write((int)tEntry.TaxesForNoTown);

                    // Townlogs
                    writer.Write((bool)tEntry.InformLeader);
                    writer.Write((int)tEntry.TownLogs.Count);
                    foreach (TownLog tl in tEntry.TownLogs)
                    {
                        writer.Write((DateTime)tl.LogDate);
                        writer.Write((int)tl.LogType);
                        writer.Write((String)tl.Text);
                        writer.Write((int)tl.A);
                        writer.Write((int)tl.B);
                        writer.Write((int)tl.C);
                    }
                }
            }

            // Serializacja obywateli
            writer.Write((int)m_CitizenList.Count);
            TownCitizenship entry;
            if (m_CitizenList.Count > 0)
            {
                //TownCitizenship entry;
                foreach (DictionaryEntry cit in m_CitizenList)
                {
                    entry = (TownCitizenship)cit.Value;
                    writer.Write(((Serial)cit.Key).Value);//Serial
                    writer.Write((int)entry.CurrentTown);
                    writer.Write((int)entry.SpentDevotion);
                    writer.Write((DateTime)entry.JoinedDate);
                    writer.Write((int)entry.CurrentTownStatus);
                    writer.Write((int)entry.CurrentTownConselourStatus);
                    // Resources
                    writer.Write(entry.ResourceAmount(TownResourceType.Zloto));
                    writer.Write(entry.ResourceAmount(TownResourceType.Deski));
                    writer.Write(entry.ResourceAmount(TownResourceType.Sztaby));
                    writer.Write(entry.ResourceAmount(TownResourceType.Skora));
                    writer.Write(entry.ResourceAmount(TownResourceType.Material));
                    writer.Write(entry.ResourceAmount(TownResourceType.Kosci));
                    writer.Write(entry.ResourceAmount(TownResourceType.Kamienie));
                    writer.Write(entry.ResourceAmount(TownResourceType.Piasek));
                    writer.Write(entry.ResourceAmount(TownResourceType.Klejnoty));
                    writer.Write(entry.ResourceAmount(TownResourceType.Ziola));
                    writer.Write(entry.ResourceAmount(TownResourceType.Zbroje));
                    writer.Write(entry.ResourceAmount(TownResourceType.Bronie));

                }
            }
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

            TownCitizenship entry;
            Serial entrySerial;
            TownManager tEntry;
            Towns tEntryTown;
            
            switch (version)
            {
                case 8:
                {
                    m_announcer = new TownAnnouncer(reader.ReadDateTime());
                    int townsDatabaseSize = reader.ReadInt();
                    for (int i = 0; i < townsDatabaseSize; i++)
                    {
                        tEntryTown = (Towns)reader.ReadInt();
                        tEntry = new TownManager(tEntryTown);
                        // Resources - ponizsze zasoby zostana powiekszone niezaleznie od stanu maksymalnego, ktory moze ulec zmianie przy braniu pod uwage budynkow
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Zloto, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Deski, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Sztaby, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Skora, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Material, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Kosci, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Kamienie, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Piasek, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Klejnoty, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Ziola, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Zbroje, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Bronie, reader.ReadInt());
                        m_TownList.Add(tEntry.Town, tEntry); // Miasto zostaje dodane przed budynkami zeby uzyc API bezposrednio z modulu TownDatabase
                        // Buildings
                        int buildingsSize = reader.ReadInt();
                        TownBuildingName bName;
                        TownBuildingStatus bStatus;

                        for (int ib = 0; ib < buildingsSize; ib++)
                        {
                            bName = (TownBuildingName)reader.ReadInt();
                            bStatus = (TownBuildingStatus)reader.ReadInt();
                            /* Jesli kontruktor miasta TownManager ustawia budynek jako niedostepny, oznacza to, ze nie bedzie on nadpisany serializowana wartoscia
                                * Inaczej mowiac, ekipa stwierdzila, ze budynek powinien byc niedostepny, spowodoje to, nadpisanie wartosci serializowanych niezaleznie od ich statusu
                                * Jesli kontruktor dopuszcza budowe, status budynku bedzie pobierany z zapisanej wartosci
                            */
                            if (GetBuildingStatus(tEntry.Town, bName) != TownBuildingStatus.Niedostepny && bStatus != TownBuildingStatus.Niedostepny)
                            {
                                ChangeBuildingStatus(tEntry.Town, bName, bStatus);
                            }
                        }

                        // Relacje
                        Towns t;
                        int rel;
                        int relCount = reader.ReadInt();

                        for (int ib = 0; ib < relCount; ib++)
                        {
                            t = (Towns)reader.ReadInt();
                            rel = reader.ReadInt();
                            tEntry.TownRelations.Add(new TownRelation(t, rel));
                        }

                        // Posterunko i zwiazanych z nimi parametrami
                        tEntry.RessurectFrequency = reader.ReadInt();
                        tEntry.LastRessurectTime = reader.ReadDateTime();

                        int x, y, z;
                        Towns ht;
                        int guardSerialInt;
                        Serial guardSerial;
                        bool isAlive;
                        DateTime ActivatedDate;
                        string postName;
                        TownGuards tg;
                        TownBuildingStatus tbs;
                        int ra;
                        TownPost m_tp;

                        int postsCount = reader.ReadInt();
                        for (int ib = 0; ib < postsCount; ib++)
                        {
                            x = reader.ReadInt();
                            y = reader.ReadInt();
                            z = reader.ReadInt();
                            ht = (Towns)reader.ReadInt();
                            guardSerialInt = reader.ReadInt();
                            guardSerial = new Serial(guardSerialInt);
                            isAlive = guardSerial == 0 ? false : true;
                            ActivatedDate = reader.ReadDateTime();
                            postName = reader.ReadString();
                            tg = (TownGuards)reader.ReadInt();
                            tbs = (TownBuildingStatus)reader.ReadInt();
                            ra = reader.ReadInt();

                            if (tbs != TownBuildingStatus.Dziala)
                            {
                                m_tp = new TownPost(postName, x, y, z, ht, this.Map, tg, tbs);
                            }
                            else
                            {
                                // Dodac deserializacje straznika
                                // Zabic istniejacego i stworzyc nowego lub podobnie
                                if (isAlive)
                                {
                                    CommandHandlers.BroadcastMessage(AccessLevel.GameMaster, 300, World.FindMobile(guardSerial).Name);
                                    m_tp = new TownPost(postName, x, y, z, ht, this.Map, tg, ActivatedDate, guardSerial);
                                }
                                else
                                {
                                    m_tp = new TownPost(postName, x, y, z, ht, this.Map, tg, ActivatedDate);
                                }
                            }
                            tEntry.TownPosts.Add(m_tp);
                        }


                        // Podatki
                        tEntry.TaxesForThisTown = reader.ReadInt();
                        tEntry.TaxesForOtherTowns = reader.ReadInt();
                        tEntry.TaxesForNoTown = reader.ReadInt();

                        // Town log
                        tEntry.InformLeader = reader.ReadBool();
                        int logCount = reader.ReadInt();
                        for (int ib = 0; ib < logCount; ib++)
                        {
                            tEntry.AddLogOnDeserialization(reader.ReadDateTime(), (TownLogTypes)reader.ReadInt(), reader.ReadString(), reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
                        }
                    }

                    // Obywatele
                    int citizenDatabaseSize = reader.ReadInt();
                    for (int i = 0; i < citizenDatabaseSize; i++)
                    {
                        entrySerial = reader.ReadSerial();
                        Mobile mob = World.FindMobile(entrySerial);

                        entry = new TownCitizenship((Towns)reader.ReadInt());
                        entry.SpentDevotion = reader.ReadInt();
                        entry.JoinedDate = reader.ReadDateTime();
                        entry.ChangeCurrentTownStatus((TownStatus)reader.ReadInt());
                        entry.ChangeCurrentTownConselourStatus((TownCounsellor)reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Zloto, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Deski, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Sztaby, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Skora, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Material, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Kosci, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Kamienie, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Piasek, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Klejnoty, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Ziola, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Zbroje, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Bronie, reader.ReadInt());

                        // Check does exist
                        if (mob != null)
                        {
                            // Check is player
                            if (mob.Player)
                            {
                                // Check is character creation data older than joining the city
                                if (entry.JoinedDate.CompareTo(mob.CreationTime) == 1)
                                {
                                    // Add citizen to database
                                    m_CitizenList.Add(entrySerial, entry);
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("Mobile with serial {0} and name {1} was created later than joining the city, removing from TownDatabase", entrySerial.ToString(), mob.Name));
                                }
                            }
                            else
                            {
                                Console.WriteLine(string.Format("Mobile with serial {0} and name {1} is not player, removing from TownDatabase", entrySerial.ToString(), mob.Name));
                            }
                        }
                        else
                        {
                            Console.WriteLine(string.Format("Mobile with serial {0} does not exist, removing from TownDatabase", entrySerial.ToString()));
                        }
                    }

                    // Sprawdzi czy ilosc relacji jest poprawna w stosunku do ilosci miast
                    CheckRelations();
                    break;
                }
                case 7:
                {
                    m_announcer = new TownAnnouncer(reader.ReadDateTime());
                    int townsDatabaseSize = reader.ReadInt();
                    for (int i = 0; i < townsDatabaseSize; i++)
                    {
                        tEntryTown = (Towns)reader.ReadInt();
                        tEntry = new TownManager(tEntryTown);
                        // Resources - ponizsze zasoby zostana powiekszone niezaleznie od stanu maksymalnego, ktory moze ulec zmianie przy braniu pod uwage budynkow
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Zloto, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Deski, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Sztaby, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Skora, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Material, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Kosci, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Kamienie, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Piasek, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Klejnoty, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Ziola, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Zbroje, reader.ReadInt());
                        tEntry.Resources.ResourceIncreaseAmount(TownResourceType.Bronie, reader.ReadInt());
                        m_TownList.Add(tEntry.Town, tEntry); // Miasto zostaje dodane przed budynkami zeby uzyc API bezposrednio z modulu TownDatabase
                        // Buildings
                        int buildingsSize = reader.ReadInt();
                        TownBuildingName bName;
                        TownBuildingStatus bStatus;

                        for (int ib = 0; ib < buildingsSize; ib++)
                        {
                            bName = (TownBuildingName)reader.ReadInt();
                            bStatus = (TownBuildingStatus)reader.ReadInt();
                            /* Jesli kontruktor miasta TownManager ustawia budynek jako niedostepny, oznacza to, ze nie bedzie on nadpisany serializowana wartoscia
                                * Inaczej mowiac, ekipa stwierdzila, ze budynek powinien byc niedostepny, spowodoje to, nadpisanie wartosci serializowanych niezaleznie od ich statusu
                                * Jesli kontruktor dopuszcza budowe, status budynku bedzie pobierany z zapisanej wartosci
                            */
                            if (GetBuildingStatus(tEntry.Town, bName) != TownBuildingStatus.Niedostepny && bStatus != TownBuildingStatus.Niedostepny)
                            {
                                ChangeBuildingStatus(tEntry.Town, bName, bStatus);
                            }
                        }

                        // Relacje
                        Towns t;
                        int rel;
                        int relCount = reader.ReadInt();

                        for (int ib = 0; ib < relCount; ib++)
                        {
                            t = (Towns)reader.ReadInt();
                            rel = reader.ReadInt();
                            tEntry.TownRelations.Add(new TownRelation(t, rel));
                        }

                        // Posterunko i zwiazanych z nimi parametrami
                        tEntry.RessurectFrequency = reader.ReadInt();
                        tEntry.LastRessurectTime = reader.ReadDateTime();

                        int x, y, z;
                        Towns ht;
                        int guardSerialInt;
                        Serial guardSerial;
                        bool isAlive;
                        DateTime ActivatedDate;
                        string postName;
                        TownGuards tg;
                        TownBuildingStatus tbs;
                        int ra;
                        TownPost m_tp;

                        int postsCount = reader.ReadInt();
                        for (int ib = 0; ib < postsCount; ib++)
                        {
                            x = reader.ReadInt();
                            y = reader.ReadInt();
                            z = reader.ReadInt();
                            ht = (Towns)reader.ReadInt();
                            guardSerialInt = reader.ReadInt();
                            guardSerial = new Serial(guardSerialInt);
                            isAlive = guardSerial == 0 ? false : true;
                            ActivatedDate = reader.ReadDateTime();
                            postName = reader.ReadString();
                            tg = (TownGuards)reader.ReadInt();
                            tbs = (TownBuildingStatus)reader.ReadInt();
                            ra = reader.ReadInt();

                            if (tbs != TownBuildingStatus.Dziala)
                            {
                                m_tp = new TownPost(postName, x, y, z, ht, this.Map, tg, tbs);
                            }
                            else
                            {
                                // Dodac deserializacje straznika
                                // Zabic istniejacego i stworzyc nowego lub podobnie
                                if (isAlive)
                                {
                                    CommandHandlers.BroadcastMessage(AccessLevel.GameMaster, 300, World.FindMobile(guardSerial).Name);
                                    m_tp = new TownPost(postName, x, y, z, ht, this.Map, tg, ActivatedDate, guardSerial);
                                }
                                else
                                {
                                    m_tp = new TownPost(postName, x, y, z, ht, this.Map, tg, ActivatedDate);
                                }
                            }
                            tEntry.TownPosts.Add(m_tp);
                        }


                        // Podatki
                        tEntry.TaxesForThisTown = reader.ReadInt();
                        tEntry.TaxesForOtherTowns = reader.ReadInt();
                        tEntry.TaxesForNoTown = reader.ReadInt();

                        // Town log
                        tEntry.InformLeader = reader.ReadBool();
                        int logCount = reader.ReadInt();
                        for (int ib = 0; ib < logCount; ib++)
                        {
                            tEntry.AddLogOnDeserialization(reader.ReadDateTime(), (TownLogTypes)reader.ReadInt(), reader.ReadString(), reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
                        }
                    }

                    // Obywatele
                    int citizenDatabaseSize = reader.ReadInt();
                    for (int i = 0; i < citizenDatabaseSize; i++)
                    {
                        entrySerial = reader.ReadSerial();
                        Mobile mob = World.FindMobile(entrySerial);

                        entry = new TownCitizenship((Towns)reader.ReadInt());
                        entry.JoinedDate = reader.ReadDateTime();
                        entry.ChangeCurrentTownStatus((TownStatus)reader.ReadInt());
                        entry.ChangeCurrentTownConselourStatus((TownCounsellor)reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Zloto, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Deski, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Sztaby, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Skora, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Material, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Kosci, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Kamienie, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Piasek, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Klejnoty, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Ziola, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Zbroje, reader.ReadInt());
                        entry.ResourceIncreaseAmount(TownResourceType.Bronie, reader.ReadInt());

                        // Check does exist
                        if (mob != null)
                        {
                            // Check is player
                            if (mob.Player)
                            {
                                // Check is character creation data older than joining the city
                                if (entry.JoinedDate.CompareTo(mob.CreationTime) == 1 )
                                {
                                    // Add citizen to database
                                    m_CitizenList.Add(entrySerial, entry);
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("Mobile with serial {0} and name {1} was created later than joining the city, removing from TownDatabase", entrySerial.ToString(), mob.Name));
                                }
                            }
                            else
                            {
                                Console.WriteLine(string.Format("Mobile with serial {0} and name {1} is not player, removing from TownDatabase", entrySerial.ToString(), mob.Name));
                            }
                        }
                        else
                        {
                            Console.WriteLine(string.Format("Mobile with serial {0} does not exist, removing from TownDatabase", entrySerial.ToString()));
                        }
                    }

                    // Sprawdzi czy ilosc relacji jest poprawna w stosunku do ilosci miast
                    CheckRelations();
                    break;
                }
            }
		}

        public static float ChargeMultipier()
        {
            return m_announcer.ChargeMultipier();
        }

        public static bool ChargeForBuildings()
        {
            return m_announcer.ChargeForBuildings();
        }

        void CheckRelations()
        {
            int amountOfTowns = TownDatabase.GetTownsNames().Count;
            TownManager tmpTown;

            foreach (Towns tm in TownDatabase.GetTownsNames())
            {
                if (tm != Towns.None)
                {
                    tmpTown = TownDatabase.GetTown(tm);

                    // Jesli miasto nie posiada Relacji w ogole, dodajemy wszystkie miasta
                    if (tmpTown.TownRelations.Count == 0)
                    {
                        foreach (Towns tmRel in TownDatabase.GetTownsNames())
                        {
                            if (tmRel != Towns.None && tm != tmRel)
                            {
                                tmpTown.TownRelations.Add( new TownRelation(tmRel, 0));
                            }
                        }
                    }
                    // Jesli miasto posiada relacje, ale jest ich mniej niz ilosc miast - 1 (bo nie liczym tego samego miasta) dodajemy tych, ktorych brakuje
                    else if (tmpTown.TownRelations.Count < amountOfTowns - 1)
                    {
                        foreach (Towns tmRel in TownDatabase.GetTownsNames())
                        {
                            if (tmRel != Towns.None && tm != tmRel && tmpTown.TownRelations.Find(obj => obj.TownOfRelation == tmRel) == null)
                            {
                                tmpTown.TownRelations.Add(new TownRelation(tmRel, 0));
                            }
                        }
                    }
                }
            }
        }

        #region Citizens
        public static bool IsCitizenOfAnyTown(Mobile from)
        {
            return m_CitizenList.Contains(from.Serial);
        }

        public static bool IsCitizenOfAnyTown(Serial fromSerial)
        {
            return m_CitizenList.Contains(fromSerial);
        }

        public static Towns IsCitizenOfWhichTown(Mobile from)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return Towns.None;
            }
            else
            {
                return ((TownCitizenship)m_CitizenList[from.Serial]).CurrentTown;
            }
        }

        public static bool IsCitizenOfGivenTown(Mobile from, Towns town)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return false;
            }
            else
	        {
                return ((TownCitizenship)m_CitizenList[from.Serial]).CurrentTown == town;
	        }
        }

        public static bool IsCitizenOfGivenTown(Serial fromSerial, Towns town)
        {
            if (!IsCitizenOfAnyTown(fromSerial))
            {
                return false;
            }
            else
            {
                return ((TownCitizenship)m_CitizenList[fromSerial]).CurrentTown == town;
            }
        }

        public static SortedList<string, Mobile> GetMobilesByName(Towns town)
        {
            SortedList<string, Mobile> m_citizens = new SortedList<string, Mobile>();
            Mobile tmpMobile;

            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, town))
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    if (tmpMobile != null)
                        m_citizens.Add(tmpMobile.Name, tmpMobile);
                }
            }

            return m_citizens;
        }

        public static SortedDictionary<string, string> GetCitizensByName(Towns town)
        {
            SortedDictionary<string, string> m_citizens = new SortedDictionary<string, string>();
            Mobile tmpMobile;
            string citStatus = "";

            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator(); 
			while ( citNum.MoveNext() ) 
			{ 
                if (IsCitizenOfGivenTown((Serial)citNum.Key, town))
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    if (tmpMobile != null)
                    {
                        switch (GetCurrentTownStatus(tmpMobile))
                        {
                            case TownStatus.Leader:
                                citStatus = "Przywodca";
                                break;
                            case TownStatus.Counsellor:
                                citStatus = "Kanclerz";
                                break;
                            case TownStatus.Citizen:
                                citStatus = "Obywatel";
                                break;
                            case TownStatus.NPC:
                                break;
                            case TownStatus.Vendor:
                                break;
                            case TownStatus.Guard:
                                break;
                            case TownStatus.None:
                                break;
                            default:
                                break;
                        }
                        m_citizens.Add(tmpMobile.Name, citStatus);
                    }
                }
			}

            return m_citizens;
        }

        public static SortedDictionary<string, string> GetCitizensByNameWithStatusAsDict(Towns town, TownStatus status)
        {
            SortedDictionary<string, string> m_citizens = new SortedDictionary<string, string>();
            Mobile tmpMobile;
            string conStatus = "";
            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, town))
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    if (tmpMobile != null && GetCurrentTownStatus(tmpMobile) == status)
                    {
                        switch (GetCurrentTownConselourStatus(tmpMobile))
	                    {
		                case TownCounsellor.Prime:
                            conStatus = "Kanclerz glowny";
                            break;
                        case TownCounsellor.Army:
                            conStatus = "Kanclerz armii";
                            break;
                        case TownCounsellor.Diplomacy:
                            conStatus = "Kanclerz dyplomacji";
                            break;
                        case TownCounsellor.Economy:
                            conStatus = "Kanclerz ekonomii";
                            break;
                        case TownCounsellor.Architecture:
                            conStatus = "Kanclerz budownictwa";
                            break;
                        case TownCounsellor.None:
                            break;
                        default:
                            break;
	                    }
                        m_citizens.Add(tmpMobile.Name, conStatus);
                    }
                }
            }

            return m_citizens;
        }

        public static SortedList<string, Mobile> GetCitizensByNameWithStatusAsList(Towns town, TownStatus status)
        {
            SortedList<string, Mobile> m_citizens = new SortedList<string, Mobile>();
            Mobile tmpMobile;

            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, town))
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    if (tmpMobile != null && GetCurrentTownStatus(tmpMobile) == status)
                        m_citizens.Add(tmpMobile.Name, tmpMobile);
                }
            }

            return m_citizens;
        }

        public static int GetAmountOfCitizensWithGivenStatus(Towns town, TownStatus status)
        {
            int amount = 0;
            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, town) && ((TownCitizenship)m_CitizenList[(Serial)citNum.Key]).CurrentTownStatus == status)
                {
                    amount += 1;
                }
            }

            return amount;
        }

        public static Dictionary<string, int> GetCitizensByResource(Towns town, TownResourceType res)
        {
            Dictionary<string, int> m_citizens = new Dictionary<string, int>();
            Mobile tmpMobile;

            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, town))
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    if (tmpMobile != null)
                        m_citizens.Add(tmpMobile.Name, GetResourceAmountOfCitizen(tmpMobile, res));
                }
            }

            return m_citizens;
        }

        public static Dictionary<string, DateTime> GetCitizensByJoinDate(Towns town)
        {
            Dictionary<string, DateTime> m_citizens = new Dictionary<string, DateTime>();
            Mobile tmpMobile;

            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, town))
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    if (tmpMobile != null)
                        m_citizens.Add(tmpMobile.Name, GetJoinDate(tmpMobile));
                }
            }

            return m_citizens;
        }

        public static bool AddCitizen(Mobile from, Towns newCurrentTown)
        {
            if (IsCitizenOfAnyTown(from))
            {
                return false;
            }
            else
            {
                TownCitizenship entry = new TownCitizenship(newCurrentTown);
                m_CitizenList.Add(from.Serial, entry);
                return true;
            }
        }

        public static Hashtable GetCitizens()
        {
            return m_CitizenList;
        }

        public static bool LeaveCurrentTown(Mobile from)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return false;
            }
            else
            {
                TownCitizenship entry = (TownCitizenship)m_CitizenList[from.Serial];
                entry.LeaveCurrentTown();
                m_CitizenList.Remove(from.Serial);
                return true;
            }
            
        }

        public static bool ChangeCurrentTownStatus(Mobile from, TownStatus newStatus)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return false;
            }
            else
            {
                TownCitizenship entry = (TownCitizenship)m_CitizenList[from.Serial];
                entry.ChangeCurrentTownStatus(newStatus);
                m_CitizenList[from.Serial] = entry;
                return true;
            }
        }

        public static bool ChangeCurrentConselourStatus(Mobile from, TownCounsellor newStatus)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return false;
            }
            else
            {
                TownCitizenship entry = (TownCitizenship)m_CitizenList[from.Serial];
                entry.ChangeCurrentTownConselourStatus(newStatus);
                m_CitizenList[from.Serial] = entry;
                return true;
            }
        }

        public static TownStatus GetCurrentTownStatus(Mobile from)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return TownStatus.None;
            }
            else
            {
                TownCitizenship entry = (TownCitizenship)m_CitizenList[from.Serial];
                return entry.CurrentTownStatus;
            }
        }

        public static TownCounsellor GetCurrentTownConselourStatus(Mobile from)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return TownCounsellor.None;
            }
            else
            {
                TownCitizenship entry = (TownCitizenship)m_CitizenList[from.Serial];
                if (GetCurrentTownStatus(from) == TownStatus.Counsellor)
                    return entry.CurrentTownConselourStatus;
                else
                    return TownCounsellor.None;
            }
        }

        public static DateTime GetJoinDate(Mobile from)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return DateTime.Now;
            }
            else
            {
                TownCitizenship entry = (TownCitizenship)m_CitizenList[from.Serial];
                return entry.JoinedDate;
            }
        }

        public static bool IncreaseResourceAmountForCitizen(Mobile from, TownResourceType nType, int amount)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return false;
            }
            else
            {
                TownCitizenship entry = (TownCitizenship)m_CitizenList[from.Serial];
                entry.ResourceIncreaseAmount(nType, amount);
                m_CitizenList[from.Serial] = entry;
                return true;
            }
        }

        public static int GetResourceAmountOfCitizen(Mobile from, TownResourceType nType)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return -1;
            }
            else
            {
                return ((TownCitizenship)m_CitizenList[from.Serial]).ResourceAmount(nType);
            }
        }

        public static Towns GetCitizenCurrentCity(Mobile from)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return Towns.None;
            }
            else
            {
                return ((TownCitizenship)m_CitizenList[from.Serial]).CurrentTown;
            }
        }

        public static TownStatus GetCitizenCurrentStatus(Mobile from)
        {
            if (!IsCitizenOfAnyTown(from))
            {
                return TownStatus.None;
            }
            else
            {
                return ((TownCitizenship)m_CitizenList[from.Serial]).CurrentTownStatus;
            }
        }

        public static TownStatus GetCitizenCurrentStatus(Serial fromSerial)
        {
            if (!IsCitizenOfAnyTown(fromSerial))
            {
                return TownStatus.None;
            }
            else
            {
                return ((TownCitizenship)m_CitizenList[fromSerial]).CurrentTownStatus;
            }
        }

        public static TownCitizenship GetCitinzeship(Mobile from)
        {
            return (TownCitizenship)m_CitizenList[from.Serial];
        }

        public static TownStatus GetCitizenTownStatusInGivenTown(Mobile from, Towns mTown)
        {
            if (!IsCitizenOfGivenTown(from, mTown))
            {
                return TownStatus.None;
            }
            else
            {
                return ((TownCitizenship)m_CitizenList[from.Serial]).CurrentTownStatus;
            }
        }

        public static bool HasCitizenInTownGivenStatus(Towns mTown, TownStatus mStatus)
        {
            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, mTown) && GetCitizenCurrentStatus((Serial)citNum.Key) == mStatus)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetCitizenNameFromTownWithStatus(Towns mTown, TownStatus mStatus)
        {
            Mobile tmpMobile;

            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, mTown) && GetCitizenCurrentStatus((Serial)citNum.Key) == mStatus)
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    if (tmpMobile != null)
                        return tmpMobile.Name;
                    else
                        return "brak";
                }
            }
            return "brak";
        }

        public static Mobile CitizenMobileFromTownWithStatus(Towns mTown, TownStatus mStatus)
        {
            Mobile tmpMobile;

            IDictionaryEnumerator citNum = m_CitizenList.GetEnumerator();
            while (citNum.MoveNext())
            {
                if (IsCitizenOfGivenTown((Serial)citNum.Key, mTown) && GetCitizenCurrentStatus((Serial)citNum.Key) == mStatus)
                {
                    tmpMobile = World.FindMobile((Serial)citNum.Key);
                    return tmpMobile;
                }
            }
            return null;
        }

        public static void InformLeader(Towns mTown, string stringToSend)
        { 
            Mobile mLeader = CitizenMobileFromTownWithStatus(mTown, TownStatus.Leader);
            if (mLeader != null)
            {
                mLeader.SendAsciiMessage(stringToSend);
            }
        }
        #endregion

        #region Towns
        public static bool CreateTown(Towns townName) 
        {
            if (IsTown(townName) || townName == Towns.None)
            {
                return false;
            }
            else
            {
                TownManager entry = new TownManager(townName);
                m_TownList.Add(entry.Town, entry);
                return true;
            }
        }

        public static TownManager GetTown(Towns townName)
        {
            if (!IsTown(townName))
            {
                CreateTown(townName);
            }
            return (TownManager)m_TownList[townName];
        }

        public static void AddTownLog(Towns townName, TownLogTypes tlp, string txt, int a, int b, int c)
        {
            if (townName != Towns.None)
            {
                GetTown(townName).AddLog(tlp, txt, a, b, c);
            }
        }

        public static int GetRelationOfATownWithTown(Towns fromTown, Towns toTown)
        {
            return GetTown(fromTown).TownRelations.Find(obj => obj.TownOfRelation == toTown).AmountOfRelation;
        }

        public static void ChangeRelationOfATownWithTownByAmount(Towns fromTown, Towns toTown, int amount)
        {
            TownManager tm = GetTown(fromTown);
            int result = tm.TownRelations.Find(obj => obj.TownOfRelation == toTown).AmountOfRelation + amount;
            if (result >= -100 && result <= 100)
            {
                SetRelationOfATownWithTownByAmount(fromTown, toTown, result);
            }
        }

        static void SetRelationOfATownWithTownByAmount(Towns fromTown, Towns toTown, int amount)
        {
            TownManager tm = GetTown(fromTown);
            tm.TownRelations.Find(obj => obj.TownOfRelation == toTown).AmountOfRelation = amount;
        }

        public static ICollection GetTownsNames()
        {
            if (m_TownList.Count > 0)
            {
                return m_TownList.Keys;
            }
            else 
            {
                return null;
            }
        }

        static bool IsTown(Towns townName)
        {
            return m_TownList.Contains(townName);
        }

        static TownResources GetTownResources(Towns townName)
        {
            if (!IsTown(townName))
            {
                CreateTown(townName); 
            }
            return ((TownManager)m_TownList[townName]).Resources;
        }

        public static List<TownBuilding> GetBuildingsList(Towns townName)
        {
            if (!IsTown(townName))
            {
                CreateTown(townName);
            }
            return ((TownManager)m_TownList[townName]).Buildings;
        }

        public static List<TownBuilding> GetBuildingsListWithStatus(Towns townName, TownBuildingStatus status)
        {
            List<TownBuilding> m_buildings = new List<TownBuilding>();
            List<TownBuilding> m_fromBuildings = ((TownManager)m_TownList[townName]).Buildings;

            for (int i = 0; i < m_fromBuildings.Count; i++)
            {
                if (m_fromBuildings[i].Status == status)
                {
                    m_buildings.Add(m_fromBuildings[i]);
                }
            }
            return m_buildings;
        }

        public static TownBuilding GetBuilding(Towns townName, TownBuildingName buildingType)
        {
            List<TownBuilding> m_buildingsList = GetBuildingsList(townName);
            return m_buildingsList.Find(obj => obj.BuildingType == buildingType);
        }

        public static TownBuildingStatus GetBuildingStatus(Towns townName, TownBuildingName buildingType)
        {
            List<TownBuilding> m_buildingsList = GetBuildingsList(townName);
            return m_buildingsList.Find(obj => obj.BuildingType == buildingType).Status;
        }

        public static void ChangeBuildingStatus(Towns townName, TownBuildingName buildingType, TownBuildingStatus newStatus, bool onDes = false)
        {
            List<TownBuilding> m_buildingsList = GetBuildingsList(townName);
            m_buildingsList.Find(obj => obj.BuildingType == buildingType).Status = newStatus;
        }

        public static bool HaveTownRequiredBuildingsToBuildGivenBuilding(Towns townName, TownBuildingName buildingType)
        {
            List<TownBuilding> m_buildingsList = GetBuildingsList(townName);
            // Nie ma zaleznosci od budynkow
            if (m_buildingsList.Find(obj => obj.BuildingType == buildingType).Dependecies.Count == 0) return true;
            // Sprawdzamy czy wymagane budynki zostaly zbudowane (moga byc zawieszone)
            foreach (TownBuildingName dep in m_buildingsList.Find(obj => obj.BuildingType == buildingType).Dependecies)
            {
                if (!(m_buildingsList.Find(obj => obj.BuildingType == dep).Status == TownBuildingStatus.Dziala ||
                    m_buildingsList.Find(obj => obj.BuildingType == dep).Status == TownBuildingStatus.Zawieszony))
                {
                    return false;
                }
            }
            // Jesli funkcja dotarla do tego miejsca oznacza to, ze wszystkie wymagane budynki sa zbudowane
            return true;
        }

        public static bool HaveTownRequiredResourcesToBuildGivenBuilding(Towns townName, TownBuildingName buildingType)
        {
            return HaveTownRequiredResources(townName, buildingType, 1f * TownDatabase.ChargeMultipier());
        }

        public static bool HaveTownRequiredResourcesOnePromil(Towns townName, TownBuildingName buildingType)
        {
            return HaveTownRequiredResources(townName, buildingType, 0.001f * TownDatabase.ChargeMultipier());
        }

        public static bool HaveTownRequiredResourcesOnePercent(Towns townName, TownBuildingName buildingType)
        {
            return HaveTownRequiredResources(townName, buildingType, 0.01f * TownDatabase.ChargeMultipier());
        }

        static bool HaveTownRequiredResources(Towns townName, TownBuildingName buildingType, float multiplier)
        {
            List<TownBuilding> m_buildingsList = GetBuildingsList(townName);
            TownResources m_townRes = GetTownResources(townName);
            List<TownResource> ress = m_buildingsList.Find(obj => obj.BuildingType == buildingType).Resources.Resources;
            // Nie ma zaleznosci od zasobow
            if (ress.Count == 0) return true;
            // Sprawdzamy czy wymagane zasoby sa w posiadaniu skarbca miasta
            foreach (TownResource res in ress)
            {
                if (m_townRes.ResourceAmount(res.ResourceType) < (res.Amount * multiplier))
                {
                    return false;
                }
            }
            // Jesli funkcja dotarla do tego miejsca oznacza to, ze miasto posiada wszystkie surowce w wymaganej ilosci
            return true;
        }

        public static void UseTownRequiredResources(Towns townName, TownBuildingName buildingType)
        {
            UseTownRequiredResources(townName, buildingType, 1f * TownDatabase.ChargeMultipier());
        }

        public static void UseTownRequiredResourcesOnePromil(Towns townName, TownBuildingName buildingType)
        {
            UseTownRequiredResources(townName, buildingType, 0.001f * TownDatabase.ChargeMultipier());
        }

        public static void UseTownRequiredResourcesOnePercent(Towns townName, TownBuildingName buildingType)
        {
            UseTownRequiredResources(townName, buildingType, 0.01f * TownDatabase.ChargeMultipier());
        }

        static void UseTownRequiredResources(Towns townName, TownBuildingName buildingType, float multiplier)
        {
            List<TownBuilding> m_buildingsList = GetBuildingsList(townName);
            TownResources m_townRes = GetTownResources(townName);
            List<TownResource> ress = m_buildingsList.Find(obj => obj.BuildingType == buildingType).Resources.Resources;
            if (ress.Count == 0) return;
            foreach (TownResource res in ress)
            {
                m_townRes.ResourceDecreaseAmount(res.ResourceType, (int)(res.Amount * multiplier));
            }
        }
        #endregion
    }
}

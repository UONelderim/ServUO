using System;
using System.Collections;
using Server.Engines.XmlSpawner2;
using Server.Gumps;

/*
** DeathmatchGauntlet
** ArteGordon
** updated 12/05/04
**
** used to set up a Deathmatch pvp challenge game through the XmlPoints system.
*/
namespace Server.Items
{
    public class DeathmatchGauntlet : BaseChallengeGame
    {
        public static bool OnlyInChallengeGameRegion = false;
        private static readonly TimeSpan MaximumOutOfBoundsDuration = TimeSpan.FromSeconds(15);// maximum time allowed out of bounds before disqualification
        private static readonly TimeSpan MaximumOfflineDuration = TimeSpan.FromSeconds(60);// maximum time allowed offline before disqualification
        private static readonly TimeSpan MaximumHiddenDuration = TimeSpan.FromSeconds(10);// maximum time allowed hidden before disqualification
        private static readonly TimeSpan RespawnTime = TimeSpan.FromSeconds(6);// delay until autores if autores is enabled
        private readonly ArrayList m_Organizers = new ArrayList();
        private Mobile m_Challenger;
        private ArrayList m_Participants = new ArrayList();
        private bool m_GameLocked;
        private bool m_GameInProgress;
        private int m_TotalPurse;
        private int m_EntryFee;
        private int m_TargetScore = 10;// default target score to end match is 10
        private DateTime m_MatchStart;
        private TimeSpan m_MatchLength = TimeSpan.FromMinutes(10);// default match length is 10 mins
        private int m_ArenaSize = 0;// maximum distance from the challenge gauntlet allowed before disqualification.  Zero is unlimited range
        public DeathmatchGauntlet(Mobile challenger)
            : base(0x1414)
        {
            this.m_Challenger = challenger;
            
            this.m_Organizers.Add(challenger);

            // check for points attachments
            XmlPoints afrom = (XmlPoints)XmlAttach.FindAttachment(challenger, typeof(XmlPoints));

            this.Movable = false;

            this.Hue = 33;

            if (challenger == null || afrom == null || afrom.Deleted)
            {
                this.Delete();
            }
            else
            {
                this.Name = XmlPoints.SystemText(100400) + " " + String.Format(XmlPoints.SystemText(100315), challenger.Name); // "Challenge by {0}"
            }
        }

        public DeathmatchGauntlet(Serial serial)
            : base(serial)
        {
        }

        // how long before the gauntlet decays if a gauntlet is dropped but never started
        public override TimeSpan DecayTime
        {
            get
            {
                return TimeSpan.FromMinutes(15);
            }
        }// this will apply to the setup
        public override ArrayList Organizers
        {
            get
            {
                return this.m_Organizers;
            }
        }
        public override bool AllowPoints
        {
            get
            {
                return false;
            }
        }// determines whether kills during the game will award points.  If this is false, UseKillDelay is ignored
        public override bool UseKillDelay
        {
            get
            {
                return true;
            }
        }// determines whether the normal delay between kills of the same player for points is enforced
        public bool AutoRes
        {
            get
            {
                return true;
            }
        }// determines whether players auto res after being killed
        public bool AllowOnlyInChallengeRegions
        {
            get
            {
                return false;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan MatchLength
        {
            get
            {
                return this.m_MatchLength;
            }
            set
            {
                this.m_MatchLength = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime MatchStart
        {
            get
            {
                return this.m_MatchStart;
            }
            set
            {
                this.m_MatchStart = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public override Mobile Challenger
        {
            get
            {
                return this.m_Challenger;
            }
            set
            {
                this.m_Challenger = value;
            }
        }
        public override bool GameInProgress
        {
            get
            {
                return this.m_GameInProgress;
            }
            set
            {
                this.m_GameInProgress = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public override bool GameCompleted
        {
            get
            {
                return !this.m_GameInProgress && this.m_GameLocked;
            }
        }
        public override bool GameLocked
        {
            get
            {
                return this.m_GameLocked;
            }
            set
            {
                this.m_GameLocked = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public override int ArenaSize
        {
            get
            {
                return this.m_ArenaSize;
            }
            set
            {
                this.m_ArenaSize = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int TargetScore
        {
            get
            {
                return this.m_TargetScore;
            }
            set
            {
                this.m_TargetScore = value;
            }
        }
        public override ArrayList Participants
        {
            get
            {
                return this.m_Participants;
            }
            set
            {
                this.m_Participants = value;
            }
        }
        public override int TotalPurse
        {
            get
            {
                return this.m_TotalPurse;
            }
            set
            {
                this.m_TotalPurse = value;
            }
        }
        public override int EntryFee
        {
            get
            {
                return this.m_EntryFee;
            }
            set
            {
                this.m_EntryFee = value;
            }
        }
        public override bool InsuranceIsFree(Mobile from, Mobile awardto)
        {
            return true;
        }

        public override void OnTick()
        {
            this.CheckForDisqualification();

            if (this.MatchLength > TimeSpan.Zero && DateTime.UtcNow >= this.MatchStart + this.MatchLength)
            {
                this.CheckForGameEnd();
            }
            else if (this.MatchLength > TimeSpan.Zero && DateTime.UtcNow >= this.MatchStart + this.MatchLength - TimeSpan.FromSeconds(10))
            {
                this.GameBroadcast((this.MatchStart + this.MatchLength - DateTime.UtcNow).ToString());
            }
        }

        public override void StartGame()
        {
            base.StartGame();
            
            this.MatchStart = DateTime.UtcNow;
        }

        public void CheckForDisqualification()
        {
            if (this.Participants == null || !this.GameInProgress)
                return;
            
            bool statuschange = false;

            foreach (ChallengeEntry entry in this.Participants)
            {
                if (entry.Participant == null || entry.Status != ChallengeStatus.Active)
                    continue;

                bool hadcaution = (entry.Caution != ChallengeStatus.None);

                // and a map check
                if (entry.Participant.Map != this.Map)
                {
                    // check to see if they are offline
                    if (entry.Participant.Map == Map.Internal)
                    {
                        // then give them a little time to return before disqualification
                        if (entry.Caution == ChallengeStatus.Offline)
                        {
                            // were previously out of bounds so check for disqualification
                            // check to see how long they have been out of bounds
                            if (DateTime.UtcNow - entry.LastCaution > MaximumOfflineDuration)
                            {
                                // penalize them
                                this.SubtractScore(entry);
                                entry.LastCaution = DateTime.UtcNow;
                            }
                        }
                        else
                        {
                            entry.LastCaution = DateTime.UtcNow;
                            statuschange = true;
                        }
    
                        entry.Caution = ChallengeStatus.Offline;
                    }
                    else
                    {
                        // changing to any other map results in instant
                        // teleport back to the gauntlet
                        // and point loss
                        this.RespawnWithPenalty(entry);
                        entry.Caution = ChallengeStatus.None;
                    }
                }
                else if (this.m_ArenaSize > 0 && !Utility.InRange(entry.Participant.Location, this.Location, this.m_ArenaSize) ||
                         (this.IsInChallengeGameRegion && !(Region.Find(entry.Participant.Location, entry.Participant.Map) is ChallengeGameRegion)))
                {
                    if (entry.Caution == ChallengeStatus.OutOfBounds)
                    {
                        // were previously out of bounds so check for disqualification
                        // check to see how long they have been out of bounds
                        if (DateTime.UtcNow - entry.LastCaution > MaximumOutOfBoundsDuration)
                        {
                            // teleport them back to the gauntlet
                            this.RespawnWithPenalty(entry);
                            this.GameBroadcast(100401, entry.Participant.Name);  // "{0} was penalized."
                            entry.Caution = ChallengeStatus.None;
                            statuschange = true;
                        }
                    }
                    else
                    {
                        entry.LastCaution = DateTime.UtcNow;
                        // inform the player
                        XmlPoints.SendText(entry.Participant, 100309, MaximumOutOfBoundsDuration.TotalSeconds);  // "You are out of bounds!  You have {0} seconds to return"
                        statuschange = true;
                    }

                    entry.Caution = ChallengeStatus.OutOfBounds;
                }
                else if (entry.Participant.Hidden)
                {
                    if (entry.Caution == ChallengeStatus.Hidden)
                    {
                        // were previously hidden so check for disqualification
                        // check to see how long they have hidden
                        if (DateTime.UtcNow - entry.LastCaution > MaximumHiddenDuration)
                        {
                            // penalize them
                            this.SubtractScore(entry);
                            entry.Participant.Hidden = false;
                            this.GameBroadcast(100401, entry.Participant.Name);  // "{0} was penalized."
                            entry.Caution = ChallengeStatus.None;
                            statuschange = true;
                        }
                    }
                    else
                    {
                        entry.LastCaution = DateTime.UtcNow;
                        // inform the player
                        XmlPoints.SendText(entry.Participant, 100310, MaximumHiddenDuration.TotalSeconds); // "You have {0} seconds become unhidden"
                        statuschange = true;
                    }

                    entry.Caution = ChallengeStatus.Hidden;
                }
                else
                {
                    entry.Caution = ChallengeStatus.None;
                }

                if (hadcaution && entry.Caution == ChallengeStatus.None)
                    statuschange = true;
            }
            
            if (statuschange)
            {
                // update gumps with the new status
                DeathmatchGump.RefreshAllGumps(this, false);
            }

            // it is possible that the game could end like this so check
            this.CheckForGameEnd();
        }

        public override void CheckForGameEnd()
        {
            if (this.Participants == null || !this.GameInProgress)
                return;

            ArrayList winner = new ArrayList();

            int leftstanding = 0;
            IChallengeEntry lastentry = null;
            int maxscore = -99999;

            // has anyone reached the target score
            foreach (IChallengeEntry entry in this.Participants)
            {
                if (entry.Status == ChallengeStatus.Active)
                {
                    if (this.TargetScore > 0 && entry.Score >= this.TargetScore)
                    {
                        winner.Add(entry);
                        entry.Winner = true;
                    }
                    
                    if (entry.Score >= maxscore)
                    {
                        maxscore = entry.Score;
                    }
                    leftstanding++;
                    lastentry = entry;
                }
            }
            
            // if only one is left then they are the winner
            if (leftstanding == 1 && winner.Count == 0)
            {
                winner.Add(lastentry);
                lastentry.Winner = true;
            }
                
            if (winner.Count == 0 && this.MatchLength > TimeSpan.Zero && (DateTime.UtcNow >= this.MatchStart + this.MatchLength))
            {
                // find the highest score
                // has anyone reached the target score
                foreach (IChallengeEntry entry in this.Participants)
                {
                    if (entry.Status == ChallengeStatus.Active)
                    {
                        if (entry.Score >= maxscore)
                        {
                            winner.Add(entry);
                            entry.Winner = true;
                        }
                    }
                }
            }

            // and then check to see if this is the Deathmatch
            if (winner.Count > 0)
            {
                // declare the winner(s) and end the game
                foreach (IChallengeEntry entry in winner)
                {
                    if (entry.Participant != null)
                    {
                        XmlPoints.SendText(entry.Participant, 100311, this.ChallengeName);  // "You have won {0}"
                        this.GameBroadcast(100312, entry.Participant.Name); // "The winner is {0}"
                        this.AwardWinnings(entry.Participant, this.TotalPurse / winner.Count);
                    }
                }

                this.EndGame();
                DeathmatchGump.RefreshAllGumps(this, true);
            }
        }

        public void SubtractScore(ChallengeEntry entry)
        {
            if (entry == null)
                return;
            
            entry.Score--;

            // refresh the gumps
            DeathmatchGump.RefreshAllGumps(this, false);
        }

        public void AddScore(ChallengeEntry entry)
        {
            if (entry == null)
                return;
            
            entry.Score++;
            
            // refresh the gumps
            DeathmatchGump.RefreshAllGumps(this, false);
        }

        public void RespawnWithPenalty(ChallengeEntry entry)
        {
            if (entry == null)
                return;
            
            this.SubtractScore(entry);
            
            // move the participant to the gauntlet
            if (entry.Participant != null)
            {
                entry.Participant.MoveToWorld(this.Location, this.Map);
                entry.Participant.PlaySound(0x214);
                entry.Participant.FixedEffect(0x376A, 10, 16);
                this.GameBroadcast(100401, entry.Participant.Name);  // "{0} was penalized."
            }
        }

        public override void OnPlayerKilled(Mobile killer, Mobile killed)
        {
            if (killed == null)
                return;

            if (this.AutoRes)
            {
                // prepare the autores callback
                Timer.DelayCall(RespawnTime, new TimerStateCallback(XmlPoints.AutoRes_Callback),
                    new object[] { killed, true });
            }

            // add 15 seconds of res kill protection
            //XmlAttach.AttachTo(killed, new XmlBless(15.0));

            // find the player in the participants list and announce it
            if (this.m_Participants != null)
            {
                foreach (ChallengeEntry entry in this.m_Participants)
                {
                    if (entry.Status == ChallengeStatus.Active && entry.Participant == killed)
                    {
                        this.GameBroadcast(100314, killed.Name); // "{0} has been killed"
                        this.SubtractScore(entry);
                    }
                }
            }

            // see if the game is over
            this.CheckForGameEnd();
        }

        public override void OnKillPlayer(Mobile killer, Mobile killed)
        {
            if (killer == null)
                return;

            // find the player in the participants list and announce it
            if (this.m_Participants != null)
            {
                foreach (ChallengeEntry entry in this.m_Participants)
                {
                    if (entry.Status == ChallengeStatus.Active && entry.Participant == killer)
                    {
                        this.AddScore(entry);
                    }
                }
            }
        }

        public override bool AreTeamMembers(Mobile from, Mobile target)
        {
            // there are no teams, its every man for himself
            if (from == target)
                return true;
            return false;
        }

        public override bool AreChallengers(Mobile from, Mobile target)
        {
            // everyone participant is a challenger to everyone other participant, so just being a participant
            // makes you a challenger
            return(this.AreInGame(from) && this.AreInGame(target));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(this.m_Challenger);
            writer.Write(this.m_GameLocked);
            writer.Write(this.m_GameInProgress);
            writer.Write(this.m_TotalPurse);
            writer.Write(this.m_EntryFee);
            writer.Write(this.m_ArenaSize);
            writer.Write(this.m_TargetScore);
            writer.Write(this.m_MatchLength);
            
            if (this.GameTimer != null && this.GameTimer.Running)
            {
                writer.Write(DateTime.UtcNow - this.m_MatchStart);
            }
            else
            {
                writer.Write(TimeSpan.Zero);
            }

            if (this.Participants != null)
            {
                writer.Write(this.Participants.Count);

                foreach (ChallengeEntry entry in this.Participants)
                {
                    writer.Write(entry.Participant);
                    writer.Write(entry.Status.ToString());
                    writer.Write(entry.Accepted);
                    writer.Write(entry.PageBeingViewed);
                    writer.Write(entry.Score);
                    writer.Write(entry.Winner);
                }
            }
            else
            {
                writer.Write((int)0);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch(version)
            {
                case 0:
                    this.m_Challenger = reader.ReadMobile();
                
                    this.m_Organizers.Add(this.m_Challenger);

                    this.m_GameLocked = reader.ReadBool();
                    this.m_GameInProgress = reader.ReadBool();
                    this.m_TotalPurse = reader.ReadInt();
                    this.m_EntryFee = reader.ReadInt();
                    this.m_ArenaSize = reader.ReadInt();
                    this.m_TargetScore = reader.ReadInt();
                    this.m_MatchLength = reader.ReadTimeSpan();
                
                    TimeSpan elapsed = reader.ReadTimeSpan();
                
                    if (elapsed > TimeSpan.Zero)
                    {
                        this.m_MatchStart = DateTime.UtcNow - elapsed;
                    }
                
                    int count = reader.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        ChallengeEntry entry = new ChallengeEntry();
                        entry.Participant = reader.ReadMobile();
                        string sname = reader.ReadString();
                        // look up the enum by name
                        ChallengeStatus status = ChallengeStatus.None;
                        try
                        {
                            status = (ChallengeStatus)Enum.Parse(typeof(ChallengeStatus), sname);
                        }
                        catch
                        {
                        }
                        entry.Status = status;
                        entry.Accepted = reader.ReadBool();
                        entry.PageBeingViewed = reader.ReadInt();
                        entry.Score = reader.ReadInt();
                        entry.Winner = reader.ReadBool();
                    
                        this.Participants.Add(entry);
                    }
                    break;
            }
            
            if (this.GameCompleted)
                Timer.DelayCall(this.PostGameDecayTime, new TimerCallback(Delete));
            
            // start the challenge timer
            this.StartChallengeTimer();
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new DeathmatchGump(this, from));
        }

        public class ChallengeEntry : BaseChallengeEntry
        {
            public ChallengeEntry(Mobile m, int team)
                : base(m)
            {
                this.Team = team;
            }

            public ChallengeEntry(Mobile m)
                : base(m)
            {
            }

            public ChallengeEntry()
                : base()
            {
            }
        }
    }
}
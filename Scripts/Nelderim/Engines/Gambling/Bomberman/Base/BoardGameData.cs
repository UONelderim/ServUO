using System;
using System.IO;
using System.Collections.Generic;
using Server;

namespace Solaris.BoardGames
{
    /// <summary>
    /// Data class used to keep track of player scores for various board games,
    /// and handles global save/load to disk.
    /// </summary>
    public class BoardGameData
    {
        /// <summary>
        /// The sub-folder under ServUO’s Saves where Nelderim persists data.
        /// </summary>
        public const string BaseFolder = "Save/BoardGame Data";

        /// <summary>
        /// Full path to where BoardGameData lives (e.g. ".../Saves/Nelderim/BoardGame Data").
        /// </summary>
        public static string SAVE_PATH
        {
            get
            {
                // World.SavePath is ServUO's configured ".../Saves" folder.
                return Path.Combine("Saves", "BoardGame Data");
            }
        }

        /// <summary>
        /// Name of the binary file storing all board-game data.
        /// </summary>
        public const string FILENAME = "boardgames.bin";

        protected static List<BoardGameData> _GameData;
        /// <summary>
        /// In-memory list of all loaded board-game data.
        /// </summary>
        public static List<BoardGameData> GameData
        {
            get { return _GameData ?? (_GameData = new List<BoardGameData>()); }
        }

        protected string _GameName;
        protected List<BoardGamePlayerScore> _Scores;

        /// <summary>
        /// Name of the board game (e.g. "Chess").
        /// </summary>
        public string GameName { get { return _GameName; } }

        /// <summary>
        /// List of player scores for this game.
        /// </summary>
        public List<BoardGamePlayerScore> Scores
        {
            get { return _Scores ?? (_Scores = new List<BoardGamePlayerScore>()); }
        }

        /// <summary>
        /// Creates new game data for a given game name.
        /// </summary>
        protected BoardGameData(string gamename)
        {
            _GameName = gamename;
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected BoardGameData(GenericReader reader)
        {
            Deserialize(reader);
        }

        /// <summary>
        /// Serialize this game’s data (version, name, then per-player scores).
        /// </summary>
        protected virtual void Serialize(GenericWriter writer)
        {
            writer.Write(0);                   // version
            writer.Write(_GameName);
            writer.Write(Scores.Count);

            foreach (var score in Scores)
                score.Serialize(writer);
        }

        /// <summary>
        /// Deserialize this game’s data (must match Serialize).
        /// </summary>
        protected virtual void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            _GameName = reader.ReadString();

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                var playerscore = new BoardGamePlayerScore(reader);
                if (playerscore.Player != null && !playerscore.Player.Deleted)
                    Scores.Add(playerscore);
            }
        }

        protected static BoardGamePlayerScore GetScoreData(string gamename, Mobile player)
        {
            var scores = GetScores(gamename);
            if (scores == null)
            {
                var gamedata = new BoardGameData(gamename);
                GameData.Add(gamedata);
                scores = gamedata.Scores;
            }

            int index = BoardGamePlayerScore.IndexOf(scores, player);
            if (index == -1)
            {
                var newscore = new BoardGamePlayerScore(player);
                scores.Add(newscore);
                return newscore;
            }

            return scores[index];
        }

        public static List<BoardGamePlayerScore> GetScores(string gamename)
        {
            int gameindex = IndexOf(gamename);
            return gameindex == -1 ? null : GameData[gameindex].Scores;
        }

        public static void SetScore(string gamename, Mobile player, int score)
        {
            var scoredata = GetScoreData(gamename, player);
            if (scoredata != null)
                scoredata.Score = score;
        }

        public static int GetScore(string gamename, Mobile player)
        {
            var scoredata = GetScoreData(gamename, player);
            return scoredata != null ? scoredata.Score : 0;
        }

        public static void ChangeScore(string gamename, Mobile player, int delta)
        {
            SetScore(gamename, player, Math.Max(0, GetScore(gamename, player) + delta));
        }

        public static void AddWin(string gamename, Mobile player)
        {
            var playerscore = GetScoreData(gamename, player);
            playerscore.Wins += 1;
        }

        public static void AddLose(string gamename, Mobile player)
        {
            var playerscore = GetScoreData(gamename, player);
            playerscore.Losses += 1;
        }

        public static void ResetScores(string gamename)
        {
            int gameindex = IndexOf(gamename);
            if (gameindex > -1)
                GameData.RemoveAt(gameindex);
        }

        public static int IndexOf(string gamename)
        {
            for (int i = 0; i < GameData.Count; i++)
                if (GameData[i].GameName == gamename)
                    return i;
            return -1;
        }

        /// <summary>
        /// Hooks into WorldLoad and WorldSave to persist game data.
        /// </summary>
        public static void Configure()
        {
            EventSink.WorldLoad += new WorldLoadEventHandler(OnLoad);
            EventSink.WorldSave += new WorldSaveEventHandler(OnSave);
        }

        /// <summary>
        /// Serialize all board-game stats, overwriting any prior file.
        /// </summary>
        private static void OnSave(WorldSaveEventArgs e)
        {
            if (!Directory.Exists(SAVE_PATH))
                Directory.CreateDirectory(SAVE_PATH);

            var filePath = Path.Combine(SAVE_PATH, FILENAME);

            // Overwrite (append = false) to avoid corrupting the file
            var writer = new BinaryFileWriter(filePath, false);
            writer.Write(0);                  // file version
            writer.Write(GameData.Count);     // how many games
            foreach (var data in GameData)
	            data.Serialize(writer);
        }

        /// <summary>
        /// Read back what we wrote; if missing, do nothing.
        /// </summary>
        private static void OnLoad()
        {
            var filePath = Path.Combine(SAVE_PATH, FILENAME);
            if (!File.Exists(filePath))
                return;

            // Clear existing in-memory data (e.g. on script reload)
            GameData.Clear();

            using (var bin = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
	            var reader = new BinaryFileReader(new BinaryReader(bin));
	            int version = reader.ReadInt(); // should be 0
	            int count = reader.ReadInt(); // number of games

	            for (int i = 0; i < count; i++)
		            GameData.Add(new BoardGameData(reader));

	            reader.End();
            }
        }
    }

    /// <summary>
    /// Stores a single player's score, wins, and losses for a board game.
    /// </summary>
    public class BoardGamePlayerScore : IComparable
    {
        protected Mobile _Player;
        public Mobile Player { get { return _Player; } }

        public int Score;
        public int Wins;
        public int Losses;

        public BoardGamePlayerScore(Mobile player) : this(player, 0) { }

        public BoardGamePlayerScore(Mobile player, int score)
        {
            _Player = player;
            Score = score;
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        public BoardGamePlayerScore(GenericReader reader)
        {
            Deserialize(reader);
        }

        /// <summary>
        /// Sorts descending by Score.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (!(obj is BoardGamePlayerScore))
                return 0;

            var comparescore = (BoardGamePlayerScore)obj;
            return -Score.CompareTo(comparescore.Score);
        }

        /// <summary>
        /// Serialize this player's data.
        /// </summary>
        public virtual void Serialize(GenericWriter writer)
        {
            writer.Write(0);         // version
            writer.Write(_Player);
            writer.Write(Score);
            writer.Write(Wins);
            writer.Write(Losses);
        }

        /// <summary>
        /// Deserialize this player's data.
        /// </summary>
        public virtual void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            _Player = reader.ReadMobile();
            Score = reader.ReadInt();
            Wins = reader.ReadInt();
            Losses = reader.ReadInt();
        }

        /// <summary>
        /// Finds the index of a given player in the list (or –1 if not found).
        /// </summary>
        public static int IndexOf(List<BoardGamePlayerScore> scores, Mobile player)
        {
            if (scores == null)
                return -1;

            for (int i = 0; i < scores.Count; i++)
                if (scores[i].Player == player)
                    return i;

            return -1;
        }
    }
}

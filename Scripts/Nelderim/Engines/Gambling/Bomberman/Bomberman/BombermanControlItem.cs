using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Gumps; 
using Server.Network;
using Solaris.BoardGames;

namespace Server.Items
{
	//a test of the boardgame system
	public class BombermanControlItem : BoardGameControlItem
	{
		public override string GameName { get { return "Wybuchowiec"; } }
		public override string GameDescription { get { return "Wybuchaj ściany i graczy bombami. Zbieraj ulepszenia, aby zwiększyć liczbę bomb, siłę wybuchu itp."; } }
		public override string GameRules
		{ 
			get
			{ 
				return 
					"Każda gra może mieć do ośmiu graczy, a każdy zaczyna w rogu lub przy krawędzi areny. " +
					"W plecakach graczy umieszczona jest torba z bombami. Gracze używają tej torby, aby umieszczać bomby u swoich stóp. " +
					"Bomba wybuchnie po pewnym czasie i ma ograniczony zasięg wybuchu. Liczba bomb, które gracz może umieścić w dowolnym momencie, jest ograniczona.<BR><BR>" +
					
					"Gracze muszą wysadzać niszczalne ściany, aby poruszać się po arenie. " +
					"Podczas wysadzania można znaleźć ulepszenia, które zwiększają zasięg wybuchu lub liczbę bomb, które gracz może umieścić jednocześnie. " +
					"Istnieje także ulepszenie detonatora, które pozwala graczowi wybrać moment wybuchu bomb. " +
					"Uważaj na wybuchy innych graczy! Bomba może spowodować wybuch kolejnej bomby, tworząc efekt łańcuchowej reakcji!<BR><BR>" +
					
					"Gra kończy się, gdy zostanie tylko jeden gracz.";
			}
		}
		
		public override bool CanCastSpells { get { return false; } }
		public override bool CanUseSkills { get { return false; } }
		public override bool CanUsePets   { get { return false; } }
		
		public override TimeSpan WinDelay { get { return TimeSpan.FromSeconds(5); } }

		//bomberman main controller must be accessed from ground
		public override bool UseFromBackpack { get { return false; } }

		//only 1 to 8 players allowed in a bomberman game
		public override int MinPlayers { get { return 2; } }
		public override int MaxPlayers { get { return 8; } }
		
		protected BombermanStyle _Style;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public BombermanStyle Style
		{
			get { return _Style; }
			set
			{
				if ((int)_State <= (int)BoardGameState.Recruiting)
				{
					_Style = value;
					ResetBoard();
				}
			}
		}
		
		//reference to the bomb bags that are handed out for the game
		protected List<BombBag> _BombBags;
		
		public List<BombBag> BombBags
		{
			get
			{
				if (_BombBags == null)
				{
					_BombBags = new List<BombBag>();
				}
				return _BombBags;
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public override int BoardWidth
		{
			get
			{
				_BoardWidth = Math.Max(BombermanSettings.MIN_BOARD_SIZE, Math.Min(BombermanSettings.MAX_BOARD_SIZE, _BoardWidth));
				return _BoardWidth;
			}
			set
			{
				if ((int)_State <= (int)BoardGameState.Recruiting)
				{
					_BoardWidth = Math.Max(BombermanSettings.MIN_BOARD_SIZE, Math.Min(BombermanSettings.MAX_BOARD_SIZE, value));
					
					if ((_BoardWidth & 1) == 0)
					{
						_BoardWidth += 1;
					}
					ResetBoard();
				}
			}
		}
			
		[CommandProperty( AccessLevel.GameMaster )]
		public override int BoardHeight
		{
			get
			{
				_BoardHeight = Math.Max(BombermanSettings.MIN_BOARD_SIZE, Math.Min(BombermanSettings.MAX_BOARD_SIZE, _BoardHeight));
				return _BoardHeight;
			}
			set
			{
				if ((int)_State <= (int)BoardGameState.Recruiting)
				{
					_BoardHeight = Math.Max(BombermanSettings.MIN_BOARD_SIZE, Math.Min(BombermanSettings.MAX_BOARD_SIZE, value));
				
					if ((_BoardHeight & 1) == 0)
					{
						_BoardHeight += 1;
					}
					ResetBoard();
				}
			}
		}
		
		protected int  _DefaultMaxBombs       = 2;
		protected int  _DefaultBombStrength   = 1;
		protected bool _DefaultDetonatorMode  = false;
		protected bool _DefaultBaddaBoom      = false;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int DefaultMaxBombs
		{
			get { return _DefaultMaxBombs; }
			set { _DefaultMaxBombs = Math.Max(1, value); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int DefaultBombStrength
		{
			get { return _DefaultBombStrength; }
			set { _DefaultBombStrength = Math.Max(1, value); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool DefaultDetonatorMode
		{
			get { return _DefaultDetonatorMode; }
			set { _DefaultDetonatorMode = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool DefaultBaddaBoom
		{
			get { return _DefaultBaddaBoom; }
			set { _DefaultBaddaBoom = value; }
		}
		
		//main constructor
		
		[Constructable]
		public BombermanControlItem()
		{
			ItemID = 0xED4;			//guild gravestone
			Name   = "Kontroler gry Bomberman";
			
			BoardWidth  = BombermanSettings.DEFAULT_BOARD_SIZE;
			BoardHeight = BombermanSettings.DEFAULT_BOARD_SIZE;
			
			_State       = BoardGameState.Inactive;
			_BoardOffset = new Point3D(2, 2, 0);
		}
		
		//deserialization constructor
		public BombermanControlItem( Serial serial ) : base(serial)
		{
		}
		
		//this method initializes the game control and connects it with this item
		protected override void InitializeControl()
		{
			base.InitializeControl();
		}
		
		public override void UpdatePosition()
		{
			GameZone = new Rectangle3D(
				new Point3D( X + BoardOffset.X, Y + BoardOffset.X, BoardOffset.Z - 100 ),
				new Point3D( X + BoardOffset.X + BoardWidth, Y + BoardOffset.Y + BoardHeight, BoardOffset.Z + 100 )
			);
			
			base.UpdatePosition();
		}
		
		public override void AddPlayer( Mobile m )
		{
			base.AddPlayer(m);
			
			PublicOverheadMessage( MessageType.Regular, 1153, false, "Dodano " + m.Name + " do gry!" );
			
			//if this is the first player to be added, they can also choose the board style
			if (Players.Count == 1 && _AllowPlayerConfiguration)
			{
				PublicOverheadMessage( MessageType.Regular, 1153, false, Players[0].Name + " jest teraz odpowiedzialny za tę grę!" );
				m.SendGump( new SelectBombermanStyleGump(m, this) );
			}
			
			if (Players.Count < CurrentMaxPlayers)
			{
				int requiredplayers = CurrentMaxPlayers - Players.Count;
				PublicOverheadMessage( MessageType.Regular, 1153, false, "Potrzebnych jeszcze " + requiredplayers.ToString() + " graczy!" );
			}
			else
			{
				PublicOverheadMessage( MessageType.Regular, 1153, false, Players[0].Name + " musi potwierdzić styl!" );
			}
		}
		
		public override void RemovePlayer( Mobile m )
		{
			if (Players.IndexOf(m) > -1)
			{
				PublicOverheadMessage( MessageType.Regular, 1153, false, "Usunięto " + m.Name + " z gry!" );
			}
			
			if (Players.IndexOf(m) == 0 && Players.Count > 1)
			{
				PublicOverheadMessage( MessageType.Regular, 1153, false, Players[1].Name + " jest teraz odpowiedzialny za tę grę!" );
				SettingsReady = false;
				
				Players[1].SendGump( new SelectBombermanStyleGump(Players[1], this) );
				Players[1].SendMessage( "Teraz jesteś odpowiedzialny za konfigurację gry!" );
			}
			
			base.RemovePlayer(m);
			
			if (Players.Count > 0)
			{
				int requiredplayers = CurrentMaxPlayers - Players.Count;
				PublicOverheadMessage( MessageType.Regular, 1153, false, "Potrzebnych jeszcze " + requiredplayers.ToString() + " graczy!" );
			}
			else
			{
				PublicOverheadMessage( MessageType.Regular, 1153, false, "Brak graczy... resetowanie." );
			}
		}
		
		public override void BuildBoard()
		{
			UpdatePosition();
			
			for (int i = 0; i < BoardWidth; i++)
			{
				for (int j = 0; j < BoardHeight; j++)
				{
					//build the ground
					BombermanFloorTile groundpiece = new BombermanFloorTile(_Style);
					groundpiece.RegisterToBoardGameControlItem(this, new Point3D(i, j, 0));
					BackgroundItems.Add(groundpiece);
					
					//build the outer walls and inner grid walls
					if (i == 0 || i == BoardWidth - 1 || j == 0 || j == BoardHeight - 1 || (j % 2 == 0 && i % 2 == 0))
					{
						IndestructableWall wallpiece = new IndestructableWall(_Style, true);
						wallpiece.RegisterToBoardGameControlItem(this, new Point3D(i, j, 0));
						BackgroundItems.Add(wallpiece);
					}
					else 
					{
						if (_State == BoardGameState.Active)  //if a game is active, then build obstacles and such
						{
							//don't put obstacles in the player starting positions
							if ((i < 3 && j < 3) || (i > BoardWidth - 4 && j < 3) || (i < 3 && j > BoardHeight - 4) || (i > BoardWidth - 4 && j > BoardHeight - 4))
							{
								continue;
							}
							else if (j > BoardHeight / 2 - 2 && j < BoardHeight / 2 + 2 && (i < 3 || i > BoardWidth - 4))
							{
								continue;
							}
							else if (i > BoardWidth / 2 - 2 && i < BoardWidth / 2 + 2 && (j < 3 || j > BoardHeight - 4))
							{
								continue;
							}
							
							//obstacles
							if (Utility.RandomDouble() < BombermanSettings.OBSTACLE_CHANCE)
							{
								DestructableWall wallpiece = new DestructableWall(_Style);
								wallpiece.RegisterToBoardGameControlItem(this, new Point3D(i, j, 0));
								BackgroundItems.Add(wallpiece);
							}
						}
					}
				}
			}
			
			base.BuildBoard();
		}
		
		protected override void PrimePlayers()
		{
			base.PrimePlayers();
			
			for (int i = 0; i < Players.Count; i++)
			{
				Mobile player = Players[i];
				Point3D movepoint;
				
				switch (i)
				{
					case 0: movepoint = new Point3D(X + BoardOffset.X + 1, Y + BoardOffset.Y + 1, Z + BoardOffset.Z); break;
					case 1: movepoint = new Point3D(X + BoardOffset.X + BoardWidth - 2, Y + BoardOffset.Y + 1, Z + BoardOffset.Z); break;
					case 2: movepoint = new Point3D(X + BoardOffset.X + 1, Y + BoardOffset.Y + BoardHeight - 2, Z + BoardOffset.Z); break;
					case 3: movepoint = new Point3D(X + BoardOffset.X + BoardWidth - 2, Y + BoardOffset.Y + BoardHeight - 2, Z + BoardOffset.Z); break;
					case 4: movepoint = new Point3D(X + BoardOffset.X + BoardWidth / 2, Y + BoardOffset.Y + 1, Z + BoardOffset.Z); break;
					case 5: movepoint = new Point3D(X + BoardOffset.X + BoardWidth - 2, Y + BoardOffset.Y + BoardHeight / 2, Z + BoardOffset.Z); break;
					case 6: movepoint = new Point3D(X + BoardOffset.X + BoardWidth / 2, Y + BoardOffset.Y + BoardHeight - 2, Z + BoardOffset.Z); break;
					case 7:
					default: movepoint = new Point3D(X + BoardOffset.X + 1, Y + BoardOffset.Y + BoardHeight / 2, Z + BoardOffset.Z); break;
				}
				
				player.MoveToWorld(movepoint, BoardMap);
				
				BombBag bag = new BombBag(this, _DefaultMaxBombs, _DefaultBombStrength);
				BombBags.Add(bag);
				bag.Owner = player;
				player.Backpack.DropItem(bag);
				
				if (_DefaultDetonatorMode)
				{
					BombDetonator detonator = new BombDetonator(bag);
					bag.Detonator = detonator;
					player.Backpack.DropItem(detonator);
				}
				
				bag.BaddaBoom = _DefaultBaddaBoom;
			}
		}
		
		public void CheckForMobileVictims(Point3D location, Map map, BombBag sourcebag)
		{
			IPooledEnumerable ie = map.GetMobilesInRange(location, 0);
			List<Mobile> tomove = new List<Mobile>();
			
			foreach (Mobile m in ie)
			{
				if (Players.IndexOf(m) > -1)
				{
					if (m != sourcebag.Owner)
					{
						m.SendMessage("Zostałeś zniszczony przez wybuch " + sourcebag.Owner.Name + "!");
						sourcebag.Owner.SendMessage("Wysadziłeś " + m.Name + "!");
						BoardGameData.ChangeScore(GameName, sourcebag.Owner, BombermanSettings.KILL_SCORE);
						BoardGameData.ChangeScore(GameName, m, BombermanSettings.DEATH_SCORE);
						PublicOverheadMessage(MessageType.Regular, 1153, false, sourcebag.Owner.Name + " wysadził " + m.Name + "!");
					}
					else
					{
						m.SendMessage("Właśnie wysadziłeś się!");
						PublicOverheadMessage(MessageType.Regular, 1153, false, m.Name + " właśnie się wysadził!");
						BoardGameData.ChangeScore(GameName, m, BombermanSettings.SUICIDE_SCORE);
					}
					
					BoardGameData.AddLose(GameName, m);
					m.PlaySound(m.Female ? 0x32E : 0x549); // 0x54A - yelp1 
					tomove.Add(m);
				}
			}
			ie.Free();
			
			foreach (Mobile m in tomove)
			{
				m.MoveToWorld(new Point3D(X - 1, Y - 1, Z), Map);
				m.SendGump(new BoardGameLostGump(m, this));
				Players.Remove(m);
				
				BombBag bag = (BombBag)m.Backpack.FindItemByType(typeof(BombBag));
				if (bag != null)
					bag.Active = false;
			}
			
			//start the timer to check for endgame, delay for 1s
			StartEndGameTimer(TimeSpan.FromSeconds(1));
		}
		
		//TODO: move into base group?
		protected override void OnEndGameTimer()
		{
			base.OnEndGameTimer();
			
			if (Players.Count < 2)
			{
				AnnounceWinner();
			}
		}
		
		protected override void AnnounceWinner()
		{
			base.AnnounceWinner();
			
			if (Players.Count == 1)
			{
				Players[0].SendGump(new BoardGameWonGump(Players[0], this));
				BoardGameData.ChangeScore(GameName, Players[0], BombermanSettings.WIN_SCORE);
				BoardGameData.AddWin(GameName, Players[0]);
				
				BombBag bag = (BombBag)Players[0].Backpack.FindItemByType(typeof(BombBag));
				if (bag != null)
					bag.Active = false;
				
				PublicOverheadMessage(MessageType.Regular, 1153, false, Players[0].Name + " wygrał grę!");
			}
			else
			{
				PublicOverheadMessage(MessageType.Regular, 1153, false, "Remis!");
			}
		}
		
		public override void EndGame()
		{
			base.EndGame();
			
			if (Map != null)
			{
				IPooledEnumerable ie = Map.GetItemsInBounds(
					new Rectangle2D(
						new Point2D(GameZone.Start.X, GameZone.Start.Y),
						new Point2D(GameZone.End.X,   GameZone.End.Y)
					)
				);
			
				List<BombermanUpgrade> todelete = new List<BombermanUpgrade>();
				foreach (Item item in ie)
				{
					if (item is BombermanUpgrade)
						todelete.Add((BombermanUpgrade)item);
				}
				ie.Free();
				
				foreach (BombermanUpgrade item in todelete)
					item.Destroy();
				
				//there should only be one left.. the winner
				foreach (Mobile player in Players)
					player.MoveToWorld(new Point3D(X - 1, Y - 1, Z), Map);
			}
			
			foreach (BombBag bag in BombBags)
				if (bag != null)
					bag.Delete();
			_BombBags = null;
			
			//announce winner?
			if (Players.Count == 1)
				Players[0].SendMessage("Wygrałeś grę!");
			
			_Players = null;
			_State    = BoardGameState.Inactive;
			InvalidateProperties();
		}
		
		public override void OnAfterDelete()
		{
			base.OnAfterDelete();
			
			foreach (BombBag bag in BombBags)
				if (bag != null)
					bag.Delete();
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
			
			writer.Write((int)_Style);
			writer.Write(_DefaultMaxBombs);
			writer.Write(_DefaultBombStrength);
			writer.Write(_DefaultDetonatorMode);
			writer.Write(_DefaultBaddaBoom);
			
			writer.Write(BombBags.Count);
			foreach (BombBag bag in BombBags)
				writer.Write((Item)bag);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			
			_Style                 = (BombermanStyle)reader.ReadInt();
			_DefaultMaxBombs       = reader.ReadInt();
			_DefaultBombStrength   = reader.ReadInt();
			_DefaultDetonatorMode  = reader.ReadBool();
			_DefaultBaddaBoom      = reader.ReadBool();
			
			int count = reader.ReadInt();
			for (int i = 0; i < count; i++)
				BombBags.Add((BombBag)reader.ReadItem());
		}
	}
}


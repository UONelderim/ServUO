using System;
using Server;
using Server.Items;
using Server.Network;
using Solaris.BoardGames;

namespace Server.Gumps
{
    // oferuje nową grę graczowi
    public class OfferNewGameGump : BoardGameGump
    {
        protected bool _ControlNumberOfPlayers;
        
        public override int Height { get { return 500; } }
        public override int Width  { get { return 400; } }
        
        public OfferNewGameGump(Mobile owner, BoardGameControlItem controlitem, bool controlnumberofplayers)
            : base(owner, controlitem)
        {
            _ControlNumberOfPlayers = controlnumberofplayers;
            
            AddLabel(40, 20, 1152, "Gra:");
            AddLabel(140, 20, 1172, _ControlItem.GameName);
            
            AddLabel(40, 50, 1152, "Opis:");
            AddHtml(40, 70, 300, 100, _ControlItem.GameDescription, true, true);
            
            AddLabel(40, 180, 1152, "Zasady:");
            AddHtml(40, 200, 300, 150, _ControlItem.GameRules, true, true);

            if (_ControlItem.CostToPlay > 0)
            {
                AddLabel(40, 370, 1152, "Koszt gry:");
                AddLabel(240, 370, 1172, _ControlItem.CostToPlay.ToString() + " szt. złota");
            }
            
            if (_ControlItem.MaxPlayers != _ControlItem.MinPlayers)
            {
                AddLabel(40, 430, 1152, "Liczba graczy (" 
                    + _ControlItem.MinPlayers.ToString() + "-" 
                    + _ControlItem.MaxPlayers.ToString() + "):");
                
                if (_ControlNumberOfPlayers)
                {
                    AddLabel(60, 410, 1172, "Wybierz liczbę graczy");
                    AddTextField(240, 430, 30, 20, 0, _ControlItem.CurrentMaxPlayers.ToString());
                }
                else
                {
                    AddLabel(240, 430, 1152, _ControlItem.CurrentMaxPlayers.ToString());
                }
            }
            
            AddLabel(40, 470, 1152, "Zagrać w tę grę?");
            AddButton(200, 460, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);
            AddButton(300, 460, 0xF1, 0xF2, 0, GumpButtonType.Reply, 0);
        }
        
        protected override void DeterminePageLayout()
        {
        }
        
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int buttonid = info.ButtonID;
            
            // anulowanie
            if (buttonid == 0)
            {
                _ControlItem.RemovePlayer(_Owner);
                _Owner.SendMessage("Zrezygnowałeś z udziału w tej grze.");
            }
            else
            {
                if (_ControlItem.MaxPlayers != _ControlItem.MinPlayers && _ControlNumberOfPlayers)
                {
                    try
                    {
                        _ControlItem.CurrentMaxPlayers = Int32.Parse(GetTextField(info, 0));
                        if (_ControlItem.CurrentMaxPlayers > _ControlItem.MaxPlayers 
                         || _ControlItem.CurrentMaxPlayers < _ControlItem.MinPlayers)
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        _Owner.SendMessage("Nieprawidłowa liczba graczy. Spróbuj ponownie.");
                        _Owner.SendGump(new OfferNewGameGump(_Owner, _ControlItem, _ControlNumberOfPlayers));
                        return;
                    }
                }
                
                _Owner.SendMessage("Zapisałeś się do tej gry.");
                _ControlItem.AddPlayer(_Owner);
            }
        }
    }
}

using Server;
using System;
using System.Collections;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Regions;

namespace Solaris.BoardGames
{
    // region gry planszowej to niestandardowy region, który obsługuje zasady dla graczy w regionie (brak rzucania czarów, brak zwierząt, itp.)
    public class BoardGameRegion : BaseRegion
    {
        protected static int _RegionIndex = 0;
        protected BoardGameControlItem _BoardGameControlItem;
        
        public static string GetUniqueName(BoardGameControlItem controlitem)
        {
            return controlitem.GameName + " " + (_RegionIndex++).ToString();
        }
        
        public BoardGameControlItem BoardGameControlItem
        {
            get { return _BoardGameControlItem; }
        }
        
        public BoardGameRegion(BoardGameControlItem controlitem)
            : base(GetUniqueName(controlitem), controlitem.Map, 255, controlitem.GameZone)
        {
            _BoardGameControlItem = controlitem;
        }
        
        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }
        
        public override bool AllowSpawn()
        {
            return false;
        }
        
        public override bool CanUseStuckMenu(Mobile m)
        {
            return false;
        }
        
        public override bool OnDamage(Mobile m, ref int Damage)
        {
            return false;
        }
        
        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if (from.AccessLevel > AccessLevel.Player)
            {
                return true;
            }
            return _BoardGameControlItem.CanCastSpells;
        }
        
        public override void OnEnter(Mobile m)
        {
            m.SendMessage("Wchodzisz do gry planszowej");
            if (_BoardGameControlItem.Players.IndexOf(m) == -1 && m.AccessLevel == AccessLevel.Player)
            {
                m.SendMessage("Nie masz tu wstępu.");
                m.MoveToWorld(_BoardGameControlItem.Location, _BoardGameControlItem.Map);
            }
        }

        public override void OnExit(Mobile m)
        {
            m.SendMessage("Opuszczasz grę planszową");
        }
        
        public override bool OnSkillUse(Mobile m, int skill)
        {
            return _BoardGameControlItem.CanUseSkills;
        }
        
        public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
        {
            if (m is BaseCreature && !_BoardGameControlItem.CanUsePets)
            {
                return false;
            }
            
            if (_BoardGameControlItem.Players.IndexOf(m) == -1)
            {
                if (!(m is PlayerMobile) || m.AccessLevel > AccessLevel.Player)
                {
                    return true;
                }
                return false;
            }
        
            return true;
        }
        
        public override bool OnDoubleClick(Mobile m, object o)
        {
            //TODO: umieścić tutaj kontrolę pomniejszonych zwierząt
            return base.OnDoubleClick(m, o);
        }
    }
}

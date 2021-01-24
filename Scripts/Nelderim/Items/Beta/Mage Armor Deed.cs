using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
[TypeAlias( "Server.Items.MageArmorScroll" )]
public class MageArmorScroll : Item
{
public override int LabelNumber{ get{ return 1060437; } } 

[Constructable]
public MageArmorScroll() : base( 0x14F0 )
{ 
Weight = 1.0;
Name = "zwoj magicznej zbroi";
Hue = 1170;
LootType = LootType.Blessed;
}

public override void OnDoubleClick( Mobile from )
{
if ( IsChildOf( from.Backpack ) )
{
from.BeginTarget( 6, false, TargetFlags.None, new TargetCallback( OnTarget ) );
from.SendLocalizedMessage( 502450 );
}
else
{
from.SendLocalizedMessage( 1042001 );
}
}

public virtual void OnTarget( Mobile from, object targeted )
{
if ( Deleted )
return;

if(targeted is BaseArmor)
{
BaseArmor arm = (BaseArmor)targeted;

if ( !arm.IsChildOf( from.Backpack ) )
{
from.SendLocalizedMessage( 1042010 );
}
else
{
if ( arm.ArmorAttributes.MageArmor == 0 )
{
from.PlaySound( 0xFE );
Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x376A, 1, 29, 0x47D, 2, 9962, 0 ); 
arm.ArmorAttributes.MageArmor = 1;
from.SendLocalizedMessage( 500038 );
this.Delete();
}
else
{
}
}
}
else
{
from.SendLocalizedMessage( 501026 );
}
}

public MageArmorScroll( Serial serial ) : base( serial )
{
}

public override void Serialize( GenericWriter writer )
{
base.Serialize( writer );
writer.Write( (int) 0 );

}

public override void Deserialize(GenericReader reader)
{
base.Deserialize( reader );
int version = reader.ReadInt();
}
}
}
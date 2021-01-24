using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Undead
{
	public class UndeadBag : ScrollBag
	{
		[Constructable]
		public UndeadBag()
		{
			Hue = 38;
	        PlaceItemIn( 50, 35, new UndeadAngelicFaithScroll() );
			PlaceItemIn( 70, 35, new UndeadHammerOfFaithScroll() );
			PlaceItemIn( 30, 75, new UndeadCauseFearScroll() );
			PlaceItemIn( 90, 35, new UndeadGraspingRootsScroll() );
			PlaceItemIn( 30, 55, new UndeadHollowReedScroll() );
			PlaceItemIn( 50, 55, new UndeadLureStoneScroll() );
			PlaceItemIn( 70, 55, new UndeadMushroomGatewayScroll() );
			PlaceItemIn( 90, 55, new UndeadNaturesPassageScroll() );
			PlaceItemIn( 30, 35, new UndeadSeanceScroll() );
			PlaceItemIn( 50, 95, new UndeadSwarmOfInsectsScroll() );
			PlaceItemIn( 70, 95, new UndeadLeafWhirlwindScroll() );
			PlaceItemIn( 90, 95, new UndeadVolcanicEruptionScroll() );

		//	PlaceItemIn( 50, 75, new DruidRestorativeSoilScroll() );
		//	PlaceItemIn( 70, 75, new DruidShieldOfEarthScroll() );
		//	PlaceItemIn( 90, 75, new DruidSpringOfLifeScroll() );
		//	PlaceItemIn( 30, 95, new DruidStoneCircleScroll() );

		}

		public UndeadBag( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}

namespace Nelderim.Races
{
    class None : NRace
    {
        public None( int raceId, int raceIndex ) : base( raceId, raceIndex )
        {
        }

        public override string[] Names => new[] { "None", "Czlowieka", "Czlowiekowi", "Czlowieka", "Czlowiekiem", "Czlowieku", "Czlowieku" };
        public override string[] PluralNames => new[] { "Ludzie", "Ludzi", "Ludziom", "Ludzi", "Ludzmi", "Ludziach", "Ludzie" };
        public override int DescNumber => 505817;
    }
}

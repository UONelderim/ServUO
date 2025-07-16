
using System;
using Xunit;

namespace Server.Items;

public class AritfactHelperFixture : IDisposable
{
	public AritfactHelperFixture()
	{
		ArtifactHelper.Configure();
	}
	
	public void Dispose()
	{
	}
}

public class ArtifactHelperTest : IClassFixture<AritfactHelperFixture>
{
	[Fact]
	public void AllArtifactsAreDistinct()
	{
		Assert.Contains(typeof(Aegis), ArtifactHelper.ArtifactsSelectedSeason(ArtGroup.Boss, ArtSeason.Autumn));
		Assert.Contains(typeof(Aegis), ArtifactHelper.ArtifactsSelectedSeason(ArtGroup.Boss, ArtSeason.Summer));
		Assert.Single(ArtifactHelper.AllArtifacts, x => x == typeof(Aegis));
		
		Assert.Contains(typeof(PrzysiegaTriamPergi), ArtifactHelper.ArtifactsSelectedSeason(ArtGroup.Fishing, ArtSeason.Spring));
		Assert.Contains(typeof(PrzysiegaTriamPergi), ArtifactHelper.ArtifactsSelectedSeason(ArtGroup.Cartography, ArtSeason.Autumn));
		Assert.Single(ArtifactHelper.AllArtifacts, x => x == typeof(PrzysiegaTriamPergi));
	}

	[Fact]
	public void ArtifactsPerSeasonAreDistinct()
	{
		Assert.Contains(typeof(Retorta), ArtifactHelper.ArtifactsSelectedSeason(ArtGroup.Cartography, ArtSeason.Autumn));
		Assert.Contains(typeof(Retorta), ArtifactHelper.ArtifactsSelectedSeason(ArtGroup.Boss, ArtSeason.Autumn));
		Assert.Single(ArtifactHelper.AllArtifactsSelectedSeason(ArtSeason.Autumn), x => x == typeof(Retorta));
	}
}

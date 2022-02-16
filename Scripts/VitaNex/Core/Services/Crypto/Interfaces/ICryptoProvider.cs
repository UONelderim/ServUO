﻿#region Header
//   Vorspire    _,-'/-'/  ICryptoProvider.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Security.Cryptography;
#endregion

namespace VitaNex.Crypto
{
	public interface ICryptoProvider : IDisposable
	{
		bool IsDisposed { get; }

		int ProviderID { get; }
		HashAlgorithm Provider { get; }

		string Seed { get; }
		byte[] Buffer { get; }

		string Generate(string seed);
	}

	public interface ICryptoMutator : ICryptoProvider
	{
		string Transform(string seed);
		string Mutate(string code);
	}
}
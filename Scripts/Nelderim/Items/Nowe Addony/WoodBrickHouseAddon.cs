////////////////////////////////////////
//                                     //
//   Generated by CEO's YAAAG - Ver 2  //
// (Yet Another Arya Addon Generator)  //
//    Modified by Hammerhand for       //
//      SA & High Seas content         //
//                                     //
////////////////////////////////////////

namespace Server.Items
{
	public class WoodBrickHouseAddon : BaseAddon
	{
		private static readonly int[,] m_AddOnSimpleComponents =
		{
			{ 7, -3, 0, 43 }, { 10494, 3, 0, 66 }, { 10486, 0, 0, 63 } // 1	3	4	
			,
			{ 10486, -1, 0, 63 }, { 10486, -2, 0, 64 }, { 10486, -3, 0, 66 } // 5	6	7	
			,
			{ 10486, 2, 0, 65 }, { 10486, 1, 0, 63 }, { 10486, 0, 0, 63 } // 8	9	10	
			,
			{ 10486, -1, 0, 63 }, { 10486, -2, 0, 63 }, { 10486, -3, 0, 63 } // 11	12	13	
			,
			{ 10495, 4, 1, 63 }, { 10489, 4, 0, 63 }, { 10487, 3, 1, 63 } // 14	15	16	
			,
			{ 10487, 2, 1, 63 }, { 10487, 1, 1, 63 }, { 10487, 0, 1, 63 } // 17	18	19	
			,
			{ 10487, -1, 1, 63 }, { 10487, -2, 1, 63 }, { 10487, -3, 1, 63 } // 20	21	22	
			,
			{ 1180, 3, 0, 43 }, { 24, -3, 4, 43 }, { 24, -2, 4, 43 } // 23	24	25	
			,
			{ 24, -1, 4, 43 }, { 24, 0, 4, 43 }, { 24, 7, 0, 43 } // 26	27	28	
			,
			{ 24, 6, 0, 43 }, { 24, 5, 0, 43 }, { 24, 4, 0, 43 } // 29	30	31	
			,
			{ 7, -1, 0, 43 }, { 7, 1, 0, 43 }, { 7, 2, 0, 43 } // 32	33	34	
			,
			{ 25, 0, 1, 43 }, { 25, 0, 3, 43 }, { 25, 0, 4, 43 } // 35	36	37	
			,
			{ 25, 0, 2, 43 }, { 25, 7, 0, 43 }, { 6, 3, 0, 43 } // 38	39	40	
			,
			{ 1180, 2, 0, 43 }, { 1180, 1, 0, 43 }, { 1180, 0, 0, 43 } // 41	42	43	
			,
			{ 1180, -1, 0, 43 }, { 1180, -2, 0, 43 }, { 1180, -3, 0, 43 } // 44	45	46	
			,
			{ 1180, 0, 4, 43 }, { 1180, 0, 3, 43 }, { 1180, 0, 2, 43 } // 47	48	49	
			,
			{ 1180, 0, 1, 43 }, { 1180, 0, 0, 43 }, { 1180, -1, 4, 43 } // 50	51	52	
			,
			{ 1180, -1, 3, 43 }, { 1180, -1, 2, 43 }, { 1180, -1, 1, 43 } // 53	54	55	
			,
			{ 1180, -1, 0, 43 }, { 1180, -2, 4, 43 }, { 1180, -2, 3, 43 } // 56	57	58	
			,
			{ 1180, -2, 2, 43 }, { 1180, -2, 1, 43 }, { 1180, -2, 0, 43 } // 59	60	61	
			,
			{ 1180, -3, 4, 43 }, { 1180, -3, 3, 43 }, { 1180, -3, 2, 43 } // 62	63	64	
			,
			{ 1180, -3, 1, 43 }, { 1180, -3, 0, 43 }, { 1180, 7, 0, 43 } // 65	66	67	
			,
			{ 1180, 6, 0, 43 }, { 1180, 5, 0, 43 }, { 1180, 4, 0, 43 } // 68	69	70	
			,
			{ 9, 0, 0, 21 }, { 72, -2, 0, 0 }, { 22, -2, 0, 21 } // 71	72	73	
			,
			{ 22, -3, 0, 21 }, { 3213, 4, 0, 22 }, { 3213, 5, 0, 22 } // 74	75	76	
			,
			{ 8, 5, 0, 21 }, { 8, 3, 0, 21 }, { 6, 7, 0, 21 } // 77	78	79	
			,
			{ 7, 1, 0, 21 }, { 8, 0, 4, 21 }, { 8, 0, 3, 21 } // 80	81	82	
			,
			{ 8, 0, 2, 21 }, { 8, 0, 1, 21 }, { 7, 0, 4, 21 } // 83	84	85	
			,
			{ 7, -1, 4, 21 }, { 7, -2, 4, 21 }, { 7, -3, 4, 21 } // 86	87	88	
			,
			{ 7, 6, 0, 21 }, { 7, 3, 0, 21 }, { 7, 2, 0, 21 } // 89	90	91	
			,
			{ 1193, 7, 0, 21 }, { 1193, 6, 0, 21 }, { 1193, 5, 0, 21 } // 92	93	94	
			,
			{ 1193, 4, 0, 21 }, { 1193, 3, 0, 21 }, { 1193, 2, 0, 21 } // 95	96	97	
			,
			{ 1193, 1, 0, 21 }, { 1193, 0, 0, 21 }, { 1193, -1, 0, 21 } // 98	99	100	
			,
			{ 1193, -2, 0, 21 }, { 1193, -3, 0, 21 }, { 1193, 7, 0, 21 } // 101	102	103	
			,
			{ 1825, -1, 1, 0 }, { 10489, 1, 5, 17 }, { 1304, -2, 5, 0 } // 106	107	108	
			,
			{ 1304, -2, 3, 0 }, { 1304, -3, 0, 0 }, { 51, 0, 6, 0 } // 109	110	111	
			,
			{ 1304, 0, 1, 0 }, { 1826, -1, 4, 0 }, { 1304, 0, 2, 0 } // 112	113	114	
			,
			{ 1825, -1, 3, 0 }, { 1825, -1, 2, 0 }, { 1304, -1, 2, 0 } // 115	116	117	
			,
			{ 1304, -1, 1, 0 }, { 1304, -2, 2, 0 }, { 1304, -1, 0, 0 } // 118	119	120	
			,
			{ 1304, -2, 0, 0 }, { 52, 0, 0, 0 }, { 1304, -2, 1, 0 } // 121	122	123	
			,
			{ 1304, -1, 5, 0 }, { 1304, -1, 4, 0 }, { 319, 5, 0, 0 } // 124	125	126	
			,
			{ 1825, -1, 2, 5 }, { 10487, -1, 7, 17 }, { 10495, 1, 7, 20 } // 127	128	129	
			,
			{ 10494, 0, 6, 21 }, { 1304, -2, 4, 0 }, { 10487, 0, 7, 17 } // 130	131	132	
			,
			{ 307, 6, 0, 0 }, { 1304, 6, 0, 0 }, { 53, 0, 1, 0 } // 133	134	135	
			,
			{ 1304, 0, 5, 0 }, { 53, 0, 3, 0 }, { 1304, 1, 0, 0 } // 136	137	138	
			,
			{ 1304, -3, 4, 0 }, { 1304, -3, 1, 0 }, { 1304, 5, 0, 0 } // 139	140	141	
			,
			{ 53, 0, 5, 0 }, { 1304, -3, 2, 0 }, { 10486, -1, 6, 34 } // 142	143	144	
			,
			{ 10486, -2, 6, 34 }, { 10486, -3, 7, 31 }, { 1825, -1, 1, 5 } // 145	146	147	
			,
			{ 10486, -2, 7, 31 }, { 10487, -3, 7, 17 }, { 318, 3, 0, 0 } // 148	149	150	
			,
			{ 1826, -1, 3, 5 }, { 1304, 7, 0, 0 }, { 1304, 4, 0, 0 } // 151	152	153	
			,
			{ 51, 7, 0, 0 }, { 1304, -1, 3, 0 }, { 1304, 0, 6, 0 } // 154	155	156	
			,
			{ 1304, 0, 3, 0 }, { 53, 0, 2, 0 }, { 1304, -3, 6, 0 } // 157	158	159	
			,
			{ 312, 2, 0, 0 }, { 1304, -1, 6, 0 }, { 1304, -3, 3, 0 } // 160	161	162	
			,
			{ 1304, -3, 5, 0 }, { 53, 0, 4, 0 }, { 1304, -2, 6, 0 } // 163	164	165	
			,
			{ 1304, 0, 4, 0 }, { 1304, 3, 0, 0 }, { 10489, 1, 6, 17 } // 166	167	168	
			,
			{ 1304, 2, 0, 0 }, { 52, 1, 0, 0 }, { 1304, 0, 0, 0 } // 169	170	171	
			,
			{ 10487, -2, 7, 17 }, { 1826, -1, 4, 0 }, { 10486, 0, 7, 31 } // 172	173	174	
			,
			{ 10486, -1, 7, 31 }, { 10494, 0, 6, 34 }, { 1826, -1, 1, 15 } // 175	176	177	
			,
			{ 1825, -1, 1, 10 }, { 307, -1, 6, 0 }, { 10488, 1, 6, 31 } // 178	179	180	
			,
			{ 54, 0, 0, 0 }, { 1826, -1, 2, 10 }, { 10481, -1, -1, 82 } // 181	182	183	
			,
			{ 10485, -1, -2, 71 }, { 10485, 0, -2, 71 }, { 10483, 1, -2, 71 } // 184	185	186	
			,
			{ 10494, 2, -1, 69 }, { 10496, -3, -1, 69 }, { 10498, 2, -3, 68 } // 187	188	189	
			,
			{ 10498, 3, -4, 65 }, { 10500, -2, -2, 75 }, { 10500, -3, -3, 70 } // 190	191	192	
			,
			{ 10492, 2, -4, 65 }, { 10492, 1, -3, 67 }, { 10492, 1, -4, 65 } // 193	194	195	
			,
			{ 10492, 0, -3, 67 }, { 10492, 0, -4, 65 }, { 10492, -1, -3, 67 } // 196	197	198	
			,
			{ 10492, -1, -4, 65 }, { 10492, -2, -3, 68 }, { 10492, -2, -4, 65 } // 199	200	201	
			,
			{ 10492, -3, -4, 65 }, { 10489, 4, -4, 64 }, { 10490, -3, -2, 67 } // 202	203	204	
			,
			{ 10488, 3, -1, 65 }, { 10488, 3, -3, 65 }, { 10488, 3, -2, 63 } // 205	206	207	
			,
			{ 10488, 2, -2, 68 }, { 10488, 3, -2, 65 }, { 10486, 0, -1, 67 } // 208	209	210	
			,
			{ 10486, 1, -1, 66 }, { 10486, -1, -1, 67 }, { 10486, -2, -1, 68 } // 211	212	213	
			,
			{ 10499, 4, -5, 63 }, { 10493, 3, -5, 63 }, { 10493, 2, -5, 63 } // 214	215	216	
			,
			{ 10493, 1, -5, 63 }, { 10493, 0, -5, 63 }, { 10493, -1, -5, 63 } // 217	218	219	
			,
			{ 10493, -2, -5, 63 }, { 10493, -3, -5, 63 }, { 10489, 4, -1, 63 } // 220	221	222	
			,
			{ 10489, 4, -2, 63 }, { 10489, 4, -3, 63 }, { 1180, 3, -1, 43 } // 223	224	225	
			,
			{ 1180, 3, -2, 43 }, { 1180, 3, -3, 43 }, { 1180, 3, -4, 43 } // 226	227	228	
			,
			{ 1180, 3, -5, 43 }, { 1180, 2, -2, 43 }, { 1180, 2, -3, 43 } // 229	230	231	
			,
			{ 1180, 2, -4, 43 }, { 1180, 2, -5, 43 }, { 1180, 1, -2, 43 } // 232	233	234	
			,
			{ 1180, 1, -3, 43 }, { 1180, 1, -4, 43 }, { 1180, 1, -5, 43 } // 235	236	237	
			,
			{ 1180, 1, -6, 43 }, { 1180, 5, -5, 43 }, { 24, 7, -6, 43 } // 238	239	240	
			,
			{ 24, 6, -6, 43 }, { 24, 5, -6, 43 }, { 24, 4, -6, 43 } // 241	242	243	
			,
			{ 7, 3, -6, 43 }, { 25, 7, -1, 43 }, { 25, 7, -2, 43 } // 244	245	246	
			,
			{ 25, 7, -3, 43 }, { 25, 7, -4, 43 }, { 25, 7, -5, 43 } // 247	248	249	
			,
			{ 8, 3, -1, 43 }, { 8, 3, -2, 43 }, { 8, 3, -4, 43 } // 250	251	252	
			,
			{ 8, 3, -5, 43 }, { 7, -3, -6, 43 }, { 7, -2, -6, 43 } // 253	254	255	
			,
			{ 7, -1, -6, 43 }, { 7, 2, -6, 43 }, { 7, 1, -6, 43 } // 256	257	258	
			,
			{ 7, 0, -6, 43 }, { 1180, 2, -1, 43 }, { 1180, 2, -2, 43 } // 259	260	261	
			,
			{ 1180, 2, -3, 43 }, { 1180, 2, -4, 43 }, { 1180, 2, -5, 43 } // 262	263	264	
			,
			{ 1180, 1, -1, 43 }, { 1180, 1, -2, 43 }, { 1180, 1, -3, 43 } // 265	266	267	
			,
			{ 1180, 1, -4, 43 }, { 1180, 1, -5, 43 }, { 1180, 0, -1, 43 } // 268	269	270	
			,
			{ 1180, 0, -2, 43 }, { 1180, 0, -3, 43 }, { 1180, 0, -4, 43 } // 271	272	273	
			,
			{ 1180, 0, -5, 43 }, { 1180, -1, -1, 43 }, { 1180, -1, -2, 43 } // 274	275	276	
			,
			{ 1180, -1, -3, 43 }, { 1180, -1, -4, 43 }, { 1180, -1, -5, 43 } // 277	278	279	
			,
			{ 1180, -2, -1, 43 }, { 1180, -2, -2, 43 }, { 1180, -2, -3, 43 } // 280	281	282	
			,
			{ 1180, -2, -4, 43 }, { 1180, -2, -5, 43 }, { 1180, -3, -1, 43 } // 283	284	285	
			,
			{ 1180, -3, -2, 43 }, { 1180, -3, -3, 43 }, { 1180, -3, -4, 43 } // 286	287	288	
			,
			{ 1180, -3, -5, 43 }, { 1180, 7, -1, 43 }, { 1180, 7, -2, 43 } // 289	290	291	
			,
			{ 1180, 7, -3, 43 }, { 1180, 7, -4, 43 }, { 1180, 7, -5, 43 } // 292	293	294	
			,
			{ 1180, 6, -1, 43 }, { 1180, 6, -2, 43 }, { 1180, 6, -3, 43 } // 295	296	297	
			,
			{ 1180, 6, -4, 43 }, { 1180, 6, -5, 43 }, { 1180, 5, -1, 43 } // 298	299	300	
			,
			{ 1180, 5, -2, 43 }, { 1180, 5, -3, 43 }, { 1180, 5, -4, 43 } // 301	302	303	
			,
			{ 1180, 4, -1, 43 }, { 1180, 4, -2, 43 }, { 1180, 4, -3, 43 } // 304	305	306	
			,
			{ 1180, 4, -4, 43 }, { 1180, 4, -5, 43 }, { 9, 3, -1, 21 } // 307	308	309	
			,
			{ 9, 6, -4, 21 }, { 52, 7, -6, 21 }, { 3213, 7, -2, 22 } // 310	311	316	
			,
			{ 3213, 7, -3, 22 }, { 8, 7, -5, 21 }, { 8, 7, -4, 21 } // 317	318	319	
			,
			{ 8, 7, -1, 21 }, { 7, 7, -2, 21 }, { 1193, 5, -2, 21 } // 320	321	324	
			,
			{ 7, 7, -4, 21 }, { 1193, 7, -1, 21 }, { 1193, 7, -2, 21 } // 325	329	330	
			,
			{ 1193, 7, -3, 21 }, { 1193, 7, -4, 21 }, { 1193, 7, -4, 21 } // 331	332	333	
			,
			{ 1193, 6, -1, 21 }, { 1193, 6, -2, 21 }, { 1193, 6, -3, 21 } // 334	335	336	
			,
			{ 1193, 7, -5, 21 }, { 1193, 6, -5, 21 }, { 1193, 5, -1, 21 } // 337	338	339	
			,
			{ 1193, 5, -3, 21 }, { 1193, 6, -4, 21 }, { 1193, 5, -4, 21 } // 340	341	342	
			,
			{ 1193, 5, -5, 21 }, { 1193, 4, -1, 21 }, { 1193, 4, -2, 21 } // 343	344	345	
			,
			{ 1193, 4, -3, 21 }, { 1193, 4, -4, 21 }, { 1193, 4, -5, 21 } // 346	347	348	
			,
			{ 1193, 3, -1, 21 }, { 1193, 3, -2, 21 }, { 1193, 3, -3, 21 } // 349	350	351	
			,
			{ 1193, 3, -4, 21 }, { 1193, 3, -5, 21 }, { 1193, 2, -1, 21 } // 352	353	354	
			,
			{ 1193, 2, -2, 21 }, { 1193, 2, -3, 21 }, { 1193, 2, -4, 21 } // 355	356	357	
			,
			{ 1193, 2, -5, 21 }, { 1193, 1, -1, 21 }, { 1193, 1, -2, 21 } // 358	359	360	
			,
			{ 1193, 1, -3, 21 }, { 1193, 1, -4, 21 }, { 1193, 1, -5, 21 } // 361	362	363	
			,
			{ 1193, 0, -1, 21 }, { 1193, 0, -2, 21 }, { 1193, 0, -3, 21 } // 364	365	366	
			,
			{ 1193, 0, -4, 21 }, { 1193, 0, -5, 21 }, { 1193, -1, -1, 21 } // 367	368	369	
			,
			{ 1193, -1, -2, 21 }, { 1193, -1, -3, 21 }, { 1193, -1, -4, 21 } // 370	371	372	
			,
			{ 1193, -1, -5, 21 }, { 1193, -2, -1, 21 }, { 1193, -2, -2, 21 } // 373	374	375	
			,
			{ 1193, -2, -3, 21 }, { 1193, -2, -4, 21 }, { 1193, -2, -5, 21 } // 376	377	378	
			,
			{ 1193, -3, -1, 21 }, { 1193, -3, -2, 21 }, { 1193, -3, -3, 21 } // 379	380	381	
			,
			{ 1193, -3, -4, 21 }, { 1193, -3, -5, 21 }, { 52, -1, -6, 21 } // 382	383	388	
			,
			{ 1304, 2, -5, 0 }, { 52, -1, -6, 0 }, { 1304, -1, -5, 0 } // 389	390	391	
			,
			{ 1304, 0, -5, 0 }, { 52, 1, -6, 0 }, { 52, 2, -6, 0 } // 392	393	394	
			,
			{ 1304, 6, -5, 0 }, { 53, 7, -5, 0 }, { 1304, 7, -5, 0 } // 395	396	397	
			,
			{ 52, 0, -6, 21 }, { 52, 7, -6, 0 }, { 1304, 4, -5, 0 } // 398	399	400	
			,
			{ 1304, 1, -5, 0 }, { 52, 3, -6, 0 }, { 52, 0, -6, 0 } // 401	402	403	
			,
			{ 1304, 5, -5, 0 }, { 1304, -3, -5, 0 }, { 1304, 3, -5, 0 } // 404	405	406	
			,
			{ 1304, -2, -5, 0 }, { 1304, -1, -2, 0 }, { 1304, 2, -4, 0 } // 407	408	409	
			,
			{ 1304, 4, -4, 0 }, { 1304, -1, -3, 0 }, { 1304, 1, -4, 0 } // 410	411	412	
			,
			{ 1304, -2, -1, 0 }, { 1304, 5, -1, 0 }, { 1304, 3, -2, 0 } // 413	414	415	
			,
			{ 1304, -3, -2, 1 }, { 1304, 1, -3, 0 }, { 1304, 0, -4, 0 } // 416	417	418	
			,
			{ 1304, 1, -2, 0 }, { 1304, 0, -3, 0 }, { 1304, -3, -4, 0 } // 419	420	421	
			,
			{ 1304, -2, -4, 0 }, { 1304, 1, -1, 0 }, { 1304, 3, -1, 0 } // 422	423	424	
			,
			{ 1304, 6, -3, 0 }, { 1304, 4, -1, 0 }, { 1304, 5, -4, 0 } // 425	426	427	
			,
			{ 1304, 3, -3, 0 }, { 1304, -2, -3, 0 }, { 1304, -3, -3, 0 } // 428	429	430	
			,
			{ 1304, 6, -4, 0 }, { 1304, 7, -3, 0 }, { 1304, 2, -3, 0 } // 431	432	433	
			,
			{ 1304, 4, -2, 0 }, { 1304, 7, -4, 0 }, { 1304, 5, -3, 0 } // 434	435	436	
			,
			{ 1304, 5, -2, 0 }, { 1304, 7, -1, 0 }, { 52, 2, -6, 21 } // 437	438	439	
			,
			{ 1304, 6, -3, 0 }, { 1304, 0, -1, 0 }, { 1304, 0, -2, 0 } // 440	441	442	
			,
			{ 1304, -1, -4, 0 }, { 1304, 3, -4, 0 }, { 1304, 4, -3, 0 } // 443	444	445	
			,
			{ 1304, 2, -1, 0 }, { 1304, 2, -2, 0 }, { 1304, 6, -1, 0 } // 446	447	448	
			,
			{ 1304, -1, -1, 0 }, { 1304, -3, -1, 0 }, { 52, 1, -6, 21 } // 449	450	451	
			,
			{ 1304, 6, -2, 0 }, { 307, 6, -6, 0 }, { 307, -2, -6, 0 } // 452	453	454	
			,
			{ 308, 7, -1, 0 }, { 312, 4, -6, 0 }, { 313, 7, -4, 0 } // 455	456	457	
			,
			{ 52, 3, -6, 21 }, { 1304, -2, -2, 0 }, { 1304, 7, -2, 0 } // 458	459	460	
			,
			{ 7, -5, 0, 43 }, { 10496, -4, 0, 66 }, { 10497, -5, 1, 63 } // 461	463	464	
			,
			{ 10491, -5, 0, 63 }, { 10487, -4, 1, 63 }, { 24, -4, 4, 43 } // 465	466	467	
			,
			{ 24, -5, 4, 43 }, { 25, -6, 1, 43 }, { 25, -6, 2, 43 } // 468	469	470	
			,
			{ 25, -6, 4, 43 }, { 25, -6, 3, 43 }, { 8, -6, 0, 43 } // 471	472	473	
			,
			{ 1180, -4, 0, 43 }, { 1180, -5, 0, 43 }, { 1180, -4, 4, 43 } // 474	475	476	
			,
			{ 1180, -4, 3, 43 }, { 1180, -4, 2, 43 }, { 1180, -4, 1, 43 } // 477	478	479	
			,
			{ 1180, -4, 0, 43 }, { 1180, -5, 4, 43 }, { 1180, -5, 3, 43 } // 480	481	482	
			,
			{ 1180, -5, 2, 43 }, { 1180, -5, 1, 43 }, { 1180, -5, 0, 43 } // 483	484	485	
			,
			{ 71, -4, 0, 0 }, { 22, -4, 0, 21 }, { 22, -5, 0, 21 } // 486	487	488	
			,
			{ 7, -4, 4, 21 }, { 7, -5, 4, 21 }, { 8, -6, 4, 21 } // 489	490	491	
			,
			{ 8, -6, 3, 21 }, { 8, -6, 1, 21 }, { 8, -6, 2, 21 } // 492	493	494	
			,
			{ 8, -6, 0, 21 }, { 1193, -4, 0, 21 }, { 1193, -5, 0, 21 } // 495	496	497	
			,
			{ 53, -6, 0, 0 }, { 1304, -5, 1, 0 }, { 1304, -5, 5, 0 } // 499	500	501	
			,
			{ 53, -6, 6, 0 }, { 1304, -5, 3, 0 }, { 10487, -4, 7, 17 } // 502	503	504	
			,
			{ 53, -6, 1, 0 }, { 10487, -5, 7, 18 }, { 10491, -6, 6, 18 } // 505	506	507	
			,
			{ 10490, -4, 6, 31 }, { 1304, -4, 0, 0 }, { 1304, -4, 0, 0 } // 508	509	510	
			,
			{ 1304, -4, 1, 0 }, { 1304, -5, 6, 0 }, { 1304, -5, 2, 0 } // 511	512	513	
			,
			{ 1304, -4, 4, 0 }, { 1304, -4, 5, 0 }, { 1304, -4, 3, 0 } // 514	515	516	
			,
			{ 10496, -4, 5, 24 }, { 10497, -6, 7, 19 }, { 10491, -6, 5, 18 } // 517	518	519	
			,
			{ 52, -5, 0, 0 }, { 10496, -5, 6, 22 }, { 1304, -4, 6, 0 } // 520	521	522	
			,
			{ 52, -5, 6, 0 }, { 53, -6, 2, 0 }, { 1304, -5, 4, 0 } // 523	524	525	
			,
			{ 1304, -4, 2, 0 }, { 1304, -5, 0, 1 }, { 308, -6, 5, 0 } // 526	527	528	
			,
			{ 312, -4, 6, 0 }, { 313, -6, 3, 0 }, { 10500, -4, -4, 66 } // 529	530	531	
			,
			{ 10490, -4, -3, 65 }, { 10490, -4, -1, 65 }, { 10490, -4, -2, 65 } // 532	533	534	
			,
			{ 10501, -5, -5, 63 }, { 10493, -4, -5, 63 }, { 10491, -5, -1, 63 } // 535	536	537	
			,
			{ 10491, -5, -2, 63 }, { 10491, -5, -3, 63 }, { 10491, -5, -4, 63 } // 538	539	540	
			,
			{ 1826, -5, -5, 36 }, { 1825, -5, -5, 31 }, { 1825, -5, -5, 26 } // 541	542	543	
			,
			{ 1825, -5, -5, 21 }, { 1826, -5, -4, 31 }, { 1825, -5, -4, 26 } // 544	545	546	
			,
			{ 1825, -5, -4, 21 }, { 1826, -5, -3, 26 }, { 1825, -5, -3, 21 } // 547	548	549	
			,
			{ 1826, -5, -2, 21 }, { 1826, -5, -2, 21 }, { 8, -6, -1, 43 } // 550	551	552	
			,
			{ 8, -6, -2, 43 }, { 8, -6, -3, 43 }, { 8, -6, -4, 43 } // 553	554	555	
			,
			{ 8, -6, -5, 43 }, { 7, -4, -6, 43 }, { 7, -5, -6, 43 } // 556	557	558	
			,
			{ 1180, -4, -1, 43 }, { 1180, -4, -2, 43 }, { 1180, -4, -3, 43 } // 559	560	561	
			,
			{ 1180, -4, -4, 43 }, { 1180, -4, -5, 43 }, { 1180, -5, -1, 43 } // 562	563	564	
			,
			{ 1180, -5, -2, 43 }, { 8, -6, -1, 21 }, { 8, -6, -2, 21 } // 565	567	568	
			,
			{ 8, -6, -3, 21 }, { 8, -6, -4, 21 }, { 8, -6, -5, 21 } // 569	570	571	
			,
			{ 9, -6, -6, 21 }, { 1193, -4, -1, 21 }, { 1193, -4, -2, 21 } // 572	573	574	
			,
			{ 1193, -4, -3, 21 }, { 1193, -4, -4, 21 }, { 1193, -4, -5, 21 } // 575	576	577	
			,
			{ 1193, -5, -1, 21 }, { 1193, -5, -2, 21 }, { 1193, -5, -3, 21 } // 578	579	580	
			,
			{ 1193, -5, -4, 21 }, { 1193, -5, -5, 21 }, { 54, -6, -6, 0 } // 581	582	583	
			,
			{ 1304, -4, -5, 0 }, { 52, -5, -6, 0 }, { 53, -6, -5, 0 } // 584	585	586	
			,
			{ 1304, -5, -5, 0 }, { 1304, -4, -4, 0 }, { 1304, -4, -2, 0 } // 587	588	589	
			,
			{ 1304, -5, -1, 1 }, { 1304, -4, -1, 0 }, { 1304, -4, -3, 0 } // 590	591	592	
			,
			{ 1304, -5, -2, 0 }, { 1304, -5, -4, 0 }, { 52, -5, -6, 21 } // 593	594	595	
			,
			{ 53, -6, -1, 0 }, { 1304, -5, -3, 0 }, { 308, -6, -2, 0 } // 596	597	599	
			,
			{ 313, -6, -4, 0 }, { 312, -4, -6, 0 } // 600	601	
		};


		public override BaseAddonDeed Deed
		{
			get
			{
				return new WoodBrickHouseAddonDeed();
			}
		}

		[Constructable]
		public WoodBrickHouseAddon()
		{
			for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
				AddComponent(new AddonComponent(m_AddOnSimpleComponents[i, 0]), m_AddOnSimpleComponents[i, 1],
					m_AddOnSimpleComponents[i, 2], m_AddOnSimpleComponents[i, 3]);


			AddComplexComponent(this, 314, 0, 0, 43, 0, 0, "", 1); // 2
			AddComplexComponent(this, 314, -3, 6, 0, 0, 0, "", 1); // 104
			AddComplexComponent(this, 314, -2, 6, 0, 0, 0, "", 1); // 105
			AddComplexComponent(this, 14, 4, -6, 21, 0, 0, "", 1); // 312
			AddComplexComponent(this, 14, 6, -6, 21, 0, 0, "", 1); // 313
			AddComplexComponent(this, 14, -3, -6, 21, 0, 0, "", 1); // 314
			AddComplexComponent(this, 14, -2, -6, 21, 0, 0, "", 1); // 315
			AddComplexComponent(this, 14, 5, -1, 21, 0, 0, "", 1); // 322
			AddComplexComponent(this, 14, 4, -1, 21, 0, 0, "", 1); // 323
			AddComplexComponent(this, 14, 5, -6, 21, 0, 0, "", 1); // 326
			AddComplexComponent(this, 15, 6, -2, 21, 0, 1, "", 1); // 327
			AddComplexComponent(this, 15, 6, -3, 21, 0, 1, "", 1); // 328
			AddComplexComponent(this, 314, -3, -6, 0, 0, 0, "", 1); // 384
			AddComplexComponent(this, 314, 5, -6, 0, 0, 0, "", 1); // 385
			AddComplexComponent(this, 315, 7, -3, 0, 0, 0, "", 1); // 386
			AddComplexComponent(this, 315, 7, -2, 0, 0, 0, "", 1); // 387
			AddComplexComponent(this, 314, -4, 0, 43, 0, 0, "", 1); // 462
			AddComplexComponent(this, 315, -6, 4, 0, 0, 0, "", 1); // 498
			AddComplexComponent(this, 14, -4, -6, 21, 0, 0, "", 1); // 566
			AddComplexComponent(this, 315, -6, -3, 0, 0, 0, "", 1); // 598
		}

		public WoodBrickHouseAddon(Serial serial) : base(serial)
		{
		}

		private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset,
			int hue, int lightsource)
		{
			AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
		}

		private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset,
			int hue, int lightsource, string name, int amount)
		{
			AddonComponent ac;
			ac = new AddonComponent(item);
			if (name != null && name.Length > 0)
				ac.Name = name;
			if (hue != 0)
				ac.Hue = hue;
			if (amount > 1)
			{
				ac.Stackable = true;
				ac.Amount = amount;
			}

			if (lightsource != -1)
				ac.Light = (LightType)lightsource;
			addon.AddComponent(ac, xoffset, yoffset, zoffset);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class WoodBrickHouseAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new WoodBrickHouseAddon();
			}
		}

		[Constructable]
		public WoodBrickHouseAddonDeed()
		{
			Name = "WoodBrickHouse";
		}

		public WoodBrickHouseAddonDeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}


using System;
using System.Collections.Generic;

namespace PokeSave
{
	public static class ItemList
	{

		#region #	Hex	Bag	Item	Pocket
		const string Datasource = @"0	 0x0000	None	 Nothing	Bag Unknown pocket icon.png	 Unknown pocket
1	 0x0001	Master Ball	Master Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
2	 0x0002	Ultra Ball	Ultra Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
3	 0x0003	Great Ball	Great Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
4	 0x0004	Poké Ball	Poké Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
5	 0x0005	Safari Ball	Safari Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
6	 0x0006	Net Ball	Net Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
7	 0x0007	Dive Ball	Dive Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
8	 0x0008	Nest Ball	Nest Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
9	 0x0009	Repeat Ball	Repeat Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
10	 0x000A	Timer Ball	Timer Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
11	 0x000B	Luxury Ball	Luxury Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
12	 0x000C	Premier Ball	Premier Ball	Bag Poké Balls pocket icon.png	 Poké Balls pocket
13	 0x000D	Potion	Potion	Bag Items pocket icon.png	 Items pocket
14	 0x000E	Antidote	Antidote	Bag Items pocket icon.png	 Items pocket
15	 0x000F	Burn Heal	Burn Heal	Bag Items pocket icon.png	 Items pocket
16	 0x0010	Ice Heal	Ice Heal	Bag Items pocket icon.png	 Items pocket
17	 0x0011	Awakening	Awakening	Bag Items pocket icon.png	 Items pocket
18	 0x0012	Parlyz Heal	Parlyz Heal	Bag Items pocket icon.png	 Items pocket
19	 0x0013	Full Restore	Full Restore	Bag Items pocket icon.png	 Items pocket
20	 0x0014	Max Potion	Max Potion	Bag Items pocket icon.png	 Items pocket
21	 0x0015	Hyper Potion	Hyper Potion	Bag Items pocket icon.png	 Items pocket
22	 0x0016	Super Potion	Super Potion	Bag Items pocket icon.png	 Items pocket
23	 0x0017	Full Heal	Full Heal	Bag Items pocket icon.png	 Items pocket
24	 0x0018	Revive	Revive	Bag Items pocket icon.png	 Items pocket
25	 0x0019	Max Revive	Max Revive	Bag Items pocket icon.png	 Items pocket
26	 0x001A	Fresh Water	Fresh Water	Bag Items pocket icon.png	 Items pocket
27	 0x001B	Soda Pop	Soda Pop	Bag Items pocket icon.png	 Items pocket
28	 0x001C	Lemonade	Lemonade	Bag Items pocket icon.png	 Items pocket
29	 0x001D	Moomoo Milk	Moomoo Milk	Bag Items pocket icon.png	 Items pocket
30	 0x001E	EnergyPowder	EnergyPowder	Bag Items pocket icon.png	 Items pocket
31	 0x001F	Energy Root	Energy Root	Bag Items pocket icon.png	 Items pocket
32	 0x0020	Heal Powder	Heal Powder	Bag Items pocket icon.png	 Items pocket
33	 0x0021	Revival Herb	Revival Herb	Bag Items pocket icon.png	 Items pocket
34	 0x0022	Ether	Ether	Bag Items pocket icon.png	 Items pocket
35	 0x0023	Max Ether	Max Ether	Bag Items pocket icon.png	 Items pocket
36	 0x0024	Elixir	Elixir	Bag Items pocket icon.png	 Items pocket
37	 0x0025	Max Elixir	Max Elixir	Bag Items pocket icon.png	 Items pocket
38	 0x0026	Lava Cookie	Lava Cookie	Bag Items pocket icon.png	 Items pocket
39	 0x0027	Blue Flute	Blue Flute	Bag Items pocket icon.png	 Items pocket
40	 0x0028	Yellow Flute	Yellow Flute	Bag Items pocket icon.png	 Items pocket
41	 0x0029	Red Flute	Red Flute	Bag Items pocket icon.png	 Items pocket
42	 0x002A	Black Flute	Black Flute	Bag Items pocket icon.png	 Items pocket
43	 0x002B	White Flute	White Flute	Bag Items pocket icon.png	 Items pocket
44	 0x002C	Berry Juice	Berry Juice	Bag Items pocket icon.png	 Items pocket
45	 0x002D	Sacred Ash	Sacred Ash	Bag Items pocket icon.png	 Items pocket
46	 0x002E	Shoal Salt	Shoal Salt	Bag Items pocket icon.png	 Items pocket
47	 0x002F	Shoal Shell	Shoal Shell	Bag Items pocket icon.png	 Items pocket
48	 0x0030	Red Shard	Red Shard	Bag Items pocket icon.png	 Items pocket
49	 0x0031	Blue Shard	Blue Shard	Bag Items pocket icon.png	 Items pocket
50	 0x0032	Yellow Shard	Yellow Shard	Bag Items pocket icon.png	 Items pocket
51	 0x0033	Green Shard	Green Shard	Bag Items pocket icon.png	 Items pocket
52	 0x0034	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
53	 0x0035	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
54	 0x0036	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
55	 0x0037	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
56	 0x0038	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
57	 0x0039	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
58	 0x003A	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
59	 0x003B	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
60	 0x003C	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
61	 0x003D	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
62	 0x003E	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
63	 0x003F	HP Up	HP Up	Bag Items pocket icon.png	 Items pocket
64	 0x0040	Protein	Protein	Bag Items pocket icon.png	 Items pocket
65	 0x0041	Iron	Iron	Bag Items pocket icon.png	 Items pocket
66	 0x0042	Carbos	Carbos	Bag Items pocket icon.png	 Items pocket
67	 0x0043	Calcium	Calcium	Bag Items pocket icon.png	 Items pocket
68	 0x0044	Rare Candy	Rare Candy	Bag Items pocket icon.png	 Items pocket
69	 0x0045	PP Up	PP Up	Bag Items pocket icon.png	 Items pocket
70	 0x0046	Zinc	Zinc	Bag Items pocket icon.png	 Items pocket
71	 0x0047	PP Max	PP Max	Bag Items pocket icon.png	 Items pocket
72	 0x0048	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
73	 0x0049	Guard Spec.	Guard Spec.	Bag Items pocket icon.png	 Items pocket
74	 0x004A	Dire Hit	Dire Hit	Bag Items pocket icon.png	 Items pocket
75	 0x004B	X Attack	X Attack	Bag Items pocket icon.png	 Items pocket
76	 0x004C	X Defend	X Defend	Bag Items pocket icon.png	 Items pocket
77	 0x004D	X Speed	X Speed	Bag Items pocket icon.png	 Items pocket
78	 0x004E	X Accuracy	X Accuracy	Bag Items pocket icon.png	 Items pocket
79	 0x004F	X Special	X Special	Bag Items pocket icon.png	 Items pocket
80	 0x0050	Poké Doll	Poké Doll	Bag Items pocket icon.png	 Items pocket
81	 0x0051	Fluffy Tail	Fluffy Tail	Bag Items pocket icon.png	 Items pocket
82	 0x0052	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
83	 0x0053	Super Repel	Super Repel	Bag Items pocket icon.png	 Items pocket
84	 0x0054	Max Repel	Max Repel	Bag Items pocket icon.png	 Items pocket
85	 0x0055	Escape Rope	Escape Rope	Bag Items pocket icon.png	 Items pocket
86	 0x0056	Repel	Repel	Bag Items pocket icon.png	 Items pocket
87	 0x0057	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
88	 0x0058	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
89	 0x0059	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
90	 0x005A	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
91	 0x005B	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
92	 0x005C	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
93	 0x005D	Sun Stone	Sun Stone	Bag Items pocket icon.png	 Items pocket
94	 0x005E	Moon Stone	Moon Stone	Bag Items pocket icon.png	 Items pocket
95	 0x005F	Fire Stone	Fire Stone	Bag Items pocket icon.png	 Items pocket
96	 0x0060	Thunder Stone	Thunder Stone	Bag Items pocket icon.png	 Items pocket
97	 0x0061	Water Stone	Water Stone	Bag Items pocket icon.png	 Items pocket
98	 0x0062	Leaf Stone	Leaf Stone	Bag Items pocket icon.png	 Items pocket
99	 0x0063	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
100	 0x0064	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
101	 0x0065	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
102	 0x0066	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
103	 0x0067	TinyMushroom	TinyMushroom	Bag Items pocket icon.png	 Items pocket
104	 0x0068	Big Mushroom	Big Mushroom	Bag Items pocket icon.png	 Items pocket
105	 0x0069	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
106	 0x006A	Pearl	Pearl	Bag Items pocket icon.png	 Items pocket
107	 0x006B	Big Pearl	Big Pearl	Bag Items pocket icon.png	 Items pocket
108	 0x006C	Stardust	Stardust	Bag Items pocket icon.png	 Items pocket
109	 0x006D	Star Piece	Star Piece	Bag Items pocket icon.png	 Items pocket
110	 0x006E	Nugget	Nugget	Bag Items pocket icon.png	 Items pocket
111	 0x006F	Heart Scale	Heart Scale	Bag Items pocket icon.png	 Items pocket
112	 0x0070	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
113	 0x0071	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
114	 0x0072	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
115	 0x0073	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
116	 0x0074	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
117	 0x0075	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
118	 0x0076	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
119	 0x0077	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
120	 0x0078	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
121	 0x0079	Orange Mail	Orange Mail	Bag Items pocket icon.png	 Items pocket
122	 0x007A	Harbor Mail	Harbor Mail	Bag Items pocket icon.png	 Items pocket
123	 0x007B	Glitter Mail	Glitter Mail	Bag Items pocket icon.png	 Items pocket
124	 0x007C	Mech Mail	Mech Mail	Bag Items pocket icon.png	 Items pocket
125	 0x007D	Wood Mail	Wood Mail	Bag Items pocket icon.png	 Items pocket
126	 0x007E	Wave Mail	Wave Mail	Bag Items pocket icon.png	 Items pocket
127	 0x007F	Bead Mail	Bead Mail	Bag Items pocket icon.png	 Items pocket
128	 0x0080	Shadow Mail	Shadow Mail	Bag Items pocket icon.png	 Items pocket
129	 0x0081	Tropic Mail	Tropic Mail	Bag Items pocket icon.png	 Items pocket
130	 0x0082	Dream Mail	Dream Mail	Bag Items pocket icon.png	 Items pocket
131	 0x0083	Fab Mail	Fab Mail	Bag Items pocket icon.png	 Items pocket
132	 0x0084	Retro Mail	Retro Mail	Bag Items pocket icon.png	 Items pocket
133	 0x0085	Cheri Berry	Cheri Berry	Bag Berries pocket icon.png	 Berries pocket
134	 0x0086	Chesto Berry	Chesto Berry	Bag Berries pocket icon.png	 Berries pocket
135	 0x0087	Pecha Berry	Pecha Berry	Bag Berries pocket icon.png	 Berries pocket
136	 0x0088	Rawst Berry	Rawst Berry	Bag Berries pocket icon.png	 Berries pocket
137	 0x0089	Aspear Berry	Aspear Berry	Bag Berries pocket icon.png	 Berries pocket
138	 0x008A	Leppa Berry	Leppa Berry	Bag Berries pocket icon.png	 Berries pocket
139	 0x008B	Oran Berry	Oran Berry	Bag Berries pocket icon.png	 Berries pocket
140	 0x008C	Persim Berry	Persim Berry	Bag Berries pocket icon.png	 Berries pocket
141	 0x008D	Lum Berry	Lum Berry	Bag Berries pocket icon.png	 Berries pocket
142	 0x008E	Sitrus Berry	Sitrus Berry	Bag Berries pocket icon.png	 Berries pocket
143	 0x008F	Figy Berry	Figy Berry	Bag Berries pocket icon.png	 Berries pocket
144	 0x0090	Wiki Berry	Wiki Berry	Bag Berries pocket icon.png	 Berries pocket
145	 0x0091	Mago Berry	Mago Berry	Bag Berries pocket icon.png	 Berries pocket
146	 0x0092	Aguav Berry	Aguav Berry	Bag Berries pocket icon.png	 Berries pocket
147	 0x0093	Iapapa Berry	Iapapa Berry	Bag Berries pocket icon.png	 Berries pocket
148	 0x0094	Razz Berry	Razz Berry	Bag Berries pocket icon.png	 Berries pocket
149	 0x0095	Bluk Berry	Bluk Berry	Bag Berries pocket icon.png	 Berries pocket
150	 0x0096	Nanab Berry	Nanab Berry	Bag Berries pocket icon.png	 Berries pocket
151	 0x0097	Wepear Berry	Wepear Berry	Bag Berries pocket icon.png	 Berries pocket
152	 0x0098	Pinap Berry	Pinap Berry	Bag Berries pocket icon.png	 Berries pocket
153	 0x0099	Pomeg Berry	Pomeg Berry	Bag Berries pocket icon.png	 Berries pocket
154	 0x009A	Kelpsy Berry	Kelpsy Berry	Bag Berries pocket icon.png	 Berries pocket
155	 0x009B	Qualot Berry	Qualot Berry	Bag Berries pocket icon.png	 Berries pocket
156	 0x009C	Hondew Berry	Hondew Berry	Bag Berries pocket icon.png	 Berries pocket
157	 0x009D	Grepa Berry	Grepa Berry	Bag Berries pocket icon.png	 Berries pocket
158	 0x009E	Tamato Berry	Tamato Berry	Bag Berries pocket icon.png	 Berries pocket
159	 0x009F	Cornn Berry	Cornn Berry	Bag Berries pocket icon.png	 Berries pocket
160	 0x00A0	Magost Berry	Magost Berry	Bag Berries pocket icon.png	 Berries pocket
161	 0x00A1	Rabuta Berry	Rabuta Berry	Bag Berries pocket icon.png	 Berries pocket
162	 0x00A2	Nomel Berry	Nomel Berry	Bag Berries pocket icon.png	 Berries pocket
163	 0x00A3	Spelon Berry	Spelon Berry	Bag Berries pocket icon.png	 Berries pocket
164	 0x00A4	Pamtre Berry	Pamtre Berry	Bag Berries pocket icon.png	 Berries pocket
165	 0x00A5	Watmel Berry	Watmel Berry	Bag Berries pocket icon.png	 Berries pocket
166	 0x00A6	Durin Berry	Durin Berry	Bag Berries pocket icon.png	 Berries pocket
167	 0x00A7	Belue Berry	Belue Berry	Bag Berries pocket icon.png	 Berries pocket
168	 0x00A8	Liechi Berry	Liechi Berry	Bag Berries pocket icon.png	 Berries pocket
169	 0x00A9	Ganlon Berry	Ganlon Berry	Bag Berries pocket icon.png	 Berries pocket
170	 0x00AA	Salac Berry	Salac Berry	Bag Berries pocket icon.png	 Berries pocket
171	 0x00AB	Petaya Berry	Petaya Berry	Bag Berries pocket icon.png	 Berries pocket
172	 0x00AC	Apicot Berry	Apicot Berry	Bag Berries pocket icon.png	 Berries pocket
173	 0x00AD	Lansat Berry	Lansat Berry	Bag Berries pocket icon.png	 Berries pocket
174	 0x00AE	Starf Berry	Starf Berry	Bag Berries pocket icon.png	 Berries pocket
175	 0x00AF	Enigma Berry	Enigma Berry	Bag Berries pocket icon.png	 Berries pocket
176	 0x00B0	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
177	 0x00B1	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
178	 0x00B2	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
179	 0x00B3	BrightPowder	BrightPowder	Bag Items pocket icon.png	 Items pocket
180	 0x00B4	White Herb	White Herb	Bag Items pocket icon.png	 Items pocket
181	 0x00B5	Macho Brace	Macho Brace	Bag Items pocket icon.png	 Items pocket
182	 0x00B6	Exp. Share	Exp. Share	Bag Items pocket icon.png	 Items pocket
183	 0x00B7	Quick Claw	Quick Claw	Bag Items pocket icon.png	 Items pocket
184	 0x00B8	Soothe Bell	Soothe Bell	Bag Items pocket icon.png	 Items pocket
185	 0x00B9	Mental Herb	Mental Herb	Bag Items pocket icon.png	 Items pocket
186	 0x00BA	Choice Band	Choice Band	Bag Items pocket icon.png	 Items pocket
187	 0x00BB	King's Rock	King's Rock	Bag Items pocket icon.png	 Items pocket
188	 0x00BC	SilverPowder	SilverPowder	Bag Items pocket icon.png	 Items pocket
189	 0x00BD	Amulet Coin	Amulet Coin	Bag Items pocket icon.png	 Items pocket
190	 0x00BE	Cleanse Tag	Cleanse Tag	Bag Items pocket icon.png	 Items pocket
191	 0x00BF	Soul Dew	Soul Dew	Bag Items pocket icon.png	 Items pocket
192	 0x00C0	DeepSeaTooth	DeepSeaTooth	Bag Items pocket icon.png	 Items pocket
193	 0x00C1	DeepSeaScale	DeepSeaScale	Bag Items pocket icon.png	 Items pocket
194	 0x00C2	Smoke Ball	Smoke Ball	Bag Items pocket icon.png	 Items pocket
195	 0x00C3	Everstone	Everstone	Bag Items pocket icon.png	 Items pocket
196	 0x00C4	Focus Band	Focus Band	Bag Items pocket icon.png	 Items pocket
197	 0x00C5	Lucky Egg	Lucky Egg	Bag Items pocket icon.png	 Items pocket
198	 0x00C6	Scope Lens	Scope Lens	Bag Items pocket icon.png	 Items pocket
199	 0x00C7	Metal Coat	Metal Coat	Bag Items pocket icon.png	 Items pocket
200	 0x00C8	Leftovers	Leftovers	Bag Items pocket icon.png	 Items pocket
201	 0x00C9	Dragon Scale	Dragon Scale	Bag Items pocket icon.png	 Items pocket
202	 0x00CA	Light Ball	Light Ball	Bag Items pocket icon.png	 Items pocket
203	 0x00CB	Soft Sand	Soft Sand	Bag Items pocket icon.png	 Items pocket
204	 0x00CC	Hard Stone	Hard Stone	Bag Items pocket icon.png	 Items pocket
205	 0x00CD	Miracle Seed	Miracle Seed	Bag Items pocket icon.png	 Items pocket
206	 0x00CE	BlackGlasses	BlackGlasses	Bag Items pocket icon.png	 Items pocket
207	 0x00CF	Black Belt	Black Belt	Bag Items pocket icon.png	 Items pocket
208	 0x00D0	Magnet	Magnet	Bag Items pocket icon.png	 Items pocket
209	 0x00D1	Mystic Water	Mystic Water	Bag Items pocket icon.png	 Items pocket
210	 0x00D2	Sharp Beak	Sharp Beak	Bag Items pocket icon.png	 Items pocket
211	 0x00D3	Poison Barb	Poison Barb	Bag Items pocket icon.png	 Items pocket
212	 0x00D4	NeverMeltIce	NeverMeltIce	Bag Items pocket icon.png	 Items pocket
213	 0x00D5	Spell Tag	Spell Tag	Bag Items pocket icon.png	 Items pocket
214	 0x00D6	TwistedSpoon	TwistedSpoon	Bag Items pocket icon.png	 Items pocket
215	 0x00D7	Charcoal	Charcoal	Bag Items pocket icon.png	 Items pocket
216	 0x00D8	Dragon Fang	Dragon Fang	Bag Items pocket icon.png	 Items pocket
217	 0x00D9	Silk Scarf	Silk Scarf	Bag Items pocket icon.png	 Items pocket
218	 0x00DA	Up-Grade	Up-Grade	Bag Items pocket icon.png	 Items pocket
219	 0x00DB	Shell Bell	Shell Bell	Bag Items pocket icon.png	 Items pocket
220	 0x00DC	Sea Incense	Sea Incense	Bag Items pocket icon.png	 Items pocket
221	 0x00DD	Lax Incense	Lax Incense	Bag Items pocket icon.png	 Items pocket
222	 0x00DE	Lucky Punch	Lucky Punch	Bag Items pocket icon.png	 Items pocket
223	 0x00DF	Metal Powder	Metal Powder	Bag Items pocket icon.png	 Items pocket
224	 0x00E0	Thick Club	Thick Club	Bag Items pocket icon.png	 Items pocket
225	 0x00E1	Stick	Stick	Bag Items pocket icon.png	 Items pocket
226	 0x00E2	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
227	 0x00E3	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
228	 0x00E4	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
229	 0x00E5	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
230	 0x00E6	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
231	 0x00E7	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
232	 0x00E8	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
233	 0x00E9	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
234	 0x00EA	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
235	 0x00EB	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
236	 0x00EC	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
237	 0x00ED	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
238	 0x00EE	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
239	 0x00EF	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
240	 0x00F0	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
241	 0x00F1	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
242	 0x00F2	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
243	 0x00F3	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
244	 0x00F4	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
245	 0x00F5	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
246	 0x00F6	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
247	 0x00F7	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
248	 0x00F8	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
249	 0x00F9	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
250	 0x00FA	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
251	 0x00FB	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
252	 0x00FC	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
253	 0x00FD	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
254	 0x00FE	Red Scarf	Red Scarf	Bag Items pocket icon.png	 Items pocket
255	 0x00FF	Blue Scarf	Blue Scarf	Bag Items pocket icon.png	 Items pocket
256	 0x0100	Pink Scarf	Pink Scarf	Bag Items pocket icon.png	 Items pocket
257	 0x0101	Green Scarf	Green Scarf	Bag Items pocket icon.png	 Items pocket
258	 0x0102	Yellow Scarf	Yellow Scarf	Bag Items pocket icon.png	 Items pocket
259	 0x0103	Mach Bike	Mach Bike	Bag Key items pocket icon.png	 Key items pocket
260	 0x0104	Coin Case	Coin Case	Bag Key items pocket icon.png	 Key items pocket
261	 0x0105	Itemfinder	Itemfinder	Bag Key items pocket icon.png	 Key items pocket
262	 0x0106	Old Rod	Old Rod	Bag Key items pocket icon.png	 Key items pocket
263	 0x0107	Good Rod	Good Rod	Bag Key items pocket icon.png	 Key items pocket
264	 0x0108	Super Rod	Super Rod	Bag Key items pocket icon.png	 Key items pocket
265	 0x0109	S.S. Ticket	S.S. Ticket	Bag Key items pocket icon.png	 Key items pocket
266	 0x010A	Contest Pass	Contest Pass	Bag Key items pocket icon.png	 Key items pocket
267	 0x010B	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
268	 0x010C	Wailmer Pail	Wailmer Pail	Bag Key items pocket icon.png	 Key items pocket
269	 0x010D	Devon Goods	Devon Goods	Bag Key items pocket icon.png	 Key items pocket
270	 0x010E	Soot Sack	Soot Sack	Bag Key items pocket icon.png	 Key items pocket
271	 0x010F	Basement Key III	Basement Key	Bag Key items pocket icon.png	 Key items pocket
272	 0x0110	Acro Bike	Acro Bike	Bag Key items pocket icon.png	 Key items pocket
273	 0x0111	Pokéblock Case	Pokéblock Case	Bag Key items pocket icon.png	 Key items pocket
274	 0x0112	Letter	Letter	Bag Key items pocket icon.png	 Key items pocket
275	 0x0113	Eon Ticket	Eon Ticket	Bag Key items pocket icon.png	 Key items pocket
276	 0x0114	Red Orb III	Red Orb	Bag Key items pocket icon.png	 Key items pocket
277	 0x0115	Blue Orb III	Blue Orb	Bag Key items pocket icon.png	 Key items pocket
278	 0x0116	Scanner	Scanner	Bag Key items pocket icon.png	 Key items pocket
279	 0x0117	Go-Goggles	Go-Goggles	Bag Key items pocket icon.png	 Key items pocket
280	 0x0118	Meteorite	Meteorite	Bag Key items pocket icon.png	 Key items pocket
281	 0x0119	Rm. 1 Key	Rm. 1 Key	Bag Key items pocket icon.png	 Key items pocket
282	 0x011A	Rm. 2 Key	Rm. 2 Key	Bag Key items pocket icon.png	 Key items pocket
283	 0x011B	Rm. 4 Key	Rm. 4 Key	Bag Key items pocket icon.png	 Key items pocket
284	 0x011C	Rm. 6 Key	Rm. 6 Key	Bag Key items pocket icon.png	 Key items pocket
285	 0x011D	Storage Key III	Storage Key	Bag Key items pocket icon.png	 Key items pocket
286	 0x011E	Root Fossil	Root Fossil	Bag Key items pocket icon.png	 Key items pocket
287	 0x011F	Claw Fossil	Claw Fossil	Bag Key items pocket icon.png	 Key items pocket
288	 0x0120	Devon Scope	Devon Scope	Bag Key items pocket icon.png	 Key items pocket
289	 0x0121	TM Fighting	TM01	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
290	 0x0122	TM Dragon	TM02	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
291	 0x0123	TM Water	TM03	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
292	 0x0124	TM Psychic	TM04	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
293	 0x0125	TM Normal	TM05	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
294	 0x0126	TM Poison	TM06	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
295	 0x0127	TM Ice	TM07	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
296	 0x0128	TM Fighting	TM08	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
297	 0x0129	TM Grass	TM09	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
298	 0x012A	TM Normal	TM10	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
299	 0x012B	TM Fire	TM11	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
300	 0x012C	TM Dark	TM12	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
301	 0x012D	TM Ice	TM13	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
302	 0x012E	TM Ice	TM14	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
303	 0x012F	TM Normal	TM15	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
304	 0x0130	TM Psychic	TM16	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
305	 0x0131	TM Normal	TM17	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
306	 0x0132	TM Water	TM18	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
307	 0x0133	TM Grass	TM19	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
308	 0x0134	TM Normal	TM20	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
309	 0x0135	TM Normal	TM21	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
310	 0x0136	TM Grass	TM22	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
311	 0x0137	TM Steel	TM23	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
312	 0x0138	TM Electric	TM24	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
313	 0x0139	TM Electric	TM25	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
314	 0x013A	TM Ground	TM26	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
315	 0x013B	TM Normal	TM27	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
316	 0x013C	TM Ground	TM28	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
317	 0x013D	TM Psychic	TM29	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
318	 0x013E	TM Ghost	TM30	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
319	 0x013F	TM Fighting	TM31	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
320	 0x0140	TM Normal	TM32	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
321	 0x0141	TM Psychic	TM33	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
322	 0x0142	TM Electric	TM34	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
323	 0x0143	TM Fire	TM35	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
324	 0x0144	TM Poison	TM36	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
325	 0x0145	TM Rock	TM37	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
326	 0x0146	TM Fire	TM38	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
327	 0x0147	TM Rock	TM39	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
328	 0x0148	TM Flying	TM40	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
329	 0x0149	TM Dark	TM41	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
330	 0x014A	TM Normal	TM42	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
331	 0x014B	TM Normal	TM43	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
332	 0x014C	TM Psychic	TM44	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
333	 0x014D	TM Normal	TM45	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
334	 0x014E	TM Dark	TM46	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
335	 0x014F	TM Steel	TM47	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
336	 0x0150	TM Psychic	TM48	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
337	 0x0151	TM Dark	TM49	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
338	 0x0152	TM Fire	TM50	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
339	 0x0153	HM Normal	HM01	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
340	 0x0154	HM Flying	HM02	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
341	 0x0155	HM Water	HM03	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
342	 0x0156	HM Normal	HM04	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
343	 0x0157	HM Normal	HM05	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
344	 0x0158	HM Fighting	HM06	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
345	 0x0159	HM Water	HM07	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
346	 0x015A	HM Water	HM08	Bag TMs and HMs pocket icon.png	 TMs and HMs pocket
347	 0x15B	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
348	 0x15C	unknown	 unknown	Bag Unknown pocket icon.png	 Unknown pocket
349	 0x015D	Oak's Parcel	Oak's Parcel*	Bag Key items pocket icon.png	 Key items pocket
350	 0x015E	Poké Flute	Poké Flute*	Bag Key items pocket icon.png	 Key items pocket
351	 0x015F	Secret Key III	Secret Key*	Bag Key items pocket icon.png	 Key items pocket
352	 0x0160	Bike Voucher	Bike Voucher*	Bag Key items pocket icon.png	 Key items pocket
353	 0x0161	Gold Teeth	Gold Teeth*	Bag Key items pocket icon.png	 Key items pocket
354	 0x0162	Old Amber	Old Amber*	Bag Key items pocket icon.png	 Key items pocket
355	 0x0163	Card Key III	Card Key*	Bag Key items pocket icon.png	 Key items pocket
356	 0x0164	Lift Key	Lift Key*	Bag Key items pocket icon.png	 Key items pocket
357	 0x0165	Dome Fossil	Dome Fossil*	Bag Key items pocket icon.png	 Key items pocket
358	 0x0166	Helix Fossil	Helix Fossil*	Bag Key items pocket icon.png	 Key items pocket
359	 0x0167	Silph Scope	Silph Scope*	Bag Key items pocket icon.png	 Key items pocket
360	 0x0168	Bicycle	Bicycle*	Bag Key items pocket icon.png	 Key items pocket
361	 0x0169	Town Map III	Town Map*	Bag Key items pocket icon.png	 Key items pocket
362	 0x016A	Vs. Seeker	Vs. Seeker*	Bag Key items pocket icon.png	 Key items pocket
363	 0x016B	Fame Checker	Fame Checker*	Bag Key items pocket icon.png	 Key items pocket
364	 0x016C	TM Case	TM Case*	Bag Key items pocket icon.png	 Key items pocket
365	 0x016D	Berry Pouch	Berry Pouch*	Bag Key items pocket icon.png	 Key items pocket
366	 0x016E	Teachy TV	Teachy TV*	Bag Key items pocket icon.png	 Key items pocket
367	 0x016F	Tri-Pass	Tri-Pass*	Bag Key items pocket icon.png	 Key items pocket
368	 0x0170	Rainbow Pass	Rainbow Pass*	Bag Key items pocket icon.png	 Key items pocket
369	 0x0171	Tea	Tea*	Bag Key items pocket icon.png	 Key items pocket
370	 0x0172	MysticTicket	MysticTicket*	Bag Key items pocket icon.png	 Key items pocket
371	 0x0173	AuroraTicket	AuroraTicket*	Bag Key items pocket icon.png	 Key items pocket
372	 0x0174	Powder Jar	Powder Jar*	Bag Key items pocket icon.png	 Key items pocket
373	 0x0175	Ruby	Ruby*	Bag Key items pocket icon.png	 Key items pocket
374	 0x0176	Sapphire	Sapphire*	Bag Key items pocket icon.png	 Key items pocket
375	 0x0177	Magma Emblem	Magma Emblem*	Bag Key items pocket icon.png	 Key items pocket
376	 0x0178	Old Sea Map	Old Sea Map*	Bag Key items pocket icon.png	 Key items pocket";
		#endregion

		static Dictionary<uint, string> _names;
		public static void Init()
		{
			if( _names != null )
				return;

			_names = new Dictionary<uint, string>();
			var lines = Datasource.Split( new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries );
			foreach( var entry in lines )
			{
				var d = entry.Split( '\t' );
				_names.Add( UInt32.Parse( d[0] ), d[3] );
			}
		}

		public static string Get( uint index )
		{
			Init();
			if( _names.ContainsKey( index ) )
				return _names[index];
			return "BAD ITEM ID";
		}

		public static string Get( string index )
		{
			Init();

			return _names[UInt32.Parse( index )];
		}
	}
}


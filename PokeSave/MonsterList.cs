﻿
using System;
using System.Collections.Generic;

namespace PokeSave
{
	public static class MonsterList
	{

		#region # = Name
		const string Datasource = @"001 = BULBASAUR 
002 = IVYSAUR 
003 = VENUSAUR 
004 = CHARMANDER 
005 = CHARMELEON 
006 = CHARIZARD 
007 = SQUIRTLE 
008 = WARTORTLE 
009 = BLASTOISE 
010 = CATERPIE 
011 = METAPOD 
012 = BUTTERFREE 
013 = WEEDLE 
014 = KAKUNA 
015 = BEEDRILL 
016 = PIDGEY 
017 = PIDGEOTTO 
018 = PIDGEOT 
019 = RATTATA 
020 = RATICATE 
021 = SPEAROW 
022 = FEAROW 
023 = EKANS 
024 = ARBOK 
025 = PIKACHU 
026 = RAICHU 
027 = SANDSHREW 
028 = SANDSLASH 
029 = NIDORAN Female 
030 = NIDORINA 
031 = NIDOQUEEN 
032 = NIDORAN Male 
033 = NIDORINO 
034 = NIDOKING 
035 = CLEFAIRY 
036 = CLEFABLE 
037 = VULPIX 
038 = NINETALES 
039 = JIGGLYPUFF 
040 = WIGGLYTUFF 
041 = ZUBAT 
042 = GOLBAT 
043 = ODDISH 
044 = GLOOM 
045 = VILEPLUME 
046 = PARAS 
047 = PARASECT 
048 = VENONAT 
049 = VENOMOTH 
050 = DIGLETT 
051 = DUGTRIO 
052 = MEOWTH 
053 = PERSIAN 
054 = PSYDUCK 
055 = GOLDUCK 
056 = MANKEY 
057 = PRIMEAPE 
058 = GROWLITHE 
059 = ARCANINE 
060 = POLIWAG 
061 = POLIWHIRL 
062 = POLIWRATH 
063 = ABRA 
064 = KADABRA 
065 = ALAKAZAM 
066 = MACHOP 
067 = MACHOKE 
068 = MACHAMP 
069 = BELLSPROUT 
070 = WEEPINBELL 
071 = VICTREEBEL 
072 = TENTACOOL 
073 = TENTACRUEL 
074 = GEODUDE 
075 = GRAVELER 
076 = GOLEM 
077 = PONYTA 
078 = RAPIDASH 
079 = SLOWPOKE 
080 = SLOWBRO
081 = MAGNEMITE
082 = MAGNETON
083 = FARFETCH'D
084 = DODUO
085 = DODRIO
086 = SEEL
087 = DEWGONG
088 = GRIMER
089 = MUK
090 = SHELLDER
091 = CLOYSTER
092 = GASTLY
093 = HAUNTER
094 = GENGAR
095 = ONIX
096 = DROWZEE
097 = HYPNO
098 = KRABBY
099 = KINGLER
100 = VOLTORB
101 = ELECTRODE
102 = EXEGGCUTE
103 = EXEGGUTOR
104 = CUBONE
105 = MAROWAK
106 = HITMONLEE
107 = HITMONCHAN
108 = LICKITUNG
109 = KOFFING
110 = WEEZING
111 = RHYHORN
112 = RHYDON
113 = CHANSEY
114 = TANGELA
115 = KANGASKHAN
116 = HORSEA
117 = SEADRA
118 = GOLDEEN
119 = SEAKING
120 = STARYU
121 = STARMIE
122 = MR.MIME
123 = SCYTHER
124 = JYNX
125 = ELECTABUZZ
126 = MAGMAR
127 = PINSIR
128 = TAUROS
129 = MAGIKARP
130 = GYARADOS
131 = LAPRAS
132 = DITTO
133 = EEVEE
134 = VAPOREON
135 = JOLTEON
136 = FLAREON
137 = PORYGON
138 = OMANYTE
139 = OMASTAR
140 = KABUTO
141 = KABUTOPS
142 = AERODACTYL
143 = SNORLAX
144 = ARTICUNO
145 = ZAPDOS
146 = MOLTRES
147 = DRATINI
148 = DRAGONAIR
149 = DRAGONITE
150 = MEWTWO
151 = MEW
152 = CHIKORITA
153 = BAYLEEF
154 = MEGANIUM
155 = CYNDAQUIL
156 = QUILAVA
157 = TYPHLOSION
158 = TOTODILE
159 = CROCONAW
160 = FERALIGATR
161 = SENTRET
162 = FURRET
163 = HOOTHOOT
164 = NOCTOWL
165 = LEDYBA
166 = LEDIAN
167 = SPINARAK
168 = ARIADOS
169 = CROBAT
170 = CHINCHOU
171 = LANTURN
172 = PICHU
173 = CLEFFA
174 = IGGLYBUFF
175 = TOGEPI
176 = TOGETIC
177 = NATU
178 = XATU
179 = MAREEP
180 = FLAAFFY
181 = AMPHAROS
182 = BELLOSSOM
183 = MARILL
184 = AZUMARILL
185 = SUDOWOODO
186 = POLITOED
187 = HOPPIP
188 = SKIPLOOM
189 = JUMPLUFF
190 = AIPOM
191 = SUNKERN
192 = SUNFLORA
193 = YANMA
194 = WOOPER
195 = QUAGSIRE
196 = ESPEON
197 = UMBREON
198 = MURKROW
199 = SLOWKING
200 = MISDREAVUS
201 = UNOWN
202 = WOBBUFFET
203 = GIRAFARIG
204 = PINECO
205 = FORRETRESS
206 = DUNSPARCE
207 = GLIGAR
208 = STEELIX
209 = SNUBBULL
210 = GRANBULL
211 = QWILFISH
212 = SCIZOR
213 = SHUCKLE
214 = HERACROSS
215 = SNEASEL
216 = TEDDIURSA
217 = URSARING
218 = SLUGMA
219 = MAGCARGO
220 = SWINUB
221 = PILOSWINE
222 = CORSOLA
223 = REMORAID
224 = OCTILLERY
225 = DELIBIRD
226 = MANTINE
227 = SKARMORY
228 = HOUNDOUR
229 = HOUNDOOM
230 = KINGDRA
231 = PHANPY
232 = DONPHAN
233 = PORYGON
234 = STANTLER
235 = SMEARGLE
236 = TYROGUE
237 = HITMONTOP
238 = SMOOCHUM
239 = ELEKID
240 = MAGBY
241 = MILTANK
242 = BLISSEY
243 = RAIKOU
244 = ENTEI
245 = SUICUNE
246 = LARVITAR
247 = PUPITAR
248 = TYRANITAR
249 = LUGIA
250 = HO-OH
251 = CELEBI
277 = TREECKO
278 = GROVYLE
279 = SCEPTILE
280 = TORCHIC
281 = COMBUSKEN
282 = BLAZIKEN
283 = MUDKIP
284 = MARSHTOMP
285 = SWAMPERT
286 = POOCHYENA
287 = MIGHTYENA
288 = ZIGZAGOON
289 = LINOONE
290 = WURMPLE
291 = SILCOON
292 = BEAUTIFLY
293 = CASCOON
294 = DUSTOX
295 = LOTAD
296 = LOMBRE
297 = LUDICOLO
298 = SEEDOT
299 = NUZLEAF
300 = SHIFTRY
301 = NINCADA
302 = NINJASK
303 = SHEDINJA
304 = TAILLOW
305 = SWELLOW
306 = SHROOMISH
307 = BRELOOM
308 = SPINDA
309 = WINGULL
310 = PELIPPER
311 = SURSKIT
312 = MASQUERAIN
313 = WAILMER
314 = WAILORD
315 = SKITTY
316 = DELCATTY
317 = KECLEON
318 = BALTOY
319 = CLAYDOL
320 = NOSEPASS
321 = TORKOAL
322 = SABLEYE
323 = BARBOACH
324 = WHISCASH
325 = LUVDISC
326 = CORPHISH
327 = CRAWDAUNT
328 = FEEBAS
329 = MILOTIC
330 = CARVANHA
331 = SHARPEDO
332 = TRAPINCH
333 = VIBRAVA
334 = FLYGON
335 = MAKUHITA
336 = HARIYAMA
337 = ELECTRIKE
338 = MANECTRIC
339 = NUMEL
340 = CAMERUPT
341 = SPHEAL
342 = SEALEO
343 = WALREIN
344 = CACNEA
345 = CACTURNE
346 = SNORUNT
347 = GLALIE
348 = LUNATONE
349 = SOLROCK
350 = AZURILL
351 = SPOINK
352 = GRUMPIG
353 = PLUSLE
354 = MINUN
355 = MAWILE
356 = MEDITITE
357 = MEDICHAM
358 = SWABLU
359 = ALTARIA
360 = WYNAUT
361 = DUSKULL
362 = DUSCLOPS
363 = ROSELIA
364 = SLAKOTH
365 = VIGOROTH
366 = SLAKING
367 = GULPIN
368 = SWALOT
369 = TROPIUS
370 = WHISMUR
371 = LOUDRED
372 = EXPLOUD
373 = CLAMPERL
374 = HUNTAIL
375 = GOREBYSS
376 = ABSOL
377 = SHUPPET
378 = BANETTE
379 = SEVIPER
380 = ZANGOOSE
381 = RELICANTH
382 = ARON
383 = LAIRON
384 = AGGRON
385 = CASTFORM
386 = VOLBEAT
387 = ILLUMISE
388 = LILEEP
389 = CRADILY
390 = ANORITH
391 = ARMALDO
392 = RALTS
393 = KIRLIA
394 = GARDEVOIR
395 = BAGON
396 = SHELGON
397 = SALAMENCE
398 = BELDUM
399 = METANG
400 = METAGROSS
401 = REGIROCK
402 = REGICE
403 = REGISTEEL
404 = KYOGRE
405 = GROUDON
406 = RAYQUAZA
407 = LATIAS
408 = LATIOS
409 = JIRACHI
410 = DEOXYS
411 = CHIMECHO";
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
				var d = entry.Split( new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries );
				_names.Add( UInt32.Parse( d[0] ), d[1] );
			}
		}

		public static string Get( uint index )
		{
			Init();
			if( _names.ContainsKey( index ) )
				return _names[index];
			return "BAD EGG";
		}

		public static string Get( string index )
		{
			Init();

			return _names[UInt32.Parse( index )];
		}
	}
}


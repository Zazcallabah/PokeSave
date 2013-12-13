PokeSave
========

Todo
----
* There are still some substructures not fully mapped.
* How should we best parse time? Is it even needed?
* held Item should be combo box
* add button to make not outsider
* Level met doesnt show properly
* location met could use combo box with valid values
* Gameoforigin could use combo box with valid values
* Trying to change gender for certain types crashes (shouldn crash, may still be invalid)
* some stats are zero?
* language needs combo? - nah
* map ribbons
* add a bunch of typeinformation info
* Feature that automerges entries in all loaded files.
* Editable pokedex data and autoset that data when automerge.
* Show filename somewhere
* Feature to set 'Guess?'field
* simplify, Combine all typed lists into just stringlist
* Test pokedex seen and owned. Why are the different seen fields set?
* test set national? repair, section, merge
* add box wallpapers and names

Testing and code coverage
-------------------------
There are nunit tests in the cleverly named TestProject1 project.
I used OpenCover and ReportGenerator to generate statistics.
Put all bin-folders into a bin folder in the root of this repo, then run runtests.bat

* https://github.com/sawilde/opencover/
* http://reportgenerator.codeplex.com/
* http://www.nunit.org/


Thanks and sources
------------------
* [Bulbapedia](http://bulbapedia.bulbagarden.net/wiki/Save_data_structure_in_Generation_III)
* [KazoWAR](http://projectpokemon.org/forums/showthread.php?31254-A-Save-(3rd-Generation-Save-Editor))

License
-------
The source code of this application is licensed with a Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License. This does not include any lists of data and names used.

PokeSave
========

Todo
----
* There are still some substructures not fully mapped.
* How should we best parse time? Is it even needed?
* held Item should be combo box
* some layouting issues
* add button to make not outsider
* PC items not found
* Level met doesnt show properly
* location met could use combo box with valid values
* Gameoforigin could use combo box with valid values
* Trying to change gender for certain types crashes (shouldn crash, may still be invalid)
* PCBUffer takes ages to load
* some stats are zero?
* language needs combo? - nah
* map ribbons
* add a bunch of typeinformation info

Testing and code coverage
-------------------------
There are nunit tests in the cleverly named TestProject1 project.
I used OpenCover and ReportGenerator to generate statistics.
Put all bin-folders into a bin folder in the root of this repo, then run runtests.bat

* https://github.com/sawilde/opencover/
* http://reportgenerator.codeplex.com/
* http://www.nunit.org/

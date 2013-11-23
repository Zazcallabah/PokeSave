PokeSave
========

Todo
----
* Mainer is missing, so some sort of read line - execute command needs to be done. Possibly a GUI?
* There are still some substructures not fully mapped.
* Missing tests for some properties.
* How should we best parse time, is it even needed?
* Should data be validated, or should that be left to the user?

Testing and code coverage
-------------------------
There are nunit tests in the cleverly named TestProject1 project.
I used OpenCover and ReportGenerator to generate statistics.
Put all bin-folders into a bin folder in the root of this repo, then run runtests.bat

* https://github.com/sawilde/opencover/
* http://reportgenerator.codeplex.com/
* http://www.nunit.org/
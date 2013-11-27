@echo off
IF NOT EXIST bin\OpenCover.Console.exe (
 echo Missing opencover.console.exe
 pause
 exit 1
)

IF NOT EXIST bin\nunit-console.exe (
 echo Missing nunit-console.exe
 pause
 exit 1
)

IF NOT EXIST bin\ReportGenerator.exe (
 echo Missing ReportGenerator.exe
 pause
 exit 1
)
bin\OpenCover.Console.exe -register:user -target:bin\nunit-console.exe -targetdir:"TestProject1\bin\Debug" -targetargs:"TestProject1.dll" -filter:"+[Poke*]* -[TestProject1*]*" -output:output-PS.xml

bin\ReportGenerator.exe -reports:output-PS.xml -targetdir:testcoverage

testcoverage\index.htm

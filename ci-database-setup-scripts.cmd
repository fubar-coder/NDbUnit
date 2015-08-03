@echo on

rem SQL Server setup *****
sqlcmd -S ".\SQL2008R2SP2" -U dbunit -P Password12! -i "%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\sqlserver-testdb-create.sql"

rem MySQL setup *****
mysql --user=dbunit --password=Password12! < "%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\mysql-testdb-create.sql"

rem PostgreSQL setup *****
rem NOTE: postgres doesn't support password as args to psql so ensure that PGPASSWORD env var is set...
set PGPASSWORD=Password12!

rem NOTE: postgres won't support a CREATE DATABASE call inside a larger script, so CREATE has to be its own invocation...
psql --username=dbunit --no-password --command="CREATE DATABASE testdb;"

psql --username=dbunit --no-password --dbname=testdb --file="%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\postgres-testdb-create.sql"

rem ORACLE XE setup *****
rem *** DISABLED UNTIL APPVEYOR SUPPORTS ORA XE ON CI SERVERS ***
rem %ORACLE_HOME%\bin\sqlplus dbunit/Password12! @"%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\oracle-testdb-create.sql"


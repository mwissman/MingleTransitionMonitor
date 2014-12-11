@echo off
cls
for /f %%i in ('dir /s /b packages\Fake.exe') do SET FakePath=%%~i
%FakePath% build.fsx %*

@echo off
for /D %%s in (*) do (
    if exist %%s\CS (
        cd %%s\CS
        echo %%s starting...
        call RunTest.bat
        cd ..\..
        echo %%s finished.
    )
)

echo Run all tests finished.

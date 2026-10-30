@echo off
chcp 65001 >nul
cd /d "%~dp0"
python main.py
python -m unittest discover -s tests -v
pause

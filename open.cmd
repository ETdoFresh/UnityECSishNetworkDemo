git pull
start "Unity" "c:\Program Files\Unity\Hub\Editor\2019.1.14f1\Editor\Unity.exe" -projectPath .
for %%F IN (*.sln) do start %%F
start https://github.com/ETdoFresh/UnityECSishNetworkDemo/projects/1
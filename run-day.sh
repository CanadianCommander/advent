#!/bin/bash

if [ -a "src/$1/$2/main.ts" ]
then
  ts-node src/$1/$2/main.ts
else 
  dotnet run --project src/$1/$2
fi

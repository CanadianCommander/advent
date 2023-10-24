#!/bin/bash

if [ -a "src/$1/$2/main.ts" ]
then
  ts-node src/$1/$2/main.ts
else 
  pushd src/$1/$2 > /dev/null
  dotnet run 
  popd > /dev/null
fi

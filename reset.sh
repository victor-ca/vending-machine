#!/bin/bash

dotnet restore

dotnet tool list -g | grep "dotnet-ef"
hasEfCoreGlobal=$?

if [ $hasEfCoreGlobal -ne "0" ]; then
    echo "installing dotnet ef"
    dotnet tool install --global dotnet-ef
else
    echo "dotnet ef ok"
fi

dotnet ef database drop --project VendingMachine.EF -f && dotnet ef migrations remove --project VendingMachine.EF -f && dotnet ef migrations add InitialCreate --project VendingMachine.EF && dotnet ef database update --project VendingMachine.EF

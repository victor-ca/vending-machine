#!/bin/bash
npm i --prefix ./vending-machine-client && npm start --prefix ./vending-machine-client &
dotnet run --project ./VendingMachine.API/VendingMachine.API.csproj
